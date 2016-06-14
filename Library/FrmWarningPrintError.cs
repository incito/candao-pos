using System;
using System.Windows.Forms;

namespace Library
{
    public partial class FrmWarningPrintError : frmBase
    {
        public FrmWarningPrintError()
        {
            InitializeComponent();
        }

        public FrmWarningPrintError(string msg)
            : this()
        {
            RtbMsg.Text = msg;
        }

        /// <summary>
        /// 是否选中10分钟以内不再提醒。
        /// </summary>
        public bool IsCheckedNoWarning
        {
            get { return CbNoWarning.Checked; }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void frmWarning_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void frmWarning_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Close();
            }
        }
    }
}
