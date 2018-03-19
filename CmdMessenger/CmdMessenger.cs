using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO.Ports;
using InTheHand.Net.Sockets;
using CmdMessenger.Commands;
using CmdMessenger.CmdComms;

namespace CmdMessenger
{
    public class CmdMessenger : IDisposable
    {
        private class CommandWarper
        {
            #region Fields
            private TaskCompletionSource<IReceivedCommand> task;
            private DateTime timeSent;
            #endregion

            #region Constructors

            public CommandWarper() {
                task = new TaskCompletionSource<IReceivedCommand>();
                timeSent = DateTime.Now;
            }

            #endregion

            #region Properties

            public DateTime TimeSent {
                get { return timeSent; }
            }

            public TaskCompletionSource<IReceivedCommand> Task {
                get { return task; }
            }

            #endregion

            public void TrySetCanceled() {
                task.TrySetCanceled();

            }

            public void TrySetResult(IReceivedCommand command) {
                task.TrySetResult(command);
            }
        }

        private const int CommandTimeOutDueTime = 1000;
        private ICmdComms client;
        private readonly Dictionary<int, List<ICommandObserver>> commandHandlers;
        private readonly Timer commandTimeOut;
        private CancellationTokenSource cancellationTokenSource;
        private readonly Dictionary<int, CommandWarper> inflightCommands = new Dictionary<int, CommandWarper>();
        private bool disposed;
        private ILogger logger;

        /// <summary> 
        /// Initializes a new instance of the <see cref="CmdMessenger"/> class. 
        /// </summary>
        /// <param name="client">The command client.</param>
        /// <param name="logger">Logger object for logging process messages</param>
        public CmdMessenger(ICmdComms client, ILogger logger)
            : this(client, logger, Escaping.Default) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdMessenger"/> class. 
        /// </summary>
        /// <param name="client">The command client.</param>
        /// <param name="logger">Logger object for logging process messages</param>
        /// <param name="escaping">The escaping instance.</param>
        public CmdMessenger(ICmdComms client, ILogger logger, IEscaping escaping) {
            commandHandlers = new Dictionary<int, List<ICommandObserver>>();
            commandTimeOut = new Timer(ProcessTimedOutCommands);
            cancellationTokenSource = new CancellationTokenSource();
            this.client = client;
            this.logger = logger ?? throw new ArgumentNullException("logger");
        }

        /// <summary>
        /// Gets or sets whether to print a carriage return linefeed
        /// </summary>
        public bool PrintCarriageReturnLineFeed { get; set; }

        /// <summary>
        /// Gets the transport channel
        /// </summary>
        public TransportChannel TransportChannel {
            get {
                if (client == null)
                    return TransportChannel.Undefined;
                return client.TransportChannel;
            }
        }

        /// <summary>
        /// Gets or sets the command to check if the Arduino is alive
        /// </summary>
        public ISendCommand PingCommand { get; set; }

        /// <summary>
        /// Detect if the Arduino is a known bluetooth device
        /// </summary>
        /// <returns></returns>
        public async Task<bool> FindBluetooth() {
            bool arduinoFound = false;
            var tcs = new TaskCompletionSource<bool>();
            await Task.Run(async () => {
                try {
                    var localClient = new BluetoothClient();
                    var deviceList = localClient.DiscoverDevices();
                    foreach (BluetoothDeviceInfo device in deviceList) {
                        logger.LogMessage("Trying Bluetooth device: " + device.DeviceName);
                        var clientTmp = new BluetoothCmdClient(logger);
                        clientTmp.BtAddress = device.DeviceAddress;
                        if (await clientTmp.OpenAsync()) {

                            client = clientTmp;
                            if (await DetectArduino()) {
                                logger.LogMessage("Arduino is connected to Bluetooth device: " + device.DeviceName);
                                arduinoFound = true;
                                break;
                            }
                        }
                    }

                }
                catch (Exception e) {
                    logger.LogMessage("Exception while scanning bluetooth devices: " + e.Message);
                }
                if (!arduinoFound) {
                    logger.LogMessage("None of the known bluetooth devices is the Arduino.");
                }
                tcs.SetResult(arduinoFound);
            });
            return await tcs.Task;
        }

        /// <summary>
        /// Find the COM port to which the Arduino is connected
        /// </summary>
        /// <returns></returns>
        public async Task<bool> FindComPort() {
            bool arduinoFound = false;
            var tcs = new TaskCompletionSource<bool>();
            await Task.Run(async () => {
                try {
                    string[] ports = SerialPort.GetPortNames();
                    var clientTmp = new SerialCmdClient(logger);
                    foreach (string port in ports) {
                        logger.LogMessage("Trying serial port: " + port);
                        clientTmp.PortName = port;
                        clientTmp.BaudRate = 9600;
                        if (await clientTmp.OpenAsync()) {

                            client = clientTmp;
                            if (await DetectArduino()) {
                                logger.LogMessage("Arduino is connected to serial port: " + port);
                                arduinoFound = true;
                                break;
                            }
                        }
                    }
                }
                catch (Exception e) {
                    logger.LogMessage("Exception while detecting serial port: " + e.Message);
                }
                if (!arduinoFound) {
                    logger.LogMessage("No Arduino found on a serial port.");
                }
                tcs.SetResult(arduinoFound);
            });
            return await tcs.Task;
        }

        private async Task<bool> DetectArduino() {
            bool found = false;

            try {
                var cancellationTokenSourceTimeout = new CancellationTokenSource(1000);
                var cancellationTokenSourceSend = new CancellationTokenSource(1000);
                try {
                    var op = client.SendAsync(PingCommand, cancellationTokenSourceSend.Token);
                    await op.WithCancellation(cancellationTokenSourceTimeout.Token);
                }
                catch (OperationCanceledException) {
                    logger.LogMessage("Cancelled after time-out while sending on serial port.");
                    cancellationTokenSourceSend.Cancel();
                    return false;
                }

            }
            catch (Exception e) {
                logger.LogMessage("Exception while sending on serial port: " + e.Message);
                return false;
            }

            try {
                var cancellationTokenSourceTimeout = new CancellationTokenSource(1000);
                try {
                    Task<IReceivedCommand> op = client.ReadAsync(cancellationTokenSource.Token);
                    var command = await op.WithCancellation(cancellationTokenSourceTimeout.Token);
                    found = (command != null) && (command.ReadString() == "BFAF4176-766E-436A-ADF2-96133C02B03C");
                }
                catch (OperationCanceledException) {
                    logger.LogMessage("Cancelled after time-out while receiving on serial port.");
                    return false;
                }

            }
            catch (Exception e) {
                logger.LogMessage("Exception while reading serial port: " + e.Message);
                return false;
            }

            return found;
        }

        /// <summary>
        /// Open the connection and start receiving commands
        /// </summary>
        public async void Start() {
            commandTimeOut.Change(500, 500);

            if (client == null) {
                var arduinoFound = await FindBluetooth();
                if (!arduinoFound)
                    arduinoFound = await FindComPort();
                if (!arduinoFound)
                    return;
            }

            if (!client.IsOpen())
                await client.OpenAsync();

            logger.LogMessage("Connection opened");
            ProcessCommands();
        }

        private async void ProcessCommands() {
            logger.LogMessage("Processing commands");
            cancellationTokenSource = new CancellationTokenSource();
            while (!cancellationTokenSource.IsCancellationRequested) {
                try {
                    IReceivedCommand command = await client.ReadAsync(cancellationTokenSource.Token);
                    Debug.WriteLine("Received: "+ command.CommandId.ToString());
                    logger.LogMessage("Received (ProcessCommands): " + command.CommandId.ToString());
                    if (inflightCommands.ContainsKey(command.CommandId)) {
                        logger.LogMessage("Inflight command");
                        inflightCommands[command.CommandId].TrySetResult(command);
                    }

                    if (commandHandlers.ContainsKey(command.CommandId)) {
                        logger.LogMessage("Command handler");
                        var handler = commandHandlers[command.CommandId];
                        handler.ForEach(c => c.Update(command));
                    }
                }
                catch (TaskCanceledException) {
                    logger.LogMessage("TaskCanceledException");
                }
            }
        }

        /// <summary>
        /// Close the connection.
        /// </summary>
        public void Stop() {
            logger.LogMessage("Stop");
            cancellationTokenSource.Cancel();
            client.Close();
        }

        /// <summary>
        /// Send a command synchronously 
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <returns>The commands response.</returns>
        public IReceivedCommand Send(ISendCommand command) {
            logger.LogMessage("Send: " + command.CommandId.ToString());
            Task<IReceivedCommand> t = SendAsync(command);
            try {
                if (t.Result != null) {
                    logger.LogMessage("Received (Send): " + t.Result.CommandId.ToString());
                }
                return t.Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Send a command asynchronously.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <returns>A task completion source for the command.</returns>
        public Task<IReceivedCommand> SendAsync(ISendCommand command) {
            var tcs = new CommandWarper();
            if (command.AckCommandId.HasValue) {
                if (inflightCommands.ContainsKey(command.AckCommandId.Value)) {
                    inflightCommands[command.AckCommandId.Value] = tcs;
                }
                else {
                    inflightCommands.Add(command.AckCommandId.Value, tcs);
                }
            }
            else {
                tcs.TrySetResult(null);
            }

            try {
                client.SendAsync(command, cancellationTokenSource.Token);
            }
            catch (Exception ex) {
                tcs.Task.TrySetException(ex);
            }

            return tcs.Task.Task;
        }

        private void ProcessTimedOutCommands(object obj) {
            foreach (var command in inflightCommands.Values.ToList()
                .Where(command => (DateTime.Now - command.TimeSent)
                .TotalSeconds > CommandTimeOutDueTime)) {
                command.TrySetCanceled();
            }
        }

        /// <summary>
        /// Listen for commands of this type.
        /// </summary>
        /// <param name="commandId">The command Id to listen for.</param>
        /// <param name="observer">The action to execute when the command is received.</param>
        public void Register(int commandId, ICommandObserver observer) {
            if (commandHandlers.ContainsKey(commandId)) {
                commandHandlers[commandId].Add(observer);
            }
            else {
                commandHandlers[commandId] = new List<ICommandObserver>(new[] { observer });
            }
        }

        /// <summary>
        /// Listen for commands of this type.
        /// </summary>
        /// <param name="commandId">The command Id to listen for.</param>
        /// <param name="observer">The action to execute when the command is received.</param>
        public void Register(int commandId, Action<IReceivedCommand> observer) {
            Register(commandId, new CommandObserver(observer));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    client.Close();
                    commandTimeOut.Dispose();
                }

                disposed = true;
            }
        }

    }
}
