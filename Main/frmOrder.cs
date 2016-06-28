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
using System.Threading;
using Models;
using WebServiceReference;
using ReportsFastReport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using CanDao.Pos.UI.Library.View;

namespace Main
{
    public partial class frmOrder : frmBase
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hwnd);
        public delegate void ShoppingChange(object serder, EventArgs e);
        public delegate void Accounts(object serder, EventArgs e, int ordertype);
        public delegate void FrmClose(object serder, EventArgs e);
        private int downx = 0;
        private int downy = 0;
        private DataView dv = null;
        private Library.UserControls.ucDish[] btntables;
        private Library.UserControls.ucDish[] btnTypetables;

        private const int rowcount = 4;
        private int btnWidth = 126;
        private int btnHeight = 50;
        private int btnSpace = 5;
        private JArray jarrTables = null;
        private JArray jarrType = null;
        private string py2 = "";
        private int btncount = 32;//jarrTables.Count 36
        public event ShoppingChange shoppingChange;
        public event Accounts accounts;
        public event Action OrderRemarkChanged;
        private Library.UserControls.ucDish selectbtn = null;
        private bool iswm = true;
        private string menuid = "";
        private int dishcount_type = 0;//分类中的菜品数量
        private int pagecount_type = 0;//一个分类中的菜品有多少页
        private int currpage_type = 0;//当前分类第几页菜品
        private string selectSource = "";

        public frmOrder()
        {
            InitializeComponent();
        }

        public void shoppingchange()
        {
            try
            {
                showTypeNum();
            }
            catch { }
            showbtnOrderText();
        }

        private void OnShoppingChange()
        {
            try
            {
                showTypeNum();
            }
            catch { }
            //如果购物车不为空就显示为下单
            if (shoppingChange != null)
                shoppingChange(this, new EventArgs());

            showbtnOrderText();
        }

        public void showbtnOrderText()
        {
            if (!iswm)
            {
                if (Globals.ShoppTable.Rows.Count <= 0)
                {
                    btnOrder.Text = "        结帐";
                }
                else
                {
                    btnOrder.Text = "        下单";
                }
            }
            showTypeNum();
            CheckBtnRemarkOrderStatus();
        }
        private void OnAccounts(int ordertype)
        {
            if (accounts != null)
                accounts(this, new EventArgs(), ordertype);
        }
        private void frmPettyCash_Activated(object sender, EventArgs e)
        {
            edtPy.Focus();

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

        }

        private void button28_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void edtRoom_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void frmPettyCash_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                LockWindowUpdate(this.Handle);
                setBtnFocus();
                getTypeBtns();
                try
                { CreateBtnArr(); }
                catch { }
                try
                {
                    btnRefresh_Click(btnRefresh, e);
                }
                catch { }

                Left = 0;
                Top = 0;
                //CreateBtnArr();
                refreshBtn();
            }
            finally
            {
                LockWindowUpdate(IntPtr.Zero);
                this.Cursor = Cursors.Default;
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                getAllData();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            try
            {
                refreshBtn();
            }
            catch { }
            btnTypeUp.Tag = 0;
            btnTypeDown.Tag = 0;
            try
            {
                refreshTypeBtn();
            }
            catch { }
            try
            {
                showTypeNum();
            }
            catch { }
            try
            {
                showTypeNum();
            }
            catch { }
            try
            {
                btnType1_Click(btnType1.lbl2, e);
            }
            catch { }

        }
        public void getAllData()
        {
            //
            JArray jrOrder = null;
            try
            {
                if (!RestClient.getAllWmFood(Globals.UserInfo.UserName, out jrOrder))
                {
                    return;
                }
            }
            catch { }
            jarrTables = jrOrder;
            JArray jrtype = null;
            try
            {
                if (!RestClient.getFoodType(out jrtype))
                {
                    return;
                }
            }
            catch { }
            //倒序jrtype
            jarrType = jrtype;// new JArray();
            /*for (int i = jrtype.Count - 1; i >= 0;i-- )
            {
               jarrType.Add(jrtype[i]);
            }*/
            //DataTable dtOrder = null;
            //dtOrder = Models.Bill_Order.getFood_List(jrOrder);
            //dv = new DataView(dtOrder);
            //dv.AllowNew = false;
            //this.dgvBill.AutoGenerateColumns = false;
            //this.dgvBill.DataSource = dv;
        }

        private void dgvBill_MouseDown(object sender, MouseEventArgs e)
        {
            downx = e.X;
            downy = e.Y;
        }

        private void dgvBill_MouseUp(object sender, MouseEventArgs e)
        {
            int movex = e.X - downx;
            int movey = e.Y - downy;
            if (movey > 10)
            {
                pageUp();
            }
            if (movey < -10)
            {
                pageDown();
            }
        }
        private void pageUp()
        {
            //dgvBill.Select();
            //SendKeys.Send("{PGDN}");
            SendKeys.Send("{PGUP}");
        }

        private void pageDown()
        {
            //dgvBill.Select();
            SendKeys.Send("{PGDN}");
            //SendKeys.Send("{PGUP}");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            edtPy.Focus();
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            edtPy.Text = "";
            edtPy.Focus();
        }
        private void setBtnFocus()
        {
            Common.Globals.SetButton(button12);
            Common.Globals.SetButton(button11);
            Common.Globals.SetButton(button10);
            Common.Globals.SetButton(button9);
            Common.Globals.SetButton(button8);
            Common.Globals.SetButton(button7);
            Common.Globals.SetButton(button6);
            Common.Globals.SetButton(button5);
            Common.Globals.SetButton(button4);
            Common.Globals.SetButton(button3);
            Common.Globals.SetButton(button2);
            Common.Globals.SetButton(button13);
            Common.Globals.SetButton(button14);
            Common.Globals.SetButton(button15);
            Common.Globals.SetButton(button16);
            Common.Globals.SetButton(button17);
            Common.Globals.SetButton(button18);
            Common.Globals.SetButton(button19);
            Common.Globals.SetButton(button20);
            Common.Globals.SetButton(button22);
            Common.Globals.SetButton(button23);
            Common.Globals.SetButton(button24);
            Common.Globals.SetButton(button25);
            Common.Globals.SetButton(button26);
            Common.Globals.SetButton(btnOrder);
            Common.Globals.SetButton(button28);
            Common.Globals.SetButton(button29);
            Common.Globals.SetButton(button30);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ///
            if (Globals.ShoppTable.Rows.Count <= 0)
                return;
            if (!AskQuestion("确定要清空已选吗?"))
            {
                return;
            }
            Globals.ShoppTable.Clear();
            showTypeNum();
            showbtnOrderText();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (btnGd.Visible)
            {
                if (Globals.ShoppTable.Rows.Count <= 0)
                {
                    Warning("请先选择菜品...");
                    return;
                }
            }
            else
            {
                if (Globals.ShoppTable.Rows.Count > 0)
                {
                    if (!AskQuestion(Globals.CurrTableInfo.tableName + "确定下单吗?"))
                    {
                        return;
                    }
                }
            }
            ///转到结帐页 //如果是堂食，就是下单
            OnAccounts(0);
        }

        private void edtTableNo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (edtPy.Text.Length <= 0)
                {
                    dv.RowFilter = "";
                }
                else
                {
                    dv.RowFilter = "py like '%" + edtPy.Text + "%'";
                }

            }
            catch
            { }
        }
        private void getTypeBtns()
        {
            btnTypetables = new Library.UserControls.ucDish[10];
            btnTypetables[0] = btnType1;
            btnTypetables[1] = btnType2;
            btnTypetables[2] = btnType3;
            btnTypetables[3] = btnType4;
            btnTypetables[4] = btnType5;
            btnTypetables[5] = btnType6;
            btnTypetables[6] = btnType7;
            btnTypetables[7] = btnType8;
            btnTypetables[8] = btnType9;
            btnTypetables[9] = btnType10;
            for (int i = 0; i <= btnTypetables.Length - 1; i++)
            {
                btnTypetables[i].lbl2.Text = "";

                btnTypetables[i].lbl2.TextAlign = ContentAlignment.BottomLeft;
                btnTypetables[i].lblNo.TextAlign = ContentAlignment.BottomLeft;
                btnTypetables[i].lblNo.Text = "";
                btnTypetables[i].Enabled = false;
                btnTypetables[i].lblNo.Click += new EventHandler(btnType1_Click);
                btnTypetables[i].lbl2.Click += new EventHandler(btnType1_Click);
                btnTypetables[i].lblNo.MouseDown += ucTable1_MouseDown;
                btnTypetables[i].lblNo.MouseUp += ucTable1_MouseUp;
                btnTypetables[i].lbl2.MouseDown += ucTable1_MouseDown;
                btnTypetables[i].lbl2.MouseUp += ucTable1_MouseUp;
                btnTypetables[i].lblNo.MouseLeave += ucTable1_MouseLeave;
                btnTypetables[i].lbl2.MouseLeave += ucTable1_MouseLeave;
                btnTypetables[i].lblNo.Font = new System.Drawing.Font("Tahoma", 13F);
                btnTypetables[i].lbl2.Font = new System.Drawing.Font("Tahoma", 10F);
                btnTypetables[i].lbl2.ForeColor = Color.Black;
                btnTypetables[i].lblNo.TextAlign = ContentAlignment.TopLeft;
                btnTypetables[i].lblNo.Height = 20;
                btnTypetables[i].lbl2.TextAlign = ContentAlignment.TopRight;

            }

        }

        private void refreshTypeBtn()
        {
            JObject ja = null;
            int j = 0;
            string itemdesc = "";
            string itemsort = "";
            string itemid = "";
            string isShow = "";
            selectbtn = null;
            setSelectTypeColor();
            if (jarrType.Count <= 10)
            {
                btnTypeUp.Enabled = false;
                btnTypeDown.Enabled = false;
            }
            else
            {
                btnTypeUp.Enabled = true;
                btnTypeDown.Enabled = true;
            }
            for (int i = 0; i <= btnTypetables.Length - 1; i++)
            {
                btnTypetables[i].Tag = -1;
                btnTypetables[i].lblNo.Text = "";
                btnTypetables[i].lbl2.Text = "";
                btnTypetables[i].lblNo.Tag = null;
                btnTypetables[i].Enabled = false;
            }
            if (jarrType.Count <= 0)
                return;
            int pagecount = jarrType.Count / 10;
            if (jarrType.Count % 10 > 0)
            { pagecount = pagecount + 1; }
            int currpage = int.Parse(btnTypeUp.Tag.ToString());
            if (currpage > pagecount)
                currpage = pagecount;
            btnTypeDown.Tag = pagecount;
            int ivalue = (currpage) * 10;
            if (jarrType != null)
            {
                for (int i = ivalue; i <= jarrType.Count - 1; i++)
                {
                    ja = (JObject)jarrType[i];
                    itemdesc = ja["itemdesc"].ToString();
                    itemsort = ja["itemsort"].ToString();
                    itemid = ja["itemid"].ToString();
                    isShow = ja["isShow"].ToString();
                    btnTypetables[j].Visible = true;
                    btnTypetables[j].lblNo.Text = InternationaHelper.GetBeforeSeparatorFlagData(itemdesc);
                    btnTypetables[j].lblNo.Tag = ja;
                    btnTypetables[j].lbl2.Tag = ja;
                    btnTypetables[j].Tag = i;
                    btnTypetables[j].Enabled = true;
                    j++;
                    if (j >= btnTypetables.Length)
                    {
                        break;
                    }
                }
            }
            showTypeNum();
        }
        private void setTypePage()
        {
            JObject ja = null;
            String py = edtPy.Text.Trim();
            string dishpy = "";
            if (jarrTables != null)
            {
                for (int i = 0; i <= jarrTables.Count - 1; i++)
                {
                    ja = (JObject)jarrTables[i];
                    if (py.Length > 0)
                    {
                        //有拼音过滤条件
                        dishpy = ja["py"].ToString();
                        if (dishpy.IndexOf(py) < 0)
                        {
                            continue;
                        }
                    }
                    //分类条件过滤 source
                    if (!selectSource.Equals(""))//selectbtn != null)
                    {
                        //string selectSource = ((JObject)selectbtn.lbl2.Tag)["itemid"].ToString();
                        if (!selectSource.Equals(ja["source"].ToString()))
                        {
                            continue;
                        }
                    }
                    dishcount_type = dishcount_type + 1;
                }
            }
            btnDishPageDown.Enabled = false;
            btnDishPageUp.Enabled = false;
            pagecount_type = 0;
            currpage_type = 0;
            if (dishcount_type > 0)
            {
                currpage_type = 1;
                pagecount_type = dishcount_type / btncount;
                if (dishcount_type % btncount > 0)
                { pagecount_type = pagecount_type + 1; }
            }
            if (pagecount_type > 1)
            {
                btnDishPageDown.Enabled = true;
                btnDishPageUp.Enabled = true;
            }
        }
        private void refreshBtn()
        {
            JObject ja = null;
            string tableid = "";
            string tableName = "";
            string tableNo = "";
            string tabletype = "";
            int orderstatus = 0;
            string dishpy = "";
            string py = edtPy.Text;
            int weigh = 0;
            int j = 0;
            for (int i = 0; i <= btncount - 1; i++)
            {
                btntables[i].Tag = -1;
            }
            //用两次循环实现，第一次填充，第二次隐藏  加入分页功能
            if (jarrTables != null)
            {
                int i = 0;                //int tmpi = (currpage_type - 1) * btncount;
                int tmpi = (currpage_type - 1) * btncount;
                int typecount = 0;
                for (i = 0; i <= jarrTables.Count - 1; i++)
                {
                    ja = (JObject)jarrTables[i];
                    if (py.Length > 0)
                    {
                        //有拼音过滤条件
                        dishpy = ja["py"].ToString();
                        if (dishpy.IndexOf(py) < 0)
                        {
                            continue;
                        }
                    }
                    //分类条件过滤 source
                    if (!selectSource.Equals(""))//selectbtn != null)
                    {
                        //string selectSource = ((JObject)selectbtn.lbl2.Tag)["itemid"].ToString();
                        if (!selectSource.Equals(ja["source"].ToString()))
                        {
                            continue;
                        }
                    }
                    //分页条件
                    typecount = typecount + 1;
                    if (typecount <= tmpi)
                        continue;
                    btntables[j].Visible = true;
                    tableid = ja["dishid"].ToString();
                    tableName = ja["title"].ToString();
                    tableNo = ja["title"].ToString();
                    tableNo = InternationaHelper.GetBeforeSeparatorFlagData(tableNo);
                    tabletype = ja["dishtype"].ToString();
                    weigh = int.Parse(ja["weigh"].ToString());
                    orderstatus = 0;
                    btntables[j].lblNo.Text = tableNo;
                    if (Globals.CurrOrderInfo.memberno == null)
                        Globals.CurrOrderInfo.memberno = "";
                    bool ismember = Globals.CurrOrderInfo.memberno.Length > 0;
                    string price = "";
                    if (ismember)
                    {
                        price = ja["vipprice"].ToString(); //可能还会有多单位的问题
                        if (price.Equals("") || price.Equals(""))
                        {
                            price = ja["price"].ToString();
                        }
                    }
                    else
                    {
                        price = ja["price"].ToString();
                    }
                    if (tabletype.Equals("1"))
                        btntables[j].lbl2.Text = "";
                    else
                        btntables[j].lbl2.Text = string.Format("{0}/{1}", price, InternationaHelper.GetBeforeSeparatorFlagData(ja["unit"].ToString()));

                    if (weigh == 1)
                        btntables[j].lbl2.Text += "  称重";

                    btntables[j].status = orderstatus;
                    btntables[j].lblNo.Tag = ja;
                    btntables[j].lbl2.Tag = ja;
                    btntables[j].Tag = i;
                    j++;
                    if (j >= btncount)
                    {
                        break;
                    }
                }
            }
            if (ja != null)
                menuid = ja["menuid"].ToString();
            for (int i = 0; i <= btncount - 1; i++)
            {
                int btntag = int.Parse(btntables[i].Tag.ToString());
                if (btntag < 0)
                {
                    btntables[i].Visible = false;
                }
            }
            if (j <= 0)
            {
                if (edtPy.Text.Length > 0)
                {
                    py2 = edtPy.Text.Substring(0, edtPy.Text.Length - 1);
                    tmrClear.Enabled = true;
                }
            }
        }
        private void CreateBtnArr()
        {
            btntables = new Library.UserControls.ucDish[btncount];
            JObject ja = null;
            string tableid = "";
            string tableName = "";
            string tableNo = "";
            string tabletype = "";
            int orderstatus = 0;
            int btnleft = 0;
            int btntop = 0;
            int rowindex = 0;
            int colindex = 0;
            int personnum = 0;
            for (int i = 0; i <= btntables.Length - 1; i++)
            {
                btntables[i] = new Library.UserControls.ucDish();
                btntables[i].lblNo.Click += new EventHandler(ucTable1_Click);
                btntables[i].lbl2.Click += new EventHandler(ucTable1_Click);
                //btntables[i].MouseDown += ucTable1_MouseDown;
                //btntables[i].MouseUp += ucTable1_MouseDown;
                btntables[i].lblNo.MouseDown += ucTable1_MouseDown;
                btntables[i].lblNo.MouseUp += ucTable1_MouseUp;
                btntables[i].lbl2.MouseDown += ucTable1_MouseDown;
                btntables[i].lbl2.MouseUp += ucTable1_MouseUp;
                btntables[i].lblNo.MouseLeave += ucTable1_MouseLeave;
                btntables[i].lbl2.MouseLeave += ucTable1_MouseLeave;


                /*ja = (JObject)jarrTables[i];
                tableid = ja["dishid"].ToString();
                tableName = ja["title"].ToString();
                tableNo = ja["title"].ToString();
                tabletype = ja["dishtype"].ToString();
                orderstatus = 0;// int.Parse(ja["status"].ToString());
                btntables[i].lblNo.Text = tableNo;*/
                btntables[i].lblNo.Font = new System.Drawing.Font("Tahoma", 12F);
                btntables[i].lbl2.Font = ucTable1.lbl2.Font;//; btntables[i].lblNo.Font;
                btntables[i].lbl2.ForeColor = Color.Black;
                btntables[i].lblNo.ForeColor = Color.White;
                btntables[i].BorderStyle = BorderStyle.FixedSingle;
                /*btntables[i].lbl2.Text = string.Format("{0}", 20.00);
                btntables[i].status = orderstatus;
                btntables[i].lblNo.Tag = btntables[i];
                btntables[i].lbl2.Tag = btntables[i];*/
                //setbtnColor(btntables[i], orderstatus);
                btntables[i].BackColor = ucTable1.BackColor;//Color.FromArgb(241, 73, 91);// Color.Tomato;// LightSalmon;
                //位置
                btntables[i].Parent = pnlFood;
                btntables[i].Width = btnWidth;
                btntables[i].Height = btnHeight;
                colindex = (i % rowcount);
                btnleft = colindex * btnWidth + ucTable1.Left + (colindex * btnSpace);
                btntables[i].Left = btnleft;
                rowindex = i / rowcount;
                btntop = btnHeight * rowindex + ucTable1.Top + (rowindex * btnSpace);
                btntables[i].Top = btntop;
            }
        }

        private void edtPy_EditValueChanged(object sender, EventArgs e)
        {
            ///
            refreshBtn();
        }

        private void button17_Click_2(object sender, EventArgs e)
        {
            edtPy.Text = "";
        }

        private void tmrClear_Tick(object sender, EventArgs e)
        {
            ///
            tmrClear.Enabled = false;
            edtPy.Text = py2;
            edtPy.SelectionStart = edtPy.Text.Length;
        }

        private void ucTable1_Load(object sender, EventArgs e)
        {
            ((Library.UserControls.ucDish)sender).lblNo.Click += new EventHandler(ucTable1_Click);
            ((Library.UserControls.ucDish)sender).lbl2.Click += new EventHandler(ucTable1_Click);
        }
        private void addDish(JObject ja)
        {
            //增加菜品到购物车
            if (ja == null)
                return;
            //如果是多单位的要选择单位
            string userid = Globals.CurrOrderInfo.userid ?? Globals.UserInfo.UserID;

            t_shopping dishinfo = new t_shopping();
            dishinfo.Orderid = Globals.CurrOrderInfo.orderid;
            dishinfo.PrimaryKey = Guid.NewGuid().ToString();
            dishinfo.Userid = userid;// Globals.UserInfo.UserID;
            dishinfo.Ordertime = DateTime.Now;
            dishinfo.Orderstatus = 0;
            dishinfo.Dishnum = 1;
            dishinfo.Tableid = Globals.CurrTableInfo.tableNo;
            dishinfo.Dishid = ja["dishid"].ToString();
            dishinfo.Avoid = "";
            dishinfo.Dishidleft = 1;
            dishinfo.Title = InternationaHelper.GetBeforeSeparatorFlagData(ja["title"].ToString());
            dishinfo.DishName = InternationaHelper.GetBeforeSeparatorFlagData(ja["title"].ToString());
            dishinfo.DishType = ja["dishtype"].ToString();
            dishinfo.DishUnitSrc = ja["unit"].ToString();
            dishinfo.Weigh = int.Parse(ja["weigh"].ToString());
            dishinfo.Taste = ja["imagetitle"].ToString();
            dishinfo.Memberprice = 0;
            dishinfo.Level = "0";
            try
            {
                dishinfo.Level = ja["level"].ToString();
            }
            catch { }
            if (Globals.CurrOrderInfo.memberno == null)
                Globals.CurrOrderInfo.memberno = "";
            bool ismember = Globals.CurrOrderInfo.memberno.Length > 0;
            decimal price = 0;
            string pricestr = "";
            pricestr = ja["vipprice"].ToString(); //可能还会有多单位的问题
            if (pricestr.Equals("") || pricestr.Equals(""))
            {
                pricestr = ja["price"].ToString();
            }
            dishinfo.Memberprice = strtofloat(pricestr);
            pricestr = ja["price"].ToString();
            dishinfo.Price2 = strtofloat(pricestr);
            if (dishinfo.Memberprice <= 0)
            {
                dishinfo.Memberprice = dishinfo.Price2;
            }
            if (ismember)
            {
                price = dishinfo.Memberprice;
            }
            else
            {
                price = dishinfo.Price2;
            }
            dishinfo.Price = price;

            dishinfo.Amount = 0;
            dishinfo.Source = ja["source"].ToString();
            int dishStatus = RestClient.getFoodStatus(dishinfo.Dishid, dishinfo.Dishunit);
            if (dishStatus == 1)
            {
                Warning("选择的菜品已沽清！");
                return;
            }
            //
            if (dishinfo.DishType.Equals("2"))
            {
                //套餐getMenuCombodish
                dishinfo.Menuid = menuid;
                TComboDish comboDish = null;
                if (frmCombodish.ShowCombodish(dishinfo, comboDish))//, out potDishInfo
                {

                }
            }
            else
                if (dishinfo.DishType.Equals("1")) ////如果是鱼锅 
                {
                    TPotDishInfo potDishInfo;
                    //如果level=1是新双拼类型鱼锅
                    if (dishinfo.Level.Equals("1"))
                    {
                        //显示新双拼选择界面

                        return;
                    }
                    if (frmFish.ShowFish(dishinfo, out potDishInfo))
                    {
                        potDishInfo.PotInfo.Orderid = Globals.CurrOrderInfo.orderid;
                        potDishInfo.PotInfo.PrimaryKey = Guid.NewGuid().ToString();
                        potDishInfo.PotInfo.Userid = userid;// Globals.UserInfo.UserID;
                        potDishInfo.PotInfo.Ordertime = DateTime.Now;
                        potDishInfo.PotInfo.Orderstatus = 0;
                        potDishInfo.PotInfo.Tableid = Globals.CurrTableInfo.tableNo;
                        potDishInfo.PotInfo.Primarydishtype = 1;
                        potDishInfo.FishDishInfo1.Orderid = Globals.CurrOrderInfo.orderid;
                        potDishInfo.FishDishInfo1.PrimaryKey = Guid.NewGuid().ToString();
                        potDishInfo.FishDishInfo1.Userid = userid;// Globals.UserInfo.UserID;
                        potDishInfo.FishDishInfo1.Ordertime = DateTime.Now;
                        potDishInfo.FishDishInfo1.Orderstatus = 0;
                        potDishInfo.FishDishInfo1.Tableid = Globals.CurrTableInfo.tableNo;
                        potDishInfo.FishDishInfo1.Primarydishtype = 1;
                        dishinfo.Dishnum = 1;
                        dishinfo.Parentdishid = potDishInfo.PotInfo.Parentdishid;
                        dishinfo.Groupid = potDishInfo.PotInfo.Groupid;
                        dishinfo.DishUnitSrc = "份";
                        dishinfo.Orderstatus = 0;
                        potDishInfo.PotInfo.Orderstatus = 2;
                        potDishInfo.FishDishInfo1.Orderstatus = 3;
                        dishinfo.Ordertype = 1;
                        dishinfo.Primarydishtype = 1;
                        t_shopping.add(ref Globals.ShoppTable, dishinfo, true);
                        potDishInfo.PotInfo.Ordertype = 1;
                        t_shopping.add(ref Globals.ShoppTable, potDishInfo.PotInfo, true);
                        potDishInfo.FishDishInfo1.Ordertype = 1;
                        t_shopping.add(ref Globals.ShoppTable, potDishInfo.FishDishInfo1, true);
                        if (potDishInfo.FishDishInfo2 != null)
                        {
                            potDishInfo.FishDishInfo2.Orderid = Globals.CurrOrderInfo.orderid;
                            potDishInfo.FishDishInfo2.PrimaryKey = Guid.NewGuid().ToString();
                            potDishInfo.FishDishInfo2.Userid = userid;// Globals.UserInfo.UserID;
                            potDishInfo.FishDishInfo2.Ordertime = DateTime.Now;
                            potDishInfo.FishDishInfo2.Tableid = Globals.CurrTableInfo.tableNo;
                            potDishInfo.FishDishInfo2.Orderstatus = 3;
                            potDishInfo.FishDishInfo2.Ordertype = 1;
                            potDishInfo.FishDishInfo2.Primarydishtype = 1;
                            t_shopping.add(ref Globals.ShoppTable, potDishInfo.FishDishInfo2, true);
                        }
                    }
                }
                else//单品
                {
                    var tasteString = ja["imagetitle"].ToString();//口味
                    if (!string.IsNullOrEmpty(tasteString))
                    {
                        var list = tasteString.Split(',').ToList();
                        var wnd = new SetDishTasteAndDietWindow(list, GenerateDishSimpleInfo(dishinfo));
                        if (wnd.ShowDialog() == true)
                        {
                            dishinfo.SelectedTaste = wnd.Taste;
                            dishinfo.Avoid = wnd.Diet;
                            dishinfo.Dishnum = wnd.DishNum;
                            dishinfo.Ordertype = 0;
                            dishinfo.Primarydishtype = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                    t_shopping.add(ref Globals.ShoppTable, dishinfo, false);
                }
            //通知调用页更新页面
            OnShoppingChange();

        }

        private DishSimpleInfo GenerateDishSimpleInfo(t_shopping dishInfo)
        {
            return new DishSimpleInfo
            {
                DishName = dishInfo.DishName,
                DishPrice = dishInfo.Price,
                DishUnit = dishInfo.Dishunit,
                DishNum = 1,
            };
        }

        private void ucTable1_Click(object sender, EventArgs e)
        {
            JObject ja = null;
            try
            {
                ja = (JObject)((Label)sender).Tag;
            }
            catch
            {
                return;
            }

            addDish(ja);
        }

        public static decimal strtofloat(string str)
        {
            decimal re = 0;
            try
            {
                re = decimal.Parse(str);
            }
            catch { re = 0; }
            return re;
        }

        private void ucTable1_MouseDown(object sender, MouseEventArgs e)
        {
            ((Library.UserControls.ucDish)((Label)sender).Parent).BorderStyle = BorderStyle.Fixed3D;
        }

        private void ucTable1_MouseUp(object sender, MouseEventArgs e)
        {
            ((Library.UserControls.ucDish)((Label)sender).Parent).BorderStyle = BorderStyle.FixedSingle;
        }

        private void ucTable1_MouseLeave(object sender, EventArgs e)
        {
            ((Library.UserControls.ucDish)((Label)sender).Parent).BorderStyle = BorderStyle.FixedSingle;
        }

        private void btnGd_Click(object sender, EventArgs e)
        {
            if (Globals.ShoppTable.Rows.Count <= 0)
                return;
            TGzInfo gzinfo = new TGzInfo();
            if (!frmWMInfo.ShowWMInfo(out gzinfo))
            {
                return;

                //if (!AskQuestion("确定要挂帐吗?"))
                //{
                //    return;
                // }
            }
            //挂帐 开台/下单/（关台，不结账）
            setOrder(gzinfo);
            //保存挂帐人信息到数据库
            string settleorderorderid = Globals.CurrOrderInfo.orderid;
        }
        private void setOrder(TGzInfo gzinfo)
        {
            //getTakeOutTable
            string orderid = "";
            if (!RestClient.setorder(Globals.CurrTableInfo.tableNo, Globals.UserInfo.UserID, ref orderid))
            {
                Warning("开外卖台失败！");
                return;
            }
            try
            {
                Thread.Sleep(1000);
            }
            catch { }
            Globals.CurrOrderInfo.orderid = orderid;
            Globals.CurrTableInfo.tableid = RestClient.getTakeOutTableID();
            if (!bookOrder())
            {
                return;
            }
            RestClient.putOrder(Globals.CurrTableInfo.tableNo, orderid, gzinfo);
            Globals.ShoppTable.Clear();
            try
            {
                RestClient.caleTableAmount(Globals.UserInfo.UserID, orderid);
            }
            catch { }
            Warning("挂单成功,单号：" + orderid);
            //挂完单，把台关掉，再清掉购物车开新单
            //
        }

        private bool bookOrder()
        {
            //下单
            var result = bookorder();
            if (!string.IsNullOrEmpty(result))
            {
                RestClient.cancelOrder(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, Globals.CurrTableInfo.tableNo);
                Warning(result);
                return false;
            }
            return true;
        }

        private string bookorder()
        {
            //下单
            //public static String bookorder = HTTP + URL_HOST + "/newspicyway/padinterface/bookorder.json";
            return RestClient.bookorder(Globals.ShoppTable, Globals.CurrTableInfo.tableNo, Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, 1, 0, Globals.OrderRemark);
        }

        private void btnType1_Load(object sender, EventArgs e)
        {

        }
        private void btnType1_Click(object sender, EventArgs e)
        {
            selectbtn = (Library.UserControls.ucDish)((Label)sender).Parent;
            selectSource = ((JObject)selectbtn.lbl2.Tag)["itemid"].ToString();
            setSelectTypeColor();
            pagecount_type = 0;
            dishcount_type = 0;
            currpage_type = 0;
            setTypePage();
            refreshBtn();
        }

        private void btnTypeUp_Click(object sender, EventArgs e)
        {
            int currpage = int.Parse(btnTypeUp.Tag.ToString());
            if (currpage <= 0)
                return;
            currpage = currpage - 1;
            btnTypeUp.Tag = currpage;
            refreshTypeBtn();
            //btnDishPageUp.Enabled = false;
            //btnDishPageDown.Enabled = false;
        }

        private void btnTypeDown_Click(object sender, EventArgs e)
        {
            int currpage = int.Parse(btnTypeUp.Tag.ToString());
            int pagecount = int.Parse(btnTypeDown.Tag.ToString());
            if (currpage >= (pagecount - 1))
                return;
            currpage = currpage + 1;
            btnTypeUp.Tag = currpage;
            refreshTypeBtn();
            //btnDishPageUp.Enabled = false;
            //btnDishPageDown.Enabled = false;
        }
        public void hideGz()
        {
            ///堂食开台下单，如果有已点把按钮显示为下单
            iswm = false;
            btnGd.Visible = false;
        }
        private void setSelectTypeColor()
        {

            for (int i = 0; i <= btnTypetables.Length - 1; i++)
            {
                btnTypetables[i].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(150)))), ((int)(((byte)(128)))));
                btnTypetables[i].ForeColor = Color.Black;
                if (btnTypetables[i].Equals(selectbtn))
                {
                    btnTypetables[i].BackColor = Color.Blue;
                    btnTypetables[i].ForeColor = Color.White;
                }
            }
        }
        /// <summary>
        /// 根据已点统计每个分类的点菜数量
        /// </summary>
        public void showTypeNum()
        {
            if (btnTypetables == null)
                return;
            try
            {
                for (int i = 0; i <= btnTypetables.Length - 1; i++)
                {
                    btnTypetables[i].lbl2.Text = "";
                }
            }
            catch { }
            try
            {
                //Globals.ShoppTable.Rows.Count
                JObject ja = null;
                for (int i = 0; i <= jarrType.Count - 1; i++)
                {
                    ja = (JObject)jarrType[i];
                    ja["itemsort"] = "0";
                }
                foreach (DataRow dr in Globals.ShoppTable.Rows)
                {
                    string source = dr["source"].ToString();
                    for (int i = 0; i <= jarrType.Count - 1; i++)
                    {
                        ja = (JObject)jarrType[i];
                        if (ja["itemid"].ToString().Equals(source))
                        {
                            //鱼锅只算一个
                            string Orderstatus = dr["Orderstatus"].ToString();
                            if (Orderstatus.Equals("0"))
                            {
                                float dishnum = float.Parse(ja["itemsort"].ToString());
                                dishnum = dishnum + float.Parse(dr["dishnum"].ToString());
                                ja["itemsort"] = dishnum.ToString();
                            }
                        }
                    }
                }
            }
            catch { }

            for (int i = 0; i <= btnTypetables.Length - 1; i++)
            {
                //把分类中的数量显示到按钮上
                if (btnTypetables[i].lblNo.Tag != null)
                {
                    string itemid = ((JObject)btnTypetables[i].lblNo.Tag)["itemid"].ToString();
                    for (int j = 0; j <= jarrType.Count - 1; j++)
                    {
                        JObject ja = (JObject)jarrType[j];
                        if (itemid.Equals(ja["itemid"].ToString()))
                        {
                            if (!ja["itemsort"].ToString().Equals("0"))
                            {
                                btnTypetables[i].lbl2.Text = ja["itemsort"].ToString();
                                break;
                            }
                            else
                                btnTypetables[i].lbl2.Text = "";
                        }
                        else
                            btnTypetables[i].lbl2.Text = "";
                    }
                }
            }
        }

        private void btnZD_Click(object sender, EventArgs e)
        {
            //赠菜权限 用的是退菜权限
            if (Globals.ShoppTable.Rows.Count <= 0)
            {
                Warning("请先选择菜品...");
                return;
            }

            var wnd = new DishGiftReasonSelectWindow();
            if (wnd.ShowDialog() == true)
            {
                Globals.DishGiftReason = wnd.SelectedReason;
                string zdUserId = "";                                                   //030102
                if (!frmAuthorize.ShowAuthorize("赠送权限验证", Globals.UserInfo.UserID, "030207", out zdUserId))
                {
                    return;
                }
                OnAccounts(1);
            }
        }

        private void btnDishPageUp_Click(object sender, EventArgs e)
        {
            if (currpage_type > 1)
            {
                currpage_type = currpage_type - 1;
                refreshBtn();
            }
        }

        private void btnDishPageDown_Click(object sender, EventArgs e)
        {
            if (currpage_type < pagecount_type)
            {
                currpage_type = currpage_type + 1;
                refreshBtn();
            }
        }

        /// <summary>
        /// 点击全单备注按钮时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRemarkOrder_Click(object sender, EventArgs e)
        {
            var wnd = new RemarkOrderWindow();
            if (wnd.ShowDialog() == true)
                OnOrderRemarkChanged(wnd.Diet);
        }

        /// <summary>
        /// 检测设置全单备注按钮的状态。
        /// </summary>
        private void CheckBtnRemarkOrderStatus()
        {
            if (Globals.ShoppTable.Rows.Count > 0)
                OnOrderRemarkChanged("");
            BtnRemarkOrder.Enabled = Globals.ShoppTable.Rows.Count > 0;
        }

        /// <summary>
        /// 触发全单备注改变事件。
        /// </summary>
        private void OnOrderRemarkChanged(string orderRemark)
        {
            Globals.OrderRemark = orderRemark;
            if (OrderRemarkChanged != null)
                OrderRemarkChanged();
        }
    }
}