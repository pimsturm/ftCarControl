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

        /// <summary> 
        /// Initializes a new instance of the <see cref="TcpCmdClient"/> class.
        /// </summary>
        /// <param name="address">The servers address.</param>
        /// <param name="port">The port number.</param>
        /// <param name="logger">Logger object for logging process messages</param>
        public TcpCmdClient(string address, int port, ILogger logger)
            : this(new TcpClient(address, port), logger) {
            transportChannel = TransportChannel.Network;
            this.address = address;
            this.port = port;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpCmdClient"/> class.
        /// </summary>
        /// <param name="tcpClient">The tcp client</param>
        /// <param name="logger">Logger object for logging process messages</param>
        public TcpCmdClient(TcpClient tcpClient, ILogger logger) : base(logger) {
            client = tcpClient;
        }

        /// <summary>
        /// Connect to the device.
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public sealed override Task OpenAsync() {
            cts = new CancellationTokenSource();
            return WaitForConnection();
        }

        public sealed override bool IsOpen() {
            throw new NotImplementedException();
        }

        private Task WaitForConnection() {
            var tcp = new TaskCompletionSource<bool>();

            if (client.Connected) {
                tcp.SetResult(true);
            }
            else {

                Task.Factory.StartNew(
                    () =>
                    {
                        bool result;
                        do {
                            result = CanConnect();
                        }
                        while (!result && !cts.IsCancellationRequested);
                        tcp.SetResult(result);
                    },
                    cts.Token);
            }

            return tcp.Task;
        }

        private bool CanConnect() {
            bool result = false;
            try {
                client.Connect(address, port);
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
            if (cts != null) {
                cts.Cancel();
            }

            if (client != null) {
                client.Close();
                client = null;
            }
        }

        /// <summary>
        /// Get the stream.
        /// </summary>
        /// <returns>The stream.</returns>
        protected sealed override Stream GetStream() {
            return client.GetStream();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    if (client != null) {
                        client.Close();
                    }
                }

                disposed = true;
            }
        }

    }
}
