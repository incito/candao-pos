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
    public partial class frmPermission2 : frmBase
    {
        private DevExpress.XtraEditors.TextEdit focusedt = null;
        private eRightType loginType = eRightType.right2;
        public static bool ShowPermission2(string msg, eRightType loginType)
        {
            frmPermission2 frm = new frmPermission2();
            frm.label10.Text = msg;
            frm.loginType = loginType;
            frm.ShowDialog();
            return frm.DialogResult == DialogResult.OK;
        }
        public frmPermission2()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPermission2_Load(object sender, EventArgs e)
        {
            focusedt = txtUser;
            ReadLoginInfo();
            setBtnFocus();
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
            Common.Globals.SetButton(button11);
        }
        private void txtUser_Enter(object sender, EventArgs e)
        {
            focusedt = txtUser;
        }

        private void txtPwd_Enter(object sender, EventArgs e)
        {
            focusedt = txtPwd;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (focusedt == txtUser)
            {
                if (focusedt.Text.Trim().ToString().Equals(""))
                {
                    focusedt.Focus();
                }
                else
                {
                    txtPwd.Focus();
                    txtPwd.SelectAll();
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
                button2.Enabled = false;
                this.Update();//必须
                string userID = txtUser.Text;
                string password = CEncoder.GenerateMD5Hash(txtPwd.Text);/*常规加密*/
                int tmploginType = -1;
                string loginReturn = RestClient.Login(userID, password, ((int)loginType).ToString());
                if (loginReturn == "0") //调用登录策略
                {
                    //Msg.ShowError(loginReturn);
                    this.SaveLoginInfo();//跟据选项保存登录信息   
                    string reinfo = "";
                    if (loginType.Equals(eRightType.right2))
                    {
                        if (!RestClient.OpenUp(txtUser.Text, txtPwd.Text, 1, out reinfo))
                        {
                            Msg.Warning(reinfo);
                            button2.Enabled = true;
                            txtPwd.Focus();
                            txtPwd.SelectAll();
                            return;
                        }
                    }
                    this.DialogResult = DialogResult.OK; //成功
                    this.Close(); //关闭登陆窗体
                }
                else
                    if (loginReturn == "")
                    {

                        Warning("验证失败，请检查网络和后台服务!");
                        button2.Enabled = true;
                        txtPwd.Focus();
                        txtPwd.SelectAll();
                    }
                    else
                    {
                        Warning("验证失败，请检查用户名和密码!");
                        button2.Enabled = true;
                        txtPwd.Focus();
                        txtPwd.SelectAll();
                    }
            }
            catch (CustomException ex)
            {
                button2.Enabled = true;
                Warning(ex.Message);
                txtPwd.Focus();
                txtPwd.SelectAll();
            }
            catch (Exception ex)
            {
                button2.Enabled = true;
                Warning("后台返回数据错误：" + Globals.UserInfo.msg);
                txtPwd.Focus();
                txtPwd.SelectAll();
            }
            this.Cursor = Cursors.Default;
        }
        private void SaveLoginInfo()
        {
            //存在用户配置文件，自动加载登录信息
            string cfgINI = Application.StartupPath + SystemConfig.INI_CFG;

            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("LoginWindow", "SuperUser", txtUser.Text);
        }
        private void ReadLoginInfo()
        {
            //存在用户配置文件，自动加载登录信息
            string cfgINI = Application.StartupPath + SystemConfig.INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                txtUser.Text = ini.IniReadValue("LoginWindow", "User");
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            focusedt.Focus();
            System.Threading.Thread.Sleep(10);
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100); 
            SendKeys.Flush();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            focusedt.Focus();
            System.Threading.Thread.Sleep(10);
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100); 
            SendKeys.Flush();
        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
            focusedt.Focus();
            SendKeys.Send("{Backspace}");
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            focusedt.Focus();
            focusedt.Text = "";
        }

        private void frmPermission2_Activated(object sender, EventArgs e)
        {
            txtUser.Focus();
            txtUser.SelectAll();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                button2_Click(button2, e);
            }
        }
    }
}