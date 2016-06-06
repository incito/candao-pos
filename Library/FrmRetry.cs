using System;
using System.Windows.Forms;
using WebServiceReference;

namespace Library
{
    public partial class FrmRetry : Form
    {
        public FrmRetry(string errMsg)
        {
            InitializeComponent();
            RtbMsg.Text = errMsg;
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            var result = RestClient.CheckServerConnection();
            if (string.IsNullOrEmpty(result))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            RtbMsg.Text = result;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
