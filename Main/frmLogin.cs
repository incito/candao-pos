
///*************************************************************************/
///*
///* 文件名    ：frmLogin.cs    
///*
///* 程序说明  : 登录窗体
///*              登录成功后加载MDI主窗体，同时加载进度在登录窗体内显示
///
///* 原创作者  ： 
///* 
///* Copyright 2010-2011 
///*
///**************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using Library;
using System.IO;
using Models;
using Models.Enum;
using WebServiceReference;
using ReportsFastReport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Main
{
    /// <summary>
    /// 登录窗体
    /// </summary>
    public partial class frmLogin : frmBase
    {

        private DevExpress.XtraEditors.TextEdit focusedt = null;
        /// <summary>
        /// 私有构造器
        /// </summary>
        /// 
        private frmLogin()
        {
            InitializeComponent();
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            InitLoginWindow(); //初始化登陆窗体
            focusedt = txtUser;
            txtUser.Focus();
            setBtnFocus();
            lblVer.Text = String.Format("版本:{0}", Globals.ProductVersion);
            lblbranchid.Text = String.Format("店铺编号：{0}", RestClient.getbranch_id());
        }
        


        private void SetButton(Button button)
        {
          //MethodInfo methodinfo = button.GetType().GetMethod("SetStyle",BindingFlags.NonPublic|BindingFlags.Instance | BindingFlags.InvokeMethod);
          //METHODINFO.iNVOKE(button,BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.InvokeMethod,null,new object[]{ControlStyles.Selectable,false},Applicaton.CurrentCulture);
        }

        /// <summary>
        /// 显示登陆窗体.(公共静态方法)
        /// </summary>        
        public static bool Login()
        {
            frmLogin form = new frmLogin();
            try
            { frmStart.frm.Close(); }
            catch { }
            DialogResult result = form.ShowDialog();
            bool ret = (result == DialogResult.OK) ;
            return ret;
        }

        /// <summary>
        /// 初始化登陆窗体, 创建登录策略
        /// </summary>
        private void InitLoginWindow()
        {
            //this.BindingDataSet();//绑定帐套数据源
            this.ReadLoginInfo(); //读取保存的登录信息
            this.Text = "系统登录 (后台连接:WebService)";
            lblServer.Text = String.Format("服务器:{0}", RestClient.server);


        }

        private void ReadLoginInfo()
        {
            //存在用户配置文件，自动加载登录信息
            string cfgINI = Application.StartupPath + SystemConfig.INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                txtUser.Text = ini.IniReadValue("LoginWindow", "User");
                //txtDataset.EditValue = ini.IniReadValue("LoginWindow", "DataSetID");
                //txtPwd.Text = CEncoder.Decode(ini.IniReadValue("LoginWindow", "Password"));
                chkSaveLoginInfo.Checked = ini.IniReadValue("LoginWindow", "SaveLogin") == "Y";
            }
        }

        private void SaveLoginInfo()
        {
            //存在用户配置文件，自动加载登录信息
            string cfgINI = Application.StartupPath + SystemConfig.INI_CFG;

            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("LoginWindow", "User", txtUser.Text);
            //ini.IniWriteValue("LoginWindow", "DataSetID", txtDataset.EditValue.ToString());
            //ini.IniWriteValue("LoginWindow", "Password", CEncoder.Encode(txtPwd.Text));
            ini.IniWriteValue("LoginWindow", "SaveLogin", chkSaveLoginInfo.Checked ? "Y" : "N");
        }

        private void BindingDataSet()
        {

        }
        private void setBtnFocus()
        {
            Common.Globals.SetButton(button1);
            Common.Globals.SetButton(button2);
            Common.Globals.SetButton(button3);
            Common.Globals.SetButton(button4);
            Common.Globals.SetButton(button5);
            Common.Globals.SetButton(button6);
            Common.Globals.SetButton(button7);
            Common.Globals.SetButton(button8);
            Common.Globals.SetButton(button9);
            Common.Globals.SetButton(button10);
            Common.Globals.SetButton(button11);
            Common.Globals.SetButton(button12);
            Common.Globals.SetButton(button13);
       
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (focusedt.Text.Trim().ToString().Equals(""))
            {
                focusedt.Focus();
                return;
            }
            else
                if (txtPwd.Text.Trim().ToString().Length <= 0)
                {
                    txtPwd.Focus();
                    txtPwd.SelectAll();
                    focusedt = txtPwd;
                    return;
                }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.SetButtonEnable(false);
                this.Update();//必须
                this.ShowLoginInfo("正在验证用户名及密码");
             string userID = txtUser.Text;
             string password = CEncoder.GenerateMD5Hash(txtPwd.Text);/*常规加密*/
             //string dataSetID = txtDataset.EditValue.ToString();//帐套编号
             string dataSetDB = "user";// GetDataSetDBName();
             
             //LoginUser loginUser = new LoginUser(userID, password, dataSetID, dataSetDB);
             string loginReturn=RestClient.Login(userID, password,((int)EnumRightType.Login).ToString());
             
             if (loginReturn=="0") //调用登录策略
             {
                 //Msg.ShowError(loginReturn);
                 if (chkSaveLoginInfo.Checked) this.SaveLoginInfo();//跟据选项保存登录信息                    
                 Program.MainForm = new frmAllTable();//登录成功创建主窗体    
                 //登录成功，获取权限
                 getUserRigth();
                 if (!Globals.userRight.right6)
                 {
                     getUserRigth();
                 }
                 this.DialogResult = DialogResult.OK; //成功
                 this.Close(); //关闭登陆窗体
             }
             else
                 if (loginReturn=="")
                 {
                     this.ShowLoginInfo("登录失败，请检查网络和后台服务!");
                     Warning("登录失败，请检查网络和后台服务!");
                     this.SetButtonEnable(true);
                     txtPwd.Focus();
                     txtPwd.SelectAll();
                 }
                 else
                 {
                     this.ShowLoginInfo(Globals.UserInfo.msg);//"登录失败，请检查用户名和密码!"
                     Warning(Globals.UserInfo.msg);//"登录失败，请检查用户名和密码!"
                     this.SetButtonEnable(true);
                     txtPwd.Focus();
                     txtPwd.SelectAll();
                 }
         }
         catch (CustomException ex)
         {
             this.SetButtonEnable(true);
             this.ShowLoginInfo(ex.Message);
             Warning(ex.Message);
             txtPwd.Focus();
             txtPwd.SelectAll();
         }
         catch (Exception ex)
         {
             this.SetButtonEnable(true);
             this.ShowLoginInfo("登录失败，请检查用户名和密码!"+ex.Message);
             Warning("登录失败，请检查用户名和密码!" );
             txtPwd.Focus();
             txtPwd.SelectAll();
         }
         this.Cursor = Cursors.Default;
        }

        private string GetDataSetDBName()
        {
            DataRowView view = (DataRowView)txtDataset.Properties.GetDataSourceRowByKeyValue(txtDataset.EditValue);
            return ConvertEx.ToString(view.Row["DBName"]);
        }

        public void getUserRigth()
        {
            try
            {
                Globals.userRight.initRight();
                if(RestClient.getRightCode("1").Equals("1"))
                {
                    JArray jrRight = null;
                    try
                    {
                        if (!RestClient.getUserRights(Globals.UserInfo.UserID, out jrRight))
                        {
                            return;
                        }
                    }
                    catch { }
                    Globals.tbUserRigth = Models.Bill_Order.getRight_List(jrRight);
                    getuserRight(Globals.tbUserRigth);
                }
                else
                {
                    JObject jrRight2 = null;
                    try
                    {
                        if (!RestClient.getuserrights(Globals.UserInfo.UserID, Globals.UserInfo.PassWord, out jrRight2))
                        {
                            return;
                        }
                    }
                    catch { }
                    getuserRight_New(jrRight2);
                }

            }
            catch { }
        }
        public void getuserRight(DataTable dt)
        {
            foreach(DataRow dr in dt.Rows)
            {
                //获取权限
                int resourcesPath = int.Parse(dr["resourcesPath"].ToString());
                switch (resourcesPath)
                {
                    case 1:
                        Globals.userRight.right1 = true;
                        break;
                    case 2:
                        Globals.userRight.right2 = true;
                        break;
                    case 3:
                        Globals.userRight.right3 = true;
                        break;
                    case 4:
                        Globals.userRight.right4 = true;
                        break;
                    case 5:
                        Globals.userRight.right5 = true;
                        break;
                    case 6:
                        Globals.userRight.right6 = true;
                        break;
                    case 7:
                        Globals.userRight.right7 = true;
                        break;
                    case 8:
                        Globals.userRight.right8 = true;
                        break;
                }
            }
        }
        public void getuserRight_New(JObject jr)
        {
            //获取权限
            Globals.userRight.right1 = jr["030201"].ToString().Equals("1");
            Globals.userRight.right2 = jr["030202"].ToString().Equals("1");
            Globals.userRight.right3 = jr["030203"].ToString().Equals("1");
            Globals.userRight.right4 = jr["030204"].ToString().Equals("1");
            Globals.userRight.right5 = jr["030205"].ToString().Equals("1");
            Globals.userRight.right6 = jr["030206"].ToString().Equals("1");
            Globals.userRight.right7 = false;
            Globals.userRight.right8 = false;
        }
        private void ShowLoginInfo(string info)
        {
            lblLoadingInfo.Text = info;
            lblLoadingInfo.Update();
        }

        private void SetButtonEnable(bool value)
        {
            btnLogin.Enabled = value;
            btnCancel.Enabled = value;
            btnLogin.Update();
            btnCancel.Update();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (AskQuestion("确定要退出系统吗？")) Application.Exit();
        }

        private void btnModifyPwd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lblTime.Text = "当前时间：" + dt.ToLocalTime().ToString();//2005-11-5 21:21:25
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //
            /*if(txtUser.Focused)
            {
                txtUser.Text = txtUser.Text + ((Button)sender).Text;
            }
            else
            if(txtPwd.Focused)
            {
                txtPwd.Text = txtUser.Text + ((Button)sender).Text;
            }*/
            //focusedt.Focus();
            System.Threading.Thread.Sleep(10); 
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100); 
            SendKeys.Flush();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            //
            focusedt.Text = "";

            //focusedt.Focus();
        }

        private void txtUser_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtUser_Enter(object sender, EventArgs e)
        {
            focusedt = txtUser;
        }

        private void txtPwd_Enter(object sender, EventArgs e)
        {
            focusedt = txtPwd;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //focusedt.Focus();
            SendKeys.Send("{Backspace}");
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if(focusedt==txtUser)
            {
                if(focusedt.Text.Trim().ToString().Equals(""))
                {
                    focusedt.Focus();
                }
                else
                {
                    txtPwd.Focus();
                    txtPwd.SelectAll();
                    focusedt = txtPwd;
                }
            }
            else
            {
                btnLogin_Click(btnLogin,e);
            }
        }

        private void frmLogin_Activated(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        private void txtPwd_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==13)
            {
                button13_Click(button13, e);
            }
        }
    }
}