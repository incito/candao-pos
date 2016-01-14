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
    public partial class frmInvoice : frmBase
    {
        public string inputNo="";
        public decimal maxnum = 0;
        public string cardno = "";

        public static bool ShowInvoice(string orderid, string tableno, string invoicetitle, decimal maxnum, string cardno)
        {
            frmInvoice frm = new frmInvoice();
            frm.lblMsg.Text = String.Format("桌号：{0} 开发票",tableno);
            frm.lblOrderID.Text = orderid;
            frm.lblInvoice.Text=invoicetitle;
            frm.cardno = cardno;
            frm.maxnum=maxnum;
            //frm.button2.Visible = false;
            frm.ShowDialog();
            bool ret=frm.DialogResult == DialogResult.OK ;
            return ret; 
        }

        public frmInvoice()
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
            if(!AskQuestion("确定不开发票吗？"))
            {
                return;
            }
            Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            decimal inputNum = 0;
            try
            {
                inputNum = decimal.Parse(tbxNum.Text);
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
            if(inputNum>this.maxnum)
            {
                frmWarning.Warning(String.Format("不能大于帐单金额:{0}", this.maxnum));
                tbxNum.SelectAll();
                tbxNum.Focus();
                return;
            }
            try
            {
                if (WebServiceReference.WebServiceReference.isPrint_invoice)
                    ReportsFastReport.ReportPrint.PrintInvoice(lblOrderID.Text, Globals.CurrOrderInfo.Invoicetitle, Globals.CurrTableInfo.tableNo, inputNum);
            }
            catch { }
                        
            try
            {
                WebServiceReference.MessageCenter.updateInvoice(lblOrderID.Text, inputNum,cardno);
            }
            catch { }
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
