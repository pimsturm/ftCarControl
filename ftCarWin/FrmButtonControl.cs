using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using ArduinoCommunicator;
using ArduinoCommunicator.Commands;
using CmdMessenger.CmdComms;
using CmdMessenger.Commands;
using CmdMessenger;

namespace ftCarWin
{
    public partial class FrmButtonControl : Form, IDockedForm, ILogger
    {
        private readonly CmdFactory factory;
        private readonly CancellationTokenSource connectCancellationToken;
        private CmdMessenger.CmdMessenger messenger;
        private readonly SynchronizationContext synchronizationContext;

        private Boolean _lightOn = false;

        public FrmButtonControl()
        {
            InitializeComponent();

            this.connectCancellationToken = new CancellationTokenSource();

            this.factory = new CmdFactory();
        }

        /// <summary>
        /// Reinitialize the handlers
        /// </summary>
        public void InitHandlers() {
            Debug.WriteLine(" Start communication.");
            // Clear log
            infoTextBox.Text = "";
            LogMessage("Start communication.");

            if (this.messenger != null && (int)messenger.TransportChannel != Properties.Settings.Default.TransportChannel) {
                this.messenger.Stop();
                this.messenger.Dispose();
                this.messenger = null;
            }

            if (this.messenger == null) {
                ICmdComms client;
                if (Properties.Settings.Default.TransportChannel == (int)TransportChannel.BlueTooth) {
                    LogMessage("Create new bluetooth client");
                    client = new BluetoothCmdClient("30:15:01:07:11:28");
                }
                else {
                    LogMessage("Create new serial client");
                    client = new SerialCmdClient("COM7", 9600);
                }
                this.messenger = new CmdMessenger.CmdMessenger(client);
                this.messenger.Register((int)ArduinoCommands.Ping, r => this.messenger.Send(new PingResponse()));
                this.messenger.Register((int)Command.kStatus, r => {
                    this.messenger.Send(new SendCommand((int)Command.kStatus));
                    Debug.WriteLine(" Arduino is ready");
                    LogMessage(" Arduino is ready");
                });

                this.messenger.Start();
            }

        }

        /// Log a message
        /// </summary>
        /// <param name="message">The message</param>
        public void LogMessage (string message) {
            infoTextBox.Text += message + Environment.NewLine;
        }

        private void btnLight_Click(object sender, EventArgs e)
        {
            _lightOn = !_lightOn;
            if (_lightOn)
            {
                btnLight.Text = "Off";
                this.messenger.Send(this.factory.CreateCommandLightOn());
            }
            else
            {
                btnLight.Text = "On";
                this.messenger.Send(this.factory.CreateCommandLightOff());
            }
        }

        private void btnForward_Click(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandLeftMotorForward());
            this.messenger.Send(this.factory.CreateCommandRightMotorForward());
        }

        private void btnBackward_Click(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandLeftMotorBackward());
            this.messenger.Send(this.factory.CreateCommandRightMotorBackward());
        }

        private void btnLeft_Click(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandLeftMotorBackward());
            this.messenger.Send(this.factory.CreateCommandRightMotorForward());
        }

        private void btnRight_Click(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandLeftMotorForward());
            this.messenger.Send(this.factory.CreateCommandRightMotorBackward());
        }

        private void btnStop_Click(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandStopMotors());
        }


    }
}
