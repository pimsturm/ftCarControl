using System;
using CommandMessenger;
using CommandMessenger.Transport.Serial;

namespace ftCarWin
{
    // List of recognized commands
    enum Command
    {
      kLeftMotor,
      kRightMotor,
      kLight,
      kSettings,
      kStatus              , // Command to report status
    };

    public class ArduinoCommunicator
    {
        private SerialTransport _serialTransport;
        private CmdMessenger _cmdMessenger;

        // Setup function
        public void Setup()
        {
            // Create Serial Port object
            // Note that for some boards (e.g. Sparkfun Pro Micro) DtrEnable may need to be true.
            _serialTransport = new SerialTransport
            {
                CurrentSerialSettings = { PortName = "COM3", BaudRate = 9600, DtrEnable = false } // object initializer
            };

            // Initialize the command messenger with the Serial Port transport layer
            // Set if it is communicating with a 16- or 32-bit Arduino board
            _cmdMessenger = new CmdMessenger(_serialTransport, BoardType.Bit16);

            // Attach the callbacks to the Command Messenger
            AttachCommandCallBacks();

            // Attach to NewLinesReceived for logging purposes
            _cmdMessenger.NewLineReceived += NewLineReceived;

            // Attach to NewLineSent for logging purposes
            _cmdMessenger.NewLineSent += NewLineSent;

            // Start listening
            _cmdMessenger.Connect();
        }

        // Send Command to the Arduino to switch on the light
        public void SwitchLightOn()
        {
            var command = new SendCommand((int)Command.kLight);
            command.AddArgument(1);
            _cmdMessenger.SendCommand(command);
        }

        // Send Command to the Arduino to switch off the light
        public void SwitchLightOff()
        {
            var command = new SendCommand((int)Command.kLight);
            command.AddArgument(0);
            _cmdMessenger.SendCommand(command);
        }

        /// Attach command call backs. 
        private void AttachCommandCallBacks()
        {
            _cmdMessenger.Attach(OnUnknownCommand);
            _cmdMessenger.Attach((int)Command.kStatus, OnAcknowledge);
        }

        // ------------------  C A L L B A C K S ---------------------

        // Called when a received command has no attached function.
        void OnUnknownCommand(ReceivedCommand arguments)
        {
            Console.WriteLine("Command without attached callback received");
        }

        // Callback function that prints that the Arduino has acknowledged
        void OnAcknowledge(ReceivedCommand arguments)
        {
            Console.WriteLine(" Arduino is ready");
        }

        // Log received line to console
        private void NewLineReceived(object sender, CommandEventArgs e)
        {
            Console.WriteLine(@"Received > " + e.Command.CommandString());
        }

        // Log sent line to console
        private void NewLineSent(object sender, CommandEventArgs e)
        {
            Console.WriteLine(@"Sent > " + e.Command.CommandString());
        }

        // Exit function
        public void Exit()
        {
            // Stop listening
            _cmdMessenger.Disconnect();

            // Dispose Command Messenger
            _cmdMessenger.Dispose();

            // Dispose Serial Port object
            _serialTransport.Dispose();

            // Pause before stop
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}
