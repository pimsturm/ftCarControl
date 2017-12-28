using CmdMessenger.Commands;

namespace ArduinoCommunicator.Commands
{
    /// <summary>
    /// The set motor speed.
    /// </summary>
    public class SetMotorSpeed : SendCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SetMotorSpeed"/> class.
        /// </summary>
        /// <param name="leftSpeedF">
        /// The left speed f.
        /// </param>
        /// <param name="rightSpeedF">
        /// The right speed f.
        /// </param>
        /// <param name="leftSpeedR">
        /// The left speed r.
        /// </param>
        /// <param name="rightSpeedR">
        /// The right speed r.
        /// </param>
        public SetMotorSpeed(int leftSpeedF, int rightSpeedF, int leftSpeedR, int rightSpeedR)
            : base((int)ArduinoCommands.SetMotorSpeed, (int)ArduinoCommands.SendMotorSpeed) {
            this.AddArguments(leftSpeedF, rightSpeedF, leftSpeedR, rightSpeedR);

            this.LeftSpeedF = leftSpeedF;
            this.RightSpeedF = rightSpeedF;
            this.LeftSpeedR = leftSpeedR;
            this.RightSpeedR = rightSpeedR;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetMotorSpeed"/> class.
        /// </summary>
        /// <param name="receivedCommand">
        /// The received command.
        /// </param>
        public SetMotorSpeed(IReceivedCommand receivedCommand)
            : this(
                receivedCommand.ReadInt32(),
                receivedCommand.ReadInt32(),
                receivedCommand.ReadInt32(),
                receivedCommand.ReadInt32()) {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the left speed f.
        /// </summary>
        public int LeftSpeedF { get; private set; }

        /// <summary>
        /// Gets the left speed r.
        /// </summary>
        public int LeftSpeedR { get; private set; }

        /// <summary>
        /// Gets the right speed f.
        /// </summary>
        public int RightSpeedF { get; private set; }

        /// <summary>
        /// Gets the right speed r.
        /// </summary>
        public int RightSpeedR { get; private set; }

        #endregion
    }
}
