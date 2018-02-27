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
        public static ListBoxLog listBoxLog;
        private readonly CmdFactory factory;
        private readonly CancellationTokenSource connectCancellationToken;
        private CmdMessenger.CmdMessenger messenger;

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
            
            if (listBoxLog == null)
                listBoxLog = new ListBoxLog(logListBox);
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
                client.Logger = this;
                this.messenger = new CmdMessenger.CmdMessenger(client, this);
                this.messenger.Register((int)Command.kIdentify, r => {
                    Debug.WriteLine(" Arduino is ready");
                    LogMessage(" Arduino is ready " + r.CommandId.ToString());
                    LogMessage(" Arduino ID " + r.ReadString());
                });
                this.messenger.Start();
            }

        }

        /// Log a message
        /// </summary>
        /// <param name="message">The message</param>
        public void LogMessage (string message) {
            listBoxLog.Log(Level.Info, message);
        }

        private void LightButtonClick(object sender, EventArgs e)
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

        private void ForwardButtonClick(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandLeftMotorForward());
            this.messenger.Send(this.factory.CreateCommandRightMotorForward());
        }

        private void BackwardButtonClick(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandLeftMotorBackward());
            this.messenger.Send(this.factory.CreateCommandRightMotorBackward());
        }

        private void LeftButtonClick(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandLeftMotorBackward());
            this.messenger.Send(this.factory.CreateCommandRightMotorForward());
        }

        private void RightButtonClick(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandLeftMotorForward());
            this.messenger.Send(this.factory.CreateCommandRightMotorBackward());
        }

        private void StopButtonClick(object sender, EventArgs e) {
            this.messenger.Send(this.factory.CreateCommandStopMotors());
        }

        private void IdentifyButtonClick(object sender, EventArgs e) {
            messenger.Send(factory.CreatePingCommand());
        }

        }
}
