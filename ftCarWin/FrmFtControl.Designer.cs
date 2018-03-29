namespace ftCarWin {
    partial class FrmFtControl {
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Button Control");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Settings");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("ft Car Control", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFtControl));
            this.splitNavBar = new System.Windows.Forms.SplitContainer();
            this.carControlTreeView = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitNavBar)).BeginInit();
            this.splitNavBar.Panel1.SuspendLayout();
            this.splitNavBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitNavBar
            // 
            this.splitNavBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitNavBar.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitNavBar.Location = new System.Drawing.Point(0, 0);
            this.splitNavBar.Name = "splitNavBar";
            // 
            // splitNavBar.Panel1
            // 
            this.splitNavBar.Panel1.Controls.Add(this.carControlTreeView);
            this.splitNavBar.Size = new System.Drawing.Size(668, 450);
            this.splitNavBar.SplitterDistance = 153;
            this.splitNavBar.TabIndex = 0;
            // 
            // carControlTreeView
            // 
            this.carControlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.carControlTreeView.Location = new System.Drawing.Point(0, 0);
            this.carControlTreeView.Name = "carControlTreeView";
            treeNode1.Name = "NodeButtonControl";
            treeNode1.Text = "Button Control";
            treeNode2.Name = "NodeSettings";
            treeNode2.Text = "Settings";
            treeNode3.Name = "NodeFt";
            treeNode3.Text = "ft Car Control";
            this.carControlTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.carControlTreeView.Size = new System.Drawing.Size(153, 450);
            this.carControlTreeView.TabIndex = 0;
            this.carControlTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.CarControlTreeViewAfterSelect);
            // 
            // FrmFtControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 450);
            this.Controls.Add(this.splitNavBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmFtControl";
            this.Text = "fischertechnik Car Control";
            this.Load += new System.EventHandler(this.FrmFtControl_Load);
            this.splitNavBar.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitNavBar)).EndInit();
            this.splitNavBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitNavBar;
        private System.Windows.Forms.TreeView carControlTreeView;

    }
}