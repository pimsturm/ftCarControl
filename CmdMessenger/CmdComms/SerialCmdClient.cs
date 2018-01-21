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
        #region Fields 

        /// <summary>
        /// The serial port instance.
        /// </summary>
        private readonly SerialPort serial;
        private bool disposed;

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialCmdClient"/> class.
        /// </summary>
        /// <param name="port">The port name.</param>
        /// <param name="baud">The baud rate.</param>
        public SerialCmdClient(string port, int baud) {
            transportChannel = TransportChannel.SerialPort;
            this.serial = new SerialPort(port, baud);
        }

        #endregion 

        #region Methods 

        /// <summary>
        /// Connect to the device.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public sealed override Task OpenAsync() {
            var tcs = new TaskCompletionSource<bool>();
            Task.Factory.StartNew(() => {
                this.serial.Open();
                tcs.SetResult(true);
            });
            return tcs.Task;
        }

        /// <summary>
        /// The close.
        /// </summary>
        public sealed override void Close() {
            this.serial.Close();
        }

        /// <summary>
        /// The get stream.
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        protected sealed override Stream GetStream() {
            return this.serial.BaseStream;
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    this.serial.Dispose();
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}
