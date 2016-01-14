using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Library;
using Common;

namespace Library
{
    public partial class frmInputMemo : frmBase
    {
        public string inputNo="";
        public double maxnum = 0;

        public static bool ShowInputMemo(string msg, out string inputStr)
        {
            frmInputMemo frm = new frmInputMemo();
            frm.lblMsg.Text = msg;

            frm.button1.Left = frm.button2.Left;
            frm.button2.Visible = false;
            frm.ShowDialog();
            bool ret=frm.DialogResult == DialogResult.OK ;
            if(ret)
            {
                inputStr = frm.tbxNum.Text; 
            }
            else
            { inputStr = ""; }
            return ret; 
        }

        public frmInputMemo()
        {
            InitializeComponent();
        }

        private void frmInputText_Load(object sender, EventArgs e)
        {
            setBtnFocus();
        }

        private void frmInputText_Activated(object sender, EventArgs e)
        {
            tbxNum.Focus();
        }

        private void button26_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
 
        }

        private void button27_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (tbxNum.Text.Equals(""))
                return;
            this.DialogResult = DialogResult.OK;
            Close();
        }
        private void setBtnFocus()
        {
            Common.Globals.SetButton(button18);
            Common.Globals.SetButton(button19);
            Common.Globals.SetButton(button20);
            Common.Globals.SetButton(button21);
            Common.Globals.SetButton(button22);
            Common.Globals.SetButton(button23);
            Common.Globals.SetButton(btn3);
            Common.Globals.SetButton(button25);
            Common.Globals.SetButton(button26);
            Common.Globals.SetButton(button1);
            Common.Globals.SetButton(button16);
            Common.Globals.SetButton(button2);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbxNum.Text = "";
            tbxNum.Focus();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }
    }
}
