using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ftCarWin
{
    public partial class FrmButtonControl : Form
    {
        private Boolean _lightOn = false;

        public FrmButtonControl()
        {
            InitializeComponent();
            ArduinoCommunicator.Setup();
        }

        private void btnLight_Click(object sender, EventArgs e)
        {
            _lightOn = !_lightOn;
            if (_lightOn)
            {
                btnLight.Text = "Off";
                ArduinoCommunicator.SwitchLightOn();
            }
            else
            {
                btnLight.Text = "On";
                ArduinoCommunicator.SwitchLightOff();
            }
        }

        private void btnForward_Click(object sender, EventArgs e) {
            ArduinoCommunicator.MotorForward();
        }

        private void btnBackward_Click(object sender, EventArgs e) {
            ArduinoCommunicator.MotorBackward();
        }

        private void btnLeft_Click(object sender, EventArgs e) {
            ArduinoCommunicator.MotorToLeft();
        }

        private void btnRight_Click(object sender, EventArgs e) {
            ArduinoCommunicator.MotorToRight();
        }

        private void btnStop_Click(object sender, EventArgs e) {
            ArduinoCommunicator.MotorStop();
        }


    }
}
