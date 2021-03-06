﻿namespace ftCarWin
{
    partial class FrmButtonControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLight = new System.Windows.Forms.Button();
            this.btnForward = new System.Windows.Forms.Button();
            this.btnBackward = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.logListBox = new System.Windows.Forms.ListBox();
            this.identifyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLight
            // 
            this.btnLight.Location = new System.Drawing.Point(248, 159);
            this.btnLight.Name = "btnLight";
            this.btnLight.Size = new System.Drawing.Size(75, 23);
            this.btnLight.TabIndex = 0;
            this.btnLight.Text = "&On";
            this.btnLight.UseVisualStyleBackColor = true;
            this.btnLight.Click += new System.EventHandler(this.LightButtonClick);
            // 
            // btnForward
            // 
            this.btnForward.Location = new System.Drawing.Point(248, 130);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(75, 23);
            this.btnForward.TabIndex = 1;
            this.btnForward.Text = "&Forward";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.ForwardButtonClick);
            // 
            // btnBackward
            // 
            this.btnBackward.Location = new System.Drawing.Point(248, 189);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(75, 23);
            this.btnBackward.TabIndex = 2;
            this.btnBackward.Text = "&Backward";
            this.btnBackward.UseVisualStyleBackColor = true;
            this.btnBackward.Click += new System.EventHandler(this.BackwardButtonClick);
            // 
            // btnLeft
            // 
            this.btnLeft.Location = new System.Drawing.Point(169, 158);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(75, 23);
            this.btnLeft.TabIndex = 3;
            this.btnLeft.Text = "&Left";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.LeftButtonClick);
            // 
            // btnRight
            // 
            this.btnRight.Location = new System.Drawing.Point(330, 158);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(75, 23);
            this.btnRight.TabIndex = 4;
            this.btnRight.Text = "&Right";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.RightButtonClick);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(173, 218);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(231, 20);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "&Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.StopButtonClick);
            // 
            // logListBox
            // 
            this.logListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logListBox.FormattingEnabled = true;
            this.logListBox.Location = new System.Drawing.Point(13, 246);
            this.logListBox.Name = "logListBox";
            this.logListBox.Size = new System.Drawing.Size(509, 134);
            this.logListBox.TabIndex = 6;
            // 
            // identifyButton
            // 
            this.identifyButton.Location = new System.Drawing.Point(248, 58);
            this.identifyButton.Name = "identifyButton";
            this.identifyButton.Size = new System.Drawing.Size(75, 23);
            this.identifyButton.TabIndex = 7;
            this.identifyButton.Text = "&Id";
            this.identifyButton.UseVisualStyleBackColor = true;
            this.identifyButton.Click += new System.EventHandler(this.IdentifyButtonClick);
            // 
            // FrmButtonControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 386);
            this.Controls.Add(this.identifyButton);
            this.Controls.Add(this.logListBox);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnBackward);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.btnLight);
            this.Name = "FrmButtonControl";
            this.Text = "Button control";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLight;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ListBox logListBox;
        private System.Windows.Forms.Button identifyButton;
    }
}

