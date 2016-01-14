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
    public partial class frmInputReOrderReason : frmBase
    {
        public string inputNo="";
        public double maxnum = 0;
        private string reason1 = "";
        private string reason2 = "";
        private string reason3 = "";
        private string reason4 = "";
        private string reason5 = "";


        public static bool ShowInputReOrderReason(string msg, out string inputStr)
        {
            frmInputReOrderReason frm = new frmInputReOrderReason();
            frm.lblMsg.Text = msg;
            frm.reason1= WebServiceReference.WebServiceReference.getreorderreason1();
            frm.reason2 = WebServiceReference.WebServiceReference.getreorderreason2();
            frm.reason3 = WebServiceReference.WebServiceReference.getreorderreason3();
            frm.reason4 = WebServiceReference.WebServiceReference.getreorderreason4();
            frm.reason5 = WebServiceReference.WebServiceReference.getreorderreason5();
            frm.rob1.Text = frm.reason1;
            frm.rob2.Text = frm.reason2;
            frm.rob3.Text = frm.reason3;
            frm.rob4.Text = frm.reason4;
            frm.rob5.Text = frm.reason5;
            if (frm.reason1.Equals(""))
            {
                frm.rob1.Visible = false;
            }
            if (frm.reason2.Equals(""))
            {
                frm.rob2.Visible = false;
            }
            if (frm.reason3.Equals(""))
            {
                frm.rob3.Visible = false;
            }
            if (frm.reason4.Equals(""))
            {
                frm.rob4.Visible = false;
            }
            if (frm.reason5.Equals(""))
            {
                frm.rob5.Visible = false;
            }
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

        public frmInputReOrderReason()
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

        private void rob1_Click(object sender, EventArgs e)
        {
            tbxNum.Text = ((RadioButton)sender).Text;
        }
    }
}
