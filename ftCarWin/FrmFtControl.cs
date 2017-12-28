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
    public partial class FrmFtControl : Form {
        public FrmFtControl() {
            InitializeComponent();
        }

        private void tvCarControl_AfterSelect(object sender, TreeViewEventArgs e) {
            this.splitNavBar.Panel2.Controls.Clear();
            IDockedForm dockedForm = (IDockedForm)e.Node.Tag;
            // Check if a docked form is associated with the selected node;
            // the root node does not have a form.
            if (dockedForm != null) {
                this.splitNavBar.Panel2.Controls.Add((Form)dockedForm);
                dockedForm.InitHandlers();
                dockedForm.Show();
            }

        }

        private void SetDockedFormProperties(Form dockedForm) {
            dockedForm.TopLevel = false;
            dockedForm.MaximizeBox = false;
            dockedForm.MinimizeBox = false;
            dockedForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            dockedForm.Dock = DockStyle.Fill;

        }

        private void FrmFtControl_Load(object sender, EventArgs e) {
            Form dockedForm = new FrmSettings();
            SetDockedFormProperties(dockedForm);
            this.tvCarControl.Nodes["NodeFt"].Nodes["NodeSettings"].Tag = dockedForm;

            dockedForm = new FrmButtonControl();
            SetDockedFormProperties(dockedForm);
            this.tvCarControl.Nodes["NodeFt"].Nodes["NodeButtonControl"].Tag = dockedForm;

        }
    }
}
