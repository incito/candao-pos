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
    public partial class fInputPwd : frmBase
    {
        public string inputNo="";
        public static string ShowInputPwd()
        {
            fInputPwd inputtext = new fInputPwd();
            inputtext.ShowDialog();

            return inputtext.inputNo;
        }
        public fInputPwd()
        {
            InitializeComponent();
        }

        private void frmInputText_Load(object sender, EventArgs e)
        {

        }

        private void frmInputText_Activated(object sender, EventArgs e)
        {
            edtMemberCard.Focus();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            edtMemberCard.Focus();
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            edtMemberCard.Focus();
            SendKeys.Send(button16.Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
            SendKeys.Send(button16.Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            edtMemberCard.Focus();
            SendKeys.Send("{Backspace}");
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            //
            if (!AskQuestion("确定要使用鱼券："+edtMemberCard.Text)) return;
            if(edtMemberCard.Text.Length>5)
            {
               inputNo = edtMemberCard.Text;
               this.DialogResult= DialogResult.OK;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
