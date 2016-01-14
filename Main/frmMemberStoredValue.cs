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
    public partial class frmMemberStoredValue : frmBase
    {

        public static void ShowMemberStoredValue()
        {
            frmMemberStoredValue inputtext = new frmMemberStoredValue();
            inputtext.ShowDialog();
            return;
        }
        private DevExpress.XtraEditors.TextEdit edtCurr;

        private JObject json = null;
        public frmMemberStoredValue()
        {
            InitializeComponent();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //edtCurr.Focus();
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void frmMemberStoredValue_Load(object sender, EventArgs e)
        {
            edtCurr = edtRoom;
            setBtnFocus();
        }

        private void edtRoom_Enter(object sender, EventArgs e)
        {
            edtCurr = edtRoom;
        }

        private void edtAmount_Click(object sender, EventArgs e)
        {
            edtCurr = edtAmount;
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

        private void button28_Click(object sender, EventArgs e)
        {
            //edtCurr.Focus();
            SendKeys.Send("{Backspace}");
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(edtCurr.Equals(edtRoom))
            {
                //会员查询
                if (QueryBalance(edtRoom.Text))
                {
                    edtAmount.Focus();
                    edtAmount.SelectAll();
                    edtCurr = edtAmount;
                }
            }
            else
            {
                //会员充值
                button27_Click(button27, e);
            }
        }
        private bool QueryBalance(string memberinfo)
        {
            try
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    json = (JObject)WebServiceReference.RestClient.QueryBalance(edtRoom.Text.Trim().ToString());
                    string data = json["Data"].ToString();
                    lblAmount.Text = "卡余额:";
                    String mmtext = "";
                    if (data == "0")
                    {
                        Warning(json["Info"].ToString());
                        return false;
                    }
                    else
                    {

                        lblAmount.Text = "卡余额:" + (float.Parse(json["psStoredCardsBalance"].ToString()) / 100.00).ToString();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Warning(ex.Message);
                    return true;
                }
            }finally
            { this.Cursor = Cursors.Default; }
            
        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (rgpPayType.SelectedIndex<0)
                    {
                        Warning("请选择收款方式!");
                        return;
                    }
                    if (edtAmount.Text.Trim().Length<=0)
                    {
                        Warning("请输入储值金额!");
                        edtAmount.Focus();
                        edtAmount.SelectAll();
                        edtCurr = edtAmount;
                        return;
                    }
                    if(float.Parse(edtAmount.Text)<=0)
                    {
                        Warning("请输入正确的金额!");
                        edtAmount.Focus();
                        edtCurr = edtAmount;
                        edtAmount.SelectAll();
                        return;
                    }
                    if (!AskQuestion("确定储值 " + edtAmount.Text+ " 吗?"))
                    {
                        return;
                    }
                    this.Cursor = Cursors.WaitCursor;
                    button27.Enabled = false;
                    DateTime date =DateTime.Now;
                    string pszSerial = string.Format("{0:yyyyMMddHHmmssffff}", date);
                    int paytype = 0;
                    if (rgpPayType.SelectedIndex==1)
                        paytype = 1;
                    json = (JObject)WebServiceReference.RestClient.StoreCardDeposit(edtRoom.Text.Trim().ToString(), float.Parse(edtAmount.Text), pszSerial, paytype);
                    string data = json["Data"].ToString();
                    lblAmount.Text = "卡余额:";
                    String mmtext = "";
                    if (data == "0")
                    {
                        Warning(json["Info"].ToString());
                        return;
                    }
                    else
                    {
                        JObject jsonQuery = null;
                        TMemberStoredInfo memberstoreinfo = new TMemberStoredInfo();
                        memberstoreinfo.Cardno = json["pszPan"].ToString();
                        memberstoreinfo.Treport_membertitle = WebServiceReference.WebServiceReference.Report_membertitle;
                        memberstoreinfo.Pzh = json["pszTrace"].ToString();
                        date = DateTime.Now;
                        string datestr = string.Format("{0:yyyy-MM-dd}", date);
                        memberstoreinfo.Date = datestr;
                        datestr = string.Format("{0:hh:mm}", date);
                        memberstoreinfo.Time = datestr;
                        try
                        {
                            jsonQuery = (JObject)WebServiceReference.RestClient.QueryBalance(edtRoom.Text.Trim().ToString());
                            //json = (JObject)WebServiceReference.RestClient.QueryBalance(edtMemberCard.Text.Trim().ToString());

                            memberstoreinfo.Store = (decimal.Parse(json["psStoreCardBalance"].ToString())/100).ToString();
                            memberstoreinfo.Point = (decimal.Parse(jsonQuery["psIntegralOverall"].ToString()) / 100).ToString();
                            memberstoreinfo.Amount = edtAmount.Text;
                        }
                        catch (Exception ex) 
                        {
                            try
                            {
                                jsonQuery = (JObject)WebServiceReference.RestClient.QueryBalance(edtRoom.Text.Trim().ToString());
                                //json = (JObject)WebServiceReference.RestClient.QueryBalance(edtMemberCard.Text.Trim().ToString());

                                memberstoreinfo.Store = (decimal.Parse(json["psStoreCardBalance"].ToString()) / 100).ToString();
                                memberstoreinfo.Point = (decimal.Parse(jsonQuery["psIntegralOverall"].ToString()) / 100).ToString();
                                memberstoreinfo.Amount = edtAmount.Text;
                            }
                            catch (Exception ex3)
                            {
                                try
                                {
                                    jsonQuery = (JObject)WebServiceReference.RestClient.QueryBalance(edtRoom.Text.Trim().ToString());
                                    //json = (JObject)WebServiceReference.RestClient.QueryBalance(edtMemberCard.Text.Trim().ToString());

                                    memberstoreinfo.Store = (decimal.Parse(json["psStoreCardBalance"].ToString()) / 100).ToString();
                                    memberstoreinfo.Point = (decimal.Parse(jsonQuery["psIntegralOverall"].ToString()) / 100).ToString();
                                    memberstoreinfo.Amount = edtAmount.Text;
                                }
                                catch (Exception ex2)
                                {
                                    Warning(ex2.Message);
                                    return;
                                }
                            }
                        }
                        
                        //打印交易凭条
                        try
                        {
                            ReportsFastReport.ReportPrint.PrintMemberStore(memberstoreinfo);
                        }
                        catch {  }
                        lblAmount.Text = "卡余额:" + (float.Parse(json["psStoreCardBalance"].ToString()) / 100.00).ToString();
                        edtAmount.Text = "";
                        edtAmount.Focus();
                        Warning("储值成功,交易流水号:" + json["pszTrace"].ToString());
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Msg.Warning(ex.Message);
                    return;
                }
            }
            finally
            { this.Cursor = Cursors.Default; button27.Enabled = true; }
            
        }

        private void edtRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                string tmpmember = edtRoom.Text;
                if (tmpmember.IndexOf('=') > 0)
                {
                    tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                    //edtRoom.Text = tmpmember;
                    edtRoom.Text = tmpmember.Replace(";", "");
                }
                button2_Click(sender, e);
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
            Common.Globals.SetButton(button2);
            Common.Globals.SetButton(button27);

        }
        private void edtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                button2_Click(sender, e);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}