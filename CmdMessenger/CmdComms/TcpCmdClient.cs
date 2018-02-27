using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CmdMessenger.CmdComms
{
    /// <summary>
    /// A TCP client interface type.
    /// </summary>
    public class TcpCmdClient : CmdComms, IDisposable
    {
        #region Fields

        /// <summary>
        /// The servers address.
        /// </summary>
        private readonly string address;

        /// <summary>
        /// The port number.
        /// </summary>
        private readonly int port;

        /// <summary>
        /// The <see cref="TcpClient"/> instance.
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private CancellationTokenSource cts;

        private bool disposed;

        #endregion

        #region Constructor

        /// <summary> 
        /// Initializes a new instance of the <see cref="TcpCmdClient"/> class.
        /// </summary>
        /// <param name="address">
        /// The servers address.
        /// </param>
        /// <param name="port">
        /// The port number.
        /// </param>
        public TcpCmdClient(string address, int port)
            : this(new TcpClient(address, port)) {
            transportChannel = TransportChannel.Network;
            this.address = address;
            this.port = port;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpCmdClient"/> class.
        /// </summary>
        /// <param name="tcpClient"></param>
        public TcpCmdClient(TcpClient tcpClient) {
            this.client = tcpClient;
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
            this.cts = new CancellationTokenSource();
            return this.WaitForConnection();
        }

        public sealed override bool IsOpen() {
            throw new NotImplementedException();
        }

        private Task WaitForConnection() {
            var tcp = new TaskCompletionSource<bool>();

            if (this.client.Connected) {
                tcp.SetResult(true);
            }
            else {

                Task.Factory.StartNew(
                    () =>
                    {
                        bool result;
                        do {
                            result = this.CanConnect();
                        }
                        while (!result && !cts.IsCancellationRequested);
                        tcp.SetResult(result);
                    },
                    this.cts.Token);
            }

            return tcp.Task;
        }

        private bool CanConnect() {
            bool result = false;
            try {
                this.client.Connect(this.address, this.port);
                result = true;
            }
            catch (ObjectDisposedException ex) {
                Trace.TraceInformation("Invalid operation: {0}", ex.Message);
            }
            catch (SocketException se) {
                Trace.TraceInformation("Socket exception: {0}", se.Message);
            }

            return result;
        }

        /// <summary>
        /// The close.
        /// </summary>
        public sealed override void Close() {
            Trace.TraceInformation("Close");
            if (this.cts != null) {
                this.cts.Cancel();
            }

            if (client != null) {
                this.client.Close();
                this.client = null;
            }
        }

        /// <summary>
        /// Get the stream.
        /// </summary>
        /// <returns>The stream.</returns>
        protected sealed override Stream GetStream() {
            return this.client.GetStream();
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    if (this.client != null) {
                        this.client.Close();
                    }
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}
