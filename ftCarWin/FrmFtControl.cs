using System;
using System.Windows.Forms;

namespace ftCarWin {
    public partial class FrmFtControl : Form {
        public FrmFtControl() {
            InitializeComponent();
        }

        private void CarControlTreeViewAfterSelect(object sender, TreeViewEventArgs e) {
            splitNavBar.Panel2.Controls.Clear();
            IDockedForm dockedForm = (IDockedForm)e.Node.Tag;
            // Check if a docked form is associated with the selected node;
            // the root node does not have a form.
            if (dockedForm != null) {
                splitNavBar.Panel2.Controls.Add((Form)dockedForm);
                dockedForm.Show();
                dockedForm.InitHandlers();
            }

        }

        private void SetDockedFormProperties(Form dockedForm) {
            dockedForm.TopLevel = false;
            dockedForm.MaximizeBox = false;
            dockedForm.MinimizeBox = false;
            dockedForm.FormBorderStyle = FormBorderStyle.None;
            dockedForm.Dock = DockStyle.Fill;

        }

        private void FrmFtControl_Load(object sender, EventArgs e) {
            Form dockedForm = new FrmSettings();
            SetDockedFormProperties(dockedForm);
            carControlTreeView.Nodes["NodeFt"].Nodes["NodeSettings"].Tag = dockedForm;

            dockedForm = new FrmButtonControl();
            SetDockedFormProperties(dockedForm);
            carControlTreeView.Nodes["NodeFt"].Nodes["NodeButtonControl"].Tag = dockedForm;

        }
    }
}
