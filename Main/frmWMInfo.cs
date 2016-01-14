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
    public partial class frmWMInfo : frmBase
    {
        public delegate void FrmClose(object serder, EventArgs e);
        public delegate void OpenRm(object serder, EventArgs e);
        public event FrmClose frmClose;
        public event OpenRm openRm;
        private Models.TGzInfo gzInfo = new TGzInfo();
        public static bool ShowWMInfo(out TGzInfo gzinfo)
        {
            frmWMInfo frm = new frmWMInfo();
            frm.ShowDialog();
            gzinfo = frm.gzInfo;
            gzinfo.Gzname = frm.edtdw.Text;
            gzinfo.Relaperson = frm.edtlinkman.Text;
            gzinfo.Telephone = frm.edttele.Text;
            return frm.DialogResult == DialogResult.OK;
        }
        private void OnfrmClose()
        {
            if (frmClose != null)
                frmClose(this, new EventArgs());
        }
        public frmWMInfo()
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
            edtdw.Focus();
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
            if (e.KeyChar == 13)
            {
                button2_Click(button2, e);
            }
        }

        private void frmPettyCash_Load(object sender, EventArgs e)
        {
            setBtnFocus();
            edtdw.Focus();
        }
        public void initData(string tableNo)
        {
            edtlinkman.Text = tableNo;
        }
        private void frmOpenTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnfrmClose();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (edtdw.Text.ToString().Length <= 0)
            {
                Warning("请输入挂帐单位...");
                edtdw.Focus();
                edtdw.SelectAll();
                return;
            }

            if (!AskQuestion("确定挂帐"))
                return;
            //开台
            string orderid = "";
            //返回客户信息，再交给挂帐接口，挂帐，挂帐成功后，再调用接口保存挂帐信息
            this.DialogResult = DialogResult.OK;
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

        private void btnSelectGz_Click(object sender, EventArgs e)
        {
            String gzname = "";
            String id = "";
            string telephone = "";
            string relaperson = "";
            if (frmSelectGz.ShowSelectGz(out gzname, out id, out telephone, out relaperson))
            {
                edtdw.Text = gzname;
                edtlinkman.Text = relaperson;
                edttele.Text = telephone;
                gzInfo.Gzname = gzname;
                gzInfo.Gzcode = id;
                gzInfo.Relaperson = relaperson;
                gzInfo.Telephone = telephone;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}