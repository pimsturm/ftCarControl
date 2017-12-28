using System;
using CmdMessenger.Commands;

namespace ArduinoCommunicator.Commands
{
    /// <summary>
    /// Represents a Fault message.
    /// </summary>
    public class Fault : SendCommand, IEquatable<Fault>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Fault"/> class.
        /// </summary>
        /// <param name="commandId">
        /// The command id.
        /// </param>
        public Fault(int commandId)
            : base(commandId) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fault"/> class.
        /// </summary>
        /// <param name="commandId">
        /// The command id.
        /// </param>
        /// <param name="ackCommandId">
        /// The ack command id.
        /// </param>
        public Fault(int commandId, int ackCommandId)
            : base(commandId, ackCommandId) {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object.
        /// </param>
        public bool Equals(Fault other) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
