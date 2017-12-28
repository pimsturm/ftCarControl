
namespace ArduinoCommunicator
{
    /// <summary>
    /// Message types
    /// </summary>
    public enum ArduinoCommands
    {
        #region Motor speed message types

        /// <summary>
        ///     Request the device to send it's motor speed.
        /// </summary>
        GetMotorSpeed = 1,

        /// <summary>
        ///     Response message to a GetMotorSpeed messages.
        /// </summary>
        GetMotorSpeedResponse,

        /// <summary>
        ///     Sets the motor speed on the device.
        /// </summary>
        SetMotorSpeed,

        /// <summary>
        ///     Message send when an external device changes the motor speed.
        /// </summary>
        SendMotorSpeed,

        #endregion

        #region Pan tilt message types

        /// <summary>
        ///     Request the current pan tilt position from the device.
        /// </summary>
        GetPanTiltPos,

        /// <summary>
        ///     Response message to a GetPanTiltPos message.
        /// </summary>
        GetPanTiltPosResponse,

        /// <summary>
        ///     Set the pan tilt position on the device.
        /// </summary>
        SetPanTiltPos,

        /// <summary>
        ///     Message sent when an external device modifies the pan tilt position.
        /// </summary>
        SendPanTiltPos,

        #endregion

        #region Sensor message types

        /// <summary>
        ///     Get the current value for a specified sensor.
        /// </summary>
        GetSensorValue,

        /// <summary>
        ///     Response message to a GetSensorValue.
        /// </summary>
        GetSensorValueResponse,

        /// <summary>
        ///     Message send when a sensor value changes.
        /// </summary>
        SendSensorvalue,

        #endregion

        /// <summary>
        ///     A fault command.
        /// </summary>
        Fault,

        /// <summary>
        ///     A ping command.
        /// </summary>
        Ping = 200,

        /// <summary>
        ///     A ping response.
        /// </summary>
        PingResponse = 201
    }
}
