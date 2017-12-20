using System.Collections.Generic;
using System.Text;

namespace CmdMessenger.Commands
{
    public class SendCommand : ISendCommand
    {
        #region Fields

        private readonly List<string> arguments = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SendCommand"/> class. 
        /// </summary>
        /// <param name="commandId">
        /// The command Id.
        /// </param>
        public SendCommand(int commandId) {
            this.CommandId = commandId;
            this.Timeout = 1000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendCommand"/> class. 
        /// </summary>
        /// <param name="commandId">
        /// The command Id.
        /// </param>
        /// <param name="ackCommandId">
        /// The ack Command Id.
        /// </param>
        public SendCommand(int commandId, int ackCommandId) {
            this.CommandId = commandId;
            this.AckCommandId = ackCommandId;
            this.Timeout = 1000;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command arguments.
        /// </summary>
        public IEnumerable<string> Arguments {
            get { return this.arguments; }
        }

        /// <summary>
        /// Gets the command id
        /// </summary>
        public int CommandId { get; private set; }

        /// <summary>
        /// Gets the Ack command Id
        /// </summary>
        public int? AckCommandId { get; private set; }

        public int Timeout { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get the command.
        /// </summary>
        /// <returns>The command as a raw string.</returns>
        public string GetCommand() {
            var currentLine = new StringBuilder();

            currentLine.Append(this.CommandId);

            foreach (var arg in this.Arguments) {
                currentLine.Append(",");
                currentLine.Append(arg);
            }

            currentLine.Append(';');

            return currentLine.ToString();
        }

        /// <summary>
        /// Add a command argument.
        /// </summary>
        /// <typeparam name="T">The arguments type.</typeparam>
        /// <param name="argument">The argument to add.</param>
        public void AddArguments<T>(T argument) {
            this.arguments.Add(argument.ToString());
        }

        /// <summary>
        /// Add arguments.
        /// </summary>
        /// <typeparam name="T">The arguments type.</typeparam>
        /// <param name="arguments">The arguments to add.</param>
        public void AddArguments<T>(params T[] arguments) {
            foreach (var arg in arguments) {
                this.arguments.Add(arg.ToString());
            }
        }

        /// <summary>
        /// Add <code>bool</code> argument.
        /// </summary>
        /// <param name="argument">The boolean argument to add.</param>
        public void AddArguments(bool argument) {
            this.AddArguments(argument ? 1 : 0);
        }

        /// <summary>
        /// Add arguments.
        /// </summary>
        /// <param name="arguments">The boolean arguments to add.</param>
        public void AddArguments(params bool[] arguments) {
            foreach (var arg in arguments) {
                this.AddArguments(arg ? 1 : 0);
            }
        }

        #endregion
    }
}
