using CmdMessenger.Commands;

namespace ArduinoCommunicator.Commands
{
    /// <summary>
    /// The ping response.
    /// </summary>
    public class PingResponse : SendCommand
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PingResponse" /> class.
        /// </summary>
        public PingResponse()
            : base((int)ArduinoCommands.PingResponse) {
        }

        #endregion
    }
}
