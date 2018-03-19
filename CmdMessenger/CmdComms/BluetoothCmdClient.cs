using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
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
        private BluetoothClient localClient; 

        private bool disposed;

        /// <summary>
        /// Gets or sets the address of the Bluetooth device.
        /// </summary>
        public BluetoothAddress BtAddress { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothCmdClient"/> class.
        /// </summary>
        /// <param name="address">The address of the bluetooth device</param>
        /// <param name="logger">Logger object</param>
        public BluetoothCmdClient (string address, ILogger logger) : base(logger) {
            transportChannel = TransportChannel.BlueTooth;
            BtAddress = BluetoothAddress.Parse(address);
            localClient = new BluetoothClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothCmdClient"/> class.
        /// </summary>
        /// <param name="logger">Logger object</param>
        public BluetoothCmdClient(ILogger logger) : base(logger) {
            transportChannel = TransportChannel.BlueTooth;
            localClient = new BluetoothClient();
        }

        /// <summary>
        /// Connect to the device.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public sealed override async Task<bool> OpenAsync() {
            var tcs = new TaskCompletionSource<bool>();
            var success = true;
            try {
                var cancellationTokenSourceTimeout = new CancellationTokenSource(30000);
                var localEndpoint = new BluetoothEndPoint(BtAddress, BluetoothService.SerialPort);
                var op = ExecuteConnectAsync(localClient, localEndpoint, null);
                success = await op.WithCancellation(cancellationTokenSourceTimeout.Token);
            }
            catch (OperationCanceledException) {
                LogMessage("Canceled connecting device: " + BtAddress.ToString());
                success = false;
            }
            catch (Exception ex) {
                LogMessage("Exception while connecting device: " + ex.Message);
                success = false;
            }
            tcs.SetResult(success);
            return success;
        }

        private Task<bool> ExecuteConnectAsync(BluetoothClient localClient, BluetoothEndPoint localEndpoint, object state) {
            // this will be our sentry that will know when our async operation is completed
            var tcs = new TaskCompletionSource<bool>();

            try {
                if (IsOpen()) {
                    Close();
                }
                LogMessage("Connecting bluetooth device: ");

                localClient.BeginConnect(localEndpoint, (iar) =>
                {
                    try {
                        localClient.EndConnect(iar);
                        tcs.TrySetResult(true);
                    }
                    catch (OperationCanceledException) {
                        // if the inner operation was canceled, this task is cancelled too
                        tcs.TrySetCanceled();
                    }
                    catch (Exception ex) {
                        // general exception has been set
                        tcs.TrySetException(ex);
                        tcs.TrySetResult(false);
                    }
                }, state);
            }
            catch {
                // propagate exceptions to the outside
                throw;
            }

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
            LogMessage("Closing Bluetooth connection.");
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
