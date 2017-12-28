using CmdMessenger.Commands;

namespace ArduinoCommunicator.Commands
{
    /// <summary>
    /// The ping.
    /// </summary>
    public class Ping : SendCommand
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Ping" /> class.
        /// </summary>
        public Ping()
            : base((int)ArduinoCommands.Ping, (int)ArduinoCommands.PingResponse) {
        }

        #endregion
    }
}
