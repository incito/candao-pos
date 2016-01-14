using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common;
using Models;

namespace Main
{
    public partial class frmMemberNewCard : frmBase
    {
        private JObject json = null;
        private DevExpress.XtraEditors.TextEdit edtCurr;
        public static void ShowMemberNewCard()
        {
            frmMemberNewCard inputtext = new frmMemberNewCard();
            inputtext.ShowDialog();
            return;
        }
        public frmMemberNewCard()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (edtCardNo.Text.Length<10)
                    {
                        Warning("请输入正确的会员卡号!");
                        edtCardNo.Focus();
                        edtCardNo.SelectAll();
                        edtCurr = edtCardNo;
                        return;
                    }
                    /*if (edtPwd.Text.Length < 3)
                    {
                        Warning("请输入正确密码!");
                        edtPwd.Focus();
                        edtPwd.SelectAll();
                        edtCurr = edtPwd;
                        return;
                    }*/
                    if (edtMobile.Text.Length < 11)
                    {
                        Warning("请输入正确的手机号!");
                        edtMobile.Focus();
                        edtMobile.SelectAll();
                        edtCurr = edtMobile;
                        return;
                    }
                    if(!frmAskQuestion.AskQuestion("确定激活吗?"))
                    {
                        return;
                    }
                    
                    this.Cursor = Cursors.WaitCursor;
                    button27.Enabled = false;
                    DateTime date = DateTime.Now;
                    string pszSerial =edtMobile.Text.Trim().ToString();
                    json = (JObject)WebServiceReference.RestClient.CardActive(edtCardNo.Text.Trim().ToString(), edtPwd.Text.Trim().ToString(), pszSerial);
                    string data = json["Data"].ToString();
       
                    String mmtext = "";
                    if (data == "0")
                    {
                        Warning(json["Info"].ToString());
                        edtCardNo.Focus();
                        edtCardNo.SelectAll();
                        return;
                    }
                    else
                    {
                        //打印交易凭条
                        TMemberStoredInfo memberstoreinfo = new TMemberStoredInfo();
                        memberstoreinfo.Cardno = edtCardNo.Text;
                        memberstoreinfo.Treport_membertitle = WebServiceReference.WebServiceReference.Report_membertitle;
                        date = DateTime.Now;
                        memberstoreinfo.Pzh = string.Format("{0:yyyyMMddHHmmssffff}", date);
                        string datestr = string.Format("{0:yyyy-MM-dd}", date);
                        memberstoreinfo.Date = datestr;
                        datestr = string.Format("{0:hh:mm}", date);
                        memberstoreinfo.Time = datestr;
                        memberstoreinfo.Store = "0.00";
                        memberstoreinfo.Point = "0.00";
                        try
                        {
                            ReportsFastReport.ReportPrint.PrintMemberNewCard(memberstoreinfo);
                        }
                        catch { }
                        Warning(json["Info"].ToString());
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Warning(ex.Message);
                    return;
                }
            }
            finally
            { this.Cursor = Cursors.Default; button27.Enabled = true; }

        }

        private void frmMemberNewCard_Load(object sender, EventArgs e)
        {
            edtCurr = edtCardNo;
            setBtnFocus();
        }

        private void edtRoom_Enter(object sender, EventArgs e)
        {
            edtCurr = edtCardNo;
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
            Common.Globals.SetButton(button27);

            Common.Globals.SetButton(button15);
            Common.Globals.SetButton(button16);
            Common.Globals.SetButton(button17);
            Common.Globals.SetButton(button18);
        }
        private void edtPwd_Enter(object sender, EventArgs e)
        {
            edtCurr = edtPwd;
        }

        private void edtMobile_Enter(object sender, EventArgs e)
        {
            edtCurr = edtMobile;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(edtCurr.Equals(edtMobile))
            {
                button27_Click(button27, e);
            }
            else
              if(edtCurr.Equals(edtCardNo))
              {
                  edtPwd.Focus();
                  edtPwd.SelectAll();
              }
              else
              if(edtCurr.Equals(edtPwd))
              {
                  edtMobile.Focus();
                  edtPwd.SelectAll();
              }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //edtCurr.Focus();
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            //edtCurr.Focus();
            SendKeys.Send("{Backspace}");
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //edtCurr.Focus();
            SendKeys.Send(button16.Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
            SendKeys.Send(button16.Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void edtRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                string tmpmember = edtCardNo.Text;
                if (tmpmember.IndexOf('=') > 0)
                {
                    tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                    edtCardNo.Text = tmpmember.Replace(";","");
                }
            }
        }
    }
}