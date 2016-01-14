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
    public partial class frmPermission : frmBase
    {
        public static bool ShowPermission()
        {
            frmPermission frm = new frmPermission();
            frm.ShowDialog();
            return frm.DialogResult == DialogResult.OK;
        }
        public frmPermission()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.OK;
        }
    }
}