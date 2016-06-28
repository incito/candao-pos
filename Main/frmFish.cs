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
using System.Linq;
using System.Windows;
using CanDao.Pos.UI.Library.Model;
using Models;
using WebServiceReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Main
{
    public partial class frmFish : frmBase
    {
        private const float ChargeFlag = 0.000001f;
        public delegate void FrmClose(object serder, EventArgs e);
        public delegate void OpenRm(object serder, EventArgs e);
        public event FrmClose frmClose;
        public event OpenRm openRm;
        private t_shopping dishinfo;
        private TPotDishInfo potDishInfo;
        private DevExpress.XtraEditors.TextEdit focusEdt;
        public static bool ShowFish(t_shopping dishinfo, out TPotDishInfo potDishInfo)
        {
            frmFish frm = new frmFish { dishinfo = dishinfo };
            frm.getGroupDetail();
            frm.initView();

            var dishTasteInfos = dishinfo.Taste.Split(',').Select(t => new TasteInfo {TasteTitle = t}).ToList();
            if (dishTasteInfos.Any())
                frm.tasteSetControl.TasteInfos = dishTasteInfos;
            else
                frm.tasteSetControl.Visibility = Visibility.Collapsed;

            frm.ShowDialog();
            bool ret = frm.DialogResult == DialogResult.OK;
            potDishInfo = frm.potDishInfo;
            return ret;
        }
        private void OnfrmClose()
        {
            if (frmClose != null)
                frmClose(this, new EventArgs());
        }

        public frmFish()
        {
            InitializeComponent();
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
            Globals.SetButton(btnOK);
            Globals.SetButton(button28);
            Globals.SetButton(button2);
            Globals.SetButton(button15);
            Globals.SetButton(button16);
            Globals.SetButton(button17);
        }

        private void frmPettyCash_Activated(object sender, EventArgs e)
        {
            edtNum1.Focus();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            SendKeys.Send(((Button)sender).Text);
            SendKeys.Flush();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SendKeys.Send(button16.Text);
            SendKeys.Flush();
            SendKeys.Send(button16.Text);
            SendKeys.Flush();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{Backspace}");
            SendKeys.Flush();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmPettyCash_Load(object sender, EventArgs e)
        {
            setBtnFocus();
            edtNum1.Focus();
            focusEdt = edtNum1;
        }

        private void frmOpenTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnfrmClose();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            var dishNum1 = strtofloatdef0(edtNum1.Text);
            if (dishNum1 - 0 < ChargeFlag)
            {
                Warning("请输入数量。");
                return;
            }

            if (string.IsNullOrEmpty(tasteSetControl.SelectedTaste))
            {
                Warning("请选择口味。");
                return;
            }

            potDishInfo.FishDishInfo1.Dishnum = dishNum1;

            if (potDishInfo.FishDishInfo2 != null)
            {
                var dishNum2 = strtofloatdef0(edtNum2.Text);
                if (dishNum2 - 0 < ChargeFlag)
                {
                    Warning("请输入第二种鱼的数量。");
                    return;
                }
                potDishInfo.FishDishInfo2.Dishnum = dishNum2;
            }

            SetDishTasteAndDiet(dishinfo, tasteSetControl.SelectedTaste, dietSetControl1.Diet);//鱼锅本身设置口味和忌口。
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SetDishTasteAndDiet(t_shopping dish, string taste, string diet)
        {
            dish.Avoid = diet;
            dish.SelectedTaste = taste;
        }

        private float strtofloatdef0(string str)
        {
            float tmpret = 0;
            if (str.Length <= 0)
                return tmpret;
            try
            {
                tmpret = (float)Math.Round(float.Parse(str), 2);
            }
            catch { return 0; }
            return tmpret;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool getGroupDetail()
        {
            bool ret = false;
            JArray groupData = null;
            ret = RestClient.getGroupDetail(dishinfo.Dishid, out groupData);
            if (!ret)
            {
                Warning("获取鱼锅信息失败!");
                return false;
            }

            potDishInfo = TPotDishInfo.getPotDishInfo(Globals.CurrOrderInfo.memberno, dishinfo.Dishid, groupData);
            potDishInfo.PotInfo.Dishnum = 1;//strtointdef0(edtnum1)

            string guid = getGUID();
            if (potDishInfo.FishDishInfo2 != null)
            {
                potDishInfo.FishDishInfo2.Dishnum = strtofloatdef0(edtNum2.Text);
                potDishInfo.FishDishInfo2.Parentdishid = dishinfo.Dishid;
                potDishInfo.FishDishInfo2.Groupid = guid;
                potDishInfo.FishDishInfo2.Groupid2 = guid;
            }
            potDishInfo.PotInfo.Parentdishid = dishinfo.Dishid;
            potDishInfo.FishDishInfo1.Parentdishid = dishinfo.Dishid;
            potDishInfo.PotInfo.Groupid = guid;
            potDishInfo.FishDishInfo1.Groupid = guid;
            potDishInfo.PotInfo.Groupid2 = guid;
            potDishInfo.FishDishInfo1.Groupid2 = guid;
            return true;
        }

        private void initView()
        {
            try
            {
                lblDishName.Text = dishinfo.Title;
                lblDishNameYg.Text = potDishInfo.PotInfo.Title;
                lblDishNameFish1.Text = potDishInfo.FishDishInfo1.Title;
                lblDishNameFish2.Visible = false;
                edtNum2.Visible = false;
                lblNum2.Visible = false;
                if (potDishInfo.FishDishInfo2 != null)
                {
                    lblDishNameFish2.Text = potDishInfo.FishDishInfo2.Title;
                    lblDishNameFish2.Visible = true;
                    edtNum2.Visible = true;
                    lblNum2.Visible = true;
                }
                //如果相同的锅已经下过一个了，就可以只加鱼
                try
                {
                    if (Globals.OrderTable.Rows.Count > 0)
                    {
                        if (existsDishid(potDishInfo))
                        {
                            btnFish.Visible = true;
                            btnCancel.Left = 249;
                        }
                    }
                }
                catch { }
            }
            catch { }
        }
        private static bool existsDishid(TPotDishInfo potInfo)
        {
            bool isin = false;
            foreach (DataRow dr in Globals.OrderTable.Rows)
            {
                if (dr["dishid"].ToString().Equals(potInfo.PotInfo.Dishid))
                {
                    isin = true;
                    break;
                }
            }
            return isin;
        }
        private static string getGUID()
        {
            Guid guid = new Guid();
            guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (focusEdt == edtNum1)
            {
                if (edtNum2.Visible)
                {
                    edtNum2.Focus();
                    return;
                }
                else
                {
                    button27_Click(btnOK, e);
                }
            }
            else
                button27_Click(btnOK, e);
        }

        private void edtNum1_Enter(object sender, EventArgs e)
        {
            focusEdt = (DevExpress.XtraEditors.TextEdit)sender;
        }

        private void btnFish_Click(object sender, EventArgs e)
        {
            //只加鱼
            if (strtofloatdef0(edtNum1.Text) + strtofloatdef0(edtNum2.Text) <= 0)
            {
                Warning("请输入正确的数量！");
                edtNum1.Focus();
                return;
            }
            potDishInfo.PotInfo.Dishnum = 0;
            button27_Click(btnOK, e);

        }
    }
}