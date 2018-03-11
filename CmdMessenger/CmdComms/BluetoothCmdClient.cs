using System;
using System.IO;
using System.Threading.Tasks;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace CmdMessenger.CmdComms
{
    /// <summary>
    /// A Bluetooth interface type.
    /// </summary>
    public class BluetoothCmdClient : CmdComms, IDisposable
    {
        private BluetoothEndPoint localEndpoint;
        private BluetoothClient localClient; 

        private bool disposed;
        private BluetoothAddress btAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothCmdClient"/> class.
        /// </summary>
        /// <param name="address">The address of the bluetooth device</param>
        /// <param name="logger">Logger object</param>
        public BluetoothCmdClient (string address, ILogger logger) : base(logger) {
            transportChannel = TransportChannel.BlueTooth;
            btAddress = BluetoothAddress.Parse(address);
            localEndpoint = new BluetoothEndPoint(btAddress, BluetoothService.SerialPort);
            localClient = new BluetoothClient();
        }

        /// <summary>
        /// Connect to the device.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public sealed override Task<bool> OpenAsync() {
            var tcs = new TaskCompletionSource<bool>();
            Task.Factory.StartNew(() => {

                localClient.Connect(localEndpoint);

                tcs.SetResult(true);
            });
            return tcs.Task;
        }

        /// <summary>
        /// Checks if the communication port is open
        /// </summary>
        /// <returns>True if the port is open</returns>
        public sealed override bool IsOpen() {
            return localClient.Connected;
        }

        /// <summary>
        /// The close.
        /// </summary>
        public sealed override void Close() {
            localClient.Close();
        }

        /// <summary>
        /// The get stream.
        /// </summary>
        /// <returns>The <see cref="Stream"/>.</returns>
        protected sealed override Stream GetStream() {
            return localClient.GetStream();
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose() {
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
                    localClient.Dispose();
                }

                disposed = true;
            }
        }
    }
}
