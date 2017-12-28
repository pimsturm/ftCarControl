using System;
using CmdMessenger.Commands;

namespace ArduinoCommunicator.Commands
{
    /// <summary>
    ///     Represents a get motor speed message.
    /// </summary>
    public class GetMotorSpeedResponse : ReceivedCommand, IEquatable<GetMotorSpeedResponse>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMotorSpeedResponse"/> class.
        /// </summary>
        /// <param name="arguments">
        /// The commands arguments.
        /// </param>
        public GetMotorSpeedResponse(string[] arguments)
            : base((int)ArduinoCommands.GetMotorSpeedResponse, arguments) {
            this.LeftSpeedF = this.ReadInt32();
            this.RightSpeedF = this.ReadInt32();
            this.LeftSpeedR = this.ReadInt32();
            this.RightSpeedR = this.ReadInt32();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the left front motor speed
        /// </summary>
        public int LeftSpeedF { get; private set; }

        /// <summary>
        ///     Gets the left rear motor speed
        /// </summary>
        public int LeftSpeedR { get; private set; }

        /// <summary>
        ///     Gets the right front motor speed
        /// </summary>
        public int RightSpeedF { get; private set; }

        /// <summary>
        ///     Gets the right rear motor speed
        /// </summary>
        public int RightSpeedR { get; private set; }

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
        public bool Equals(GetMotorSpeedResponse other) {
            throw new NotImplementedException();
        }

        #endregion
    }
}
