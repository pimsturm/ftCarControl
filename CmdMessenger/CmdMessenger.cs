using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CmdMessenger.Commands;
using CmdMessenger.CmdComms;

namespace CmdMessenger
{
    public class CmdMessenger : IDisposable
    {
        private class CommandWarper
        {
            #region Fields
            private TaskCompletionSource<IReceivedCommand> task;
            private DateTime timeSent;
            #endregion

            #region Constructors

            public CommandWarper() {
                task = new TaskCompletionSource<IReceivedCommand>();
                timeSent = DateTime.Now;
            }

            #endregion

            #region Properties

            public DateTime TimeSent {
                get { return this.timeSent; }
            }

            public TaskCompletionSource<IReceivedCommand> Task {
                get { return this.task; }
            }

            #endregion

            public void TrySetCanceled() {
                task.TrySetCanceled();

            }

            public void TrySetResult(IReceivedCommand command) {
                task.TrySetResult(command);
            }
        }

        #region Fields

        private const int CommandTimeOutDueTime = 1000;
        private readonly ICmdComms client;
        private readonly Dictionary<int, List<ICommandObserver>> commandHandlers;
        private readonly Timer commandTimeOut;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly Dictionary<int, CommandWarper> inflightCommands = new Dictionary<int, CommandWarper>();
        private bool disposed;

        #endregion

        #region Constructor

        /// <summary> 
        /// Initializes a new instance of the <see cref="CmdMessenger"/> class. 
        /// </summary>
        /// <param name="client">
        /// The command client.
        /// </param>
        public CmdMessenger(ICmdComms client)
            : this(client, Escaping.Default) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmdMessenger"/> class. 
        /// </summary>
        /// <param name="client">
        /// The command client.
        /// </param>
        /// <param name="escaping">
        /// The escaping instance.
        /// </param>
        public CmdMessenger(ICmdComms client, IEscaping escaping) {
            if (client == null) {
                throw new ArgumentNullException("client");
            }

            this.commandHandlers = new Dictionary<int, List<ICommandObserver>>();
            this.commandTimeOut = new Timer(this.ProcessTimedOutCommands);
            this.cancellationTokenSource = new CancellationTokenSource();
            this.client = client;
        }

        #endregion

        #region Properties

        public bool PrintLineFeedCarrige { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Open the connection.
        /// </summary>
        public async void Start() {
            this.commandTimeOut.Change(500, 500);
            await this.client.OpenAsync();
            this.ProcessCommands();
        }

        private async void ProcessCommands() {
            while (!this.cancellationTokenSource.IsCancellationRequested) {
                try {
                    IReceivedCommand command = await this.client.ReadAsync(this.cancellationTokenSource.Token);
                    Debug.WriteLine("Received: "+ command.CommandId.ToString());
                    if (this.inflightCommands.ContainsKey(command.CommandId)) {
                        this.inflightCommands[command.CommandId].TrySetResult(command);
                    }

                    if (this.commandHandlers.ContainsKey(command.CommandId)) {
                        var handler = this.commandHandlers[command.CommandId];
                        handler.ForEach(c => c.Update(command));
                    }
                }
                catch (TaskCanceledException) {
                }
            }
        }

        /// <summary>
        /// Close the connection.
        /// </summary>
        public void Stop() {
            this.cancellationTokenSource.Cancel();
            this.client.Close();
        }

        /// <summary>
        /// Send a command synchronously 
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <returns>The commands response.</returns>
        public IReceivedCommand Send(ISendCommand command) {
            Task<IReceivedCommand> t = this.SendAsync(command);
            try {
                return t.Result;
            }
            catch (AggregateException ex) {
                throw ex.InnerException;
            }
        }

        /// <summary>
        /// Send a command asynchronously.
        /// </summary>
        /// <param name="commad">The command to send.</param>
        /// <returns>A task completion source for the command.</returns>
        public Task<IReceivedCommand> SendAsync(ISendCommand commad) {
            var tcs = new CommandWarper();
            if (commad.AckCommandId.HasValue) {
                if (this.inflightCommands.ContainsKey(commad.AckCommandId.Value)) {
                    this.inflightCommands[commad.AckCommandId.Value] = tcs;
                }
                else {
                    this.inflightCommands.Add(commad.AckCommandId.Value, tcs);
                }
            }
            else {
                tcs.TrySetResult(null);
            }

            try {
                this.client.Send(commad);
            }
            catch (Exception ex) {
                tcs.Task.TrySetException(ex);
            }

            return tcs.Task.Task;
        }

        private void ProcessTimedOutCommands(object obj) {
            foreach (var command in this.inflightCommands.Values.ToList()
                .Where(command => (DateTime.Now - command.TimeSent)
                .TotalSeconds > CommandTimeOutDueTime)) {
                command.TrySetCanceled();
            }
        }

        /// <summary>
        /// Listen for commands of this type.
        /// </summary>
        /// <param name="commandId">The command Id to listen for.</param>
        /// <param name="observer">The action to execute when the command is received.</param>
        public void Register(int commandId, ICommandObserver observer) {
            if (this.commandHandlers.ContainsKey(commandId)) {
                this.commandHandlers[commandId].Add(observer);
            }
            else {
                this.commandHandlers[commandId] = new List<ICommandObserver>(new[] { observer });
            }
        }

        /// <summary>
        /// Listen for commands of this type.
        /// </summary>
        /// <param name="commandId">The command Id to listen for.</param>
        /// <param name="observer">The action to execute when the command is received.</param>
        public void Register(int commandId, Action<IReceivedCommand> observer) {
            this.Register(commandId, new CommandObserver(observer));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    this.client.Close();
                    this.commandTimeOut.Dispose();
                }

                this.disposed = true;
            }
        }

        #endregion
    }
}
