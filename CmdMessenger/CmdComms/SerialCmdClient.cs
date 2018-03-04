using System;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;

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
        /// Initializes a new instance of the <see cref="SerialCmdClient"/> class.
        /// </summary>
        /// <param name="port">The port name.</param>
        /// <param name="baud">The baud rate.</param>
        /// <param name="logger"> Logger object</param>
        public SerialCmdClient(string port, int baud, ILogger logger) : base(logger) {
            if (string.IsNullOrEmpty(port))
                throw new ArgumentNullException("port");

            transportChannel = TransportChannel.SerialPort;
            serial = new SerialPort(port, baud);
        }

        /// <summary>
        /// Connect to the device.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public sealed override Task OpenAsync() {
            var tcs = new TaskCompletionSource<bool>();
            Task.Factory.StartNew(() => {
                try {
                    if (!IsOpen())
                        serial.Open();
                }
                catch (UnauthorizedAccessException e) {
                    LogMessage("Unauthorized: " + e.Message);
                }
                tcs.SetResult(true);
            });
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
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        protected sealed override Stream GetStream() {
            return serial.BaseStream;
        }

        public void Dispose() {
            LogMessage("Dispose serial client");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

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
