using CmdMessenger.Commands;

namespace ArduinoCommunicator.Commands
{
    /// <summary>
    /// The ping.
    /// </summary>
    public class Ping : SendCommand
    {
        private const string arduinoId = "BFAF4176-766E-436A-ADF2-96133C02B03C";


        /// <summary>
        /// Initializes a new instance of the <see cref="Ping" /> class.
        /// </summary>
        public Ping()
            : base((int)Command.kIdentify) {
        }

    }
}
