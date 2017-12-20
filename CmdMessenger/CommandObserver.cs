using System;
using CmdMessenger.Commands;

namespace CmdMessenger
{
    /// <summary>
    /// The command observer.
    /// </summary>
    public class CommandObserver : ICommandObserver
    {
        #region Fields

        /// <summary>
        /// The action.
        /// </summary>
        private readonly Action<IReceivedCommand> action;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandObserver"/> class.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public CommandObserver(Action<IReceivedCommand> action) {
            this.action = action;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        public void Update(IReceivedCommand command) {
            this.action(command);
        }

        #endregion
    }
}
