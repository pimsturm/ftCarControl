using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommandMessenger.Transport.Bluetooth;
using InTheHand.Net.Sockets;

namespace ftCarWin {

    public partial class FrmSettings : Form {
        BindingSource bs = new BindingSource();

        public FrmSettings() {
            InitializeComponent();

            radioBtnSerial.Tag = TransportChannel.SerialPort;
            radioBtnBluetooth.Tag = TransportChannel.BlueTooth;

            switch (Properties.Settings.Default.TransportChannel) {
                case (int)TransportChannel.SerialPort:
                    radioBtnSerial.Checked = true;
                    break;
                case (int)TransportChannel.BlueTooth:
                    radioBtnBluetooth.Checked = true;
                    break;
            }
            
        }

        private void radioBtnSerial_CheckedChanged(object sender, EventArgs e) {
            foreach (Control control in this.groupTransportChannel.Controls) {
                if (control is RadioButton) {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked) {
                        Properties.Settings.Default.TransportChannel = (int)radio.Tag;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        private void btnScan_Click(object sender, EventArgs e) {
            // Start discovering devices
            BluetoothDeviceInfo[] devices = BluetoothDeviceManager.LocalClient.DiscoverDevices(65536);

            // Load the discovered devices in the binding source of the grid
            bs.Clear();
            foreach (BluetoothDeviceInfo device in devices) {
                bs.Add(new BtDevice(device));
            }

        }

        private void FrmSettings_Load(object sender, EventArgs e) {
            // Format the grid and bind each column to a property in the datasource.
            bs.DataSource = typeof(BtDevice);

            gridDevices.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            gridDevices.RowHeadersVisible = false;
            gridDevices.AutoGenerateColumns = false;
            gridDevices.AutoSize = true;
            
            gridDevices.DataSource = bs;

            DataGridViewColumn deviceAddressCol = new DataGridViewTextBoxColumn();
            deviceAddressCol.HeaderText = "Device address";
            deviceAddressCol.Width = 110;
            deviceAddressCol.DataPropertyName = "DeviceAddress";

            gridDevices.Columns.Add(deviceAddressCol);

            DataGridViewColumn deviceNameCol = new DataGridViewTextBoxColumn();
            deviceNameCol.HeaderText = "Device name";
            deviceNameCol.Width = 150;
            deviceNameCol.DataPropertyName = "DeviceName";
            deviceNameCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            gridDevices.Columns.Add(deviceNameCol);

            DataGridViewColumn authenticatedCol = new DataGridViewCheckBoxColumn();
            authenticatedCol.HeaderText = "Authenticated";
            authenticatedCol.Width = 85;
            authenticatedCol.DataPropertyName = "Authenticated";

            gridDevices.Columns.Add(authenticatedCol);

            DataGridViewColumn rememberedCol = new DataGridViewCheckBoxColumn();
            rememberedCol.HeaderText = "Remembered";
            rememberedCol.Width = 85;
            rememberedCol.DataPropertyName = "Remembered";

            gridDevices.Columns.Add(rememberedCol);

        }

    }
}
