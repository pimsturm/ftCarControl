using CmdMessenger.Commands;

namespace CmdMessenger
{
    /// <summary>
    /// An interface for command observers.
    /// </summary>
    public interface ICommandObserver
    {
        #region Public Methods and Operators

        /// <summary>
        /// Updates the observer with command.
        /// </summary>
        /// <param name="command">
        /// The command received.
        /// </param>
        void Update(IReceivedCommand command);

        #endregion
    }
}
