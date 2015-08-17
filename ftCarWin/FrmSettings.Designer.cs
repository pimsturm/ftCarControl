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
            this.groupTransportChannel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDevices)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupTransportChannel.Controls.Add(this.radioBtnBluetooth);
            this.groupTransportChannel.Controls.Add(this.radioBtnSerial);
            this.groupTransportChannel.Location = new System.Drawing.Point(12, 12);
            this.groupTransportChannel.Name = "groupBox1";
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
            this.btnScan.Location = new System.Drawing.Point(12, 72);
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
            this.gridDevices.Location = new System.Drawing.Point(12, 102);
            this.gridDevices.Name = "gridDevices";
            this.gridDevices.ReadOnly = true;
            this.gridDevices.Size = new System.Drawing.Size(240, 150);
            this.gridDevices.TabIndex = 10;
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.gridDevices);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.groupTransportChannel);
            this.Name = "FrmSettings";
            this.Text = "FrmSettings";
            this.Deactivate += new System.EventHandler(this.FrmSettings_Deactivate);
            this.Load += new System.EventHandler(this.FrmSettings_Load);
            this.groupTransportChannel.ResumeLayout(false);
            this.groupTransportChannel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDevices)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupTransportChannel;
        private System.Windows.Forms.RadioButton radioBtnBluetooth;
        private System.Windows.Forms.RadioButton radioBtnSerial;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.DataGridView gridDevices;
    }
}