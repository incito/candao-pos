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
using Models.CandaoMember;
using WebServiceReference;

namespace Main
{
    public partial class frmMemberStored : frmBase
    {

        public static string valicode;
        private TCandaoMemberInfo _memberInfo = null;
        public static void ShowMemberStored(TCandaoMemberInfo memberInfo)
        {
            frmMemberStored inputtext = new frmMemberStored();
            inputtext._memberInfo = memberInfo;
            inputtext.edtMobile.Text = memberInfo.Mobile;
            inputtext.ShowDialog();
            return;
        }
        private DevExpress.XtraEditors.TextEdit edtCurr;

        private JObject json = null;
        public frmMemberStored()
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
            edtCurr = edtMobile;
            setBtnFocus();

        }

        private void edtRoom_Enter(object sender, EventArgs e)
        {
            edtCurr = edtMobile;
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

        }
        private bool QueryBalance(string memberinfo)
        {
            try
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;

                    return true;

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
            if (!checkInput())
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (rgpPayType.SelectedIndex < 0)
                {
                    Warning("请选择收款方式!");
                    return;
                }
                if (edtAmount.Text.Trim().Length <= 0)
                {
                    Warning("请输入储值金额!");
                    edtAmount.Focus();
                    edtAmount.SelectAll();
                    edtCurr = edtAmount;
                    return;
                }
                if (float.Parse(edtAmount.Text) <= 0)
                {
                    Warning("请输入正确的金额!");
                    edtAmount.Focus();
                    edtCurr = edtAmount;
                    edtAmount.SelectAll();
                    return;
                }
                if (!AskQuestion("确定储值 " + edtAmount.Text + " 吗?"))
                {
                    return;
                }

                lblAmount.Text = "卡余额:";
                String mmtext = "";
                TCandaoRet_StoreCardDeposit ret = CanDaoMemberClient.StoreCardDeposit(Globals.branch_id, "", _memberInfo.Cardno, Globals.branch_id, decimal.Parse(edtAmount.Text), 0, rgpPayType.SelectedIndex);
                if (!ret.Ret)
                {
                    Warning(ret.Retinfo);
                    return;
                }
                else
                {

                    lblAmount.Text = "卡余额:" + ret.StoreCardbalance.ToString();
                    edtAmount.Text = "";
                    edtAmount.Focus();
                    Warning("储值成功,交易流水号:" + ret.Tracecode);
                    return;
                }

            }
            finally { this.Cursor = Cursors.Default; }
        }
        private bool checkInput()
        {
            bool ret= false;
            if (edtMobile.Text.Trim().ToString().Length<=0)
            {
                Warning("请输入手机号！");
                return ret;
            }

            return true;
        }
        private void edtRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                string tmpmember = edtMobile.Text;
                if (tmpmember.IndexOf('=') > 0)
                {
                    tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                    //edtRoom.Text = tmpmember;
                    edtMobile.Text = tmpmember.Replace(";", "");
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

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGetIdentCode_Click(object sender, EventArgs e)
        {
 
            
        }

        private void tmrGetIdentCode_Tick(object sender, EventArgs e)
        {

        }
    }
}