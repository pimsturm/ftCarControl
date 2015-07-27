﻿using System;
using CommandMessenger;
using CommandMessenger.Transport;
using CommandMessenger.Transport.Serial;
using CommandMessenger.Transport.Bluetooth;

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
      Identify,           // Command to identify device
    };

    public class ArduinoCommunicator
    {
        private static ITransport _transport;
        private SerialTransport _serialTransport;
        private CmdMessenger _cmdMessenger;
        private static ConnectionManager _connectionManager;

        // Most of the time you want to be sure you are connecting with the correct device.        
        private const string CommunicationIdentifier = "BFAF4176-766E-436A-ADF2-96133C02B03C";

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

        public void Setup_bt()
        {
            // Now let us set Bluetooth transport
            _transport = new BluetoothTransport()
            {
                // If you know your bluetooth device and you have already paired
                // you can directly connect to you Bluetooth Device by adress adress.
                // Under windows you can find the adresss at:
                //    Control Panel >> All Control Panel Items >> Devices and Printers
                //    Right-click on device >> properties >> Unique id
                CurrentBluetoothDeviceInfo = BluetoothUtils.DeviceByAdress("20:13:07:26:10:08")
            };

            // Initialize the command messenger with the Serial Port transport layer
            // Set if it is communicating with a 16- or 32-bit Arduino board
            _cmdMessenger = new CmdMessenger(_transport)
            {
                PrintLfCr = false // Do not print newLine at end of command, to reduce data being sent
            };

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
            _connectionManager = new BluetoothConnectionManager(
                _transport as BluetoothTransport,
                _cmdMessenger,
                (int)Command.Identify,
                CommunicationIdentifier,
                bluetoothConnectionStorer)
            {
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

            // Show all connection progress on command line             
            _connectionManager.Progress += (sender, eventArgs) =>
            {
                if (eventArgs.Level <= 3) Console.WriteLine(eventArgs.Description);
            };

            // If connection found, tell the arduino to turn the (internal) led on
            _connectionManager.ConnectionFound += (sender, eventArgs) =>
            {
                // Create command
                var command = new SendCommand((int)Command.kLight);
                // Send command
                _cmdMessenger.SendCommand(command);
            };

            // Finally - activate connection manager
            _connectionManager.StartConnectionManager();
        }

        private static ITransport GetTransport(Transport transportChosen)
        {
            switch (transportChosen)
            {
                case Transport.Serial :
                    // Create Serial Port object
                    // Note that for some boards (e.g. Sparkfun Pro Micro) DtrEnable may need to be true.
                    return new SerialTransport
                    {
                        CurrentSerialSettings = { PortName = "COM3", BaudRate = 9600, DtrEnable = false } // object initializer
                    };

                case Transport.Bluetooth:
                    return new BluetoothTransport()
                    {
                        // If you know your bluetooth device and you have already paired
                        // you can directly connect to you Bluetooth Device by adress adress.
                        // Under windows you can find the adresss at:
                        //    Control Panel >> All Control Panel Items >> Devices and Printers
                        //    Right-click on device >> properties >> Unique id
                        CurrentBluetoothDeviceInfo = BluetoothUtils.DeviceByAdress("20:13:07:26:10:08")
                    };

                case Transport.Network:
                    throw new NotImplementedException();

            }
            return null;

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
