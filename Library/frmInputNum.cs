using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Library
{
    public partial class frmInputNum : frmBase
    {
        public string inputNo = "";
        public double maxnum = 0;

        public static bool ShowInputNum(string msg, out int inputNum, int maxnum)
        {
            frmInputNum frm = new frmInputNum();
            frm.lblMsg.Text = msg;
            frm.maxnum = maxnum;
            frm.button1.Left = frm.button2.Left;
            frm.button2.Visible = false;
            frm.ShowDialog();
            bool ret = frm.DialogResult == DialogResult.OK;
            if (ret)
            {
                inputNum = int.Parse(frm.tbxNum.Text);
            }
            else
            { inputNum = 0; }
            return ret;
        }
        public static bool ShowInputNum(string msg, string lbltext, out int inputNum, int maxnum)
        {
            frmInputNum frm = new frmInputNum();
            frm.lblMsg.Text = msg;
            frm.maxnum = maxnum;
            frm.button1.Left = frm.button2.Left;
            frm.button2.Visible = false;
            frm.label1.Text = lbltext;
            if (msg.IndexOf("退") > 0)
            {
                frm.label1.ForeColor = Color.Red;
            }
            frm.ShowDialog();
            bool ret = frm.DialogResult == DialogResult.OK;
            if (ret)
            {
                inputNum = int.Parse(frm.tbxNum.Text);
            }
            else
            { inputNum = 0; }
            return ret;
        }
        public static bool ShowInputNum(string msg, string lbltext, out int inputNum, double maxnum)
        {
            frmInputNum frm = new frmInputNum();
            frm.lblMsg.Text = msg;
            frm.maxnum = maxnum;
            frm.label1.Text = lbltext;
            frm.button1.Left = frm.button2.Left;
            frm.button2.Visible = false;
            if (msg.IndexOf("退") > 0)
            {
                frm.label1.ForeColor = Color.Red;
            }
            frm.ShowDialog();
            bool ret = frm.DialogResult == DialogResult.OK;
            if (ret)
            {
                inputNum = int.Parse(frm.tbxNum.Text);
            }
            else
            { inputNum = 0; }
            return ret;
        }

        public static bool ShowInputNum(string msg, string lbltext, out double inputNum, double maxnum)
        {
            frmInputNum frm = new frmInputNum();
            frm.lblMsg.Text = msg;
            frm.maxnum = maxnum;
            frm.label1.Text = lbltext;
            if (msg.IndexOf("退") > 0)
            {
                frm.label1.ForeColor = Color.Red;
            }
            frm.ShowDialog();
            var ret = frm.DialogResult == DialogResult.OK;
            inputNum = ret ? Math.Round(double.Parse(frm.tbxNum.Text), 2) : 0;
            return ret;
        }
        public frmInputNum()
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
            double inputNum = 0;
            try
            {
                inputNum = double.Parse(tbxNum.Text);
            }
            catch { inputNum = 0; }
            /*if (inputNum>20)
            {
                tbxNum.SelectAll();
                tbxNum.Focus();
                return;
            }*/
            if (inputNum <= 0)
            {
                tbxNum.SelectAll();
                tbxNum.Focus();
                return;
            }
            if (inputNum - maxnum > 0.00000001)
            {
                frmWarning.Warning(String.Format("数量不能多于:{0}", this.maxnum));
                tbxNum.SelectAll();
                tbxNum.Focus();
                return;
            }
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
