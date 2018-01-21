using System;
using System.Diagnostics;
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

        public BluetoothCmdClient (string address) {
            transportChannel = TransportChannel.BlueTooth;
            btAddress = BluetoothAddress.Parse(address);
            localEndpoint = new BluetoothEndPoint(btAddress, BluetoothService.SerialPort);
            localClient = new BluetoothClient();
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

                localClient.Connect(localEndpoint);

                tcs.SetResult(true);
            });
            return tcs.Task;
        }

        /// <summary>
        /// The close.
        /// </summary>
        public sealed override void Close() {
            this.localClient.Close();
        }

        /// <summary>
        /// The get stream.
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        protected sealed override Stream GetStream() {
            return this.localClient.GetStream();
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    this.localClient.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}
