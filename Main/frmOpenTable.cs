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
using Models.Enum;
using WebServiceReference;

namespace Main
{
    public partial class frmOpenTable : frmBase
    {
        public delegate void FrmClose(object serder, EventArgs e);
        public delegate void OpenRm(object serder, EventArgs e);
        public event FrmClose frmClose;
        public event OpenRm openRm;
        public static bool ShowPettyCash()
        {
            frmOpenTable frm = new frmOpenTable();
            frm.ShowDialog();
            return frm.DialogResult == DialogResult.OK;
        }
        private void OnfrmClose()
        {
            if (frmClose != null)
                frmClose(this, new EventArgs());
        }
        public frmOpenTable()
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
            SendKeys.Send("{Tab}");
        }

        private void frmPettyCash_Activated(object sender, EventArgs e)
        {
            edtUserid.Focus();
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
            edtUserid.Focus();
            if (Globals.cjSetting.Status == null)
                Globals.cjSetting.Status = "0";
            if(Globals.cjSetting.Status.Equals("1"))
            {
                lblcj.Visible = true;
                edtCj.Visible = true;
            }
            else
            {
                lblcj.Visible = false;
                edtCj.Visible = false;
            }
        }
        public void initData(string tableNo)
        {
            edtRoom.Text = tableNo;
        }
        private void frmOpenTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnfrmClose();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if(edtUserid.Text.ToString().Length<=0)
            {
                Warning("请输入服务员编号...");
                edtUserid.Focus();
                edtUserid.SelectAll();
                return;
            }
            if(edtRoom.Text.ToString().Length<=0)
            {
                Warning("请输入台号...");
                edtRoom.Focus();
                edtRoom.SelectAll();
                return;
            }
            int manNum = strtointdef0(edtmanNum.Text);
            int womanNum = strtointdef0(edtwomanNum.Text);
            if((manNum+womanNum)<=0)
            {
                Warning("请输入正确的就餐人数...");
                edtwomanNum.Focus();
                edtRoom.SelectAll();
                return;
            }
            if(strtointdef0(edtCj.Text)>0)
            {
                if (strtointdef0(edtCj.Text)>1000)
                {
                    Warning("请输入正确的餐具数量...");
                    edtCj.Focus();
                    edtCj.SelectAll();
                    return;
                }
            }
            if (!AskQuestion("确定开台吗？"))
                return;
            //开台
            if (!RestClient.verifyuser(edtUserid.Text))
            {
                Warning("请输入正确的服务员编号！");
                edtUserid.Focus();
                edtUserid.SelectAll();
                return;
            }
            string orderid = "";
            string ageperiod = "";//ageperiod
            if (bthCheck1.Checked)
                ageperiod = "1";
            if (bthCheck2.Checked)
                ageperiod = ageperiod+"2";
            if (bthCheck3.Checked)
                ageperiod = ageperiod + "3";
            if (bthCheck4.Checked)
                ageperiod = ageperiod + "4";
            if (!RestClient.setorder(edtRoom.Text.ToString(), edtUserid.Text, manNum, womanNum, ageperiod, ref orderid))
            {
                Warning("开台失败,1！");
                return;
            }
            var openTableAction = new DeviceActionInfo(EnumDeviceAction.OpenTable, orderid);
            BigDataHelper.DeviceActionAsync(openTableAction);

            Globals.CurrOrderInfo.orderid = orderid;
            Globals.CurrOrderInfo.userid = edtUserid.Text;
            IniPos.setPosIniVlaue(Application.StartupPath, "ORDERCJ", orderid,edtCj.Text);
            openRm(sender, e);
            Close();
            //开台成功
            //Globals.CurrTableInfo.tableid = RestClient.getTakeOutTableID();
        }
        private int strtointdef0(string str)
        {
            int tmpret = 0;
            if (str.Length <= 0)
                return tmpret;
            try
            {
                tmpret = int.Parse(str);
            }
            catch { return 0; }
            return tmpret;
        }

        private void edtmanNum_EditValueChanged(object sender, EventArgs e)
        {
            //
            if (edtCj.Visible)
            {
                edtCj.Text = (strtointdef0(edtmanNum.Text) + strtointdef0(edtwomanNum.Text)).ToString();
            }
            else
                edtCj.Text = "";
        }

        private void tmrFocus_Tick(object sender, EventArgs e)
        {
            tmrFocus.Enabled = false;
            edtUserid.Focus();
        }

        private void edtCj_Enter(object sender, EventArgs e)
        {
            edtCj.SelectAll();
        }
    }
}