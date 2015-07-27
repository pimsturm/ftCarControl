using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ftCarWin {
    public partial class FrmSettings : Form {
        public FrmSettings() {
            InitializeComponent();
        }

        private void radioBtnSerial_CheckedChanged(object sender, EventArgs e) {
            string result1 = null;
            foreach (Control control in this.groupBox1.Controls) {
                if (control is RadioButton) {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked) {
                        result1 = radio.Text;
                        MessageBox.Show(result1);
                    }
                }
            }
        }
    }
}
