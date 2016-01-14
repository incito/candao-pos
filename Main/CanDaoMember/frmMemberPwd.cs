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
    public partial class frmMemberPwd : frmBase
    {

        public static string valicode;
        private TCandaoMemberInfo _memberInfo = null;
        public static void ShowMemberPwd(TCandaoMemberInfo memberInfo)
        {
            frmMemberPwd inputtext = new frmMemberPwd();
            inputtext._memberInfo = memberInfo;
            inputtext.edtMobile.Text = memberInfo.Mobile;
            inputtext.ShowDialog();
            return;
        }
        private DevExpress.XtraEditors.TextEdit edtCurr;

        private JObject json = null;
        public frmMemberPwd()
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

        private void edtAmount_Click(object sender, EventArgs e)
        {

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

                //调用修改接口
                TCandaoRegMemberInfo memberinfo = new TCandaoRegMemberInfo();
                memberinfo.Branch_id = Globals.branch_id;
                memberinfo.Securitycode = "";
                memberinfo.Mobile = edtMobile.Text;
                memberinfo.Cardno =_memberInfo.Cardno;
                memberinfo.Password = edtPwd.Text.Trim();
                memberinfo.Name = _memberInfo.Name ;
                memberinfo.Gender = _memberInfo.Gender.ToString();
                memberinfo.Birthday = _memberInfo.Birthday.ToString();
                //memberinfo.Gender = _memberInfo.;
                //memberinfo.Birthday = _memberInfo.;
                memberinfo.Tenant_id = 0;
                memberinfo.Regtype = 0;
                memberinfo.Member_avatar = _memberInfo.Member_avatar;

                TCandaoRetBase ret = CanDaoMemberClient.MemberEdit(memberinfo);
                this.Cursor = Cursors.Default;
                if (!ret.Ret)
                {
                    Warning("修改失败:" + ret.Retinfo);
                }
                else
                {
                    Warning("修改成功!");
                    //清除，
                    edtMobile.Text = "";
                    edtPwd.Text = "";
                    edtPwd2.Text = "";

                    edtIdentCode.Text = "";
                    tmrGetIdentCode.Enabled = false;
                    btnGetIdentCode.Text = "发送";
                    btnGetIdentCode.Enabled = true;
                    Close();

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


            if (!edtPwd.Text.Equals(edtPwd2.Text))
            {
                Warning("两次输入的密码不符！");
                return ret;
            }
            if (!edtIdentCode.Text.Equals(valicode))
            {
                Warning("手机验证码错误！");
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
            if (e.KeyChar == 13)
            {

                button2_Click(sender, e);
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGetIdentCode_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (edtMobile.Text.Trim().ToString().Length <= 0)
                {
                    Warning("请输入手机号！");
                    edtMobile.Focus();
                    return;
                }
                try
                {
                    btnGetIdentCode.Enabled = false;
                    valicode = "";
                    CanDaoMemberClient.sendAccountByMobile(Globals.branch_id, "", edtMobile.Text, out valicode);
                    if (valicode.Equals(""))
                    {
                        Warning("发送失败，请重试！");
                        return;
                    }
                }
                finally { }
                if (valicode.Equals(""))
                {
                    btnGetIdentCode.Enabled = true;
                }
                else
                {
                    btnGetIdentCode.Tag = 60;
                    tmrGetIdentCode.Enabled = true;
                }
            }
            finally { this.Cursor = Cursors.Default; }
            
        }

        private void tmrGetIdentCode_Tick(object sender, EventArgs e)
        {
            int second=int.Parse(btnGetIdentCode.Tag.ToString());
            if(second<=0)
            {
                tmrGetIdentCode.Enabled = false;
                btnGetIdentCode.Text ="发送";
                btnGetIdentCode.Enabled = true;
                btnGetIdentCode.Focus();
                return;
            }
            else
            {
                second--;
                btnGetIdentCode.Tag = second;
                btnGetIdentCode.Text = String.Format("发送({0})", second);
            }
            Application.DoEvents();

        }
    }
}