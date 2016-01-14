using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Library;
using System.IO;
using Common;
using WebServiceReference;

namespace Library
{
    public partial class frmAuthorize : frmBase
    {
        public string inputNo="";
        public int maxnum = 0;
        private TextBox focusedt = null;
        private string loginType;

        public static bool ShowAuthorize(string msg, string userid, string loginType,out string auserid)
        {
            frmAuthorize frm = new frmAuthorize();
            frm.lblMsg.Text = msg;
            frm.loginType = loginType;
            frm.ShowDialog();
            bool ret=frm.DialogResult == DialogResult.OK ;
            auserid = frm.edtUserID.Text;
            return ret; 
        }
        public frmAuthorize()
        {
            InitializeComponent();
        }

        private void frmInputText_Load(object sender, EventArgs e)
        {
            focusedt = edtUserID;
            ReadLoginInfo();
            setBtnFocus();
        }
        private void SaveLoginInfo()
        {
            //存在用户配置文件，自动加载登录信息
            string cfgINI = Application.StartupPath + SystemConfig.INI_CFG;

            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("LoginWindow", "SuperUser", edtUserID.Text);
        }
        private void ReadLoginInfo()
        {
            //存在用户配置文件，自动加载登录信息
            string cfgINI = Application.StartupPath + SystemConfig.INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                edtUserID.Text = ini.IniReadValue("LoginWindow", "SuperUser");
            }
        }
        private void frmInputText_Activated(object sender, EventArgs e)
        {
            edtUserID.Focus();
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
            if (focusedt == edtUserID)
            {
                if (focusedt.Text.Trim().ToString().Equals(""))
                {
                    focusedt.Focus();
                }
                else
                {
                    edtPwd.Focus();
                    edtPwd.SelectAll();
                }
            }
            else
            {
                checkuserright();
            }
        }
        private void checkuserright()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnOk.Enabled = false;
                this.Update();//必须
                string userID = edtUserID.Text;
                string password = CEncoder.GenerateMD5Hash(edtPwd.Text);/*常规加密*/
                string loginReturn = RestClient.Login(userID, password, loginType);
                if (loginReturn == "0") //调用登录策略
                {
                    this.SaveLoginInfo();//跟据选项保存登录信息   
                    this.DialogResult = DialogResult.OK; //成功
                    this.Close();
                }
                else
                {
                    Warning(Globals.UserInfo.msg);
                    btnOk.Enabled = true;
                    edtPwd.Focus();
                    edtPwd.SelectAll();
                }
            }
            catch 
            {
                btnOk.Enabled = true;
                Warning("验证错误，请检查网络!");
                edtPwd.Focus();
                edtPwd.SelectAll();
            }
            this.Cursor = Cursors.Default;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            focusedt.Text = "";
            focusedt.Focus();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void edtUserID_Enter(object sender, EventArgs e)
        {
            focusedt = edtUserID;
        }

        private void edtPwd_Enter(object sender, EventArgs e)
        {
            focusedt = edtPwd;
        }
    }
}
