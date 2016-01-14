using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Library;
using Common;
using System.IO;
using Models;
using WebServiceReference;
using ReportsFastReport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Main
{
    public partial class frmSelectCardNo : frmBase
    {
        private string cardnos;
        private string selectcardno = "";
        public static bool ShowSelectCardNo(string cardnos, out string cardno)
        {
            frmSelectCardNo frm = new frmSelectCardNo();
            frm.cardnos = cardnos;
            string[] strs = cardnos.ToString().Split(',');
            if (strs.Length==2)
            {
                frm.Width = 600;
                frm.lblMember1.Text = strs[0];
                frm.lblMember2.Text = strs[1];
                frm.imgMember3.Visible = false;
            }
            else if (strs.Length > 2) { frm.lblMember3.Text = strs[2]; }

            frm.ShowDialog();
            bool ret=frm.DialogResult == DialogResult.OK;
            cardno = frm.selectcardno;
            return ret;
        }
        public frmSelectCardNo()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imgMember1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(lblMember1.Text.ToString(), lblMember1.Font, new SolidBrush(lblMember1.ForeColor), lblMember1.Left - 10, 10);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmSelectCardNo_Resize(object sender, EventArgs e)
        {
            pictureBox1.Left = 0;
        }

        private void imgMember2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(lblMember2.Text.ToString(), lblMember1.Font, new SolidBrush(lblMember1.ForeColor), lblMember1.Left - 10, 10);
        }

        private void pictureBox4_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(lblMember3.Text.ToString(), lblMember1.Font, new SolidBrush(lblMember1.ForeColor), lblMember1.Left - 10, 10);
        }

        private void imgMember1_Click(object sender, EventArgs e)
        {
            selectcardno = lblMember1.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void imgMember2_Click(object sender, EventArgs e)
        {
            selectcardno = lblMember2.Text;
            this.DialogResult = DialogResult.OK;
        }

        private void imgMember3_Click(object sender, EventArgs e)
        {
            selectcardno = lblMember3.Text;
            this.DialogResult = DialogResult.OK;
        }

    }
}