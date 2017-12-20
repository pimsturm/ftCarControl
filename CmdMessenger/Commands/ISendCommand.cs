using System.Collections.Generic;

namespace CmdMessenger.Commands
{

    public interface ISendCommand
    {
        #region Properties

        /// <summary>
        /// Gets the command arguments.
        /// </summary>
        IEnumerable<string> Arguments { get; }

        /// <summary>
        /// Gets the command id
        /// </summary>
        int CommandId { get; }

        /// <summary>
        /// Gets the Ack command Id
        /// </summary>
        int? AckCommandId { get; }

        /// <summary>
        /// Gets the timeout period.
        /// </summary>
        int Timeout { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Get the command.
        /// </summary>
        /// <returns>A raw string representation of the command.</returns>
        string GetCommand();

        /// <summary>
        /// Add a command argument.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="argument">The argument to add.</param>
        void AddArguments<T>(T argument);

        /// <summary>
        /// Add arguments.
        /// </summary>
        /// <typeparam name="T">The argument type.</typeparam>
        /// <param name="arguments">The arguments to add.</param>
        void AddArguments<T>(params T[] arguments);

        /// <summary>
        /// Add <code>bool</code> argument.
        /// </summary>
        /// <param name="argument">The boolean argument to add.</param>
        void AddArguments(bool argument);

        /// <summary>
        /// Add arguments.
        /// </summary>
        /// <param name="arguments">The boolean arguments to add.</param>
        void AddArguments(params bool[] arguments);

        #endregion
    }
}
