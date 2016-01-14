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

namespace Main
{
    public partial class frmPettyCash : frmBase
    {
        public static bool ShowPettyCash()
        {
            frmPettyCash frm = new frmPettyCash();
            frm.ShowDialog();
            return frm.DialogResult == DialogResult.OK;
        }
        public frmPettyCash()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void setBtnFocus()
        {
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
            Common.Globals.SetButton(button2);
            Common.Globals.SetButton(button15);
            Common.Globals.SetButton(button16);
            Common.Globals.SetButton(button17);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //
            double amount = 0;
            try
            {
                amount = float.Parse(edtRoom.Text);
            }
            catch { amount = 0; }
            if (amount > 0)
            {
                //
                if (amount>10000)
                {
                    edtRoom.SelectAll();
                    edtRoom.Focus();
                    return;
                }
                if (!AskQuestion("确定输入零找金：" + edtRoom.Text)) return;
                string reinfo = "";
                if (RestClient.InputTellerCash(Globals.UserInfo.UserID, amount, 1, out reinfo))
                {
                    this.DialogResult = DialogResult.OK; //成功
                    this.Close(); //关闭登陆窗体
                }
                else
                {
                    Msg.ShowError(reinfo);
                }
            }
            else
            {
                edtRoom.Text = "";
                edtRoom.Focus();
            }
        }

        private void frmPettyCash_Activated(object sender, EventArgs e)
        {
            edtRoom.Focus();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //edtRoom.Focus();
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //edtRoom.Focus();
            SendKeys.Send(button16.Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
            SendKeys.Send(button16.Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            //edtRoom.Focus();
            SendKeys.Send("{Backspace}");
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void edtRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                button2_Click(button2, e);
            }
        }

        private void frmPettyCash_Load(object sender, EventArgs e)
        {
            setBtnFocus();

        }
    }
}