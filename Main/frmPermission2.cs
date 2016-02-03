using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Common;
using DevExpress.XtraEditors;
using Library;
using Models.Enum;
using Models.Request;
using Models.Response;
using WebServiceReference;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;

namespace Main
{
    public partial class frmPermission2 : frmBase
    {
        private TextEdit focusedt = null;
        private EnumRightType _loginType = EnumRightType.OpenUp;

        public string UserId { get; set; }

        public string S { get; set; }

        public frmPermission2(string userName)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(userName))
                txtUser.Text = userName;
        }


        public static bool ShowPermission2(string msg, EnumRightType loginType, string userName = null)
        {
            frmPermission2 frm = new frmPermission2(userName);
            frm.label10.Text = msg;
            frm._loginType = loginType;
            frm.ShowDialog();
            return frm.DialogResult == DialogResult.OK;
        }

        private void frmPermission2_Load(object sender, EventArgs e)
        {
            focusedt = txtUser;
            ReadLoginInfo();
            setBtnFocus();
        }

        private void setBtnFocus()
        {
            Globals.SetButton(button18);
            Globals.SetButton(button19);
            Globals.SetButton(button20);
            Globals.SetButton(button21);
            Globals.SetButton(button22);
            Globals.SetButton(button23);
            Globals.SetButton(button24);
            Globals.SetButton(button25);
            Globals.SetButton(button26);
            Globals.SetButton(button27);
            Globals.SetButton(button28);
            Globals.SetButton(button2);
            Globals.SetButton(button11);
        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            focusedt = txtUser;
        }

        private void txtPwd_Enter(object sender, EventArgs e)
        {
            focusedt = txtPwd;
            txtPwd.SelectAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (focusedt == txtUser && !string.IsNullOrEmpty(focusedt.Text.Trim()))
            {
                txtPwd.Focus();
                return;
            }

            CheckUserRight();
        }

        private void CheckUserRight()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                button2.Enabled = false;
                Update();//必须
                string userID = txtUser.Text;
                int tmploginType = -1;

                IAccountService service = new AccountServiceImpl();
                var response = service.Login(txtUser.Text, txtPwd.Text, _loginType);
                if (!string.IsNullOrEmpty(response.Item1))
                {
                    AfterLoginError(response.Item1);
                    return;
                }

                SaveLoginInfo();//跟据选项保存登录信息   
                switch (_loginType)
                {
                    case EnumRightType.Login:
                        Globals.UserInfo.UserName = response.Item2;
                        Globals.UserInfo.PassWord = txtPwd.Text;
                        Globals.UserInfo.UserID = txtUser.Text;
                        break;
                    case EnumRightType.OpenUp:
                        Globals.authorizer = response.Item2;
                        string reinfo;
                        if (!RestClient.OpenUp(txtUser.Text, txtPwd.Text, 1, out reinfo))
                        {
                            AfterLoginError(reinfo);
                            return;
                        }
                        break;
                    case EnumRightType.ClearMachine:
                        IRestaurantService rService = new RestaurantServiceImpl();
                        var errMsg = rService.Clearner(txtUser.Text, response.Item2);
                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            AfterLoginError(errMsg);
                            return;
                        }
                        break;
                    default:
                        Globals.authorizer = response.Item2;
                        break;
                }

                DialogResult = DialogResult.OK; //成功
                Close(); //关闭登陆窗体
            }
            catch (CustomException ex)
            {
                AfterLoginError(ex.Message);
            }
            catch (Exception ex)
            {
                AfterLoginError("后台返回数据错误：" + Globals.UserInfo.msg);
            }
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// 登录失败以后的处理。
        /// </summary>
        /// <param name="errMsg">错误信息。</param>
        protected void AfterLoginError(string errMsg)
        {
            Warning(errMsg);
            button2.Enabled = true;
            txtPwd.Focus();
            txtPwd.SelectAll();
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
            Thread.Sleep(10);
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100); 
            SendKeys.Flush();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            focusedt.Focus();
            Thread.Sleep(10);
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
            if (e.KeyChar == 13)
            {
                button2_Click(button2, e);
            }
        }
    }
}