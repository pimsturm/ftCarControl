﻿namespace ftCarWin {
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rBtnBluetooth = new System.Windows.Forms.RadioButton();
            this.radioBtnSerial = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rBtnBluetooth);
            this.groupBox1.Controls.Add(this.radioBtnSerial);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 53);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // rBtnBluetooth
            // 
            this.rBtnBluetooth.AutoSize = true;
            this.rBtnBluetooth.Location = new System.Drawing.Point(12, 34);
            this.rBtnBluetooth.Name = "rBtnBluetooth";
            this.rBtnBluetooth.Size = new System.Drawing.Size(70, 17);
            this.rBtnBluetooth.TabIndex = 8;
            this.rBtnBluetooth.TabStop = true;
            this.rBtnBluetooth.Text = "Bluetooth";
            this.rBtnBluetooth.UseVisualStyleBackColor = true;
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
            // 
            // FrmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmSettings";
            this.Text = "FrmSettings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rBtnBluetooth;
        private System.Windows.Forms.RadioButton radioBtnSerial;
    }
}