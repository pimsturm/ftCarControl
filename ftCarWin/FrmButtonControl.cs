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
        private ArduinoCommunicator _arduinoCommunicator;
        private Boolean _lightOn = false;

        public FrmButtonControl()
        {
            InitializeComponent();
            _arduinoCommunicator = new ArduinoCommunicator();
        }

        private void btnLight_Click(object sender, EventArgs e)
        {
            _lightOn = !_lightOn;
            if (_lightOn)
            {
                btnLight.Text = "Off";
                _arduinoCommunicator.SwitchLightOn();
            }
            else
            {
                btnLight.Text = "On";
                _arduinoCommunicator.SwitchLightOff();
            }
        }

        private void FrmButtonControl_Enter(object sender, EventArgs e) {
            Debug.WriteLine("Enter");
            _arduinoCommunicator.Setup();
        }

        private void FrmButtonControl_Leave(object sender, EventArgs e) {
            Debug.WriteLine("Leave");
            _arduinoCommunicator.Exit();
        }

    }
}
