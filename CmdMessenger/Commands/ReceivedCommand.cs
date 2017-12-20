using System;
using System.Collections.Generic;
using System.Linq;

namespace CmdMessenger.Commands
{
    /// <summary>
    /// A command received from CmdMessenger
    /// </summary>
    public class ReceivedCommand : IReceivedCommand
    {
        #region Fields

        private List<string> arguments;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivedCommand"/> class.
        /// </summary>
        /// <param name="commandId">
        /// The commands ID
        /// </param>
        /// <param name="arguments">
        /// The commands arguments.
        /// </param>
        public ReceivedCommand(int commandId, string[] arguments) {
            this.CommandId = commandId;
            this.arguments = new List<string>(arguments);
            this.Enumerator = ((IEnumerable<string>)arguments).GetEnumerator();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command ID.
        /// </summary>
        public int CommandId { get; private set; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        protected IEnumerator<string> Enumerator { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Create a received command from a list of parameters.
        /// </summary>
        /// <param name="parameters">The commands parameters.</param>
        /// <returns>A <see cref="ReceivedCommand"/> instance with the defined parameters.</returns>
        public static IReceivedCommand Create(string[] parameters) {
            if (parameters.Length < 1) {
                throw new ArgumentException("Need at least one parameter.");
            }

            int commandId;

            if (!int.TryParse(parameters[0], out commandId)) {
                throw new ArgumentException("First parameter should be an integer", "parameters");
            }

            return new ReceivedCommand(commandId, parameters.Skip(1).ToArray());
        }

        /// <summary>
        /// Read <code>Int16</code>
        /// </summary>
        /// <returns>The 16 bit integer.</returns>
        /// <exception cref="ArgumentException">The argument is not a 16 bit integer.</exception>
        public short ReadInt16() {
            this.MoveNext();

            short value;
            if (!short.TryParse(this.Enumerator.Current, out value)) {
                throw new ArgumentException("Argument was not type Int16");
            }

            return value;
        }

        /// <summary>
        /// Read boolean.
        /// </summary>
        /// <returns>The boolean read.</returns>
        public bool ReadBool() {
            return this.ReadInt16() != 0;
        }

        /// <summary>
        /// Read <code>Int32</code>
        /// </summary>
        /// <returns>The 32 bit integer.</returns>
        /// <exception cref="ArgumentException">The argument is not a 32 bit integer.</exception>
        public int ReadInt32() {
            this.MoveNext();

            int value;
            if (!int.TryParse(this.Enumerator.Current, out value)) {
                throw new ArgumentException("The argument was not of type Int32");
            }

            return value;
        }

        /// <summary>
        /// Read <code>UInt32</code>
        /// </summary>
        /// <returns>The 32 bit unsigned integer.</returns>
        public uint ReadUInt32() {
            this.MoveNext();

            uint value;
            if (!uint.TryParse(this.Enumerator.Current, out value)) {
                throw new ArgumentException("The argument was not of type UInt32");
            }

            return value;
        }

        /// <summary>
        /// Read the current argument as string.
        /// </summary>
        /// <returns>The string argument.</returns>
        public string ReadString() {
            this.MoveNext();
            return this.Enumerator.Current;
        }

        /// <summary>
        /// Move to the next argument
        /// </summary>
        /// <exception cref="ArgumentException">Read too many arguments.</exception>
        private void MoveNext() {
            if (this.Enumerator.MoveNext()) {
                return;
            }

            throw new ArgumentException("Read too many arguments");
        }

        #endregion
    }
}
