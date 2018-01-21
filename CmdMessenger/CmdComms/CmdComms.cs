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
        #region Fields

        /// <summary>
        /// The encoder.
        /// </summary>
        private readonly ASCIIEncoding encoder = new ASCIIEncoding();

        /// <summary>
        /// The escaping.
        /// </summary>
        private readonly IEscaping escaping;

        internal TransportChannel transportChannel;

        //private Queue<byte> _buffer = new Queue<byte>();

        private MemoryStream buffer = new MemoryStream();

        #endregion

        #region Properties
        TransportChannel ICmdComms.TransportChannel {
            get {
                return transportChannel;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdComms"/> class. 
        /// </summary>
        protected CmdComms()
            : this(Escaping.Default) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdComms"/> class.
        /// </summary>
        /// <param name="escaping">
        /// The escaping instance.
        /// </param>
        protected CmdComms(IEscaping escaping) {
            this.escaping = escaping;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Connect to the device.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public abstract Task OpenAsync();

        /// <summary>
        /// The close.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Writes a parameter to the serial port.
        /// </summary>
        /// <param name="command">
        /// The command to send.
        /// </param>
        public void Send(ISendCommand command) {
            this.SendAsync(command).Wait();
        }

        public Task SendAsync(ISendCommand command) {
            byte[] bytes = this.encoder.GetBytes(command.GetCommand());
            var tcs = new TaskCompletionSource<bool>();
            try {
                Stream stream = this.GetStream();
                stream.WriteAsync(bytes, 0, bytes.Length).Wait();
                tcs.SetResult(true);
            }
            catch (Exception ex) {
                //if (ex.SocketErrorCode == SocketError.ConnectionAborted)
                //{
                //    tcs.TrySetCanceled();
                //}
                tcs.TrySetException(ex);
            }
            return tcs.Task;
        }

        public IReceivedCommand Read(CancellationToken token) {
            return this.ReadAsync(token).Result;
        }

        public Task<IReceivedCommand> ReadAsync(CancellationToken token) {
            var tcs = new TaskCompletionSource<IReceivedCommand>();
            this.ReadAsync(tcs, token);
            return tcs.Task;
        }

        private async void ReadAsync(TaskCompletionSource<IReceivedCommand> tcs, CancellationToken token) {
            if (!token.IsCancellationRequested) {
                try {

                    Stream stream = this.GetStream();
                    var buffer = new byte[5000];

                    int task = await stream.ReadAsync(buffer, 0, 5000, token);

                    //var result = this.escaping.GetCommand(buffer).FirstOrDefault();

                    //if (result != null)
                    //{
                    //    tcs.SetResult(ReceivedCommand.Create(this.escaping.GetUnescapedParameters(result)));
                    //}

                    if (task > 0) {
                        //for (int b = 0; b < task; b++)
                        //{
                        //    _buffer.Enqueue(buffer[b]);
                        //}

                        this.buffer.Write(buffer, 0, task);

                        this.buffer.Position -= task;


                        var result = this.escaping.GetCommand(this.buffer).FirstOrDefault();

                        if (result != null) {
                            tcs.SetResult(ReceivedCommand.Create(this.escaping.GetUnescapedParameters(result)));
                        }
                    }
                }
                catch (ObjectDisposedException) {
                    tcs.TrySetCanceled();
                }
                catch (Exception) {
                    tcs.TrySetCanceled();
                }
            }
        }

        protected abstract Stream GetStream();

        #endregion
    }
}
