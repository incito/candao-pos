using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Library;

namespace Main
{
    public partial class frmStart : frmBase
    {
        public static frmStart frm;
        public static void ShowStart()
        {
            frm = new frmStart();
            frm.Show();
        }
        public frmStart()
        {
            InitializeComponent();
        }
        public void setMsg(string msg)
        {
            lblMsg.Text = msg;
            lblMsg.Update();
            Application.DoEvents();
            lblMsg.Update();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
     
        }

        private void pbxMain_Click(object sender, EventArgs e)
        {

        }
    }
}