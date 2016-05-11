using System;
using System.Windows.Forms;

namespace Library
{
    public partial class frmAskQuestion : frmBase
    {
        public static bool ShowAskQuestion(string msg)
        {
            frmAskQuestion frm = new frmAskQuestion { RtbMsg = { Text = msg } };
            frm.ShowDialog();
            return frm.DialogResult == DialogResult.OK;
        }

        private frmAskQuestion()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
