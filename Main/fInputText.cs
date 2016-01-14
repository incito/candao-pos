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

namespace Main
{
    public partial class frmInputText : frmBase
    {
        public string inputNo="";
        public static string ShowInputText()
        {
            frmInputText inputtext = new frmInputText();
            inputtext.ShowDialog();

            return inputtext.inputNo;
        }

        public static bool ShowInputAmount(string caption,string lbltext,out string input)
        {
            frmInputText inputtext = new frmInputText();
            inputtext.Text = caption;
            inputtext.lblTitle.Text = lbltext;
            inputtext.ShowDialog();
            input=inputtext.inputNo;
            inputtext.rbtDiscount.Visible = false;
            bool re=  inputtext.DialogResult == DialogResult.OK;
            try
            {
                inputtext.Close();
            }
            catch { }
            try
            {
                inputtext=null;
            }
            catch { }
            return re;
        }
        public static bool ShowInputAmount2(string caption, string lbltext, out string input, out int type)
        {
            frmInputText inputtext = new frmInputText();
            inputtext.Text = caption;
            inputtext.lblTitle.Text = lbltext;
            inputtext.ShowDialog();
            input = inputtext.inputNo;
            int stype = 0;
            if (inputtext.rbtDiscount.Checked)
            {
                stype = 1;
            }
            type = stype;
            bool re = inputtext.DialogResult == DialogResult.OK;
            try
            {
                inputtext.Close();
            }
            catch { }
            try
            {
                inputtext = null;
            }
            catch { }
            return re;
        }
        public frmInputText()
        {
            InitializeComponent();
        }

        private void frmInputText_Load(object sender, EventArgs e)
        {
            setBtnFocus();
        }

        private void frmInputText_Activated(object sender, EventArgs e)
        {
            edtMemberCard.Focus();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //edtMemberCard.Focus();
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //edtMemberCard.Focus();
            SendKeys.Send(button16.Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
            SendKeys.Send(button16.Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            //edtMemberCard.Focus();
            SendKeys.Send("{Backspace}");
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            //
            //if (!AskQuestion("确定要使用鱼券："+edtMemberCard.Text)) return;
            if(edtMemberCard.Text.Length>0)
            {
                if(rbtDiscount.Checked)
                {
                    if(float.Parse(edtMemberCard.Text)>=100)
                    {
                        edtMemberCard.Focus();
                        edtMemberCard.SelectAll();
                        return;
                    }
                    if (float.Parse(edtMemberCard.Text) < 10)
                    {
                        edtMemberCard.Focus();
                        edtMemberCard.SelectAll();
                        return;
                    }
                }
               inputNo = edtMemberCard.Text;
               this.DialogResult= DialogResult.OK;
               Close();
            }
        }
        private void setBtnFocus()
        {
            Common.Globals.SetButton(button15);
            Common.Globals.SetButton(button16);
            Common.Globals.SetButton(button17);
            Common.Globals.SetButton(button18);
            Common.Globals.SetButton(button19);
            Common.Globals.SetButton(button20);
            Common.Globals.SetButton(button21);
            Common.Globals.SetButton(button22);
            Common.Globals.SetButton(button23);
            Common.Globals.SetButton(button24);
            Common.Globals.SetButton(button25);
            Common.Globals.SetButton(button26);
            Common.Globals.SetButton(button27);
            Common.Globals.SetButton(button28);
            Common.Globals.SetButton(button27);

        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rbtAmount_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "优免金额";
            edtMemberCard.Focus();
            edtMemberCard.Text = "";
        }

        private void rbtDiscount_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "优免折扣";
            edtMemberCard.Focus();
            edtMemberCard.Text = "";
        }
    }
}
