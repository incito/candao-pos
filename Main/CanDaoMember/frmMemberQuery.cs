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
using WebServiceReference;
using Models.CandaoMember;

namespace Main
{
    public partial class frmMemberQuery : frmBase
    {
        public static void ShowMemberQuery()
        {
            frmMemberQuery inputtext = new frmMemberQuery();
            inputtext.ShowDialog();

            return;
        }
        private JObject json = null;
        private string[] Ticks = null;
        private string[] tick = null;
        public TCandaoMemberInfo memberInfo = null;
        public frmMemberQuery()
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
                string tmpmember = edtMobile.Text;
                if (tmpmember.IndexOf('=') > 0)
                {
                    tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                    edtMobile.Text = tmpmember.Replace(";", "");
                }
                button2_Click(button2, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{Enter}");
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
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

        private void edtMobile_Enter(object sender, EventArgs e)
        {

        }

        private void frmMemberQuery_Activated(object sender, EventArgs e)
        {
            edtMobile.Focus();
        }

        private void edtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                //调用查询
                memberInfo = CanDaoMemberClient.QueryBalance(Globals.branch_id, "", edtMobile.Text, edtPwd.Text);
                if (!memberInfo.Retcode.Equals("0"))
                {
                    Warning(memberInfo.Retinfo);
                    return;
                }
                edtName.Text = memberInfo.Name;
                edtAmount.Text = memberInfo.Storecardbalance.ToString();
                edtPoint.Text = memberInfo.Integraloverall.ToString();
                try
                {
                    dtpBirthday.Value = DateTime.Parse(memberInfo.Birthday.ToString());
                }
                catch { }
                if (memberInfo.Gender.ToString().Equals("0"))
                    rgpGender.SelectedIndex = 0;
                else
                    rgpGender.SelectedIndex = 1;
                //生日
                btnModify.Enabled = true;
                btnStore.Enabled = true;
                btnCardLose.Enabled = true;
                btndelete.Enabled = true;
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            frmMemberPwd.ShowMemberPwd(memberInfo);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!AskQuestion("确定要挂失吗？"))
            {
                return;
            }
            TCandaoRet_CardLose cardlose = CanDaoMemberClient.CardLose(Globals.branch_id, "", memberInfo.Cardno, "", "");
            if(!cardlose.Ret)
            {
                Warning(cardlose.Retinfo);
                return;
            }
            Warning(edtMobile.Text+"挂失成功!");
            clearInof();
        }
        private void clearInof()
        {
            edtMobile.Text = "";
            edtName.Text = "";
            edtAmount.Text = "";
            edtPoint.Text = "";
            edtPwd.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!AskQuestion("确定要注销吗？"))
            {
                return;
            }
            TCandaoRet_CardLose cardlose = CanDaoMemberClient.CardCancellation(Globals.branch_id, "", edtMobile.Text, "", "");
            if (!cardlose.Ret)
            {
                Warning(cardlose.Retinfo);
                return;
            }
            Warning(edtMobile.Text + "注销成功!");
            clearInof();
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            frmMemberStored.ShowMemberStored(memberInfo);
        }
    }
}