using System;
using System.Diagnostics;
using CommandMessenger;
using CommandMessenger.Transport;
using CommandMessenger.Transport.Serial;
using CommandMessenger.Transport.Bluetooth;

namespace ftCarWin {
    // List of recognized commands
    enum Command {
        kLeftMotor,
        kRightMotor,
        kLight,
        kSettings,
        kStatus,              // Command to report status
        kIdentify,            // Command to identify device
    };

    public static class ArduinoCommunicator {
        private static ITransport _transport;
        private static CmdMessenger _cmdMessenger;
        private static ConnectionManager _connectionManager;

        // Most of the time you want to be sure you are connecting with the correct device.        
        private const string CommunicationIdentifier = "BFAF4176-766E-436A-ADF2-96133C02B03C";

        // Maximum speed interval of the Arduino motor shield
        private const int pwmMotorLeft = 255;
        private const int pwmMotorRight = 255;

        /// <summary>
        /// Setup the communicator for the chosen transport channel
        /// </summary>
        public static void Setup() {
            if (Properties.Settings.Default.TransportChannel == (int)TransportChannel.SerialPort) {
                Debug.WriteLine("Serial port");
                if (_transport != null && _transport.GetType() != typeof(SerialTransport))
                    Exit();
                if (_transport == null)
                    SetupChannel(TransportChannel.SerialPort);
            }
            else {
                Debug.WriteLine("BlueTooth");
                if (_transport != null && _transport.GetType() != typeof(BluetoothTransport))
                    Exit();
                if (_transport == null)
                    SetupChannel(TransportChannel.BlueTooth);
            }
        }

        private static void SetupChannel(TransportChannel channel) {
            // Now let us set the transport layer
            _transport = GetTransport(channel);

            // Initialize the command messenger with the chosen transport layer
            // Set if it is communicating with a 16- or 32-bit Arduino board
            _cmdMessenger = new CmdMessenger(_transport, BoardType.Bit16) {
                PrintLfCr = false // Do not print newLine at end of command, to reduce data being sent
            };

            // Attach the callbacks to the Command Messenger
            AttachCommandCallBacks();

            // Attach to NewLinesReceived for logging purposes
            _cmdMessenger.NewLineReceived += NewLineReceived;

            // Attach to NewLineSent for logging purposes
            _cmdMessenger.NewLineSent += NewLineSent;

            _connectionManager = GetConnectionManager(channel);

            // Show all connection progress in the output window
            _connectionManager.Progress += (sender, eventArgs) => {
                if (eventArgs.Level <= 3) Debug.WriteLine(eventArgs.Description);
            };

            // If connection found, tell the arduino to turn the (internal) led on
            _connectionManager.ConnectionFound += (sender, eventArgs) => {
                //TODO: enable the control buttons on the form

                Debug.WriteLine("Connection found");

                // Get the settings currently stored in the Arduino EEPROM
                var command = new SendCommand((int)Command.kSettings);
                _cmdMessenger.SendCommand(command);
            };

            // Finally - activate connection manager
            _connectionManager.StartConnectionManager();

        }

        private static ITransport GetTransport(TransportChannel transportChosen) {
            switch (transportChosen) {
                case TransportChannel.SerialPort:
                    // Create Serial Port object
                    // Note that for some boards (e.g. Sparkfun Pro Micro) DtrEnable may need to be true.
                    return new SerialTransport {
                        CurrentSerialSettings = { PortName = "COM3", BaudRate = 9600, DtrEnable = false } // object initializer
                    };

                case TransportChannel.BlueTooth:
                    return new BluetoothTransport() {
                        // If you know your bluetooth device and you have already paired
                        // you can directly connect to you Bluetooth Device by adress adress.
                        // Under windows you can find the adresss at:
                        //    Control Panel >> All Control Panel Items >> Devices and Printers
                        //    Right-click on device >> properties >> Unique id
                        CurrentBluetoothDeviceInfo = BluetoothUtils.DeviceByAdress("30:15:01:07:11:28")
                    };

                case TransportChannel.Network:
                    throw new NotImplementedException();

            }
            return null;

        }

        private static ConnectionManager GetConnectionManager(TransportChannel transportChosen) {
            switch (transportChosen) {
                case TransportChannel.SerialPort:
                    var serialConnectionStorer = new SerialConnectionStorer("SerialConnectionManagerSettings.cfg");

                    return new SerialConnectionManager(
                        _transport as SerialTransport,
                        _cmdMessenger,
                        (int)Command.kIdentify,
                        CommunicationIdentifier,
                        serialConnectionStorer) {
                            // Enable watchdog functionality.
                            WatchdogEnabled = true,
                        };

                case TransportChannel.BlueTooth:
                    // The Connection manager is capable or storing connection settings, in order to reconnect more quickly  
                    // the next time the application is run. You can determine yourself where and how to store the settings
                    // by supplying a class, that implements ISerialConnectionStorer. For convenience, CmdMessenger provides
                    //  simple binary file storage functionality
                    var bluetoothConnectionStorer = new BluetoothConnectionStorer("BluetoothConnectionManagerSettings.cfg");

                    // It is easier to let the BluetoothConnectionManager connection for you.
                    // It will:
                    //  - Auto discover Bluetooth devices
                    //  - If not yet paired, try to pair using the default Bluetooth passwords
                    //  - See if the device responds with the correct CommunicationIdentifier
                    return new BluetoothConnectionManager(
                        _transport as BluetoothTransport,
                        _cmdMessenger,
                        (int)Command.kIdentify,
                        CommunicationIdentifier,
                        bluetoothConnectionStorer) {
                            // Enable watchdog functionality.
                            WatchdogEnabled = true,

                            // You can add PIN codes for specific devices
                            DevicePins =
                {
                    {"01:02:03:04:05:06","6666"},
                    {"01:02:03:04:05:07","7777"},
                },

                            // You can also add PIN code to try on all unpaired devices
                            // (the following PINs are tried by default: 0000, 1111, 1234 )
                            GeneralPins =
                {
                    "8888",
                }
                        };

                case TransportChannel.Network:
                    throw new NotImplementedException();

            }
            return null;

        }

        /// <summary>
        /// Send Command to the Arduino to switch on the light
        /// </summary>
        public static void SwitchLightOn() {
            var command = new SendCommand((int)Command.kLight);
            command.AddArgument(1);
            _cmdMessenger.SendCommand(command);
        }

        /// <summary>
        /// Send Command to the Arduino to switch off the light
        /// </summary>
        public static void SwitchLightOff() {
            var command = new SendCommand((int)Command.kLight);
            command.AddArgument(0);
            _cmdMessenger.SendCommand(command);
        }

        /// <summary>
        /// Send commands to make the car move straight ahead
        /// </summary>
        public static void MotorForward() {
            var command = new SendCommand((int)Command.kLeftMotor);
            command.AddArgument(pwmMotorLeft);
            _cmdMessenger.SendCommand(command);

            command = new SendCommand((int)Command.kRightMotor);
            command.AddArgument(pwmMotorRight);
            _cmdMessenger.SendCommand(command);

        }

        /// <summary>
        /// Send commands to make the car move backward
        /// </summary>
        public static void MotorBackward() {
            var command = new SendCommand((int)Command.kLeftMotor);
            command.AddArgument(-pwmMotorLeft);
            _cmdMessenger.SendCommand(command);

            command = new SendCommand((int)Command.kRightMotor);
            command.AddArgument(-pwmMotorRight);
            _cmdMessenger.SendCommand(command);

        }

        /// <summary>
        /// Send commands to make the car move to the left
        /// </summary>
        public static void MotorToLeft() {
            var command = new SendCommand((int)Command.kLeftMotor);
            command.AddArgument(-pwmMotorLeft);
            _cmdMessenger.SendCommand(command);

            command = new SendCommand((int)Command.kRightMotor);
            command.AddArgument(pwmMotorRight);
            _cmdMessenger.SendCommand(command);

        }

        /// <summary>
        /// Send commands to make the car move to the right
        /// </summary>
        public static void MotorToRight() {
            var command = new SendCommand((int)Command.kLeftMotor);
            command.AddArgument(pwmMotorLeft);
            _cmdMessenger.SendCommand(command);

            command = new SendCommand((int)Command.kRightMotor);
            command.AddArgument(-pwmMotorRight);
            _cmdMessenger.SendCommand(command);

        }

        /// Attach command call backs. 
        private static void AttachCommandCallBacks() {
            _cmdMessenger.Attach(OnUnknownCommand);
            _cmdMessenger.Attach((int)Command.kStatus, OnAcknowledge);
            _cmdMessenger.Attach((int)Command.kSettings, OnSettingsReceived);
        }

        // ------------------  C A L L B A C K S ---------------------

        // Called when a received command has no attached function.
        private static void OnUnknownCommand(ReceivedCommand arguments) {
            Debug.WriteLine("Command without attached callback received");
        }

        // Callback function that prints that the Arduino has acknowledged
        private static void OnAcknowledge(ReceivedCommand arguments) {
            Debug.WriteLine(" Arduino is ready");
        }

        private static void OnSettingsReceived(ReceivedCommand arguments) {
            Debug.WriteLine("Settings received from Arduino.");

            var timeOut = arguments.ReadInt16Arg();

            Debug.WriteLine("Timeout: <1>", timeOut);
        }

        // Log received line to console
        private static void NewLineReceived(object sender, CommandEventArgs e) {
            Debug.WriteLine(@"Received > " + e.Command.CommandString());
        }

        // Log sent line to console
        private static void NewLineSent(object sender, CommandEventArgs e) {
            Debug.WriteLine(@"Sent > " + e.Command.CommandString());
        }

        // Exit function
        public static void Exit() {
            if (_connectionManager != null)
                _connectionManager.StopConnectionManager();

            // Stop listening
            _cmdMessenger.Disconnect();

            // Dispose Command Messenger
            _cmdMessenger.Dispose();

            // Dispose Serial Port object
            _transport.Dispose();
            _transport = null;

            Debug.WriteLine("Exit from Arduino Communicator");
        }
    }
}
