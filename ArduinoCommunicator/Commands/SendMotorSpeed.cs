using System;
using CmdMessenger.Commands;

namespace ArduinoCommunicator.Commands
{
    /// <summary>
    /// Represents a send motor speed message type.
    /// The message type is designed to be sent from the device to the remote terminal.
    /// </summary>
    public class SendMotorSpeed : SendCommand, IEquatable<SendMotorSpeed>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SendMotorSpeed"/> class.
        /// </summary>
        /// <param name="commandId">
        /// The command Id
        /// </param>
        public SendMotorSpeed(int commandId)
            : base(commandId) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SendMotorSpeed"/> class.
        /// </summary>
        /// <param name="commandId">
        /// The command Id.
        /// </param>
        /// <param name="ackCommandId">
        /// The ack Command Id.
        /// </param>
        public SendMotorSpeed(int commandId, int ackCommandId)
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
        public bool Equals(SendMotorSpeed other) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
