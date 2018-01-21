
namespace CmdMessenger.CmdComms
{
    /// <summary>
    /// The transport channel used for communicating with the Arduino.
    /// </summary>
    public enum TransportChannel
    {
        /// <summary>
        /// No transport channel
        /// </summary>
        Undefined,
        /// <summary>
        /// Serial port
        /// </summary>
        SerialPort,
        /// <summary>
        /// BlueTooth
        /// </summary>
        BlueTooth,
        /// <summary>
        /// Network
        /// </summary>
        Network,
    }
}
