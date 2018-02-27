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
        /// Checks if the communication port is open
        /// </summary>
        /// <returns>True if the port is open</returns>
        public sealed override bool IsOpen() {
            return this.serial.IsOpen;
        }

        /// <summary>
        /// Find the COM port to which the Arduino is connected
        /// </summary>
        /// <returns>The COM port</returns>
        public SerialPort FindComPort() {
            SerialPort serialPort;

            try {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports) {
                    serialPort = new SerialPort(port, 9600);
                    if (DetectArduino(serialPort)) {
                        return serialPort;
                    }
                    else {
                        serialPort = null;
                    }
                }
            }
            catch (Exception e) {
            }
            return null;
        }

        private bool DetectArduino(SerialPort port) {
            bool found = false;

            return found;
        }

        /// <summary>
        /// The close.
        /// </summary>
        public sealed override void Close() {
            if (Logger != null)
                Logger.LogMessage("Close serial port");
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
            if (Logger != null)
                Logger.LogMessage("Dispose serial client");
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    if (Logger != null)
                        Logger.LogMessage("Dispose serial port");
                    this.serial.Dispose();
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}
