using ArduinoCommunicator.Commands;
using CmdMessenger.Commands;

namespace ArduinoCommunicator
{
    // List of recognized commands
    public enum Command
    {
        kLeftMotor,
        kRightMotor,
        kLight,
        kSettings,
        kStatus,              // Command to report status
        kIdentify,            // Command to identify device
        kStopMotor,
    };

    /// <summary>
    /// A message factory type used to create and parse all message types.
    /// </summary>
    public class CmdFactory
    {
        // Maximum speed interval of the Arduino motor shield
        private const int pwmMotorLeft = 255;
        private const int pwmMotorRight = 255;
        
        #region Methods

        /// <summary>
        /// Creates a <see cref="Fault"/> message. 
        /// </summary>
        /// <param name="message">A message describing the fault.</param>
        /// <returns>A <see cref="Fault"/> instance.</returns>
        public Fault CreateFaultMessage(string message) {
            // return new Fault(message);
            return null;
        }

        /// <summary>
        /// Creates a command to make the right motor move forward
        /// </summary>
        /// <returns>A <see cref="SendCommand"/></returns>
        public SendCommand CreateCommandRightMotorForward() {
            var command = new SendCommand((int)Command.kRightMotor);
            command.AddArguments(pwmMotorRight);
            return command;
        }

        /// <summary>
        /// Creates a command to make the left motor move forward
        /// </summary>
        /// <returns>A <see cref="SendCommand"/></returns>
        public SendCommand CreateCommandLeftMotorForward() {
            var command = new SendCommand((int)Command.kLeftMotor);
            command.AddArguments(pwmMotorLeft);
            return command;
        }

        /// <summary>
        /// Creates a command to make the right motor move backward
        /// </summary>
        /// <returns>A <see cref="SendCommand"/></returns>
        public SendCommand CreateCommandRightMotorBackward() {
            var command = new SendCommand((int)Command.kRightMotor);
            command.AddArguments(-pwmMotorRight);
            return command;
        }

        /// <summary>
        /// Creates a command to make the left motor move backward
        /// </summary>
        /// <returns>A <see cref="SendCommand"/></returns>
        public SendCommand CreateCommandLeftMotorBackward() {
            var command = new SendCommand((int)Command.kLeftMotor);
            command.AddArguments(-pwmMotorLeft);
            return command;
        }

        /// <summary>
        /// Creates a command to stop both motors
        /// </summary>
        /// <returns>A <see cref="SendCommand"/></returns>
        public SendCommand CreateCommandStopMotors() {
            return new SendCommand((int)Command.kStopMotor);
        }

        /// <summary>
        /// Creates a command to switch the light on
        /// </summary>
        /// <returns>A <see cref="SendCommand"/></returns>
        public SendCommand CreateCommandLightOn() {
            var command = new SendCommand((int)Command.kLight);
            command.AddArguments(1);
            return command;
        }

        /// <summary>
        /// Creates a command to switch the light off
        /// </summary>
        /// <returns>A <see cref="SendCommand"/></returns>
        public SendCommand CreateCommandLightOff() {
            var command = new SendCommand((int)Command.kLight);
            command.AddArguments(0);
            return command;
        }

        /// <summary>
        /// Create a <see cref="GetMotorSpeed"/> message.
        /// </summary>
        /// <returns>A <see cref="GetMotorSpeed"/> instance.</returns>
        public GetMotorSpeed CreateGetMotorSpeedMessage() {
            // return new GetMotorSpeed();
            return null;
        }

        /// <summary>
        /// Create a <see cref="GetMotorSpeedResponse"/> message.
        /// </summary>
        /// <param name="leftSpeedF">front left motor speed.</param>
        /// <param name="rightSpeedF">front right motor speed.</param>
        /// <param name="leftSpeedR">rear left motor speed.</param>
        /// <param name="rightSpeedR">rear right motor speed.</param>
        /// <returns>A <see cref="GetMotorSpeedResponse"/> instance.</returns>
        public GetMotorSpeedResponse CreateGetMotorSpeedResponseMessage(int leftSpeedF, int rightSpeedF, int leftSpeedR, int rightSpeedR) {
            // return new GetMotorSpeedResponse(leftSpeedF, rightSpeedF, rightSpeedR, leftSpeedR);
            return null;
        }


        /// <summary>
        /// Create a <see cref="SendMotorSpeed"/> message.
        /// </summary>
        /// <param name="leftSpeedF">Front left motor speed.</param>
        /// <param name="rightSpeedF">Front right motor speed.</param>
        /// <param name="leftSpeedR">Rear left motor speed.</param>
        /// <param name="rightSpeedR">Rear right motor speed.</param>
        /// <returns>A <see cref="SendMotorSpeed"/> instance.</returns>
        public SendMotorSpeed CreateSendMotorSpeedMessage(int leftSpeedF, int rightSpeedF, int leftSpeedR, int rightSpeedR) {
            // return new SendMotorSpeed(ProtocolVersion, leftSpeedF, rightSpeedF, rightSpeedR, leftSpeedR);
            return null;
        }


        /// <summary>
        /// Create a <see cref="SetMotorSpeed"/> message.
        /// </summary>
        /// <param name="leftSpeedF">Front left motor speed.</param>
        /// <param name="rightSpeedF">Front right motor speed.</param>
        /// <param name="leftSpeedR">Rear left motor speed.</param>
        /// <param name="rightSpeedR">Rear right motor speed.</param>
        /// <returns>A<see cref="SetMotorSpeed"/> instance.</returns>
        public SetMotorSpeed CreateSetMotorSpeedMessage(int leftSpeedF, int rightSpeedF, int leftSpeedR, int rightSpeedR) {
            return new SetMotorSpeed(leftSpeedF, rightSpeedF, leftSpeedR, rightSpeedR);
        }

        #endregion
    }
}
