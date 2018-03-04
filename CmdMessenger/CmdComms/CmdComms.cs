using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CmdMessenger.Commands;

namespace CmdMessenger.CmdComms
{
    /// <summary>
    /// A base class for interface types.
    /// </summary>
    public abstract class CmdComms : ICmdComms
    {

        private readonly ASCIIEncoding encoder = new ASCIIEncoding();

        private readonly IEscaping escaping;

        private ILogger logger;

        internal TransportChannel transportChannel;

        /// <summary>
        /// Gets the transport channel
        /// </summary>
        TransportChannel ICmdComms.TransportChannel {
            get {
                return transportChannel;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdComms"/> class. 
        /// </summary>
        protected CmdComms(ILogger logger)
            : this(logger, Escaping.Default) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdComms"/> class.
        /// </summary>
        /// <param name="escaping">
        /// The escaping instance.
        /// </param>
        protected CmdComms(ILogger logger, IEscaping escaping) {
            this.escaping = escaping;
            this.logger = logger ?? throw new ArgumentNullException("logger");
        }

        /// <summary>
        /// Connect to the device.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract Task OpenAsync();

        /// <summary>
        /// Close the communication port.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Checks if the communication port is open
        /// </summary>
        /// <returns>True if the port is open</returns>
        public abstract bool IsOpen();

        /// <summary>
        /// Writes a parameter to the serial port.
        /// </summary>
        /// <param name="command">The command to send.</param>
        public void Send(ISendCommand command) {
            SendAsync(command).Wait();
        }

        /// <summary>
        /// Writes async to the serial port.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <returns>The completed task.</returns>
        public Task SendAsync(ISendCommand command) {
            byte[] bytes = encoder.GetBytes(command.GetCommand());
            var tcs = new TaskCompletionSource<bool>();
            Task.Factory.StartNew(() => {
                try {
                    Stream stream = GetStream();
                    stream.WriteAsync(bytes, 0, bytes.Length);
                }
                catch (Exception ex) {
                    LogMessage(ex.Message);
                    tcs.TrySetException(ex);
                }
                tcs.SetResult(true);
            });

            return tcs.Task;
        }

        /// <summary>
        /// Reads from the serial port
        /// </summary>
        /// <param name="token">Token for monitoring the cancellation status</param>
        /// <returns>The result of the completed read.</returns>
        public IReceivedCommand Read(CancellationToken token) {
            return ReadAsync(token).Result;
        }

        /// <summary>
        /// Reads async from the serial port
        /// </summary>
        /// <param name="token">Token for monitoring the cancellation status</param>
        /// <returns>The result of the async task.</returns>
        public Task<IReceivedCommand> ReadAsync(CancellationToken token) {
            var tcs = new TaskCompletionSource<IReceivedCommand>();
            ReadAsync(tcs, token);
            return tcs.Task;
        }

        internal void LogMessage(string message) {
            logger.LogMessage(message);
        }

        private async void ReadAsync(TaskCompletionSource<IReceivedCommand> tcs, CancellationToken token) {
            LogMessage("ReadAsync");
            if (!token.IsCancellationRequested) {
                try {
                    int packetSize = 5000;
                    Stream stream = GetStream();
                    var buffer = new byte[packetSize];
                    var ms = new MemoryStream();
                    byte[] result = null;

                    using (ms) {
                        int count = 1;
                        int bytesReceived = 0;
                        bool continueReading = true;

                        // Read Data
                        while (continueReading) {
                            continueReading = (count = await stream.ReadAsync(buffer, 0, packetSize - bytesReceived > buffer.Length ? buffer.Length : packetSize - bytesReceived)) > 0;
                            if (continueReading) {
                                // Save Data
                                ms.Write(buffer, 0, count);

                                // Count
                                bytesReceived += count;

                                // Check if a full command is read
                                continueReading = !IsCommandComplete(ms, bytesReceived, out result);
                            }
                        }

                        if (result != null) {
                            LogMessage("Result: " + Encoding.Default.GetString(result));
                            tcs.SetResult(ReceivedCommand.Create(escaping.GetUnescapedParameters(result)));
                        }
                    }

                }
                catch (ObjectDisposedException e) {
                    LogMessage("ReadAsync: ObjectDisposedException " + e.Message);
                    tcs.TrySetCanceled();
                }
                catch (Exception) {
                    LogMessage("ReadAsync: Exception");
                    tcs.TrySetCanceled();
                }
            }
        }

        private bool IsCommandComplete(MemoryStream ms, int bytesReceived, out byte[] result) {
            bool isComplete = false;

            var position = ms.Position;
            ms.Position -= bytesReceived;
            result = escaping.GetCommand(ms).FirstOrDefault();
            isComplete = result != null;
            if (!isComplete) {
                // Restore buffer position
                ms.Position = position;
            }

            return isComplete;
        }

        protected abstract Stream GetStream();

    }
}
