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

namespace Main
{
    public partial class frmQueryMember : frmBase
    {
        public static void ShowQueryMember()
        {
            frmQueryMember inputtext = new frmQueryMember();
            inputtext.ShowDialog();

            return;
        }
        private JObject json = null;
        private string[] Ticks = null;
        private string[] tick = null;
        public frmQueryMember()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > '9' || e.KeyChar < '0')//如果输入的内容不是数字
            {
                e.Handled = true;
                if (Convert.ToInt32(e.KeyChar) == 8)
                {
                    e.Handled = false;
                }
            }
        }

        private void edtRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                //输入会员卡号
                string tmpmember = edtRoom.Text;
                if (tmpmember.IndexOf('=') > 0)
                {
                    tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                    edtRoom.Text = tmpmember.Replace(";", "");
                }
                button2_Click(button2, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                button2.Enabled = false;
                try
                {
                    json = (JObject)WebServiceReference.RestClient.QueryBalance(edtRoom.Text.Trim().ToString());
                    //json = (JObject)WebServiceReference.RestClient.QueryBalance(edtMemberCard.Text.Trim().ToString());
                }
                catch (Exception ex) { Warning(ex.Message); return; }
                
                string data = json["Data"].ToString();
                textEdit1.Text = "";
                textEdit2.Text = "";
                mmList.Text = "";
                String mmtext = "";
                if (data == "0")
                {
                    Warning(json["Info"].ToString());

                }
                else
                {
                    mmList.Text = json["psTicketInfo"].ToString();
                    textEdit1.Text = (float.Parse(json["psStoredCardsBalance"].ToString())/100.00).ToString();
                    String str = json["psIntegralOverall"].ToString();
                    float psIntegralOverall = 0;
                    try
                    {
                        psIntegralOverall = float.Parse(str) / (float)100.00;

                    }
                    catch
                    {
                        Warning("获取余额失败,请重试...");
                        return;
                    }
                    textEdit2.Text = psIntegralOverall.ToString();
                    string tickstr= json["psTicketInfo"].ToString();
                    Ticks = tickstr.Split(new char[] { ';' });
                    if (tickstr.Trim().ToString().Length <= 0)
                    {
                        mmList.Text = "";
                    }
                    else
                    {
                        try
                        {
                            for (int i = 0; i < Ticks.Length; i++)
                            {
                                str = Ticks[i].ToString();
                                tick = str.Split(new char[] { '|' });
                                String tickprice = (float.Parse(tick[1]) / 100.00).ToString();

                                mmtext = mmtext + String.Format("券ID:{0} 券金额:{1} 券类型:{2} 券名称:{3} 券张数:{4}", tick[0], tickprice, tick[2], tick[3], tick[4]) + "\r\n";

                            }


                        }
                        catch { }
                    }
                    mmList.Text = mmtext;
                }
            }
            catch (Exception ex)
            {
                Warning(ex.Message);
            }
            button2.Enabled = true;
            this.Cursor = Cursors.Default;
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
            Common.Globals.SetButton(button28);
            Common.Globals.SetButton(button2);

            Common.Globals.SetButton(button15);
            Common.Globals.SetButton(button16);
            Common.Globals.SetButton(button17);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmQueryMember_Load(object sender, EventArgs e)
        {
            setBtnFocus();
        }
    }
}