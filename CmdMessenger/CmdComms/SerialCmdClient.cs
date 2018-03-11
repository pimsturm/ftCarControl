using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Threading;

namespace CmdMessenger.CmdComms
{
    /// <summary>
    /// A serial interface type.
    /// </summary>
    public class SerialCmdClient : CmdComms, IDisposable
    {
        /// <summary>
        /// The serial port instance.
        /// </summary>
        private readonly SerialPort serial;
        private bool disposed;

        /// <summary>
        /// Gets or sets the name of the serial port
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// Gets or sets the baudrate
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialCmdClient"/> class.
        /// </summary>
        /// <param name="port">The port name.</param>
        /// <param name="baud">The baud rate.</param>
        /// <param name="logger"> Logger object</param>
        public SerialCmdClient(string port, int baud, ILogger logger) : base(logger) {
            if (string.IsNullOrEmpty(port))
                throw new ArgumentNullException("port");

            transportChannel = TransportChannel.SerialPort;
            PortName = port;
            BaudRate = baud;
            serial = new SerialPort(port, baud);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialCmdClient"/> class.
        /// </summary>
        /// <param name="logger"> Logger object</param>
        public SerialCmdClient(ILogger logger) : base(logger) {

            transportChannel = TransportChannel.SerialPort;
            serial = new SerialPort();
        }

        /// <summary>
        /// Opens the serial port.
        /// </summary>
        /// <returns>The task return true of the port is successfully opened.</returns>
        public sealed override async Task<bool> OpenAsync() {
            var tcs = new TaskCompletionSource<bool>();
            var success = true;
            try {
                var cancellationTokenSourceTimeout = new CancellationTokenSource(1000);
                var op = OpenPort();
                success = await op.WithCancellation(cancellationTokenSourceTimeout.Token);
            }
            catch (OperationCanceledException) {
                LogMessage("Canceled opening port: " + PortName);
                success = false;
            }
            tcs.SetResult(success);
            return success;
        }

        private Task<bool> OpenPort() {
            var tcs = new TaskCompletionSource<bool>();
            var success = true;
            try {
                if (serial.IsOpen) {
                    LogMessage("Closing serial port.");
                    serial.DiscardInBuffer();
                    serial.DiscardOutBuffer();
                    serial.Close();
                }
                LogMessage("Opening port: " + PortName);
                serial.PortName = PortName;
                serial.BaudRate = BaudRate;
                serial.Open();
            }
            catch (UnauthorizedAccessException e) {
                LogMessage("Unauthorized: " + e.Message);
                success = false;
            }
            catch (IOException e) {
                LogMessage("IOException: " + e.Message);
                success = false;
            }
            tcs.SetResult(success);
            return tcs.Task;
        }

        /// <summary>
        /// Checks if the communication port is open
        /// </summary>
        /// <returns>True if the port is open</returns>
        public sealed override bool IsOpen() {
            return serial.IsOpen;
        }

        /// <summary>
        /// The close.
        /// </summary>
        public sealed override void Close() {
            LogMessage("Close serial port");
            serial.Close();
        }

        /// <summary>
        /// The get stream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        protected sealed override Stream GetStream() {
            return serial.BaseStream;
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose() {
            LogMessage("Dispose serial client");
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">true when the called from dispose, false when called from a finalizer.</param>
        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    LogMessage("Dispose serial port");
                    serial.Dispose();
                }

                disposed = true;
            }
        }

    }
}
