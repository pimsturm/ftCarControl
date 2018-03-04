using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using ArduinoCommunicator;
using CmdMessenger.CmdComms;
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

            connectCancellationToken = new CancellationTokenSource();

            factory = new CmdFactory();
        }

        /// <summary>
        /// Reinitialize the handlers
        /// </summary>
        public void InitHandlers() {
            Debug.WriteLine(" Start communication.");
            
            if (listBoxLog == null)
                listBoxLog = new ListBoxLog(logListBox);
            LogMessage("Start communication.");

            if (messenger != null && (int)messenger.TransportChannel != Properties.Settings.Default.TransportChannel) {
                messenger.Stop();
                messenger.Dispose();
                messenger = null;
            }

            if (messenger == null) {
                ICmdComms client;
                if (Properties.Settings.Default.TransportChannel == (int)TransportChannel.BlueTooth) {
                    LogMessage("Create new bluetooth client");
                    client = new BluetoothCmdClient("30:15:01:07:11:28", this);
                }
                else {
                    LogMessage("Create new serial client");
                    client = new SerialCmdClient("COM7", 9600, this);
                    //client = null;
                }
                messenger = new CmdMessenger.CmdMessenger(client, this)
                {
                    PingCommand = factory.CreatePingCommand()
                };
                messenger.Register((int)Command.kIdentify, r => {
                    LogMessage(" Arduino is ready " + r.CommandId.ToString());
                    LogMessage(" Arduino ID " + r.ReadString());
                });
                messenger.Start();
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
                messenger.Send(factory.CreateCommandLightOn());
            }
            else
            {
                btnLight.Text = "On";
                messenger.Send(factory.CreateCommandLightOff());
            }
        }

        private void ForwardButtonClick(object sender, EventArgs e) {
            messenger.Send(factory.CreateCommandLeftMotorForward());
            messenger.Send(factory.CreateCommandRightMotorForward());
        }

        private void BackwardButtonClick(object sender, EventArgs e) {
            messenger.Send(factory.CreateCommandLeftMotorBackward());
            messenger.Send(factory.CreateCommandRightMotorBackward());
        }

        private void LeftButtonClick(object sender, EventArgs e) {
            messenger.Send(factory.CreateCommandLeftMotorBackward());
            messenger.Send(factory.CreateCommandRightMotorForward());
        }

        private void RightButtonClick(object sender, EventArgs e) {
            messenger.Send(factory.CreateCommandLeftMotorForward());
            messenger.Send(factory.CreateCommandRightMotorBackward());
        }

        private void StopButtonClick(object sender, EventArgs e) {
            messenger.Send(factory.CreateCommandStopMotors());
        }

        private void IdentifyButtonClick(object sender, EventArgs e) {
            messenger.Send(factory.CreatePingCommand());
        }

        }
}
