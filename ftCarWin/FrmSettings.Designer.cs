namespace ftCarWin {
    partial class FrmSettings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.groupTransportChannel = new System.Windows.Forms.GroupBox();
            this.radioBtnBluetooth = new System.Windows.Forms.RadioButton();
            this.radioBtnSerial = new System.Windows.Forms.RadioButton();
            this.btnScan = new System.Windows.Forms.Button();
            this.gridDevices = new System.Windows.Forms.DataGridView();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.numSeconds = new System.Windows.Forms.NumericUpDown();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.groupTransportChannel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSeconds)).BeginInit();
            this.SuspendLayout();
            // 
            // groupTransportChannel
            // 
            this.groupTransportChannel.Controls.Add(this.radioBtnBluetooth);
            this.groupTransportChannel.Controls.Add(this.radioBtnSerial);
            this.groupTransportChannel.Location = new System.Drawing.Point(12, 27);
            this.groupTransportChannel.Name = "groupTransportChannel";
            this.groupTransportChannel.Size = new System.Drawing.Size(197, 53);
            this.groupTransportChannel.TabIndex = 8;
            this.groupTransportChannel.TabStop = false;
            this.groupTransportChannel.Text = "Connection";
            // 
            // radioBtnBluetooth
            // 
            this.radioBtnBluetooth.AutoSize = true;
            this.radioBtnBluetooth.Location = new System.Drawing.Point(12, 34);
            this.radioBtnBluetooth.Name = "radioBtnBluetooth";
            this.radioBtnBluetooth.Size = new System.Drawing.Size(70, 17);
            this.radioBtnBluetooth.TabIndex = 8;
            this.radioBtnBluetooth.TabStop = true;
            this.radioBtnBluetooth.Text = "Bluetooth";
            this.radioBtnBluetooth.UseVisualStyleBackColor = true;
            // 
            // radioBtnSerial
            // 
            this.radioBtnSerial.AutoSize = true;
            this.radioBtnSerial.Location = new System.Drawing.Point(12, 19);
            this.radioBtnSerial.Name = "radioBtnSerial";
            this.radioBtnSerial.Size = new System.Drawing.Size(72, 17);
            this.radioBtnSerial.TabIndex = 7;
            this.radioBtnSerial.TabStop = true;
            this.radioBtnSerial.Text = "Serial port";
            this.radioBtnSerial.UseVisualStyleBackColor = true;
            this.radioBtnSerial.CheckedChanged += new System.EventHandler(this.radioBtnSerial_CheckedChanged);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(12, 87);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(197, 23);
            this.btnScan.TabIndex = 9;
            this.btnScan.Text = "Scan Bluetooth devices";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // gridDevices
            // 
            this.gridDevices.AllowUserToAddRows = false;
            this.gridDevices.AllowUserToDeleteRows = false;
            this.gridDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDevices.Location = new System.Drawing.Point(12, 116);
            this.gridDevices.Name = "gridDevices";
            this.gridDevices.ReadOnly = true;
            this.gridDevices.Size = new System.Drawing.Size(262, 171);
            this.gridDevices.TabIndex = 10;
            // 
            // lblTimeout
            // 
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(18, 7);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(63, 13);
            this.lblTimeout.TabIndex = 11;
            this.lblTimeout.Text = "Car timeout:";
            // 
            // numSeconds
            // 
            this.numSeconds.Location = new System.Drawing.Point(92, 4);
            this.numSeconds.Name = "numSeconds";
            this.numSeconds.Size = new System.Drawing.Size(44, 20);
            this.numSeconds.TabIndex = 12;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(138, 8);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(47, 13);
            this.lblSeconds.TabIndex = 13;
            this.lblSeconds.Text = "seconds";
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 299);
            this.Controls.Add(this.lblSeconds);
            this.Controls.Add(this.numSeconds);
            this.Controls.Add(this.lblTimeout);
            this.Controls.Add(this.gridDevices);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.groupTransportChannel);
            this.Name = "FrmSettings";
            this.Text = "FrmSettings";
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.groupTransportChannel.ResumeLayout(false);
            this.groupTransportChannel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSeconds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupTransportChannel;
        private System.Windows.Forms.RadioButton radioBtnBluetooth;
        private System.Windows.Forms.RadioButton radioBtnSerial;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridView gridDevices;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.NumericUpDown numSeconds;
        private System.Windows.Forms.Label lblSeconds;
    }
}