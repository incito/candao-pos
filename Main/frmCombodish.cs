using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Library;
using Common;
using System.IO;
using Models;
using WebServiceReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Main
{
    public partial class frmCombodish : frmBase
    {
        public delegate void FrmClose(object serder, EventArgs e);
        public delegate void OpenRm(object serder, EventArgs e);
        public event FrmClose frmClose;
        public event OpenRm openRm;
        private t_shopping dishinfo;
        private TComboDish comboDish;
        //解析一份和PAD一样的套餐存储结构，下单的时候获取一个尾号，这样PAD才能还原选择的数量
        private ArrayList comboDishPad = new ArrayList();//TComboDish
        private DevExpress.XtraEditors.TextEdit focusEdt;
        private ArrayList onlycontrols = new ArrayList();
        private ArrayList combocontrols = new ArrayList();
        private int lblHeight = 27;
        private int edtHeight = 27;
        private int comboheight = 0;
        private string FGuid = "";

        public static bool ShowCombodish(t_shopping dishinfo, TComboDish comboDish)
        {
            frmCombodish frm = new frmCombodish();
            frm.comboDish = comboDish;
            frm.dishinfo = dishinfo;
            if (!frm.getGroupDetail())
            {
                return false;
            }
            frm.comboDish.Dishinfo = dishinfo;
            frm.initView();
            frm.ShowDialog();
            bool ret = frm.DialogResult == DialogResult.OK;
            //potDishInfo = frm.potDishInfo;
            return ret;
        }

        private void OnfrmClose()
        {
            if (frmClose != null)
                frmClose(this, new EventArgs());
        }

        public frmCombodish()
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

        private void button2_Click(object sender, EventArgs e)
        {
            SendKeys.Send("{Tab}");
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
            pnlBtn.Parent = this;
            pnlBtn.BringToFront();
            pnlBtn.Top = Height - pnlBtn.Height - 200;
        }

        private void frmOpenTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnfrmClose();
        }

        private bool checkSelected()
        {
            string lblname = "";
            TCombo combo;
            for (int i = 0; i <= comboDish.Combodishs.Count - 1; i++)
            {
                combo = (TCombo)comboDish.Combodishs[i];
                lblname = "lbl" + combo.Columnid;
                Label lbl = (Label)Controls.Find(lblname, true)[0];
                if (lbl.BackColor == lblCombo.BackColor)
                {
                    return false;

                }
            }
            return true;
        }
        private void button27_Click(object sender, EventArgs e)
        {
            //如果还有未选，提示
            if (!checkSelected())
            {
                Warning("还有未选...");
                return;
            }
            if (!AskQuestion("确定选好了吗?"))
            {
                return;
            }
            //获取一份PAD一样的套餐存储结构
            //getcomboDishPad();
            //套餐加入已选
            FGuid = getGUID();
            AddComboToShopping();
            DialogResult = DialogResult.OK;
            Close();
        }
        private void addPot(TPotDishInfo pot)
        {
            string userid = Globals.CurrOrderInfo.userid;
            if (userid == null)
                userid = Globals.UserInfo.UserID;
            //鱼锅
            string guid = getGUID();
            pot.PotDish.Orderstatus = 6;
            pot.PotDish.PrimaryKey = getGUID();
            pot.PotDish.Orderid = Globals.CurrOrderInfo.orderid;
            pot.PotDish.Userid = userid;// Globals.UserInfo.UserID;
            pot.PotDish.Ordertime = DateTime.Now;
            pot.PotDish.Tableid = Globals.CurrTableInfo.tableNo;
            pot.PotDish.Price = 0;
            pot.PotDish.Groupid = FGuid;
            pot.PotDish.Groupid2 = guid;
            pot.PotDish.IsPot = 0;
            pot.PotDish.Primarydishtype = 2;
            pot.PotDish.Avoid = dietSetControl1.Diet;

            pot.PotInfo.Orderstatus = 2;
            pot.PotInfo.Orderid = Globals.CurrOrderInfo.orderid;
            pot.PotInfo.Userid = userid;// Globals.UserInfo.UserID;
            pot.PotInfo.PrimaryKey = getGUID();
            pot.PotInfo.Ordertime = DateTime.Now;
            pot.PotInfo.Tableid = Globals.CurrTableInfo.tableNo;
            pot.PotInfo.Price = 0;
            pot.PotInfo.Groupid = FGuid;
            pot.PotInfo.Groupid2 = guid;
            pot.PotInfo.IsPot = 1;
            pot.PotInfo.Primarydishtype = 2;
            pot.PotInfo.Avoid = dietSetControl1.Diet;

            pot.FishDishInfo1.Orderstatus = 5;
            pot.FishDishInfo1.PrimaryKey = getGUID();
            pot.FishDishInfo1.Orderid = Globals.CurrOrderInfo.orderid;
            pot.FishDishInfo1.Userid = userid;// Globals.UserInfo.UserID;
            pot.FishDishInfo1.Ordertime = DateTime.Now;
            pot.FishDishInfo1.Tableid = Globals.CurrTableInfo.tableNo;
            pot.FishDishInfo1.Price = 0;
            pot.FishDishInfo1.Groupid = FGuid;
            pot.FishDishInfo1.Groupid2 = guid;
            pot.FishDishInfo1.Primarydishtype = 2;
            pot.FishDishInfo1.Avoid = dietSetControl1.Diet;
            pot.PotDish.Ordertype = 3;
            t_shopping.add(ref Globals.ShoppTable, pot.PotDish, true);
            pot.PotInfo.Ordertype = 3;
            t_shopping.add(ref Globals.ShoppTable, pot.PotInfo, true);
            pot.FishDishInfo1.Ordertype = 3;
            t_shopping.add(ref Globals.ShoppTable, pot.FishDishInfo1, true);
            if (pot.FishDishInfo2 != null)
            {
                pot.FishDishInfo2.Orderid = Globals.CurrOrderInfo.orderid;
                pot.FishDishInfo2.PrimaryKey = getGUID();
                pot.FishDishInfo2.Userid = userid;// Globals.UserInfo.UserID;
                pot.FishDishInfo2.Ordertime = DateTime.Now;
                pot.FishDishInfo2.Tableid = Globals.CurrTableInfo.tableNo;
                pot.FishDishInfo2.Orderstatus = 5;
                pot.FishDishInfo2.Price = 0;
                pot.FishDishInfo2.Ordertype = 3;
                pot.FishDishInfo2.Groupid2 = guid;
                pot.FishDishInfo2.Groupid = FGuid;
                pot.FishDishInfo2.Primarydishtype = 2;
                pot.FishDishInfo2.Avoid = dietSetControl1.Diet;
                t_shopping.add(ref Globals.ShoppTable, pot.FishDishInfo2, true);
            }
        }
        private void addNor(t_shopping dishinfo)
        {
            string userid = Globals.CurrOrderInfo.userid;
            if (userid == null)
                userid = Globals.UserInfo.UserID;
            //普通菜
            dishinfo.Groupid = FGuid;
            dishinfo.Orderid = Globals.CurrOrderInfo.orderid;
            dishinfo.PrimaryKey = getGUID();
            dishinfo.Userid = userid;
            if (dishinfo.Orderstatus == 3)
                dishinfo.Price = 0;
            dishinfo.Tableid = Globals.CurrTableInfo.tableNo;
            dishinfo.Ordertype = 3;
            dishinfo.Primarydishtype = 2;
            dishinfo.Avoid = dietSetControl1.Diet;
            t_shopping.add(ref Globals.ShoppTable, dishinfo, true);
        }
        private void AddComboToShopping()
        {
            //加入总套餐菜品
            string userid = Globals.CurrOrderInfo.userid;
            if (userid == null)
                userid = Globals.UserInfo.UserID;
            comboDish.Dishinfo.Dishnum = 1;
            comboDish.Dishinfo.Orderstatus = 0;
            addNor(comboDish.Dishinfo);
            TComboFromControl cfc;
            //组合
            for (int i = 0; i <= combocontrols.Count - 1; i++)
            {
                cfc = (TComboFromControl)combocontrols[i];
                int num = strtointdef0(cfc.edt.Text);
                if (num > 0)
                {
                    if (cfc.ispot)
                    {
                        addPot(cfc.potdishinfo);
                    }
                    else
                    {
                        //普通菜
                        cfc.dishinfo.Orderstatus = 3;
                        cfc.dishinfo.Dishnum = num;
                        addNor(cfc.dishinfo);
                    }
                }
            }
            //必选
            for (int i = 0; i <= onlycontrols.Count - 1; i++)
            {
                cfc = (TComboFromControl)onlycontrols[i];
                if (cfc.ispot)
                {

                    addPot(cfc.potdishinfo);
                }
                else
                {
                    //普通菜
                    cfc.dishinfo.Orderstatus = 3;
                    addNor(cfc.dishinfo);
                }
            }
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

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private bool getGroupDetail()
        {
            bool ret = false;
            //JArray groupData=null;
            JObject groupData = null;
            ret = RestClient.getMenuCombodish(dishinfo.Dishid, dishinfo.Menuid, out groupData);
            if (!ret)
            {
                Warning("获取套餐信息失败!");
                return false;
            }
            comboDish = TComboDish.parse(groupData);
            ret = true;
            return ret;
        }
        private void initView()
        {
            lblDishName.Text = dishinfo.Title;
            //根据套餐内容创建控件显示 controls
            ArrayList onlydishs = comboDish.Onlydishs;
            TPotDishInfo potinfo = new TPotDishInfo();
            //组合
            ArrayList combodishs = comboDish.Combodishs;
            for (int i = 0; i <= combodishs.Count - 1; i++)
            {
                TCombo combo = (TCombo)combodishs[i];
                Label lbl = getonlyLabel(combo);
                lbl.Name = "lbl" + combo.Columnid;
                addComboControl(lbl);
                for (int j = 0; j <= combo.Dishs.Count - 1; j++)
                {
                    if (combo.Dishs[j].GetType().Name.Equals(potinfo.GetType().Name))
                    {
                        //鱼锅
                        lbl = getonlyLabel((TPotDishInfo)combo.Dishs[j]);
                        DevExpress.XtraEditors.TextEdit tbx = getComboTextBox();
                        tbx.Tag = combo;
                        tbx.Click += new EventHandler(edtNum1_Click);
                        tbx.TextChanged += new EventHandler(edtNum1_EditValueChanged);
                        tbx.Enter += new EventHandler(edtNum1_Enter);
                        addControlCombo(lbl, (TPotDishInfo)combo.Dishs[j], tbx);
                    }
                    else
                    {
                        //普通菜
                        lbl = getonlyLabel((t_shopping)combo.Dishs[j], true);
                        DevExpress.XtraEditors.TextEdit tbx = getComboTextBox();
                        tbx.Tag = combo;
                        tbx.Click += new EventHandler(edtNum1_Click);
                        tbx.TextChanged += new EventHandler(edtNum1_EditValueChanged);
                        tbx.Enter += new EventHandler(edtNum1_Enter);
                        addControlCombo(lbl, (t_shopping)combo.Dishs[j], tbx);
                    }
                }
            }
            //必选
            for (int i = 0; i <= onlydishs.Count - 1; i++)
            {
                Label lbl = lblSimple;
                if (onlydishs[i].GetType().Name.Equals(potinfo.GetType().Name))
                {
                    //鱼锅
                    lbl = getonlyLabel((TPotDishInfo)onlydishs[i]);
                    addControl(lbl, (TPotDishInfo)onlydishs[i], null);
                }
                else
                {
                    //普通菜
                    lbl = getonlyLabel((t_shopping)onlydishs[i], false);
                    addControl(lbl, (t_shopping)onlydishs[i], null);
                }
                lblLine.Top = lbl.Top + lblHeight;
            }
        }

        private void addComboControl(Label lbl)
        {
            lbl.Parent = pnlMain;
            lbl.Font = lblCombo.Font;
            lbl.ForeColor = lblCombo.ForeColor;
            lbl.BackColor = lblCombo.BackColor;
            lbl.Left = 0;
            lbl.Height = lblHeight;  // + edtHeight
            lbl.Top = onlycontrols.Count * lblHeight + combocontrols.Count * (lblHeight) + comboheight;
            lbl.Width = 528;
            comboheight = comboheight + lblHeight;
        }
        private void addControl(Label lbl, TPotDishInfo potinfo, DevExpress.XtraEditors.TextEdit tbx)
        {

            TComboFromControl cfc = new TComboFromControl();
            lbl.Parent = pnlMain;
            lbl.Left = 0;
            lbl.Top = onlycontrols.Count * lblHeight + combocontrols.Count * (lblHeight) + comboheight;// onlycontrols.Count* lblHeight;
            lbl.Height = lblHeight;
            cfc.lbl = lbl;
            lbl.Font = lblSimple.Font;
            lbl.ForeColor = lblSimple.ForeColor;
            lbl.AutoSize = false;
            lbl.Width = 528;
            cfc.ispot = true;
            cfc.potdishinfo = potinfo;
            onlycontrols.Add(cfc);
        }
        private void addControl(Label lbl, t_shopping dishinfo, DevExpress.XtraEditors.TextEdit tbx)
        {
            TComboFromControl cfc = new TComboFromControl();
            lbl.Parent = pnlMain;
            lbl.Left = 0;
            lbl.Top = onlycontrols.Count * lblHeight + combocontrols.Count * (lblHeight) + comboheight; // onlycontrols.Count* lblHeight;
            cfc.lbl = lbl;
            lbl.Font = lblSimple.Font;
            lbl.Height = lblHeight;
            lbl.ForeColor = lblSimple.ForeColor;
            lbl.AutoSize = false;
            lbl.Width = 528;
            cfc.ispot = false;
            cfc.dishinfo = dishinfo;
            onlycontrols.Add(cfc);
        }
        private void addControlCombo(Label lbl, TPotDishInfo potinfo, DevExpress.XtraEditors.TextEdit tbx)
        {

            TComboFromControl cfc = new TComboFromControl();
            lbl.Parent = pnlMain;
            lbl.Left = 18;
            lbl.Top = onlycontrols.Count * lblHeight + combocontrols.Count * (lblHeight) + comboheight;
            cfc.lbl = lbl;
            lbl.Font = lblSimple.Font;
            lbl.Height = lblHeight;
            lbl.ForeColor = lblSimple.ForeColor;
            lbl.AutoSize = false;
            lbl.Width = 528;
            cfc.ispot = true;
            cfc.potdishinfo = potinfo;
            if (tbx != null)
            {
                tbx.Parent = pnlMain;
                tbx.Font = edtNum1.Font;
                tbx.Left = 10;
                tbx.Top = lbl.Top - 5;// +lblHeight;
                tbx.Left = edtNum1.Left;
                tbx.Width = edtNum1.Width;
                cfc.edt = tbx;
            }
            combocontrols.Add(cfc);
        }
        private void addControlCombo(Label lbl, t_shopping dishinfo, DevExpress.XtraEditors.TextEdit tbx)
        {

            TComboFromControl cfc = new TComboFromControl();
            lbl.Parent = pnlMain;
            lbl.Left = 18;
            lbl.Top = onlycontrols.Count * lblHeight + combocontrols.Count * (lblHeight) + comboheight;
            cfc.lbl = lbl;
            lbl.Font = lblSimple.Font;
            lbl.Height = lblHeight;
            lbl.ForeColor = lblSimple.ForeColor;
            lbl.AutoSize = false;
            lbl.Width = 528;
            cfc.ispot = false;
            cfc.dishinfo = dishinfo;
            if (tbx != null)
            {
                tbx.Parent = pnlMain;
                tbx.Left = 10;
                tbx.Font = edtNum1.Font;
                tbx.Top = lbl.Top - 5;// +lblHeight;
                tbx.Left = edtNum1.Left;
                tbx.Width = edtNum1.Width;
                cfc.edt = tbx;
            }
            combocontrols.Add(cfc);
        }
        private Label getonlyLabel(TPotDishInfo potinfo)
        {
            Label label = new Label();
            String potStr = String.Format("{0}:{1}{2}", potinfo.PotInfo.Title, potinfo.PotInfo.Dishnum, potinfo.PotInfo.Dishunit);
            String dish1Str = String.Format(",{0}:{1}{2}", potinfo.FishDishInfo1.Title, potinfo.FishDishInfo1.Dishnum, potinfo.FishDishInfo1.Dishunit);
            String dish2Str = "";
            if (potinfo.FishDishInfo2 != null)
                dish2Str = String.Format(",{0}:{1}{2}", potinfo.FishDishInfo2.Title, potinfo.FishDishInfo2.Dishnum, potinfo.FishDishInfo2.Dishunit);
            label.Text = String.Format("{0}({1}{2}{3})", potinfo.PotDish.Title, potStr, dish1Str, dish2Str);
            return label;
        }
        private Label getonlyLabel(t_shopping shoppinginfo, bool iscombo)
        {
            Label label = new Label();
            String dishStr = String.Format("{0}({1}{2})", shoppinginfo.Title, shoppinginfo.Dishnum, shoppinginfo.Dishunit);
            if (iscombo)
                dishStr = String.Format("{0}({1})", shoppinginfo.Title, shoppinginfo.Dishunit);
            label.Text = dishStr;
            return label;
        }
        private Label getonlyLabel(TCombo combo)
        {
            Label label = new Label();
            String dishStr = String.Format("{0}({1}选{2})->已选：0                               ", combo.ItemDesc, combo.Startnum, combo.Endnum);
            label.Text = dishStr;
            label.Tag = combo;
            return label;
        }

        private DevExpress.XtraEditors.TextEdit getComboTextBox()
        {
            DevExpress.XtraEditors.TextEdit tbx = new DevExpress.XtraEditors.TextEdit();
            tbx.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            //tbx.BackColor = Color.AntiqueWhite;
            return tbx;
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

        }

        private void btnFish_Click(object sender, EventArgs e)
        {

        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void edtNum1_EditValueChanged(object sender, EventArgs e)
        {
            //
            DevExpress.XtraEditors.TextEdit edt = (DevExpress.XtraEditors.TextEdit)sender;
            TCombo combo = (TCombo)edt.Tag;
            string lblname = "lbl" + combo.Columnid;
            Label lbl = (Label)Controls.Find(lblname, true)[0];
            TCombo comb = (TCombo)edt.Tag;
            int groupnum = 0;
            for (int i = 0; i <= combocontrols.Count - 1; i++)
            {
                TComboFromControl cfc = (TComboFromControl)combocontrols[i];
                if (((TCombo)cfc.edt.Tag).Columnid == comb.Columnid)
                {
                    groupnum = groupnum + strtointdef0(cfc.edt.Text);
                }
            }
            if (groupnum > combo.Endnum)
            {
                for (int i = 0; i <= combocontrols.Count - 1; i++)
                {
                    TComboFromControl cfc = (TComboFromControl)combocontrols[i];
                    if (((TCombo)cfc.edt.Tag).Columnid == comb.Columnid)
                    {
                        cfc.edt.Text = "";
                        cfc.edt.Refresh();
                        groupnum = 0;
                    }
                }
            }
            else
                if (combo.Endnum == groupnum)
                {
                    lbl.BackColor = lblCombo2.BackColor;
                }
                else
                {
                    lbl.BackColor = lblCombo.BackColor;
                }
            String dishStr = String.Format("{0}({1}选{2})->已选：{3}                               ", combo.ItemDesc, combo.Startnum, combo.Endnum, groupnum);
            lbl.Text = dishStr;
        }

        private void edtNum1_Click(object sender, EventArgs e)
        {
            //
        }
        private void edtNum1_Enter(object sender, EventArgs e)
        {
            focusEdt = (DevExpress.XtraEditors.TextEdit)sender;
            focusEdt.SelectAll();
            setedtNum(focusEdt);
        }
        private void edtNum1_Enter_1(object sender, EventArgs e)
        {
            //

        }
        private void setedtNum(DevExpress.XtraEditors.TextEdit edt)
        {
            TCombo comb = (TCombo)edt.Tag;
            if (comb.Endnum == 1)
            {
                for (int i = 0; i <= combocontrols.Count - 1; i++)
                {
                    TComboFromControl cfc = (TComboFromControl)combocontrols[i];
                    if (((TCombo)cfc.edt.Tag).Columnid == comb.Columnid)
                        cfc.edt.Text = "";
                }

                edt.Text = "1";
                edt.SelectAll();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            pnlMain.VerticalScroll.Value = Math.Min(pnlMain.VerticalScroll.Maximum, pnlMain.VerticalScroll.Value + 100);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            pnlMain.VerticalScroll.Value = Math.Max(0, pnlMain.VerticalScroll.Value - 100);
        }
    }
    public class TComboFromControl
    {
        public Label lbl;
        public DevExpress.XtraEditors.TextEdit edt;
        public bool ispot;
        public TPotDishInfo potdishinfo;
        public t_shopping dishinfo;
    }
}