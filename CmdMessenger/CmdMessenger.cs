using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
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
                get { return this.timeSent; }
            }

            public TaskCompletionSource<IReceivedCommand> Task {
                get { return this.task; }
            }

            #endregion

            public void TrySetCanceled() {
                task.TrySetCanceled();

            }

            public void TrySetResult(IReceivedCommand command) {
                task.TrySetResult(command);
            }
        }

        #region Fields

        private const int CommandTimeOutDueTime = 1000;
        private readonly ICmdComms client;
        private readonly Dictionary<int, List<ICommandObserver>> commandHandlers;
        private readonly Timer commandTimeOut;
        private CancellationTokenSource cancellationTokenSource;
        private readonly Dictionary<int, CommandWarper> inflightCommands = new Dictionary<int, CommandWarper>();
        private bool disposed;
        private ILogger logger;

        #endregion

        #region Constructor

        /// <summary> 
        /// Initializes a new instance of the <see cref="CmdMessenger"/> class. 
        /// </summary>
        /// <param name="client">The command client.</param>
        /// <param name="escaping">The escaping instance.</param>
        public CmdMessenger(ICmdComms client, ILogger logger)
            : this(client, logger, Escaping.Default) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdMessenger"/> class. 
        /// </summary>
        /// <param name="client">The command client.</param>
        /// <param name="logger">Logger object for logging process messages</param>
        public CmdMessenger(ICmdComms client, ILogger logger, IEscaping escaping) {
            if (client == null) {
                throw new ArgumentNullException("client");
            }
            if (logger == null) {
                throw new ArgumentNullException("logger");
            }

            this.commandHandlers = new Dictionary<int, List<ICommandObserver>>();
            this.commandTimeOut = new Timer(this.ProcessTimedOutCommands);
            this.cancellationTokenSource = new CancellationTokenSource();
            this.client = client;
            this.logger = logger;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets whether to print a carriage return linefeed
        /// </summary>
        public bool PrintCarriageReturnLineFeed { get; set; }

        /// <summary>
        /// Gets the transport channel
        /// </summary>
        public TransportChannel TransportChannel {
            get {
                if (this.client == null)
                    return TransportChannel.Undefined;
                return this.client.TransportChannel;
            }
        }

        /// <summary>
        /// Gets or sets the command to check if the Arduino is alive
        /// </summary>
        public ISendCommand PingCommand { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Open the connection.
        /// </summary>
        public async void Start() {
            this.commandTimeOut.Change(500, 500);
            try {
                await client.OpenAsync();
            }
            catch (UnauthorizedAccessException e) {
                logger.LogMessage("Unauthorized:" + e.Message);
            }
            logger.LogMessage("Connection opened");
            ProcessCommands();
        }

        private async void ProcessCommands() {
            logger.LogMessage("Processing commands");
            if (this.cancellationTokenSource.IsCancellationRequested)
                logger.LogMessage("Cancellation");
            while (!this.cancellationTokenSource.IsCancellationRequested) {
                try {
                    IReceivedCommand command = await this.client.ReadAsync(this.cancellationTokenSource.Token);
                    Debug.WriteLine("Received: "+ command.CommandId.ToString());
                    logger.LogMessage("Received (ProcessCommands): " + command.CommandId.ToString());
                    if (this.inflightCommands.ContainsKey(command.CommandId)) {
                        logger.LogMessage("Inflight command");
                        this.inflightCommands[command.CommandId].TrySetResult(command);
                    }

                    if (this.commandHandlers.ContainsKey(command.CommandId)) {
                        logger.LogMessage("Command handler");
                        var handler = this.commandHandlers[command.CommandId];
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
            this.cancellationTokenSource.Cancel();
            this.client.Close();
        }

        public void Cancel() {
            cancellationTokenSource.Cancel();
        }

        public void Resume() {
            cancellationTokenSource = new CancellationTokenSource();
            ProcessCommands();
        }

        /// <summary>
        /// Send a command synchronously 
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <returns>The commands response.</returns>
        public IReceivedCommand Send(ISendCommand command) {
            logger.LogMessage("Send: " + command.CommandId.ToString());
            Task<IReceivedCommand> t = this.SendAsync(command);
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
                if (this.inflightCommands.ContainsKey(command.AckCommandId.Value)) {
                    this.inflightCommands[command.AckCommandId.Value] = tcs;
                }
                else {
                    this.inflightCommands.Add(command.AckCommandId.Value, tcs);
                }
            }
            else {
                tcs.TrySetResult(null);
            }

            try {
                this.client.Send(command);
            }
            catch (Exception ex) {
                tcs.Task.TrySetException(ex);
            }

            return tcs.Task.Task;
        }

        private void ProcessTimedOutCommands(object obj) {
            foreach (var command in this.inflightCommands.Values.ToList()
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
            if (this.commandHandlers.ContainsKey(commandId)) {
                this.commandHandlers[commandId].Add(observer);
            }
            else {
                this.commandHandlers[commandId] = new List<ICommandObserver>(new[] { observer });
            }
        }

        /// <summary>
        /// Listen for commands of this type.
        /// </summary>
        /// <param name="commandId">The command Id to listen for.</param>
        /// <param name="observer">The action to execute when the command is received.</param>
        public void Register(int commandId, Action<IReceivedCommand> observer) {
            this.Register(commandId, new CommandObserver(observer));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    this.client.Close();
                    this.commandTimeOut.Dispose();
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}
