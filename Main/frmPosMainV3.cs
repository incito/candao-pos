using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using Library;
using Models;
using Business;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.VIPManage.ViewModels;
using WebServiceReference;
using ReportsFastReport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Models.CandaoMember;
using Models.Enum;
using WebServiceReference.ServiceImpl;
using CanDaoCD.Pos.PrintManage;
using Timer = System.Timers.Timer;
using System.Text.RegularExpressions;
using CanDao.Pos.UI.Library.View;
using KYPOS.Dishes;

namespace Main
{
    public partial class frmPosMainV3 : frmBase
    {
        private DevExpress.XtraEditors.TextEdit focusedt;
        private float payamount = 0;//应付
        private float getamount = 0;//实收
        private float getamountsy = 0;
        private float amountrmb = 0;//实收rmb
        private float amountyhk = 0;//实收yhk
        private float amounthyk = 0;//实收hyk
        private float amountzfb = 0;//实收支付宝
        private float amountwx = 0;//实收微信
        /// <summary>
        /// 小费。
        /// </summary>
        private float amountTip = 0;//小费

        private float returnamount = 0;//返还
        private float amountgz = 0;//挂帐
        private float amountml = 0;//抹零
        private float amountroundtz = 0;//四舍五入调整
        private string currtableno = "";
        private float ysamount = 0;//应收

        private float amountjf = 0;//会员积分 //如果使用积分 不能找零

        //优惠的挂帐和优免
        private float amountgz2 = 0;//优惠的挂帐
        private float amountym = 0;//优免

        private float decgz2 = 0;//挂帐负数
        private float decym = 0;//优免负数
        private bool maling = true;//抹零（一万只草泥马洪流滚过，什么命名）

        private string membercard = "";
        private float psStoredCardsBalance = 0;
        private float psIntegralOverall = 0;//积分余额
        private JArray jarrTables = null;
        public pszTicket[] pszTicketList;

        private int btncount = 18;
        private int pagecurr = 1;
        private int pagecount = 0;
        private int lastpagecount = 0;
        private float memberyhqamount = 0;//

        //触控
        private int downx = 0;
        private int downy = 0;
        private int movex = 0;
        private int movey = 0;
        private bool isrefresh = false;
        private string cardno = "";
        //优惠列表用本地table
        private DataTable tbyh = new DataTable("yh");

        private bool isaddmember = false;
        private bool isaddFavorale = false;
        private bool isreback = false;

        private bool iswm = false;
        private frmOrder frmorder = null;
        private frmOpenTable frmopentable = null;

        private bool isopened = false;
        private string msgorderid = "";
        private bool isopentable2 = false;

        /// <summary>
        /// 定时检查订单是否有更新
        /// </summary>
        private DishesTimer _dishesTimer;

        /// <summary>
        /// 长按计时器。
        /// </summary>
        private Timer _longPressTimer;

        /// <summary>
        /// 是否是长按模式。
        /// </summary>
        private bool _isLongPressModel;

        /// <summary>
        /// 当前选择的优惠。
        /// </summary>
        private VCouponRule _curCoupon;

        /// <summary>
        /// 是否是现金控件更改金额。为true时不自动更改金额。
        /// </summary>
        private bool _isCashTextBoxChanged;

        //记录每张台下单序列的INI文件
        public frmPosMainV3()
        {
            InitializeComponent();

            _longPressTimer = new Timer(1000);
            _longPressTimer.Elapsed += PressTimer_Elapsed;
        }

        /// <summary>
        /// 长按定时器触发时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PressTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Invoke((Action)delegate
            {
                _longPressTimer.Stop();
                _isLongPressModel = true;

                var msg = "";
                if (_curCoupon.IsUncommonlyUsed)
                    msg = string.Format("恢复\"{0}\"为常用优惠{1}（恢复后可在对应分类查看、使用）", _curCoupon.couponname, Environment.NewLine);
                else
                    msg = string.Format("设置\"{0}\"为不常用优惠{1}（设置后可在不常用优惠分类查看、使用）", _curCoupon.couponname,
                        Environment.NewLine);

                if (!AskQuestion(msg))
                    return;

                var service = new RestaurantServiceImpl();
                var errMsg = service.SetCouponFavor(_curCoupon.couponid, _curCoupon.IsUncommonlyUsed);
                if (!string.IsNullOrEmpty(errMsg))
                {
                    AllLog.Instance.E("设定优惠券喜好失败：{0}", errMsg);
                    Msg.Warning(errMsg);
                    return;
                }

                xtraTabControl2_SelectedPageChanged(xtraCoupon, null);
            });
        }

        public void ShowFrm(string tableno, int status)
        {
            try
            {
                SelectedBankInfo = Globals.BankInfos != null ? Globals.BankInfos.FirstOrDefault(t => t.Id == 0) : null;
                currtableno = tableno;
                edtRoom.Text = tableno;
                pnlCash.Enabled = false;
                maling = true;
                btnOrderML.Enabled = false;
                btnCancelOrder.Enabled = false;
                BtnBackAll.Enabled = false;
                lblMsg.Text = "";
                lblAmount2.Text = "应收:0";
                lblAmount.Text = "消费:0";
                lbTip.Text = "小费:0";
                try
                {
                    HideOpenTable();
                }

                catch
                {
                }
                if (status == 0)
                {
                    //tmrOpenTable.Enabled = true;
                    showOpenTable();
                }
                else if (status == 8)
                {
                    tmrOpen.Enabled = true;
                }
                else
                {
                    ShowAccounts(null, null, 0);
                    isreback = status == 9;
                    tmrOpen.Enabled = true;
                }
                xtraTabControl1.SelectedTabPageIndex = 0;

                //定时刷新菜单列表（比对菜单明细数量和总额）
                _dishesTimer = new DishesTimer();

                if (_dishesTimer.DataChangeAction == null)
                {
                    _dishesTimer.DataChangeAction = new Action(DataChangeHandel);
                }
                ShowDialog();
                _dishesTimer.stop();
                _dishesTimer = null;
            }
            finally
            {
            }
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        private void DataChangeHandel()
        {
            this.BeginInvoke(new EventHandler(delegate
            {
                Opentable2();
            }));
        }

        public void showFrmWm(string tableno)
        {
            try
            {
                currtableno = tableno;
                edtRoom.Text = tableno;
                iswm = true;
                maling = true;
                xtraTabControl1.SelectedTabPageIndex = 0;
                pnlCash.Enabled = true;
                BtnMark.Visible = true;
                Globals.CurrOrderInfo.orderid = "";
                lblMsg.Text = "";
                Globals.CurrTableInfo.tableNo = tableno;
                lblRs.Visible = false;
                edtRoom.Enabled = false;
                btnOrderML.Enabled = false;
                btnOpen.Enabled = false;
                btnCancelOrder.Enabled = false;
                BtnBackAll.Enabled = false;
                lblDesk.Text = string.Format("桌号：{0}", tableno);
                Globals.CurrTableInfo.tableName = tableno;
                InitWm();
                StartWm();
                Globals.ShoppTable.Clear();
                ShowDialog();
            }
            finally
            {
            }
        }

        private void button16_Click(object sender, EventArgs e)
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

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            pnlz.Parent = xtraTabControl1.SelectedTabPage;
            pnlNum.Parent = xtraTabControl1.SelectedTabPage;
            pnlSum.Parent = xtraTabControl1.SelectedTabPage;
            //pnlz.Parent = xtraTabControl1.SelectedTabPage;
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                edtAmount.Focus();
                edtAmount.SelectAll();
            }
            else
                if (xtraTabControl1.SelectedTabPageIndex == 1)
                {
                    edtYHCard.Focus();
                    edtYHCard.SelectAll();
                }
                else
                    if (xtraTabControl1.SelectedTabPageIndex == 2)
                    {
                        edtMemberCard.Focus();
                        edtMemberCard.SelectAll();
                    }
                    else
                        if (xtraTabControl1.SelectedTabPageIndex == 3)
                        {
                            edtGzAmount.Focus();
                            edtGzAmount.SelectAll();
                        }
                        else
                            if (xtraTabControl1.SelectedTabPageIndex == 4)
                            {
                                edtZfb.Focus();
                                edtZfb.SelectAll();
                            }
                            else
                                if (xtraTabControl1.SelectedTabPageIndex == 5)
                                {
                                    edtWx.Focus();
                                    edtWx.SelectAll();
                                }
        }

        private void frmPosMain_Load(object sender, EventArgs e)
        {
            Globals.CurrTableInfo.amount = 0;
            lblUser.Text = String.Format("登录员工:{0}", Globals.UserInfo.UserName);
            lblbranchid.Text = String.Format("店铺编号：{0}", RestClient.getbranch_id());
            edtRoom.Focus();
            setBtnFocus();
            membercard = "";
            lblInvoice.Text = "";
            if (jarrTables == null)
            {
                getCoupon_rule();
            }
            else if (jarrTables.Count <= 0) { getCoupon_rule(); }
            createTb();
            amountgz2 = 0;
            amountym = 0;
            pnlz.Parent = xtraTabControl1.SelectedTabPage;
            lblVer.Text = String.Format("版本:{0}", Globals.ProductVersion);
            btnPrintMember1.Enabled = false;
            btnRePrint.Enabled = false;
            btnPrintBill.Enabled = false;
            btnOrder2.Enabled = false;
            btnRePrintCust.Enabled = false;
            Globals.OrderTable.Clear();
            lblDesk.Text = String.Format("桌号：{0}", "");
            lblRs.Text = String.Format("人数：{0}", "");
            lblZd.Text = String.Format("帐单：{0}", "");
            lblMember.Text = String.Format("会员：{0}", "");

            //lblMember.TextChanged += lblMember_TextChanged;

            //btnDelete.Visible = !RestClient.isClearCoupon();
            dgvjs.Width = 330;

            InitMemberFun();
            //pnlMore.Top = 200;
            setFormToPayType1();
        }

        /// <summary>
        /// 会员卡号变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lblMember_TextChanged(object sender, EventArgs e)
        {
            var tag = btnFind.Tag.ToString();
            if (!lblMember.Text.Equals("会员：") & !string.IsNullOrEmpty(Globals.CurrOrderInfo.memberno) & tag.Equals("0"))
            {
                LoginVIP();
            }
        }

        public static bool checkInputTellerCash()
        {
            try
            {
                string reinfo = "";

                if (!RestClient.InputTellerCash(Globals.UserInfo.UserID, 0, 0, out reinfo))
                {
                    if (!frmPettyCash.ShowPettyCash())
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Msg.ShowException(ex);
                return false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lblTime.Text = "当前时间：" + dt.ToLocalTime().ToString();//2005-11-5 21:21:25
        }

        private void SetButtonEnable(bool value)
        {
            edtRoom.Enabled = value;
        }

        private void opentable()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                isopentable2 = false;
                this.SetButtonEnable(false);
                this.Update();//必须
                string TableName = edtRoom.Text;
                maling = true;
                pnlCash.Enabled = false;//false
                btnPrintMember1.Enabled = false;
                btnRePrint.Enabled = false;
                btnPrintBill.Enabled = false;
                btnOrder2.Enabled = false;
                btnRePrintCust.Enabled = false;
                btnSelect.Visible = false;
                tbyh.Clear();
                amountgz2 = 0;//优惠的挂帐
                amountym = 0;//优免
                amountml = 0;
                amountroundtz = 0;
                decym = 0;
                decgz2 = 0;
                lblInvoice.Text = "";
                cardno = "";
                Globals.CurrOrderInfo.Invoicetitle = "";
                btnRePrint.Enabled = false;
                //btnRePrintCust.Enabled = false; 
                Globals.CurrOrderInfo.memberno = "";//btnRePrintCust
                edtMemberCard.Text = "";
                edtYHCard.Text = "";
                membercard = "";
                edtMember.Text = "";
                edtJf.Text = "";
                if (ysamount == 0)
                {
                    edtAmount.Text = "";
                }
                edtWx.Text = "";
                edtWxAmount.Text = "";
                edtZfb.Text = "";
                edtZfbAmount.Text = "";
                //edtWxAmount.Text = "";
                //edtWxNo.Text = "";
                edtCard.Text = "";
                edtPwd.Text = "";
                //rgpType.SelectedIndex = 0;
                edtGzAmount.Text = "";
                edtGz.Text = "";
                btnFind.Tag = 0;
                btnFind.Text = "登录";
                label15.Text = string.Format("储值余额：{0}", 0);
                label9.Text = string.Format("积分余额：{0}", 0);

                //rgpType.SelectedIndex = 0;
                isaddmember = false;
                isaddFavorale = false;
                pszTicketList = null;
                lblMember.Text = String.Format("会员：{0}", "");
                if (jarrTables != null)
                    jarrTables.Clear();
                if (xtraCoupon.SelectedTabPageIndex == 5)
                    xtraCoupon.SelectedTabPageIndex = 0;
                Array.Resize(ref pszTicketList, 0);

                //单品部份折扣也要清除掉  启用新优惠后取消这个接口
                //try
                //{
                //    RestClient.fullDiscount(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserID, 0, " ", " ");
                //}
                //catch { }

                string loginReturn = RestClient.GetOrder(TableName, Globals.UserInfo.UserID);
                if (loginReturn == "0") //调用
                {
                    Warning("未找到帐单,请确认是否已开台!");
                    this.SetButtonEnable(true);
                    edtRoom.Focus();
                    edtRoom.SelectAll();
                }
                else
                {
                    if (loginReturn == "")
                    {
                        Warning("获取数据失败，请检查网络连接是否正常!");
                        this.SetButtonEnable(true);
                        edtRoom.Focus();
                        edtRoom.SelectAll();
                    }
                    else
                    {
                        try
                        {
                            addAutoFavorale();
                        }
                        catch { }
                        getAmount();
                        this.SetButtonEnable(true);
                        btnml_Click(btnml, null);
                        membercard = Globals.CurrOrderInfo.memberno;
                        edtMemberCard.Text = membercard;
                        getOrderInvoiceTitle();
                        //恢复帐单的优惠列表
                        if (!RestClient.isClearCoupon())
                        {
                            try
                            {
                                JArray jrOrder = null;
                                if (RestClient.getOrderCouponList(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, out jrOrder))
                                {
                                    DataTable dtOrder = null;
                                    dtOrder = Models.Bill_Order.gett_order_rule(jrOrder);
                                    addOrderCoupon(dtOrder);
                                    getAmount();
                                }
                            }
                            catch (Exception e) { }
                        }
                    }
                }

                /*string loginReturn = RestClient.GetServerTableInfo(TableName, Globals.UserInfo.UserID);
                if (loginReturn == "0") //调用
                {
                    Warning("未找到帐单,请确认是否已开台!");
                    this.SetButtonEnable(true);
                    edtRoom.Focus();
                    edtRoom.SelectAll();
                }
                else

                    if (loginReturn == "")
                    {
                        Warning("获取数据失败，请检查网络连接是否正常!");
                        this.SetButtonEnable(true);
                        edtRoom.Focus();
                        edtRoom.SelectAll();
                    }
                    else
                    {
                        //先取消会员价
                        try
                        {
                            RestClient.setMemberPrice3(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid);
                        }
                        catch { }
                        //返回成功
                        RestClient.GetServerTableList(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserID);
                        //ShowLeftInfo();
                        try
                        {
                            addAutoFavorale();
                        }
                        catch { }
                        getAmount();
                        this.SetButtonEnable(true);
                        btnml_Click(btnml, null);

                        //恢复帐单的优惠列表
                        if (!RestClient.isClearCoupon())
                        {
                            try
                            {
                                JArray jrOrder = null;
                                if (RestClient.getOrderCouponList(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, out jrOrder))
                                {
                                    DataTable dtOrder = null;
                                    dtOrder = Models.Bill_Order.gett_order_rule(jrOrder);
                                    addOrderCoupon(dtOrder);
                                    getAmount();
                                }
                            }
                            catch (Exception e) { }
                        }
                    }*/
                xtraTabControl1.SelectedTabPageIndex = 0;//不刷新优惠，加快速度 20151008
            }
            catch (CustomException ex)
            {
                this.SetButtonEnable(true);
                Warning(ex.Message);
                edtRoom.Focus();
                edtRoom.SelectAll();
            }
            catch (Exception ex)
            {
                this.SetButtonEnable(true);
                //Warning("获取数据失败!");
                edtRoom.Focus();
                edtRoom.SelectAll();
            }
            finally
            {

            }
            try
            {
                DataView dv = new DataView(Globals.OrderTable);
                dv.AllowNew = false;
                this.dgvBill.AutoGenerateColumns = false;
                this.dgvBill.Tag = 0;
                this.dgvBill.DataSource = dv;
                showGridWeight();
            }
            catch { }
            finally
            {
                //如果已点列表为空也不能重印
                if (dgvBill.RowCount <= 0)
                {
                    btnPrintBill.Enabled = false;
                    btnRePrintCust.Enabled = false;
                }
            }
            this.Cursor = Cursors.Default;
        }
        private void addOrderCoupon(DataTable dt)
        {
            foreach (DataRow drOrder in dt.Rows)
            {
                DataRow dr = tbyh.NewRow();
                double freeamount = 0;
                double debitamount = 0;
                int num = 0;
                freeamount = double.Parse(drOrder["freeamount"].ToString());
                debitamount = double.Parse(drOrder["debitamount"].ToString());
                num = int.Parse(drOrder["num"].ToString());
                dr["yhname"] = drOrder["yhname"].ToString();
                dr["partnername"] = drOrder["partnername"].ToString();
                dr["couponrate"] = drOrder["couponrate"].ToString();
                dr["freeamount"] = freeamount;
                dr["debitamount"] = debitamount;
                dr["num"] = num;
                dr["amount"] = double.Parse(drOrder["amount"].ToString());
                dr["couponsno"] = drOrder["couponsno"].ToString();
                dr["memo"] = drOrder["memo"].ToString();
                dr["type"] = drOrder["ftype"].ToString();
                dr["banktype"] = drOrder["banktype"].ToString();
                dr["ruleid"] = drOrder["ruleid"].ToString();
                dr["couponid"] = drOrder["couponid"].ToString();
                amountgz2 = amountgz2 + (float)debitamount;
                amountym = amountym + (float)freeamount;
                tbyh.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlyGetTableInfo">结算完成后仅获取餐桌信息，不必要获取餐桌详情，提高效率。</param>
        private void Opentable2(bool onlyGetTableInfo = false)
        {
            try
            {
                if (iswm)
                    return;
                this.Cursor = Cursors.WaitCursor;
                isopentable2 = true;
                Globals.CurrOrderInfo.Invoicetitle = "";
                this.SetButtonEnable(false);
                this.Update();//必须
                string tableName = edtRoom.Text;
                string loginReturn = RestClient.GetServerTableInfo(tableName, Globals.UserInfo.UserID);
                if (loginReturn == "0") //调用
                {
                    Warning("未找到帐单,请确认是否已开台!");
                    this.SetButtonEnable(true);
                    edtRoom.Focus();
                    edtRoom.SelectAll();
                }
                else
                    if (loginReturn == "")
                    {
                        Warning("获取数据失败，请检查网络和后台服务!");
                        this.SetButtonEnable(true);
                        edtRoom.Focus();
                        edtRoom.SelectAll();
                    }
                    else
                    {
                        if (!onlyGetTableInfo)
                        {
                            RestClient.GetServerTableList(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserID);
                            if (!Globals.CurrOrderInfo.memberno.Equals(""))
                            {
                                membercard = Globals.CurrOrderInfo.memberno;
                                edtMemberCard.Text = membercard;
                            }
                        }

                        //ShowLeftInfo();
                        getOrderInvoiceTitle();
                        amountml = 0;
                        amountroundtz = 0;
                        getAmount();
                        this.SetButtonEnable(true);
                    }
            }
            catch (CustomException ex)
            {
                this.SetButtonEnable(true);
                Warning(ex.Message);
                edtRoom.Focus();
                edtRoom.SelectAll();
            }
            catch (Exception ex)
            {
                this.SetButtonEnable(true);
                Warning("获取数据失败!");
                edtRoom.Focus();
                edtRoom.SelectAll();
            }
            try
            {
                DataView dv = new DataView(Globals.OrderTable);
                dv.AllowNew = false;
                this.dgvBill.AutoGenerateColumns = false;
                this.dgvBill.DataSource = dv;
                this.dgvBill.Tag = 0;
                showGridWeight();
            }
            catch { }
            this.Cursor = Cursors.Default;
        }
        private void edtRoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            edtRoom.ForeColor = Color.Black;
            if ((e.KeyChar == 13) && (edtRoom.Text.Trim().ToString().Length > 0))
            {
                opentable();
            }
        }

        private void edtRoom_Click(object sender, EventArgs e)
        {
            if (edtRoom.Text.Equals("请输入台号"))
            {
                edtRoom.Text = "";
            }
        }
        private void ShowLeftInfo()
        {
            lblDesk.Text = String.Format("桌号：{0}", Globals.CurrTableInfo.tableNo);
            lblRs.Text = String.Format("人数：{0}", Globals.CurrOrderInfo.custnum);
            lblZd.Text = String.Format("帐单：{0}", Globals.CurrOrderInfo.orderid);

            btnRBill.Enabled = false;
            btnPrintMember1.Enabled = false;
            btnRePrint.Enabled = false;
            btnCancelOrder.Enabled = false;
            BtnBackAll.Enabled = false;
            btnOrderML.Enabled = true;
            if (!iswm)
            {
                btnCancelOrder.Enabled = true;
                BtnBackAll.Enabled = Globals.OrderTable.Rows.Count > 0;
                btnPrintBill.Enabled = true;
            }
            btnRePrintCust.Enabled = true;
            btnOrder2.Enabled = true;
            lblAmount2.ForeColor = Color.Red;
            //pnlCash.Enabled = true;
            //如果订单已结算就不能结算了
            if (CheckCallBill() && (!iswm))
            {
                btnOrderML.Enabled = false;
                btnCancelOrder.Enabled = false;
                BtnBackAll.Enabled = false;
                btnRBill.Enabled = true;
                btnPrintMember1.Enabled = true;
                btnPrintBill.Enabled = false;
                btnOrder2.Enabled = false;
                btnRePrintCust.Enabled = false;
                lblAmount2.ForeColor = Color.Green;
                pnlCash.Enabled = false;
                lblSum.Text = "已结算";
                btnRePrint.Enabled = true;
            }
            else
            {
                pnlCash.Enabled = true;
            }

            lblAmount2.Text = String.Format("应收:{0}", ysamount);
            lblAmount.Text = String.Format("消费:{0}", Globals.CurrTableInfo.amount);
            lbTip.Text = string.Format("小费:{0}", Globals.CurrTableInfo.TipAmount);
            focusedt = edtAmount;
            //edtAmount.Text = Globals.CurrTableInfo.amount.ToString();
            edtAmount.Focus();
            edtReturn.Text = "0";

            edtReturn.Text = returnamount.ToString();

            try
            {
                if (Globals.CurrOrderInfo.memberno != null)
                {
                    if (Globals.CurrOrderInfo.memberno.ToString().Length > 0)
                    {
                        if (edtMemberCard.Text.ToString().Length <= 0)
                        {
                            edtMemberCard.Text = Globals.CurrOrderInfo.memberno;
                            lblMember.Text = String.Format("会员：{0}", Globals.CurrOrderInfo.memberno);
                            btnFind.Tag = 1;
                            btnFind.Text = "退出";

                            try
                            {
                                if (!isopentable2)
                                {
                                    RestClient.setMemberPrice(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, edtMemberCard.Text);
                                    Opentable2();
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 应收金额变化时，现金金额同步更新
        /// </summary>
        private void SyncCashPay()
        {
            if (_isCashTextBoxChanged)
                return;

            float cashAmount = Math.Max(0, ysamount - amountyhk - amountzfb - amountwx - amountgz - amountjf - amounthyk);
            edtAmount.EditValueChanging -= edtAmount_EditValueChanging;
            edtAmount.Text = cashAmount.ToString();
            edtAmount.EditValueChanging += edtAmount_EditValueChanging;
        }

        /// <summary>
        /// 帐单是否已结算 TRUE 
        /// </summary>
        /// <returns></returns>
        private bool CheckCallBill()
        {
            int orderstatus = Globals.CurrOrderInfo.orderstatus;
            if ((orderstatus != 1) && (orderstatus != 2) && (orderstatus != 3))
            {
                return false;
            }
            else
                return true;
        }

        private void edtRoom_Enter(object sender, EventArgs e)
        {
            edtRoom.SelectAll();
        }

        private void edtRoom_Leave(object sender, EventArgs e)
        {
            if (edtRoom.Text.Trim().ToString().Length <= 0)
            {
                edtRoom.ForeColor = Color.Silver;
                edtRoom.Text = "请输入台号";
            }
        }


        private void frmPosMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F1)
            {
                edtRoom.SelectAll();
                edtRoom.Focus();
            }
        }

        private void edtAmount_Enter(object sender, EventArgs e)
        {
            focusedt = edtAmount;
        }

        private void edtYHCard_Enter(object sender, EventArgs e)
        {
            focusedt = edtYHCard;
        }

        private void edtCard_Enter(object sender, EventArgs e)
        {
            focusedt = edtCard;
        }

        private void edtMember_Enter(object sender, EventArgs e)
        {
            focusedt = edtMember;
        }


        private void getAmount()
        {
            payamount = Globals.CurrTableInfo.amount;//应付

            amountyhk = string2float(edtCard.Text); //实收yhk
            amounthyk = string2float(edtMember.Text); ;//实收hyk
            amountgz = string2float(edtGzAmount.Text);//挂帐
            amountjf = string2float(edtJf.Text);//会员积分 会员积分不找零
            amountzfb = string2float(edtZfbAmount.Text); //实收
            amountwx = string2float(edtWxAmount.Text); //实收
            amountml = 0;
            amountroundtz = 0;

            setamountml();//获取自动抹零金额

            ysamount += Globals.CurrTableInfo.TipAmount;//应收+小费

            //应收金额同步更新现金付款
            SyncCashPay();

            amountrmb = string2float(edtAmount.Text);//实收rmb

            getamount = amountrmb + amountyhk + amounthyk + amountgz + amountgz2 + amountym + amountml + amountroundtz + amountjf + amountzfb + amountwx;//实收
            getamount = (float)Math.Round(getamount, 2);
            getamountsy = amountrmb + amountyhk + amounthyk + amountgz + amountjf + amountzfb + amountwx;//实收2

            amountTip = Math.Min(Globals.CurrTableInfo.TipAmount, Math.Max(0, (float)Math.Round(getamountsy - (ysamount - Globals.CurrTableInfo.TipAmount), 2))); //小费等于付款金额-应收金额，必须大于0且小于设定的值。
            amountTip = Math.Min(amountTip, amountrmb);//小费的来源只是现金，不能超过现金金额。
            returnamount = Math.Max(0, (float)Math.Round(getamountsy - ysamount, 2));
            returnamount = Math.Min(returnamount, (float)Math.Round(amountrmb - amountTip, 2));//找零的来源只是现金，不能超过现金金额。
            edtReturn.Text = returnamount.ToString(CultureInfo.InvariantCulture);
            String tmpstr = "";
            if (amountrmb > 0)
                tmpstr = string.Format("现金{0} ", amountrmb);
            if (amountgz > 0)
                tmpstr += string.Format("挂帐单位{0} ", amountgz);
            if (amounthyk > 0)
                tmpstr += string.Format("储值{0} ", amounthyk);
            if (amountjf > 0)
                tmpstr += string.Format("积分{0} ", amountjf);
            if (amountgz2 > 0)
                tmpstr += string.Format("挂帐{0} ", amountgz2);
            if (amountyhk > 0)
                tmpstr += string.Format("刷卡{0} ", amountyhk);
            if (amountzfb > 0)
                tmpstr += string.Format("支付宝{0} ", amountzfb);
            if (amountwx > 0)
                tmpstr += string.Format("微信{0} ", amountwx);

            if (decgz2 < 0)
                tmpstr += string.Format("挂账多收{0} ", Math.Abs(decgz2));
            if ((amountym + decym) > 0)
                tmpstr += string.Format("优免{0} ", amountym + decym);

            if (amountml > 0)
                tmpstr += string.Format("抹零{0} ", amountml);
            if (amountroundtz > 0)
                tmpstr += string.Format("舍去{0}", amountroundtz);
            else if (amountroundtz < 0)
                tmpstr += string.Format("舍入{0}", Math.Abs(amountroundtz));
            if (returnamount > 0)
                tmpstr += string.Format("找零{0} ", returnamount);
            else
            {
                if (getamount <= 0)
                {
                    tmpstr += string.Format("需收款{0} ", ysamount);
                }
                else
                {
                    var needMoreAmount = Math.Round(ysamount - getamountsy, 2);
                    if (needMoreAmount > 0)
                        tmpstr += string.Format("还需再收{0} ", needMoreAmount);
                }
            }

            lblSum.Text = String.Format("收款：{0}", tmpstr);
            pnlSum.BackColor = getamountsy >= ysamount ? Color.DarkSeaGreen : Color.Bisque;
            ShowLeftInfo();

        }
        private float string2float(string floatstr)
        {
            float tmpfloat = 0;
            try
            {
                tmpfloat = float.Parse(floatstr);
            }
            catch
            { tmpfloat = 0; }
            return tmpfloat;
        }
        private void edtAmount_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            _isCashTextBoxChanged = true;
            getAmount();
            _isCashTextBoxChanged = false;
        }

        private void edtCard_EditValueChanged(object sender, EventArgs e)
        {
            getAmount();
        }

        private void edtMember_EditValueChanged(object sender, EventArgs e)
        {
            if (psStoredCardsBalance <= 0)
            {
                edtMember.Text = "";
            }
            getAmount();
        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            bool isok = false;
            try
            {
                Cursor = Cursors.WaitCursor;
                if (edtYHCard == focusedt)
                {
                    edtCard.SelectAll();
                    edtCard.Focus();
                    return;
                }
                if (edtMemberCard == focusedt)
                {
                    querymember();
                    return;
                }

                _isCashTextBoxChanged = true;
                Opentable2();
                _isCashTextBoxChanged = false;
                //getAmount();

                if (!CheckBillCanPay())//检查有没有未称重的，有就不能结帐
                {
                    Warning("还有未称重菜品,不能结帐...");
                    return;
                }

                if (Math.Round(ysamount - Globals.CurrTableInfo.TipAmount - getamountsy, 2) > 0)
                {
                    Warning("还有未收金额...");
                    return;
                }
                if (amountTip < Globals.CurrTableInfo.TipAmount && Math.Round(getamountsy - ysamount + Globals.CurrTableInfo.TipAmount, 2) > amountTip)
                {
                    Warning(string.Format("小费{0}元，必须使用现金结算。", Globals.CurrTableInfo.TipAmount));
                    return;
                }
                if (amountTip < Globals.CurrTableInfo.TipAmount)
                {
                    if (!AskQuestion(string.Format("还有{0}元小费未结算，点击确定继续结算，点击取消取消结算", Globals.CurrTableInfo.TipAmount - amountTip)))
                        return;
                }
                if (returnamount >= 100)
                {
                    Warning("找零不能大于100...");
                    return;
                }
                if (returnamount <= 0)
                {
                    if (amountyhk > ysamount)
                    {
                        Warning("请输入正确的刷卡金额...");
                        return;
                    }
                    if (amounthyk > ysamount)
                    {
                        Warning("请输入正确的会员卡金额...");
                        return;
                    }
                    if (amountzfb > ysamount)
                    {
                        Warning("请输入正确的支付宝金额...");
                        return;
                    }
                    if (amountwx > ysamount)
                    {
                        Warning("请输入正确的微信金额...");
                        return;
                    }

                }

                if (amounthyk > 0)
                {
                    if (membercard.Length <= 0)
                    {
                        Warning("请刷会员卡...");
                        return;
                    }
                    if (amounthyk > psStoredCardsBalance)
                    {
                        Warning("会员卡余额不足...");
                        return;
                    }
                    if (amounthyk > ysamount)
                    {
                        Warning("会员卡使用金额不能大于应付额...");
                        return;
                    }
                }
                if (getamountsy > ysamount)
                {
                    Warning("实际输入金额大于应收,请确认...");
                }

                var invoiceAmount = (decimal)Globals.CurrTableInfo.amount;
                string settleorderorderid = Globals.CurrOrderInfo.orderid;
                bool ismember = false;
                if (AskQuestion("台号：" + Globals.CurrTableInfo.tableName + "确定现在结算吗?"))
                {
                    //如果是外卖，先开台,再上传菜品，再结算（外卖单品类优惠怎么用呢?,1、下单后循环优惠ID，使用一遍单品类的优惠，或者外卖我打包上传一下所有菜品的JSON数组，服务器端再返回优惠金额）
                    if (iswm)
                    {
                        Globals.CurrOrderInfo.userid = Globals.UserInfo.UserID;
                        openTableAndOrder();
                        settleorderorderid = Globals.CurrOrderInfo.orderid;
                        if (string.IsNullOrEmpty(settleorderorderid))
                            return;

                        //如果是外卖先结算会员，如果会员结算失败就不再结算帐单
                        isok = wmAccount(0);
                        if (isok)
                        {
                            ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
                        }
                    }
                    else
                    {
                        if (!RestClient.settleorder(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserID, getPayTypeJsonArray()).Equals("0"))
                        {
                            Warning("结算失败...");
                        }
                        else
                        {
                            //如果有会员卡那么会员结算，如果会员结算失败，那就反结算 

                            //现金金额，用于积分
                            if (membercard.Length > 0)
                            {
                                float psccash = amountrmb + amountyhk + amountgz + amountzfb + amountwx;//现金 用于会员积分
                                float pscpoint = amountjf; //使用积分付款
                                float pszStore = amounthyk;//使用储值余额付款
                                float tmppsccash = Math.Max(0, psccash - returnamount);

                                //使用优惠券
                                String tickstrs = getTicklistStr();
                                if (tmppsccash > 0 || pscpoint > 0 || tickstrs.Length > 0 || amounthyk > 0)
                                {
                                    ismember = true;
                                    try
                                    {
                                        string pwd = "0";
                                        if (edtPwd.Text.Trim().Length > 0)
                                        {
                                            pwd = edtPwd.Text.Substring(0, Math.Min(edtPwd.Text.Length, 6));
                                        }
                                        bool data = MemberSale(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid,
                                            membercard, Globals.CurrOrderInfo.orderid, tmppsccash, pscpoint, 1,
                                            amounthyk, tickstrs, pwd, (float)Math.Round(memberyhqamount, 2));
                                        if (data)
                                        {
                                            ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
                                            isok = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        AllLog.Instance.E("会员消费异常。" + ex.Message);
                                    }
                                    finally
                                    {
                                        if (!isok)
                                        {
                                            var memType = RestClient.getMemberSystem() == 0 ? "雅座" : "餐道";
                                            Warning(string.Format("{0}会员消费结算失败，请重试。", memType));
                                            string msg;
                                            if (!RestClient.rebacksettleorder(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName, "会员结算失败,系统自动反结", out msg))
                                            {
                                                Warning(!string.IsNullOrEmpty(msg) ? msg : "帐单自动反结算失败...");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
                                    isok = true;
                                }
                            }
                            else
                            {
                                ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
                                isok = true;
                            }

                            if (isok && Globals.CurrTableInfo.TipAmount > 0)
                            {
                                var service = new RestaurantServiceImpl();
                                var result = service.BillingTip(Globals.CurrOrderInfo.orderid, amountTip);
                                if (!string.IsNullOrEmpty(result))
                                {
                                    Warning(result);
                                    isok = false;
                                }
                            }

                            if (isok)
                            {
                                try
                                {
                                    RestClient.debitamout(Globals.CurrOrderInfo.orderid);
                                }
                                catch (Exception ex)
                                {
                                    AllLog.Instance.E("计算实收接口异常。" + ex.Message);
                                }
                            }
                        }
                    }

                    Opentable2(true);
                    if (isok)
                    {
                            try
                            {
                                PrintBill2();
                            }
                            catch (Exception ex)
                            {
                                AllLog.Instance.E("打印结账单异常。" + ex.Message);
                            }

                            Application.DoEvents();
                            if (ismember)
                            {
                                try
                                {
                                    ReportPrint.PrintMemberPay1(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName, psIntegralOverall);
                                }
                                catch (Exception ex)
                                {
                                    AllLog.Instance.E("打印会员凭条异常。" + ex.Message);
                                }
                            }
                        if (!iswm)
                        {
                            ThreadPool.QueueUserWorkItem(t => { broadMsg(); });
                        }

                        CanClose();

                        //如果是外卖 需要使用优惠券 ，并初始化外卖台，可以开始下一单
                        if (iswm)
                        {
                            InitWm();
                            Globals.ShoppTable.Clear();
                            ShowWm();
                        }

                        //如果开发票那么弹出一个框提示开发票 Globals.CurrOrderInfo.Invoicetitle
                        if (!iswm)
                        {
                            if (!Globals.CurrOrderInfo.Invoicetitle.Equals(""))
                            {
                                frmInvoice.ShowInvoice(settleorderorderid, Globals.CurrTableInfo.tableNo, Globals.CurrOrderInfo.Invoicetitle, invoiceAmount, cardno);
                            }
                        }
                    }
                }

            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void broadMsg()
        {
            var aer = Globals.CurrTableInfo.areaid;
            try
            {
                RestClient.broadcastmsg(1002, Globals.CurrOrderInfo.orderid); //这里是发结算指令1002
            }
            catch { }
            //广播给手环2002
            try
            {
                //发   服务员|台号|帐单号
                string msg = String.Format("{0}|{1}|{2}", Globals.CurrOrderInfo.userid, Globals.CurrTableInfo.tableNo, Globals.CurrOrderInfo.orderid);
                RestClient.broadcastmsg(2002, msg); //这里是发结算指令1002
            }
            catch { }
            //广播手环2011
            try
            {


                if (string.IsNullOrEmpty(aer))
                {
                    aer = Globals.CurrTableInfo.tableNo;
                }

                //发   服务员|台号|帐单号
                string msg = String.Format("{0}|17|0|{1}|{2}|{3}", Globals.CurrOrderInfo.userid, aer, Globals.CurrTableInfo.tableNo, Guid.NewGuid().ToString("N"));
                RestClient.broadcastmsg(2011, msg);
            }
            catch { }
        }
        private void broadMsg2201()
        {
            try
            {
                RestClient.broadcastmsg(2201, msgorderid); //这里是发结算指令1002
            }
            catch { }
        }
        private String getTicklistStr()
        {
            //Coupons_Name 30,Coupon_code 15,Coupon_Amount 12,Coupon_No 4
            //description,ruleid,freeamount,1
            //vcr.memo,vcr.ruleid,vcr.freeamount,vcr.num
            String tickListStr = "";
            int i = 5;
            string tickstr = "";
            foreach (DataRow dr in tbyh.Rows)
            {
                string banktype = dr["banktype"].ToString();
                if (banktype == "100")
                {
                    string Coupons_Name = dr["memo"].ToString(); //优惠券名称 消费券1名称（不足30位时后面的补空格）
                    Coupons_Name = Coupons_Name.Replace("会:", "");
                    if (Coupons_Name.IndexOf("[") > 0)
                        Coupons_Name = Coupons_Name.Substring(0, Coupons_Name.IndexOf("["));
                    int len = System.Text.Encoding.Default.GetBytes(Coupons_Name).Length;
                    len = 30 - len;
                    string space = "";
                    Coupons_Name = Coupons_Name + space.PadRight(len, ' ');
                    string Coupon_code = dr["ruleid"].ToString();//优惠券编号 消费券1券编码(不足15位时采用高位字符0)
                    Coupon_code = Coupon_code.PadLeft(15, '0');
                    float freeamount = float.Parse(dr["freeamount"].ToString());//优惠券金额 消费券1金额(不足12位时采用高位字符0)
                    string freeamountstr = (Math.Round(freeamount, 2) * 100).ToString().PadLeft(12, '0');
                    int Coupon_No = int.Parse(dr["num"].ToString());//优惠券数量 消费券1张数（不足4位时采用高位补字符0）
                    string Coupon_Nostr = Coupon_No.ToString().PadLeft(4, '0');
                    //这是会员卡券优惠，需要在雅座接口中使用
                    tickstr = tickstr + String.Format("{0}{1}{2}{3}", Coupons_Name, Coupon_code, freeamountstr, Coupon_Nostr);
                }
            }
            tickListStr = tickstr;
            return tickListStr;
        }
        private void edtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (getamount >= payamount)
                {
                    button27_Click_1(btnPay, e);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //
            try
            {
                Opentable2();
                ReportAmount ra;
                ra.orderid = Globals.CurrOrderInfo.orderid;
                ra.amount = Math.Round(Convert.ToDecimal(Globals.CurrTableInfo.amount), 2);
                ra.ysAmount = Math.Round(Convert.ToDecimal(ysamount - Globals.CurrTableInfo.TipAmount), 2);
                ra.mlAmount = Math.Round(Convert.ToDecimal(amountml), 2); //amountml;
                ra.amountgz = Math.Round(Convert.ToDecimal(amountgz2), 2); //amountgz2;
                ra.amountym = Math.Round(Convert.ToDecimal(amountym), 2); //amountym;
                ra.amountround = Math.Round(Convert.ToDecimal(amountroundtz), 2);
                this.Cursor = Cursors.WaitCursor;
                if (Globals.OrderTable.Rows.Count <= 0)
                    return;
                ReportPrint.PrintPayBill(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName, tbyh, ra);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            //ReportPrint.PrintPayBill(Globals.OrderTable,Globals.CurrTableInfo.amount);
        }
        private JArray getPayTypeJsonArray()
        {
            var obj = new List<PayType>();
            memberyhqamount = 0;
            var pay1 = new PayType
            {
                payAmount = amountrmb - returnamount - amountTip,
                payWay = "0", //现金
                memerberCardNo = "",
                bankCardNo = "",
                couponnum = "0",
                couponid = "",
                coupondetailid = "",
            };
            obj.Add(pay1);
            var pay2 = new PayType
            {
                payAmount = amountyhk,
                payWay = "1",//银行卡ShowTotal
                memerberCardNo = _selectedBankInfo != null ? _selectedBankInfo.Id.ToString() : "0",
                bankCardNo = edtYHCard.Text.Trim().ToString(),
                couponnum = "0",
                couponid = "",
                coupondetailid = "",
            };
            obj.Add(pay2);
            var pay3 = new PayType
            {
                payAmount = amounthyk,
                payWay = "8",//会员卡
                memerberCardNo = edtMemberCard.Text.Trim().ToString(),
                bankCardNo = "",
                couponnum = "0",
                couponid = "",
                coupondetailid = "",
            };
            obj.Add(pay3);
            string gztag = "";
            try
            { gztag = edtGz.Tag.ToString(); }
            catch { gztag = ""; }
            var pay5 = new PayType
            {
                payAmount = amountgz,
                payWay = "13",//挂帐2 挂帐2是13  
                memerberCardNo = "",
                bankCardNo = edtGz.Text,
                couponnum = "0",
                couponid = "",
                coupondetailid = gztag, //保存券编号   
            };
            obj.Add(pay5);
            var pay6 = new PayType
            {
                payAmount = amountjf,
                payWay = "11",//会员积分
                memerberCardNo = edtMemberCard.Text.Trim().ToString(),
                bankCardNo = "",//保存券编号
                couponnum = "0",
                couponid = "",
                coupondetailid = "",
            };
            obj.Add(pay6);
            var pay8 = new PayType
            {
                payAmount = amountwx,
                payWay = "17",//微信
                memerberCardNo = "",
                bankCardNo = edtWx.Text,//
                couponnum = "0",
                couponid = "",
                coupondetailid = "",
            };
            obj.Add(pay8);
            var pay9 = new PayType
            {
                payAmount = amountzfb,
                payWay = "18",//
                memerberCardNo = "",
                bankCardNo = edtZfb.Text,//
                couponnum = "0",
                couponid = "",
                coupondetailid = "",
            };
            obj.Add(pay9);
            //增加优惠的挂帐和优免 
            //Array.Resize(ref obj, obj.Length + tbyh.Rows.Count);
            int i = 8;
            foreach (DataRow dr in tbyh.Rows)
            {
                string couponname = dr["yhname"].ToString();
                string partnername = dr["partnername"].ToString();
                float freeamount = float.Parse(dr["freeamount"].ToString());
                float debitamount = float.Parse(dr["debitamount"].ToString());
                string couponid2 = dr["couponid"].ToString();
                string coupondetailid2 = dr["ruleid"].ToString();
                int num = int.Parse(dr["num"].ToString());
                //dr["couponsno"] = "";
                //dr["memo"] = "";
                int type = int.Parse(dr["type"].ToString());
                string banktype = dr["banktype"].ToString();
                if (banktype == null)
                    banktype = "";
                if (banktype == "100")
                {
                    //雅座会员优惠券
                    memberyhqamount = memberyhqamount + freeamount;
                    var pay = new PayType
                    {
                        payAmount = freeamount,
                        payWay = "12",//雅座会员优惠券
                        memerberCardNo = partnername,
                        bankCardNo = couponname, //保存券编号 优惠名称
                        //用哪一个字段保存 num 传给后台
                        couponnum = num.ToString(),
                        couponid = dr["ruleid"].ToString(),
                        coupondetailid = dr["ruleid"].ToString(),

                    };
                    obj.Add(pay);
                    i = i + 1;
                }
                else
                {
                    if (freeamount > 0)
                    {
                        var pay = new PayType
                        {
                            payAmount = freeamount,
                            payWay = "6",//优免
                            memerberCardNo = partnername,
                            bankCardNo = couponname, //保存券编号 优惠名称
                            couponnum = num.ToString(),
                            couponid = couponid2,
                            coupondetailid = coupondetailid2,
                        };
                        obj.Add(pay);
                        i = i + 1;
                    }
                    if (debitamount > 0)
                    {
                        var pay = new PayType
                        {
                            payAmount = debitamount,
                            payWay = "5",//挂帐
                            memerberCardNo = partnername, //挂帐单位
                            bankCardNo = couponname, //保存券编号 优惠名称
                            couponnum = num.ToString(),
                            couponid = couponid2,
                            coupondetailid = coupondetailid2,
                        };
                        obj.Add(pay);
                        i = i + 1;
                    }
                }
            }

            if (decgz2 < 0)
            {
                //挂帐调整 多收挂帐的钱
                var pay = new PayType
                {
                    payAmount = decgz2,
                    payWay = "5",//挂帐多收(挂帐调整)
                    memerberCardNo = "10000", //挂帐单位
                    bankCardNo = "挂帐多收", //保存券编号 优惠名称
                    couponnum = "0",
                    couponid = "",
                    coupondetailid = "",
                };
                obj.Add(pay);
            }
            if (decym < 0)
            {
                //负优免
                var pay = new PayType
                {
                    payAmount = decym,
                    payWay = "6",
                    memerberCardNo = "10000",
                    bankCardNo = "优免调整",
                    couponnum = "0",
                    couponid = "",
                    coupondetailid = "",
                };
                obj.Add(pay);
            }
            if (Globals.roundinfo.Itemid.Equals("1")) //四舍五入
            {
                if (amountroundtz != 0)
                {
                    //四舍五入调整
                    var pay = new PayType
                    {
                        payAmount = amountroundtz,
                        payWay = "20",
                        memerberCardNo = "",
                        bankCardNo = "四舍五入调整",
                        couponnum = "0",
                        couponid = "",
                        coupondetailid = "",
                    };
                    obj.Add(pay);
                }
            }
            else
            {
                if (amountml > 0)
                {
                    var payMl = new PayType
                    {
                        payAmount = amountml,
                        payWay = "7",//抹零
                        memerberCardNo = "",
                        bankCardNo = "",//
                        couponnum = "0",
                        couponid = "",
                        coupondetailid = "",
                    };
                    obj.Add(payMl);
                }
            }

            var json = JsonConvert.SerializeObject(obj);
            JArray ja = JArray.Parse(json.ToString());
            return ja;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmQueryMember.ShowQueryMember();
        }

        private void btnRBill_Click(object sender, EventArgs e)
        {
            //经理授权

            /*if (!checkCallBill())
            {
                return;
            }*/
            try
            {
                string errStr = "";
                if (!RestClient.rebackorder(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, ref errStr))
                {
                    Warning(errStr);
                    return;
                }
                if (!isreback)
                {
                    if (!AskQuestion("台号：" + Globals.CurrTableInfo.tableName + "确定要反结算吗?"))
                    {
                        return;
                    }
                }
                string inputMemo = "";
                if (!frmInputReOrderReason.ShowInputReOrderReason("请输入反结原因：", out inputMemo))
                {
                    return;
                }
                if (!frmPermission2.ShowPermission2("反结算经理授权", EnumRightType.AntiSettlement))
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                string info = "";
                int membersystem = RestClient.getMemberSystem();
                if (membersystem == 1)
                {
                    //餐道反结算会员要先从java后台调用接口获取当前帐单会员结算信息，如果有可以反结的会员结算信息才反结+++++++++
                    TCandaoRetBase ret = CanDaoMemberClient.GetOrderMember(Globals.CurrOrderInfo.orderid);
                    if (ret.Ret)
                    {
                        if (!VoidSale_CanDao(out info, ret))
                        {
                            Warning(info);
                            return;
                        }
                    }
                }
                else
                {
                    //先反结算雅座的，加个标记，是不是反成功
                    //如果POS反失败，下次再反看标记，如果雅座的已经成功了，就不反了
                    //select * from t_order_member where orderid='H20150117001506'
                    try
                    {
                        Globals.superpwd = RestClient.getManagerPwd();
                        if (!RestClient.VoidSale(Globals.CurrOrderInfo.orderid, "0", Globals.superpwd, out info))
                        {
                            Warning(info);
                            return;
                        }
                    }
                    catch
                    {
                        Warning("会员消费撤消失败...,请重试");
                        return;
                    }
                }
                string msg;
                if (!RestClient.rebacksettleorder(Globals.CurrOrderInfo.orderid, Globals.AuthorizerInfo.UserName, inputMemo, out msg))
                {
                    Warning("帐单反结算失败...");
                }
                else
                {
                    if (!string.IsNullOrEmpty(msg))
                        Warning(msg);

                    if (membersystem == 1)
                    {
                        try
                        {
                            CanDaoMemberClient.DeleteOrderMember(Globals.CurrOrderInfo.orderid, Globals.CurrOrderInfo.memberno);
                        }
                        catch { }
                    }
                }
                try
                {
                    HideOpenTable();
                }
                catch { }
                Thread.Sleep(1000);
                opentable();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }


        }

        private void button11_Click(object sender, EventArgs e)
        {

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
            Common.Globals.SetButton(btnPay);
            Common.Globals.SetButton(button28);
            Common.Globals.SetButton(button15);
            Common.Globals.SetButton(button16);
            Common.Globals.SetButton(button17);
            Common.Globals.SetButton(btnyh7);

            Common.Globals.SetButton(button13);
            Common.Globals.SetButton(button14);
            Common.Globals.SetButton(button24);
            Common.Globals.SetButton(button29);
            Common.Globals.SetButton(button30);
            Common.Globals.SetButton(button31);
            Common.Globals.SetButton(button32);
            Common.Globals.SetButton(button33);
            Common.Globals.SetButton(button34);
            Common.Globals.SetButton(button35);
            Common.Globals.SetButton(button36);
            Common.Globals.SetButton(button37);
            Common.Globals.SetButton(button38);
            Common.Globals.SetButton(button39);
            Common.Globals.SetButton(button40);
            Common.Globals.SetButton(button41);
            Common.Globals.SetButton(button42);
            Common.Globals.SetButton(button43);
            Common.Globals.SetButton(button44);
            Common.Globals.SetButton(button45);
            Common.Globals.SetButton(button46);
            Common.Globals.SetButton(button47);
            Common.Globals.SetButton(button48);
            Common.Globals.SetButton(button49);
            Common.Globals.SetButton(button50);
            Common.Globals.SetButton(btnZ);
            Common.Globals.SetButton(button11);
            Common.Globals.SetButton(button9);
            Common.Globals.SetButton(button10);
        }
        /// <summary>
        /// 会员储值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            frmMemberStoredValue.ShowMemberStoredValue();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMemberNewCard.ShowMemberNewCard();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //Close();
            //如果是外卖
            try
            {
                if (iswm)
                {
                    if (Globals.ShoppTable.Rows.Count > 0)
                    {
                        if (!AskQuestion("退出将清空已选,是否放弃结算?"))
                        {
                            return;
                        }
                        Globals.ShoppTable.Clear();
                    }
                }
                else
                {
                    Globals.ShoppTable.Clear();
                    try
                    {
                        frmopentable.Hide();
                    }
                    catch { }
                    ShowAccounts(null, null, 0);

                }
            }
            catch { }
            hideFrm();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tmrOpen_Tick(object sender, EventArgs e)
        {
            tmrOpen.Enabled = false;
            opentable();
            if (isreback)
            {

                btnRBill_Click(btnRBill, e);
                isreback = false;
            }

            _dishesTimer.OrderID = lblZd.Text.Replace("帐单：", "");
            _dishesTimer.Start();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (iswm)
            {
                RefreshAmount();
            }
            else
            {
                try
                {
                    HideOpenTable();
                }
                catch { }
                opentable();
                //如果台没有开，显示开台界面 //
                if (Globals.CurrTableInfo.status.Equals(0))
                {
                    showOpenTable();

                }
            }

            if (string.IsNullOrEmpty(_dishesTimer.OrderID))
            {
                _dishesTimer.OrderID = lblZd.Text.Replace("帐单：", "");
                _dishesTimer.Start();
            }
        }

        private void btnml_Click(object sender, EventArgs e)
        {
            setamountml();
        }
        private void setamountml()
        {
            var temp = (float)Math.Round(Globals.CurrTableInfo.amount - amountgz2 - amountym, 2);
            if (!maling || Globals.roundinfo.Itemid.Equals("0"))
            {
                ysamount = temp;
                amountml = 0;
                amountroundtz = 0;
                return;
            }

            if (Globals.roundinfo.Itemid.Equals("1")) //四舍五入
            {
                amountml = 0;
                if (Globals.roundinfo.Roundtype.Equals("0")) //0 分
                    ysamount = (float)Math.Round(temp, 1, MidpointRounding.AwayFromZero);
                else if (Globals.roundinfo.Roundtype.Equals("1")) //1  角
                    ysamount = (float)Math.Round(temp, 0, MidpointRounding.AwayFromZero);
                else if (Globals.roundinfo.Roundtype.Equals("2")) //2  元
                    ysamount = (float)Math.Round(temp / 10, 0, MidpointRounding.AwayFromZero) * 10;
                amountroundtz = (float)Math.Round(temp - ysamount, 2);
            }
            else//抹零。
            {
                if (Globals.roundinfo.Itemid.Equals("2"))//抹零
                {
                    if (Globals.roundinfo.Roundtype.Equals("0"))//0 分
                        ysamount = (float)Math.Round(Math.Floor(temp * 10) / 10, 2);
                    else if (Globals.roundinfo.Roundtype.Equals("1"))//1  角
                        ysamount = (float)Math.Round(Math.Floor(temp), 2);
                    else if (Globals.roundinfo.Roundtype.Equals("2"))//2  元
                        ysamount = (float)Math.Round(Math.Floor(temp / 10) * 10, 2);
                    amountml = (float)Math.Round(temp - ysamount, 2);
                }
            }
            ysamount = Math.Max(0, (float)Math.Round(ysamount, 2));
        }
        private void edtAmount_EditValueChanging(object sender, EventArgs e)
        {

        }

        private void edtGzAmount_Enter(object sender, EventArgs e)
        {
            focusedt = edtGz;
        }

        private void edtGzAmount_EditValueChanged(object sender, EventArgs e)
        {
            getAmount();
        }

        private void dgvBill_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //

        }

        private void frmPosMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            //
            if (e.KeyChar == 59)
            {
                //
                e.KeyChar = (char)0;
                if (!CheckCallBill())
                {
                    xtraTabControl1.SelectedTabPageIndex = 2;
                    edtMemberCard.Text = "";
                    edtMemberCard.Focus();
                }
            }
        }

        private void QueryMemberCard()
        {
            //多次重试获取
            btnSelect.Visible = false;
            string tmpmember = edtMemberCard.Text;
            if (tmpmember.IndexOf('=') > 0)
            {
                tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                edtMemberCard.Text = tmpmember;
            }
            if (tmpmember.Trim().Length <= 0)
            {
                //如果会员为空 ，就是取消会员价
                //调用会员价接口取消会员价
                int membersystem = RestClient.getMemberSystem();
                if (membersystem == 1)
                {
                    try
                    {
                        CanDaoMemberClient.MemberLogout(Globals.CurrOrderInfo.orderid, tmpmember);
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        RestClient.setMemberPrice2(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid);
                    }
                    catch { }
                }

                edtMemberCard.Text = "";
                edtMember.Text = "";
                edtJf.Text = "";
                btnFind.Tag = 0;
                btnFind.Text = "登录";
                lblMember.Text = String.Format("会员：{0}", "");
                opentable();
                return;
            }
            if (edtMemberCard.Text.Trim().ToString().Length <= 0)
            {
                return;
            }
            string msg = "";
            //重试3次
            lblMsg.Text = "";
            var ret = false;
            int index = 1;
            do
            {
                ret = QueryMemberCard2(out msg);
            } while (!ret && index++ < 3);

            if (!ret)
            {
                Warning(msg);
                edtMemberCard.SelectAll();
                edtMemberCard.Focus();
                return;
            }
            membercard = edtMemberCard.Text;
            btnFind.Tag = 1;
            btnFind.Text = "退出";
            lblMsg.Text = "";
            lblMember.Text = String.Format("会员：{0}", edtMemberCard.Text);
            //查询成功
            if (cardno.IndexOf(',') > 0)
            {
                btnSelect.Visible = true;
            }
        }

        private void setlblMsg(string msg)
        {
            int membersystem = RestClient.getMemberSystem();
            lblMsg.Text = String.Format(membersystem == 1 ? "餐道：{0}" : "雅座：{0}", msg);
        }

        private bool QueryMemberCard2(out string msg)
        {
            int membersystem = RestClient.getMemberSystem();
            if (membersystem == 1)
            {
                //餐道会员
                bool ret = QueryMemberCard2_CanDao(out msg);
                if (ret)
                {
                    //调用会员价接口
                    try
                    {
                        CanDaoMemberClient.MemberLogin(Globals.CurrOrderInfo.orderid, edtMemberCard.Text.Trim().ToString());
                        Opentable2();
                    }
                    catch { }
                    if (iswm)
                    {
                        ///如果是外卖把本地的价格变成会员价;
                        t_shopping.setMemberPrice(ref Globals.ShoppTable);
                        ShowTotal();
                    }
                }
                return ret;
            }
            label15.Text = string.Format("储值余额：{0}", 0);
            label9.Text = string.Format("积分余额：{0}", 0);
            membercard = "";
            //会员查询,
            string tmpmember = edtMemberCard.Text;
            if (tmpmember.IndexOf('=') > 0)
            {
                tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                edtMemberCard.Text = tmpmember;
            }
            JObject json = null;
            try
            {
                json = (JObject)WebServiceReference.RestClient.QueryBalance(edtMemberCard.Text.Trim().ToString());
            }
            catch (Exception ex) { msg = "雅座会员查询失败，请检查外网是否稳定，并重试!"; return false; }
            //JObject json = (JObject)WebServiceReference.RestClient.QueryBalance(edtMemberCard.Text.Trim().ToString());
            string data = json["Data"].ToString();
            textEdit1.Text = "";
            if (data == "0")
            {

                msg = json["Info"].ToString();
                return false;
            }
            else
            {
                string str = json["psStoredCardsBalance"].ToString();
                try
                {
                    psStoredCardsBalance = float.Parse(str) / (float)100.00;

                }
                catch
                {
                    msg = "获取余额失败,请重试...";
                    return false;
                }
                str = json["psIntegralOverall"].ToString();
                try
                {
                    psIntegralOverall = float.Parse(str) / (float)100.00;

                }
                catch
                {
                    msg = "获取余额失败,请重试...";
                    return false;
                }
                getTicketList(json["psTicketInfo"].ToString());
                if (xtraCoupon.SelectedTabPageIndex != 5)
                {
                    xtraCoupon.SelectedTabPageIndex = 5;
                }
                else
                {
                    //pnlz
                    xtraTabControl2_SelectedPageChanged(xtraCoupon, null);
                }
                label15.Text = string.Format("储值余额：{0}", psStoredCardsBalance);
                label9.Text = string.Format("积分余额：{0}", psIntegralOverall);
                membercard = edtMemberCard.Text.Trim().ToString();
                Globals.CurrOrderInfo.memberno = membercard;
                try
                {
                    cardno = json["pszTrack2"].ToString();
                }
                catch { }
                //调用会员价接口
                try
                {
                    RestClient.setMemberPrice(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, edtMemberCard.Text.Trim().ToString());
                    Opentable2();
                }
                catch { }
                if (iswm)
                {
                    ///如果是外卖把本地的价格变成会员价;
                    t_shopping.setMemberPrice(ref Globals.ShoppTable);
                    ShowTotal();
                }
                //如果是一号多卡，弹出框选择一张卡号再根据卡号查询
            }
            edtMember.Focus();
            edtMember.SelectAll();
            msg = "ok";
            return true;
        }

        private bool QueryMemberCard3(out string msg)
        {
            //会员查询,
            int membersystem = RestClient.getMemberSystem();
            if (membersystem == 1)
            {
                //餐道会员
                return QueryMemberCard3_CanDao(out msg);
            }
            string tmpmember = edtMemberCard.Text;
            if (tmpmember.IndexOf('=') > 0)
            {
                tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                edtMemberCard.Text = tmpmember;
            }
            JObject json = null;
            try
            {
                json = (JObject)WebServiceReference.RestClient.QueryBalance(edtMemberCard.Text.Trim().ToString());
            }
            catch (Exception ex) { msg = "雅座会员查询失败，请检查外网是否稳定，并重试!"; return false; }
            //JObject json = (JObject)WebServiceReference.RestClient.QueryBalance(edtMemberCard.Text.Trim().ToString());
            string data = json["Data"].ToString();
            textEdit1.Text = "";
            if (data == "0")
            {

                msg = json["Info"].ToString();
                return false;
            }
            else
            {
                string str = json["psStoredCardsBalance"].ToString();
                try
                {
                    psStoredCardsBalance = float.Parse(str) / (float)100.00;

                }
                catch
                {
                    msg = "获取余额失败,请重试...";
                    return false;
                }
                str = json["psIntegralOverall"].ToString();
                try
                {
                    psIntegralOverall = float.Parse(str) / (float)100.00;

                }
                catch
                {
                    msg = "获取余额失败,请重试...";
                    return false;
                }
                getTicketList(json["psTicketInfo"].ToString());

            }
            msg = "ok";
            return true;
        }
        private void getTicketList(String psTicketInfo)
        {
            string tickstr = psTicketInfo;
            string[] Ticks = null;
            Ticks = tickstr.Split(new char[] { ';' });
            if (tickstr.Trim().ToString().Length <= 0)
            {
                Array.Resize(ref pszTicketList, 0);
            }
            else
            {
                try
                {
                    Array.Resize(ref pszTicketList, Ticks.Length);
                    string[] tick = null;
                    String preCoupon_code = "";
                    for (int i = 0; i < Ticks.Length; i++)
                    {
                        string str = Ticks[i].ToString();
                        tick = str.Split(new char[] { '|' });
                        if (preCoupon_code != tick[0])
                        {
                            pszTicketList[i].Coupon_code = tick[0];
                            pszTicketList[i].Coupons_Name = tick[3];
                            pszTicketList[i].Coupon_Amount = 0;
                            try
                            {
                                String tickprice = (float.Parse(tick[1]) / 100.00).ToString();
                                pszTicketList[i].Coupon_Amount = float.Parse(tickprice);
                            }
                            catch { }
                            pszTicketList[i].Coupon_No = tick[4];
                            pszTicketList[i].Coupon_NoAmount = 0;
                            pszTicketList[i].Copon_Type = "0";// tick[2];
                            preCoupon_code = tick[0];
                        }
                    }
                }
                catch { }
            }
        }
        private void querymember()
        {
            try
            {
                QueryMemberCard();
            }
            catch { }

            //登录会员重新获取半价优惠
            //先删除原来的banktype=101的优惠
            if (membercard.Length <= 0)
                return;
            isaddFavorale = false;
            isaddmember = false;

            try
            {
                try
                {
                    foreach (DataRow dr in tbyh.Rows)
                    {
                        if (dr["banktype"].ToString() == "101")
                        {
                            //tbyh.Rows.Remove(dr);
                            amountym = amountym - float.Parse(dr["freeamount"].ToString());
                            dr.Delete();
                        }
                    }
                }
                catch { }
                try
                {
                    tbyh.AcceptChanges();
                }
                catch { }
                try
                {
                    foreach (DataRow dr in tbyh.Rows)
                    {
                        if (dr["banktype"].ToString() == "101")
                        {
                            //tbyh.Rows.Remove(dr);
                            amountym = amountym - float.Parse(dr["freeamount"].ToString());
                            dr.Delete();
                        }
                    }
                }
                catch { }
                try
                {
                    tbyh.AcceptChanges();
                }
                catch { }
                try
                {
                    foreach (DataRow dr in tbyh.Rows)
                    {
                        if (dr["banktype"].ToString() == "101")
                        {
                            amountym = amountym - float.Parse(dr["freeamount"].ToString());
                            //tbyh.Rows.Remove(dr);
                            dr.Delete();
                        }
                    }
                }
                catch { }
                try
                {
                    tbyh.AcceptChanges();
                }
                catch { }
            }
            catch { }
            try
            {

                try
                {
                    addAutoFavorale();
                }
                catch { }
                getAmount();
            }
            catch { }

            //如果是一卡多号
            if (cardno.IndexOf(',') > 0)
            {
                //弹出框选择一张卡号,并按新卡号重新查询

            }
        }
        private void edtMemberCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                querymember();
            }
        }

        private void dgvBill_Click(object sender, EventArgs e)
        {
            //
        }

        private void dgvBill_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void dgvBill_MouseClick(object sender, MouseEventArgs e)
        {
            pnlMore.Visible = false;
            //dgvBill.Select();
            /*DataGridViewCell sc = dgvBill.CurrentCell;
            if (e.Location.Y > dgvBill.Height/2)
            {
                SendKeys.Send("{PGDN}");
            }
            else { SendKeys.Send("{PGUP}"); }

            dgvBill.CurrentCell = sc;*/
        }

        private void button10_Click_1(object sender, EventArgs e)
        {

        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string msg = "";
                pnlyh.Parent = xtraCoupon.SelectedTabPage;
                for (int i = 0; i <= btncount - 1; i++)
                {
                    string btnname = "btnyh" + (i + 1).ToString();
                    Button btn = getbtn(btnname);
                    if (btn != null)
                    {
                        btn.Tag = null;
                        btn.Text = "";
                    }
                }
                if (xtraCoupon.SelectedTabPageIndex == 5)
                {
                    if (edtMemberCard.Text.Length > 0)
                    {
                        if ((pszTicketList == null) || (pszTicketList.Count() <= 0))
                        {
                            //重试3次
                            bool ret = QueryMemberCard3(out msg);
                            if (!ret)
                            {
                                setlblMsg(msg);
                                Thread.Sleep(1000);
                                ret = QueryMemberCard3(out msg);
                            }
                            if (!ret)
                            {
                                Thread.Sleep(1000);
                                ret = QueryMemberCard3(out msg);
                            }
                            if (!ret)
                            {
                                Warning(msg);
                                return;
                            }
                        }
                    }
                }
                getCoupon_rule();
                xtraTabControl1_SelectedPageChanged(xtraTabControl1, null);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        ///优惠列表功能
        ///getcoupon_rule
        ///

        private void setupdownbtn()
        {
            if (pagecount <= 1)
            {
                btnup.Enabled = false;
                btndown.Enabled = false;
            }
            else
            {
                if (pagecurr <= 1) { btnup.Enabled = false; btndown.Enabled = true; }
                else
                    if (pagecurr >= pagecount) { btnup.Enabled = true; btndown.Enabled = false; }
                    else { btnup.Enabled = true; btndown.Enabled = true; }
            }
        }
        private void getCoupon_rule()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Update();//必须
                //xtraTabControl2.SelectedTabPageIndex 如果是会员优惠券，那么刷卡后再显示
                if (xtraCoupon.SelectedTabPageIndex == 5)
                {
                    try
                    {
                        jarrTables.Clear();
                        jarrTables = getMemberCoupon_rule();

                    }
                    catch (Exception e) { }
                }
                else
                {
                    try
                    {
                        jarrTables.Clear();
                    }
                    catch { }
                    try
                    {
                        jarrTables = RestClient.getcoupon_rulev2(xtraCoupon.SelectedTabPage.Tag.ToString(), Globals.CurrOrderInfo.orderid);
                    }
                    catch (Exception e) { }
                }
                pagecount = 0;
                pagecurr = 1;
                if (jarrTables == null)
                    return;
                pagecount = jarrTables.Count / btncount;
                lastpagecount = jarrTables.Count % btncount;
                if (lastpagecount > 0)
                    pagecount = pagecount + 1;
                FillBtn(pagecurr);
                Update();
                setupdownbtn();
                //CreateTableArr();
                //UpdateTableStatus();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private JArray getMemberCoupon_rule()
        {
            //会员优惠
            JArray jr = new JArray();
            jr.Clear();
            if (pszTicketList == null)
                return jr;
            string jsonsstr = "[";
            StringBuilder jsonstr = new StringBuilder("");
            int i = 0;
            //Coupons_Name,Coupon_code,Coupon_Amount,Coupon_No
            //description,ruleid,freeamount,1
            foreach (pszTicket c in pszTicketList)
            {
                i = i + 1;
                if (c.Coupon_No != null)
                {
                    jsonstr.Clear();
                    jsonstr.Append("{");
                    jsonstr.Append("\"unitid\": null,");
                    jsonstr.Append("\"comsumeway\": null,");
                    int Coupon_No = int.Parse(c.Coupon_No); //数量
                    jsonstr.Append("\"dishnum\": " + Coupon_No.ToString() + ",");
                    jsonstr.Append("\"couponrate\": 0,");
                    string Coupons_Name = "";  //名称
                    Coupons_Name = String.Format("会:{0}[{1}张]", c.Coupons_Name, c.Coupon_No);
                    jsonstr.Append("\"couponname\": \"" + Coupons_Name + "\",");
                    jsonstr.Append("\"couponcash\": null,");
                    jsonstr.Append("\"couponamount\": null,");

                    jsonstr.Append("\"description\": \"" + Coupons_Name + "\",");
                    jsonstr.Append("\"begintime\": 1419927481000,");
                    jsonstr.Append("\"couponway\": null,");
                    jsonstr.Append("\"couponnum\": null,");
                    jsonstr.Append("\"freedishnum\": null,");

                    jsonstr.Append("\"ruleid\": \"" + c.Coupon_code + "\",");
                    jsonstr.Append("\"wholesingle\": \"1\",");
                    float Coupon_Amount = c.Coupon_Amount; //券金额
                    jsonstr.Append("\"freeamount\": " + Coupon_Amount.ToString() + ",");
                    jsonstr.Append("\"groupweb\": null,");
                    jsonstr.Append("\"couponid\": \"" + c.Coupon_code + "\",");
                    jsonstr.Append("\"banktype\": 100,"); //100表示是会员
                    jsonstr.Append("\"inserttime\": 1419927367000,");
                    jsonstr.Append("\"dishid\": null,");
                    jsonstr.Append("\"endtime\": 1420013883000,");
                    jsonstr.Append("\"partnername\": \"0\",");
                    jsonstr.Append("\"totalamount\": null,");
                    int Copon_Type = int.Parse(c.Copon_Type);
                    jsonstr.Append("\"type\": " + Copon_Type.ToString() + ",");
                    jsonstr.Append("\"freedishid\": null,");
                    jsonstr.Append("\"debitamount\": 0");
                    if (i < pszTicketList.Length)
                    {
                        jsonstr.Append(" },");
                    }
                    else
                    {
                        jsonstr.Append(" }");
                    }
                    jsonsstr = jsonsstr + jsonstr.ToString();
                }
                //把优惠券列表转换成通用优惠json数组

            }
            try
            {
                /*String str = jsonstr.ToString();
                string str2=str.Substring(jsonstr.Length - 1, 1);
                if (str2.Equals(","))
                { jsonsstr = jsonsstr.Substring(0, jsonsstr.Length); }*/
                jsonsstr = jsonsstr + "]";
                jr = (JArray)JsonConvert.DeserializeObject(jsonsstr);
            }
            catch { }
            return jr;
        }
        private Button getbtn(string btnname)
        {
            foreach (Control c in pnlyh.Controls)
            {
                if ((c is Button) && (c.Name == btnname))
                {
                    return (Button)c;
                }
            }
            return null;
        }
        private void FillBtn(int page)
        {
            JObject ja = null;
            VCouponRule vcr = null;
            //jarrTables.Count
            //当前页
            int addi = (page - 1) * btncount;

            for (int i = 0; i <= btncount - 1; i++)
            {
                if (i + addi > jarrTables.Count - 1)
                    return;
                ja = (JObject)jarrTables[i + addi];
                try
                {
                    vcr = VCouponRule.Parsev2(ja);
                }
                catch
                {
                    vcr = VCouponRule.Parse(ja);
                }
                string btnname = "btnyh" + (i + 1).ToString();
                Button btn = getbtn(btnname);
                if (btn != null)
                {
                    btn.Tag = vcr;
                    btn.Text = vcr.couponname;
                    if (!string.IsNullOrEmpty(vcr.Color))
                    {
                        int argb = -1;
                        if (int.TryParse(vcr.Color.TrimStart('#'), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out argb))
                            btn.BackColor = Color.FromArgb(argb);
                    }
                }
            }
        }
        private void checkjarrTables()
        {
            if (jarrTables == null)
            {

            }
        }

        private void btnyh1_Click(object sender, EventArgs e)
        {
            _longPressTimer.Stop();
            if (_isLongPressModel)
                return;

            if (Globals.CurrTableInfo.amount <= 0)
            {
                Warning("帐单还未下单,不能使用优惠...");
                return;
            }

            //根据按钮属性相应优惠
            VCouponRule vcr = null;
            VCouponRule vcr2 = null;
            try
            {
                vcr2 = (VCouponRule)((Button)sender).Tag;
            }
            catch { vcr2 = null; }
            if (vcr2 == null)
                return;

            Boolean notdiscount = true;
            vcr = (VCouponRule)vcr2.Clone();
            if (vcr.banktype.Equals("07"))//手工优免特殊处理
            {
                float amount;
                if (vcr.FreeReason == "0")
                {
                    var giftDishWnd = new SelectGiftDishWindow(Globals.OrderTable, tbyh);
                    if (giftDishWnd.ShowDialog() == true)
                    {
                        foreach (GiftDishInfo giftDishInfo in giftDishWnd.SelectedGiftDishInfos)
                        {
                            vcr.couponname = string.Format("赠菜：{0}", giftDishInfo.DishName);
                            vcr.freeamount = giftDishInfo.DishPrice;
                            addrow(vcr, 6, false, giftDishInfo.SelectGiftNum);
                        }
                    }
                }
                else
                {
                    string inputString;
                    string msg = null;
                    var inputType = (frmInputText.EnumInputType)(Convert.ToInt32(vcr.FreeReason));
                    if (!frmInputText.ShowInputAmount3(inputType, out inputString))
                        return;

                    try
                    {
                        amount = float.Parse(inputString);
                    }
                    catch (Exception ex)
                    {
                        AllLog.Instance.E(string.Format("转换输入金额\"{0}\"时异常", inputString), ex);
                        amount = 0;
                    }

                    if (vcr.FreeReason == "1")//折扣
                    {
                        notdiscount = false;
                        float disrate = float.Parse(inputString) / 10f;
                        float preferentialAmt = amountgz2 + amountym;
                        bool isok = RestClient.usePreferentialItem(vcr.ruleid, disrate, Globals.CurrOrderInfo.orderid, vcr.banktype, vcr.sub_type, ref msg, ref amount, preferentialAmt);
                        if (!isok)
                        {
                            Warning(msg);
                            return;
                        }
                        amount = (float)Math.Round((double)amount, 2);
                        vcr.couponrate = Convert.ToDecimal(disrate);
                    }

                    if (amount > payamount)
                    {
                        Warning("请输入正确的优免金额!");
                        return;
                    }
                    if (amount <= 0)
                    {
                        Warning("请输入正确的优免金额!");
                        return;
                    }
                    vcr.freeamount = (decimal)amount;
                    addrow(vcr, 6, false, 1);
                    vcr.freeamount = 0;
                }
            }
            else if (vcr.banktype.Equals("02") || vcr.banktype.Equals("01") || (vcr.banktype.Equals("09") && vcr.couponrate > 0) || (vcr.banktype.Equals("08") && vcr.couponrate > 0))
            {
                if (!AskQuestion("确定使用：" + vcr.couponname))
                    return;

                //整单折扣用本地优免的方法实现
                notdiscount = false;
                ysamount = Globals.CurrTableInfo.amount - amountgz2 - amountym - amountml;
                float amount = 0;
                string msg = "";
                float disrate = 0;
                float preferentialAmt = amountgz2 + amountym;
                bool isok = RestClient.usePreferentialItem(vcr.ruleid, disrate, Globals.CurrOrderInfo.orderid,
                    vcr.banktype, vcr.sub_type, ref msg, ref amount, preferentialAmt);
                //调用接口获取ysamount * (float)(1 - (double)vcr.couponrate / 100.00);
                if (!isok)
                {
                    Warning(msg);
                    return;
                }
                amount = (float)Math.Round((double)amount, 2);
                vcr.freeamount = (decimal)amount;
                addrow(vcr, 6, false, 1);
                vcr.freeamount = 0;
            }
            else
            {
                if ((vcr.debitamount == -1) || (vcr.freeamount == 999999))
                {
                    //如果挂帐填负1
                    string inputamount = "";
                    if (!frmInputText.ShowInputAmount("请输入挂帐金额", "金额：", out inputamount))
                        return;
                    float amount = 0;
                    try
                    {
                        amount = float.Parse(inputamount);
                    }
                    catch
                    {
                        amount = 0;
                    }
                    if (amount > payamount)
                    {
                        Warning("请输入正确的挂帐金额!");
                        return;
                    }
                    if (amount <= 0)
                    {
                        Warning("请输入正确的挂帐金额!");
                        return;
                    }
                    vcr.freeamount = 0;
                    vcr.debitamount = (decimal)amount;
                    addrow(vcr, 5, false, 1);
                    vcr.freeamount = 0;
                }
                else
                {
                    if (vcr.freeamount > 0 || vcr.debitamount > 0)
                    {
                        int intpuNum = 0;
                        if (!ShowInputNum(vcr.couponname, out intpuNum, int.Parse(vcr.dishnum.ToString())))
                            ////if (!AskQuestion("确定使用：" + vcr.couponname))
                            return;
                        //是挂帐和优免 加入结算方式,结算时一起提交给结算接口
                        if (vcr.banktype == "100")
                        {
                            //会员
                            //如果选择的券已经有，那么提示不能再加
                            if (!canAddRow(vcr.ruleid))
                            {
                                Warning("券已经选择过，不能再加入!");
                                return;
                            }

                            var type = vcr.freeamount > 0 ? 6 : 5;
                            addrow(vcr, type, true, intpuNum);
                        }
                        else
                        {
                            for (var i = 0; i < intpuNum; i++)
                            {
                                var type = vcr.freeamount > 0 ? 6 : 5;
                                addrow(vcr, type, true, 1);
                            }
                        }
                    }
                    else if (vcr.freeamount <= 0 && vcr.debitamount <= 0)
                    {
                        //如果都是0就弹出窗口输入金额   输入返回的金额
                        string inputamount = "";
                        int type = 0;
                        if (!frmInputText.ShowInputAmount2("输入", "优免金额", out inputamount, out type))
                            return;
                        float amount = 0;
                        try
                        {
                            amount = float.Parse(inputamount);
                        }
                        catch
                        {
                            amount = 0;
                        }
                        if (type == 1)
                        {
                            //后台计算折扣
                            string msg = "";
                            amount = 0;
                            notdiscount = false;
                            double disrate1 = float.Parse(inputamount) / 10.00;
                            float disrate = float.Parse(disrate1.ToString());
                            float preferentialAmt = amountgz2 + amountym;
                            bool isok = RestClient.usePreferentialItem(vcr.ruleid, disrate,
                                Globals.CurrOrderInfo.orderid, vcr.banktype, vcr.sub_type, ref msg, ref amount,
                                preferentialAmt); //调用接口获取ysamount * (float)(1 - (double)vcr.couponrate / 100.00);
                            if (!isok)
                            {
                                Warning(msg);
                                return;
                            }
                            amount = (float)Math.Round((double)amount, 2);
                            vcr.couponrate = Convert.ToDecimal(disrate);
                        }
                        if (amount > payamount)
                        {
                            Warning("请输入正确的优免金额!");
                            return;
                        }
                        if (amount <= 0)
                        {
                            Warning("请输入正确的优免金额!");
                            return;
                        }
                        vcr.freeamount = (decimal)amount;
                        addrow(vcr, 6, false, 1);
                        vcr.freeamount = 0;
                    }
                }
            }

            getAmount();
            if (notdiscount == true)
            {
                //如果使用的是优免和挂帐那么检查前面有没有折扣类的优惠，如果有就提示
                checkDisCount();
            }

            xtraTabControl1_SelectedPageChanged(xtraTabControl1, null);
            //保存优惠内容,以便还原
            JArray ja = Globals.GetTableJson(tbyh);
            string str = ja.ToString();
            try
            {
                RestClient.saveOrderPreferential(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, str);
            }
            catch { }
        }
        private void checkDisCount()
        {
            foreach (DataRow dr in tbyh.Rows)
            {
                string couponratestr = dr["couponrate"].ToString();
                double couponrate = double.Parse(couponratestr);
                if (couponrate > 0)
                {
                    //
                    Warning("帐单已选择过折扣类优惠，请注意选择使用优惠的顺序!");
                    break;
                }
            }
        }
        private void addrow(VCouponRule vcr, int type, bool isauto, int num)
        {
            DataRow dr = tbyh.NewRow();
            //Coupons_Name,Coupon_code,Coupon_Amount,Coupon_No
            //description,ruleid,freeamount,1
            //vcr.memo,vcr.ruleid,vcr.freeamount,vcr.num
            string Coupons_Name = vcr.couponname; //优惠券名称 消费券1名称（不足30位时后面的补空格）
            if (Coupons_Name.IndexOf(":") > 0 && Coupons_Name.IndexOf("[") > 1)
            {
                Coupons_Name = Coupons_Name.Replace("会:", "");
                Coupons_Name = Coupons_Name.Substring(0, Coupons_Name.IndexOf("["));
            }
            if (!isauto)
            {
                dr["yhname"] = Coupons_Name;// +vcr.freeamount.ToString();
            }
            else
            {
                dr["yhname"] = Coupons_Name;
            }
            dr["partnername"] = vcr.partnername;
            dr["couponrate"] = vcr.couponrate;
            dr["freeamount"] = (vcr.freeamount * num);
            dr["debitamount"] = (vcr.debitamount * num);
            dr["num"] = num;
            dr["amount"] = (vcr.freeamount + vcr.debitamount) * num * -1;
            dr["couponsno"] = "";
            dr["memo"] = vcr.description;
            dr["type"] = type;
            dr["banktype"] = vcr.banktype;
            dr["ruleid"] = vcr.ruleid;
            dr["couponid"] = vcr.couponid;
            tbyh.Rows.Add(dr);
            this.dgvjs.ClearSelection();
            //
            //tbyh.DataSet
            amountgz2 = amountgz2 + (float)(vcr.debitamount * num);
            amountym = amountym + (float)(vcr.freeamount * num);
            amountgz2 = (float)Math.Round(amountgz2, 2);
            amountym = (float)Math.Round(amountym, 2);
            CheckGzYm();

        }
        private bool canAddRow(string ruleid)
        {
            foreach (DataRow dr in tbyh.Rows)
            {
                string banktype = dr["banktype"].ToString();
                if (banktype == "100")
                {
                    if (ruleid == dr["ruleid"].ToString())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void createTb()
        {
            tbyh.Columns.Clear();
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "优惠";
            column.ColumnName = "yhname";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "挂帐单位";
            column.ColumnName = "partnername";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "折扣";
            column.ColumnName = "couponrate";
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "优免";
            column.ColumnName = "freeamount";
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "挂帐";
            column.ColumnName = "debitamount";
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "备注";
            column.ColumnName = "memo";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "券号";
            column.ColumnName = "couponsno";
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AllowDBNull = false;
            column.Caption = "数量";
            column.ColumnName = "num";
            column.DefaultValue = 1;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Double");
            column.AllowDBNull = false;
            column.Caption = "金额";
            column.ColumnName = "amount";
            column.DefaultValue = 1;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.AllowDBNull = false;
            column.Caption = "type";
            column.ColumnName = "type"; //1 折扣  6 优免  5 挂帐
            column.DefaultValue = 0;
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "banktype";
            column.ColumnName = "banktype"; //100 是雅座会员卡里面的优惠券
            column.DefaultValue = "0";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "ruleid";
            column.ColumnName = "ruleid"; //100 是雅座会员卡里面的优惠券编号 
            column.DefaultValue = "0";
            tbyh.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.AllowDBNull = false;
            column.Caption = "couponid";
            column.ColumnName = "couponid"; //100 是雅座会员卡里面的优惠券编号 
            column.DefaultValue = "";
            tbyh.Columns.Add(column);

            DataView dv = new DataView(tbyh);
            dv.AllowNew = false;
            this.dgvjs.AutoGenerateColumns = false;
            this.dgvjs.DataSource = dv;


        }

        private void dgvjs_Enter(object sender, EventArgs e)
        {
            //dgvBill.Focus();
            //this.dgvjs.ClearSelection();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            dgvBill.Select();
            //SendKeys.Send("{PGDN}");
            SendKeys.Send("{PGUP}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dgvBill.Select();
            SendKeys.Send("{PGDN}");
            //SendKeys.Send("{PGUP}");
        }

        private void dgvjs_Click(object sender, EventArgs e)
        {
            //dgvBill.Focus();
            //this.dgvjs.ClearSelection();
        }

        private void edtYHCard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                edtCard.SelectAll();
                edtCard.Focus();
            }
        }

        private void edtMemberCard_Enter(object sender, EventArgs e)
        {
            focusedt = edtMemberCard;
        }

        private void dgvBill_MouseDown(object sender, MouseEventArgs e)
        {
            //
            downx = e.X;
            downy = e.Y;
        }

        private void dgvBill_MouseMove(object sender, MouseEventArgs e)
        {
            //
            movex = e.X - downx;
            movey = e.Y - downy;
        }

        private void dgvBill_MouseUp(object sender, MouseEventArgs e)
        {
            //
            movex = e.X - downx;
            movey = e.Y - downy;
            if (movey > 10)
            {
                button2_Click_1(button2, e);
            }
            if (movey < -10)
            {
                button1_Click(button1, e);
            }
        }

        private void dgvjs_MouseDown(object sender, MouseEventArgs e)
        {
            //
            downx = e.X;
            downy = e.Y;
        }

        private void dgvjs_MouseUp(object sender, MouseEventArgs e)
        {
            //
            movex = e.X - downx;
            movey = e.Y - downy;
            if (movey > 10)
            {
                button8_Click(button8, e);
            }
            if (movey < -10)
            {
                button7_Click(button8, e);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dgvjs.Select();
            SendKeys.Send("{PGUP}");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dgvjs.Select();
            SendKeys.Send("{PGDN}");
        }

        private void dgvBill_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= dgvBill.FirstDisplayedScrollingRowIndex)
            {
                using (SolidBrush b = new SolidBrush(dgvBill.RowHeadersDefaultCellStyle.ForeColor))
                {
                    int linen = 0;
                    linen = e.RowIndex + 1;
                    string line = linen.ToString();
                    e.Graphics.DrawString(line, e.InheritedRowStyle.Font, b, e.RowBounds.Location.X, e.RowBounds.Location.Y + 5);
                    SolidBrush B = new SolidBrush(Color.Red);
                }
            }
            if (e.RowIndex < 0)
                return;
            if (dgvBill.Rows.Count <= 0)
                return;
            try
            {
                DataRow dr = ((DataRowView)dgvBill.Rows[e.RowIndex].DataBoundItem).Row;
                string dishstatus = dr["dishstatus"].ToString();
                if (dishstatus.Equals("1"))
                {
                    // this.dgvBill.Rows[e.RowIndex].Cells[5].Value = iltDbgBill.Images[1];
                    e.Graphics.DrawImage(imgWidgh.Image, 200, e.RowBounds.Top, 78, 23);

                }
                else
                {
                    //取消绘制待称重图标

                }
            }
            catch { }
        }

        private void dgvjs_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (e.RowIndex >= dgvjs.FirstDisplayedScrollingRowIndex)
            {
                using (SolidBrush b = new SolidBrush(dgvjs.RowHeadersDefaultCellStyle.ForeColor))
                {
                    int linen = 0;
                    linen = e.RowIndex + 1;
                    string line = linen.ToString();
                    e.Graphics.DrawString(line, e.InheritedRowStyle.Font, b, e.RowBounds.Location.X, e.RowBounds.Location.Y + 5);
                    SolidBrush B = new SolidBrush(Color.Red);
                }
            }
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            //保存优惠内容,以便还原
            JArray ja = Globals.GetTableJson(tbyh);
            string str = ja.ToString();
            try
            {
                RestClient.saveOrderPreferential(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, str);
            }
            catch { }
        }

        private void edtJf_EditValueChanged(object sender, EventArgs e)
        {
            if (psIntegralOverall <= 0)
            {
                edtJf.Text = "";
            }
            getAmount();
        }

        private void edtJf_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (getamount >= payamount)
                {
                    button27_Click_1(btnPay, e);
                }
            }
        }

        private void edtJf_Enter(object sender, EventArgs e)
        {
            focusedt = edtJf;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            pnlz.Parent = xtraTabControl1.SelectedTabPage;
            pnlz.Left = pnlNum.Left;
            pnlz.Top = pnlNum.Top;
            pnlz.Visible = true;
            pnlNum.Visible = false;
        }

        private void button10_Click_2(object sender, EventArgs e)
        {
            pnlz.Visible = false;
            pnlNum.Visible = true;
        }

        private void PrintBill2()
        {
            if (iswm)
            {

            }
            else
            {
                if (Globals.OrderTable.Rows.Count <= 0)
                    return;
            }
            ReportPrint.PrintPayBill2(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName);
        }
        private void PrintBill3()
        {
            if (Globals.OrderTable.Rows.Count <= 0)
                return;
            ReportPrint.PrintPayBill3(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName);
        }
        private void btnSelectGz_Click(object sender, EventArgs e)
        {
            String gzname = "";
            String id = "";
            if (frmSelectGz.ShowSelectGz(out gzname, out id))
            {
                edtGz.Text = gzname;
                edtGz.Tag = id;
                edtGzAmount.Focus();
                edtGzAmount.SelectAll();
            }
        }

        private void button52_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                ReportPrint.PrintMemberPay1(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName,psIntegralOverall);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                pnlMore.Visible = false;
            }

        }

        private void button52_Click_1(object sender, EventArgs e)
        {
            try
            {
                button52.Enabled = false;
                RestClient.OpenCash();
            }
            finally
            {
                button52.Enabled = true;
            }
            //如果是IBM本地钱箱，如果本地目录有文件Cash.exe那么就调用 Cash.exe开启钱箱
        }

        private void lblAmount_Click(object sender, EventArgs e)
        {

        }

        private void edtPwd_Enter(object sender, EventArgs e)
        {
            focusedt = edtPwd;
        }
        /// <summary>
        /// 打开这个界面的时候自动计算优惠
        /// </summary>
        private void addAutoFavorale()
        {
            //先删除原来的banktype=101的优惠
            /*foreach(DataRow dr in tbyh.Rows)
            {
                if(dr["banktype"].ToString()=="101")
                {
                    tbyh.Rows.Remove(dr);
                }
            }
            foreach (DataRow dr in tbyh.Rows)
            {
                if (dr["banktype"].ToString() == "101")
                {
                    tbyh.Rows.Remove(dr);
                }
            }
            foreach (DataRow dr in tbyh.Rows)
            {
                if (dr["banktype"].ToString() == "101")
                {
                    tbyh.Rows.Remove(dr);
                }
            }*/
            JArray jrOrder = null;
            JArray jsList = null;
            JArray jsDouble = null;
            string printuser = Globals.UserInfo.UserID;
            string jsorder = Globals.CurrOrderInfo.orderid;
            if (jsorder.Trim().ToString().Length <= 0)
            {
                jsorder = " ";
            }
            try
            {
                if (!RestClient.getFavorale(printuser, jsorder, out jrOrder, out jsList, out jsDouble))
                {
                    return;
                }
            }
            catch { }
            //第二份半价
            string couponname = "";
            string partnername = "";
            float freeamount = 0;
            int num = 0;
            if (jrOrder != null)
            {
                if (jrOrder.Count > 0)
                {
                    if (!isaddFavorale)
                    {
                        JObject ja = (JObject)jrOrder[0];
                        string decorderprice = ja["decorderprice"].ToString();
                        if (decorderprice.Length > 0)
                        {
                            num = int.Parse(ja["decdishnum"].ToString());
                            if (num > 0)
                            {
                                couponname = ja["couponname"].ToString();
                                freeamount = float.Parse(ja["decorderprice"].ToString());
                                addFavorale(couponname, freeamount, num, "0");
                                isaddFavorale = true;
                            }
                        }
                    }
                }
            }
            if (jsList != null)
            {
                if (jsList.Count > 0)
                {
                    JObject ja = (JObject)jsList[0];
                    string decorderprice = ja["decorderprice"].ToString();
                    if (decorderprice.Length > 0)
                    {
                        num = int.Parse(ja["decdishnum"].ToString());
                        if (num > 0)
                        {
                            couponname = ja["couponname"].ToString();
                            freeamount = float.Parse(ja["decorderprice"].ToString());
                            addFavorale(couponname, freeamount, num, RestClient.getYhID());
                        }
                    }
                }
            }
            //第二扎半价 和双拼鱼锅会员立减20
            //如果登录了会员
            try
            {
                if (isaddmember)
                {
                    return;
                }
                if (Globals.CurrOrderInfo.memberno == null)
                {
                    Globals.CurrOrderInfo.memberno = "";
                }
                if ((membercard.Length > 0) || Globals.CurrOrderInfo.memberno.Length > 0)
                {

                    if (jsDouble != null)
                    {
                        if (jsDouble.Count > 0)
                        {
                            JObject ja = (JObject)jsDouble[0];
                            string decorderprice = ja["decorderprice"].ToString();
                            if (decorderprice.Length > 0)
                            {
                                num = int.Parse(ja["decdishnum"].ToString());
                                if (num > 0)
                                {
                                    isaddmember = true;
                                    couponname = ja["couponname"].ToString();
                                    freeamount = float.Parse(ja["decorderprice"].ToString());
                                    addFavorale(couponname, freeamount, num, RestClient.getDoubleDishTicket());
                                }
                            }
                        }
                    }

                }
            }
            catch { }
        }

        private void addFavorale(string couponname, float freeamount, int num, string ruleid)
        {
            DataRow dr = tbyh.NewRow();
            string Coupons_Name = couponname; //优惠券名称 消费券1名称（不足30位时后面的补空格）
            dr["yhname"] = Coupons_Name;
            dr["partnername"] = 12;
            dr["couponrate"] = 0;
            dr["freeamount"] = freeamount * num;
            dr["debitamount"] = 0;
            dr["num"] = num;
            dr["amount"] = freeamount * num * -1;
            dr["couponsno"] = "";
            dr["memo"] = "半价";
            dr["type"] = 0;
            dr["banktype"] = "101";
            dr["ruleid"] = ruleid;//RestClient.getYhID();
            dr["couponid"] = ruleid;// RestClient.getYhID();
            tbyh.Rows.Add(dr);
            this.dgvjs.ClearSelection();
            //tbyh.DataSet
            //amountgz2 = amountgz2 + (float)(vcr.debitamount * num);
            amountym = amountym + (float)(freeamount * num);
            CheckGzYm();
        }
        /// <summary>
        /// 检查挂帐和优免金额是否已经大于应付
        /// </summary>
        private void CheckGzYm()
        {
            //如果挂帐和优免金额已经大于应收金额
            //如果挂帐就已经大于=应收
            //如果挂帐小于应收
            //decgz2 = 0;//挂帐负数
            //decym = 0;//优免负数
            payamount = Globals.CurrTableInfo.amount;//应付
            if ((amountgz2 + amountym) > payamount)
            {
                if (amountgz2 >= payamount)
                {
                    //加一笔负的优免，加一笔payamount-amountgz2的负数算 挂帐多收
                    decgz2 = payamount - amountgz2;
                    decym = amountym * -1;
                }
                else
                {
                    //优免减少
                    //加一笔负的优免 (amountgz2 + amountym)-payamount
                    decgz2 = 0;
                    decym = ((amountgz2 + amountym) - payamount) * -1;
                }
            }
            else
            {
                decgz2 = 0;
                decym = 0;
            }
        }
        public void hideFrm()
        {
            Hide();
        }
        private void tmrClose_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrClose.Enabled = false;
                //Close();
                hideFrm();
            }
            catch { }
        }

        private void CanClose()
        {
            int autoClose = RestClient.getAutoClose();
            if (autoClose > 0)
            {
                tmrClose.Enabled = false;
                tmrClose.Interval = autoClose * 1000;
                tmrClose.Enabled = true;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string cardnos = cardno;
            string selectcardno = "";
            if (frmSelectCardNo.ShowSelectCardNo(cardnos, out selectcardno))
            {
                edtMemberCard.Text = selectcardno;
                querymember();

            }
        }

        private void btnRePrint_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (!AskQuestion("台号：" + Globals.CurrTableInfo.tableName + "确定要重印结帐单吗?"))
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                PrintBill2();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                pnlMore.Visible = false;
            }
        }

        private void btnRePrintCust_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AskQuestion("台号：" + Globals.CurrTableInfo.tableName + "确定要重印客用单吗?"))
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                PrintBill3();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                pnlMore.Visible = false;
            }
        }
        public void ShoppingChange(object serder, EventArgs e)
        {
            ShowTotal();
        }
        public void ShowTotal()
        {
            decimal amount = t_shopping.getAmount(ref Globals.ShoppTable);
            Globals.CurrTableInfo.amount = (float)amount;
            lblAmountWm.Text = string.Format("消费:{0}", amount);
            lblAmount.Text = string.Format("消费:{0}", amount);
            lbTip.Text = string.Format("小费:{0}", Globals.CurrTableInfo.TipAmount);
            getAmount();
        }
        private void openfrmClose(object serder, EventArgs e)
        {
            try
            {
                HideOpenTable();
            }
            catch { }
            if (isopened)
            {
                isopened = false;
                ShowOrder();
            }
        }
        private void openRm(object serder, EventArgs e)
        {
            try
            {
                btnOpen_Click(serder, e);
            }
            catch { }
            //开台成功显示点菜界面
            isopened = true;
            IniPos.setPosIniVlaue(Application.StartupPath, "ORDER", Globals.CurrTableInfo.tableNo, "0");
        }
        /// <summary>
        /// 显示开台
        /// </summary>
        public void showOpenTable()
        {
            ///如果刷新的台还未开台就显示开台界面,已开台显示结算 POS开完台跳到堂食点菜界面
            if (frmopentable != null)
            {
                frmopentable.Close();
                frmopentable = null;
                //return;
            }
            frmopentable = new frmOpenTable();
            //frmopentable.Show();
            //frmopentable.Left = 343;
            //frmopentable.Top = 108;
            this.IsMdiContainer = true;
            frmopentable.MdiParent = this;
            frmopentable.Parent = pnlCash;
            pnlCash.Enabled = true;
            frmopentable.frmClose += new frmOpenTable.FrmClose(openfrmClose);
            frmopentable.openRm += new frmOpenTable.OpenRm(openRm);
            frmopentable.initData(currtableno);
            frmopentable.Show();
            frmopentable.tmrFocus.Enabled = true;
            xtraCoupon.Visible = false;
            xtraTabControl1.Visible = false;
            frmopentable.edtUserid.Focus();

        }

        public void HideOpenTable()
        {
            if (frmopentable == null)
                return;

            frmopentable.Hide();
            xtraCoupon.Visible = true;
            xtraTabControl1.Visible = true;
        }
        public void StartWm()
        {
            ///
            if (frmorder != null)
            {
                ShowWm();
                return;
            }
            frmorder = new frmOrder();
            frmorder.shoppingChange += new frmOrder.ShoppingChange(ShoppingChange);
            frmorder.accounts += new frmOrder.Accounts(ShowAccounts);
            frmorder.OrderRemarkChanged += FrmorderOnOrderRemarkChanged;
            this.IsMdiContainer = true;
            frmorder.MdiParent = this;
            frmorder.Parent = pnlCash;
            frmorder.btnZD.Visible = false;
            frmorder.Show();
            xtraCoupon.Visible = false;
            xtraTabControl1.Visible = false;
            pnlAmount.Visible = false;
            LbOrderMark.Visible = true;
            panel7.Visible = false;
            //btnOpen.Visible = false;
            //edtRoom.Enabled = false;
            //btnAdd.Visible = true;
            //btnDec.Visible = true;
            SetShowOrderFrm(true);
            //
            if (Globals.ShoppTable == null)
            {
                //创建购物车表
                t_shopping.createShoppTable(ref Globals.ShoppTable);
            }
            else
            {
                Globals.ShoppTable.Clear();
            }
            DataView dv = new DataView(Globals.ShoppTable);
            dv.AllowNew = false;
            this.dgvBill.AutoGenerateColumns = false;
            this.dgvBill.DataSource = dv;
            this.dgvBill.Tag = 1;
            ShowTotal();
        }

        private void FrmorderOnOrderRemarkChanged()
        {
            SetRemarkOrder(Globals.OrderRemark);
        }

        private void SetRemarkOrder(string remark)
        {
            LbOrderMark.Visible = !string.IsNullOrEmpty(remark);
            LbOrderMark.Text = string.Format("全单备注:{0}", remark);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            bool tmpwm = int.Parse(this.dgvBill.Tag.ToString()) == 1;
            if (tmpwm)
            {
                try
                {
                    if (dgvBill.SelectedRows.Count > 0)
                    {
                        DataRow dr = (this.dgvBill.SelectedRows[0].DataBoundItem as DataRowView).Row;
                        string dishid = dr["dishid"].ToString();
                        string dishunit = dr["dishunit"].ToString();
                        int dishStatus = RestClient.getFoodStatus(dishid, dishunit);
                        if (dishStatus == 1)
                        {
                            Warning("选择的菜品已沽清！");
                            return;
                        }
                        string primarydishtype = dr["primarydishtype"].ToString();
                        t_shopping.adddish(ref Globals.ShoppTable, dr);
                    }
                }
                catch { }
                ShowTotal();
                try
                {
                    frmorder.shoppingchange();
                }
                catch { }
            }
            else
            {
                if (dgvBill.SelectedRows.Count > 0)
                {
                    DataRow dr = (this.dgvBill.SelectedRows[0].DataBoundItem as DataRowView).Row;
                    int num = 1;
                    //如果选择的菜品是待称重，输入称重后数量
                    string dishstatus = dr["dishstatus"].ToString();
                    if (dishstatus.Equals("1"))
                    {
                        //修改称重数量
                        //修改称重数量
                        double maxnum2 = 100;
                        double num2 = 1;
                        if (ShowInputNum("请输入称重数量!", "称重数量：", out num2, maxnum2))
                        {
                            string dishid = dr["dishid"].ToString();
                            string primarykey = dr["primarykey"].ToString();
                            RestClient.updateDishWeight(Globals.CurrTableInfo.tableNo, dishid, primarykey, num2.ToString());
                            Opentable2();
                        }
                        return;
                    }
                    btnOrder2_Click(btnOrder2, e); //把刚加的菜加到购物车
                }
            }
        }

        private void btnDec_Click(object sender, EventArgs e)
        {
            bool tmpwm = int.Parse(this.dgvBill.Tag.ToString()) == 1;
            if (tmpwm)
            {
                if (dgvBill.SelectedRows.Count > 0)
                {
                    try
                    {
                        DataRow dr = (this.dgvBill.SelectedRows[0].DataBoundItem as DataRowView).Row;
                        t_shopping.decdish(ref Globals.ShoppTable, dr);
                    }
                    catch { }
                    ShowTotal();
                    try
                    {
                        frmorder.shoppingchange();
                    }
                    catch { }
                }
            }
            else
            {
                if (dgvBill.SelectedRows.Count == 0)
                    return;

                DataRow dr = (this.dgvBill.SelectedRows[0].DataBoundItem as DataRowView).Row;

                DeleDishes(dr);

                //double num = double.Parse(dr["dishnum"].ToString());
                //string dishid = dr["dishid"].ToString();
                //string dishunit = dr["dishunitSrc"].ToString();//dr["dishunit"].ToString();//国际化要使用原始单位。
                //string msg = "";
                ////如果选择的菜品是待称重，输入称重后数量
                //string dishstatus = dr["dishstatus"].ToString();
                //if (dishstatus.Equals("1"))
                //{
                //    //修改称重数量
                //    float maxnum2 = 100;
                //    if (ShowInputNum("请输入称重数量!", "称重数量：", out num, maxnum2))
                //    {
                //        string dishid2 = dr["dishid"].ToString();
                //        string primarykey = dr["primarykey"].ToString();
                //        RestClient.updateDishWeight(Globals.CurrTableInfo.tableNo, dishid2, primarykey, num.ToString());
                //        Opentable2();
                //    }
                //    return;
                //}

                //JArray ja = null;
                //if (!BackDish.getbackdish(Globals.CurrOrderInfo.orderid, dishid, dishunit, Globals.CurrTableInfo.tableNo, out msg, out ja))
                //{
                //    Warning(msg);
                //    return;
                //}
                //DataTable dt = Bill_Order.getBackDish_List(ja);
                //if (dt.Rows.Count <= 0)
                //{
                //    Warning("获取退菜信息错误!");
                //    return;
                //}
                //double maxnum = BackDish.getBackNum(dt);
                ////获取总共可退数量 如果是鱼锅和套餐， 那不用输入数量
                //DataRow dr2 = dt.Rows[0];
                //string ispot = dr2["ispot"].ToString(); //如果是锅底
                //dishstatus = dr2["dishstatus"].ToString();
                //string dishtype = dr2["dishtype"].ToString();
                //string ismaster = dr2["ismaster"].ToString();
                //string childdishtype = dr2["childdishtype"].ToString();
                //num = 1;
                //bool inputnum = true;
                ////如果是鱼锅
                //if (ispot.Equals("1"))
                //{
                //    inputnum = false;
                //}
                //if ((ismaster.Equals("1") && (dishtype.Equals("1"))))
                //{
                //    inputnum = false;
                //}
                ////如果是套餐
                //if (dishtype.Equals("2"))
                //{
                //    if (!childdishtype.Equals("2"))
                //    {
                //        Warning("请选择套餐名称退整个套餐!");
                //        return;
                //    }
                //    inputnum = false;
                //}
                //if (inputnum)  //还有套餐条件
                //{
                //    num = 0;
                //    if (!ShowInputNum("请输入退菜数量!", "退菜数量：", out num, maxnum))
                //        return;
                //}

                //string discardUserId;
                //if (!frmAuthorize.ShowAuthorize("退菜权限验证", Globals.UserInfo.UserID, "030102", out discardUserId))
                //    return;

                ////调用退菜接口
                //double backnum = num;
                //string discardReason = "";
                //var result = BackDish.backDish(Globals.CurrOrderInfo.orderid, Globals.CurrTableInfo.tableNo, discardUserId, Globals.UserInfo.UserID, dt, backnum, discardReason);

                //Opentable2();
                //if (!result)
                //{
                //    Warning("退菜失败!");
                //    return;
                //}

                //msgorderid = Globals.CurrOrderInfo.orderid; //广播消息到PAD同步菜单
                //ThreadPool.QueueUserWorkItem(t => { broadMsg2201(); });
                //try
                //{
                //    RestClient.DeletePosOperation(Globals.CurrTableInfo.tableNo);
                //}
                //catch (Exception ex)
                //{
                //    AllLog.Instance.E(ex);
                //}
                //Warning("退菜完成!");
            }
        }

        /// <summary>
        /// 退菜
        /// </summary>
        private void DeleDishes(DataRow selectRow)
        {
            double num = double.Parse(selectRow["dishnum"].ToString());
            string dishid = selectRow["dishid"].ToString();
            string dishunit = selectRow["dishunitSrc"].ToString();
            string msg = "";
            //如果选择的菜品是待称重，输入称重后数量
            string dishstatus = selectRow["dishstatus"].ToString();
            if (dishstatus.Equals("1"))
            {
                //修改称重数量
                float maxnum2 = 100;
                if (ShowInputNum("请输入称重数量!", "称重数量：", out num, maxnum2))
                {
                    string dishid2 = selectRow["dishid"].ToString();
                    string primarykey = selectRow["primarykey"].ToString();
                    RestClient.updateDishWeight(Globals.CurrTableInfo.tableNo, dishid2, primarykey, num.ToString());
                    Opentable2();
                }
                return;
            }


            double maxnum = double.Parse(selectRow["dishnum"].ToString());
            //获取总共可退数量 如果是鱼锅和套餐， 那不用输入数量

            string ispot = selectRow["ispot"].ToString(); //如果是锅底
            string dishtype = selectRow["dishtype"].ToString();
            string ismaster = selectRow["ismaster"].ToString();
            string childdishtype = selectRow["childdishtype"].ToString();
            num = 1;
            bool inputnum = true;
            //如果是鱼锅
            if (ispot.Equals("1"))
            {
                inputnum = false;
            }
            if ((ismaster.Equals("1") && (dishtype.Equals("1"))))
            {
                inputnum = false;
            }

            var wnd = new BackDishReasonSelectWindow();
            if (wnd.ShowDialog() != true)
                return;

            //如果是套餐
            if (dishtype.Equals("2"))
            {
                if (!childdishtype.Equals("2"))
                {
                    Warning("请选择套餐名称退整个套餐!");
                    return;
                }
                inputnum = false;
            }
            if (inputnum)  //还有套餐条件
            {
                num = 0;
                if (!ShowInputNum("请输入退菜数量!", "退菜数量：", out num, maxnum))
                    return;
            }

            string discardUserId;
            if (!frmAuthorize.ShowAuthorize("退菜权限验证", Globals.UserInfo.UserID, "030102", out discardUserId))
                return;

            //调用退菜接口
            double backnum = num;
            var result = BackDish.backDish(Globals.CurrOrderInfo.orderid, Globals.CurrTableInfo.tableNo, discardUserId, Globals.UserInfo.UserID, selectRow, backnum, wnd.SelectedReason);

            Opentable2();
            if (!result)
            {
                Warning("退菜失败!");
                return;
            }

            msgorderid = Globals.CurrOrderInfo.orderid; //广播消息到PAD同步菜单
            ThreadPool.QueueUserWorkItem(t => { broadMsg2201(); });
            try
            {
                RestClient.DeletePosOperation(Globals.CurrTableInfo.tableNo);
            }
            catch (Exception ex)
            {
                AllLog.Instance.E(ex);
            }
            Warning("退菜完成!");
        }

        public void ShowAccounts(object serder, EventArgs e, int ordertype)
        {
            //ordertype=1赠送
            //btnOrder.Visible = true;
            xtraTabControl1.SelectedTabPageIndex = 0;
            pnlCash.Enabled = true;
            xtraCoupon.Visible = true;
            xtraTabControl1.Visible = true;
            BtnMark.Visible = false;
            pnlAmount.Visible = true;
            LbOrderMark.Visible = false;
            panel7.Visible = true;
            SetShowOrderFrm(false);
            if (!iswm)
            {
                btnOpen.Visible = true;
            }
            //
            edtAmount.Focus();
            try
            {
                frmorder.Hide();
                Application.DoEvents();
                if (serder != null)
                {
                    if (!iswm)
                    {
                        //如果不是外卖就直接下单 //显示正在下单
                        btnOpen.Visible = true;
                        if (Globals.ShoppTable.Rows.Count > 0)
                        {
                            if (startorder(ordertype))
                            {
                                try
                                {
                                    msgorderid = Globals.CurrOrderInfo.orderid; //广播消息到PAD同步菜单
                                    Thread thread = new Thread(broadMsg2201);
                                    thread.Start();// 

                                }
                                catch { }
                                Globals.ShoppTable.Clear();
                                btnOpen_Click(serder, e);
                            }
                        }
                        else
                            btnOpen_Click(serder, e);
                    }
                }
            }
            catch { }
        }
        public void ShowWm()
        {
            btnOrder.Visible = false;
            xtraTabControl1.SelectedTabPageIndex = 0;
            pnlCash.Enabled = true;
            xtraCoupon.Visible = false;
            xtraTabControl1.Visible = false;
            pnlAmount.Visible = false;
            LbOrderMark.Visible = true;
            Globals.OrderRemark = "";//重置全单备注。
            SetRemarkOrder("");
            panel7.Visible = false;
            //btnOpen.Visible = false; 
            SetShowOrderFrm(true);
            frmorder.Show();
        }
        private void btnOrder_Click(object sender, EventArgs e)
        {
            ShowWm();
        }
        private void RefreshAmount()
        {
            tbyh.Clear();
            amountgz2 = 0;//优惠的挂帐
            amountym = 0;//优免
            amountml = 0;
            decym = 0;
            decgz2 = 0;
            Array.Resize(ref pszTicketList, 0);
            if (iswm)
            {
                ///还原会员价
                t_shopping.setPrice2(ref Globals.ShoppTable);
            }
            getAmount();
        }
        /// <summary>
        /// 初始化外卖台
        /// </summary>
        private void InitWm()
        {
            try
            {
                string TableName = edtRoom.Text;
                Globals.CurrTableInfo.amount = 0;
                lblAmountWm.Text = string.Format("消费:{0}", 0);
                lblAmount.Text = string.Format("消费:{0}", 0);
                lbTip.Text = "小费:0";
                //Globals.CurrOrderInfo.orderid = "";
                //Globals.CurrOrderInfo.orderstatus = 0;
                lblZd.Text = "帐单：";
                pnlCash.Enabled = true;
                edtRoom.Enabled = false;
                pbxWm.Visible = true;
                btnSelect.Visible = false;
                btnPrintMember1.Enabled = false;
                btnRePrint.Enabled = false;
                btnPrintBill.Enabled = false;
                btnOrder2.Enabled = false;
                btnRePrintCust.Enabled = false;
                tbyh.Clear();
                amountgz2 = 0;//优惠的挂帐
                amountym = 0;//优免
                amountml = 0;
                decym = 0;
                decgz2 = 0;
                btnRePrint.Enabled = false;
                //btnRePrintCust.Enabled = false;
                Globals.CurrOrderInfo.memberno = "";//btnRePrintCust
                edtMemberCard.Text = "";
                edtYHCard.Text = "";
                membercard = "";
                edtMember.Text = "";
                edtJf.Text = "";
                edtAmount.Text = "";
                edtWx.Text = "";
                edtWxAmount.Text = "";
                edtZfb.Text = "";
                edtZfbAmount.Text = "";
                edtCard.Text = "";
                edtPwd.Text = "";
                //rgpType.SelectedIndex = 0;
                edtGzAmount.Text = "";
                edtGz.Text = "";
                label15.Text = string.Format("储值余额：{0}", 0);
                label9.Text = string.Format("积分余额：{0}", 0);

                //rgpType.SelectedIndex = 0;
                isaddmember = false;
                isaddFavorale = false;
                lblMember.Text = String.Format("会员：{0}", "");
                xtraCoupon.SelectedTabPageIndex = 0;
                jarrTables.Clear();
                Array.Resize(ref pszTicketList, 0);
                //不支持预结单
                btnPrintBill.Enabled = false;
                //先取消会员价
                try
                {
                    //RestClient.setMemberPrice3(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid);
                }
                catch { }
                //单品部份折扣也要清除掉
                try
                {
                    //RestClient.fullDiscount(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserID, 0, " ", " ");
                }
                catch { }
                xtraTabControl1.SelectedTabPageIndex = 0;
            }
            catch (CustomException ex)
            {
                //this.SetButtonEnable(true);
                Warning(ex.Message);
            }
            catch (Exception ex)
            {
                //this.SetButtonEnable(true);
                //Warning("获取数据失败!");
            }
        }
        /// <summary>
        /// 开台并且下单
        /// </summary>
        private void openTableAndOrder()
        {
            //开台
            setOrder();
            //下单
            /*bool re = false;
            try
            {
                re = bookorder("1");
                if (!re)
                {
                    re = bookorder("2");
                }
                if (!re)
                {
                    re = bookorder("3");
                }
                if (!re)
                {
                    re = bookorder("4");
                }
                if (!re)
                {
                    re = bookorder("5");
                }
            }
            catch { }
            if (!re)
            {
                ///把台关掉，帐单删掉,让用户重点
                ///userid:string;orderid:string;tableno:string
                RestClient.cancelOrder(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, currtableno);
                Warning("下单失败，请检查网络!");

            }*/
        }
        ////以下为外卖开台，下单，
        /// <summary>
        /// 开台
        /// </summary>
        private void setOrder()
        {
            //getTakeOutTable
            string orderid = "";
            if (!RestClient.setorder(currtableno, Globals.UserInfo.UserID, ref orderid))
            {
                Warning("开台失败,1！");
                return;
            }
            //标记帐单的ordertpe=1为正常外卖
            try
            {
                RestClient.wmOrder(orderid);
            }
            catch { }
            Globals.CurrOrderInfo.orderid = orderid;
            Globals.CurrTableInfo.tableid = RestClient.getTakeOutTableID();
            //
        }

        /// <summary>
        /// 下单。
        /// </summary>
        /// <param name="ordertype"></param>
        /// <returns></returns>
        private string bookorder(int ordertype)
        {
            return RestClient.bookorder(Globals.ShoppTable, Globals.CurrTableInfo.tableNo, Globals.CurrOrderInfo.userid, Globals.CurrOrderInfo.orderid, 1, ordertype, Globals.OrderRemark);
        }

        private bool startorder(int ordertype)
        {
            DateTime dt = DateTime.Now;
            //下单序号
            string seqnostr = IniPos.getPosIniValue(Application.StartupPath, "ORDER", Globals.CurrTableInfo.tableNo, "0");
            if (Globals.OrderTable.Rows.Count == 0 && (Globals.cjSetting == null || Globals.cjSetting.Status == "1"))
            {
                //如果是第一次下单，如果餐具要收费，那么把餐具加入已点,一起下单
                try
                {
                    int cusNum = Globals.CurrOrderInfo.custnum != null ? Globals.CurrOrderInfo.custnum.Value : 0;
                    if (cusNum > 0)
                    {
                        string userid = !string.IsNullOrEmpty(Globals.CurrOrderInfo.userid) ? Globals.CurrOrderInfo.userid : Globals.UserInfo.UserID;
                        t_shopping.addCJ(Globals.cjFood, Globals.CurrTableInfo, Globals.CurrOrderInfo, userid, Globals.cjSetting, ref Globals.ShoppTable, cusNum);
                    }
                }
                catch { }
            }
            string seqno_str = "";
            try
            {
                int seqno = int.Parse(seqnostr) + 1;
                seqno_str = seqno.ToString();
            }
            catch { seqno_str = RestClient.getGUID(); }

            var result = bookorder(ordertype);
            if (!string.IsNullOrEmpty(result))
            {
                ShowOrder();
                Warning(result);
                return false;
            }

            IniPos.setPosIniVlaue(Application.StartupPath, "ORDER", Globals.CurrTableInfo.tableNo, seqno_str);
            Warning("下单完成");
            return true;
        }

        /// <summary>
        /// 外卖结算 需要先结算会员，如果失败就不下单不调用java结算
        /// </summary>
        private bool wmAccount(int ordertype)
        {
            bool isok = true;
            if (membercard.Length > 0)
            {
                isok = false;
                float psccash = amountrmb + amountyhk + amountgz;//现金 用于会员积分
                float pscpoint = amountjf; //使用积分付款
                float pszStore = amounthyk;//使用储值余额付款
                float tmppsccash = Math.Max(0, psccash - returnamount);

                //使用优惠券
                String tickstrs = getTicklistStr();
                if (tmppsccash > 0 || pscpoint > 0 || tickstrs.Length > 0 || amounthyk > 0)
                {
                    try
                    {
                        string pwd = "0";
                        if (edtPwd.Text.Trim().Length > 0)
                            pwd = edtPwd.Text.Substring(0, Math.Min(edtPwd.Text.Length, 6));

                        JObject json = RestClient.MemberSale(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, membercard, Globals.CurrOrderInfo.orderid, tmppsccash, pscpoint, 1, amounthyk, tickstrs, pwd, (float)Math.Round(memberyhqamount, 2));
                        string data = json["Data"].ToString();
                        if (data == "0")
                        {
                            try
                            {
                                string err = json["Info"].ToString();
                                if (err.IndexOf("密码不正确") > 0)
                                {
                                    Warning("卡号:" + cardno + err);
                                    edtPwd.Focus();
                                    edtPwd.SelectAll();
                                }
                                else
                                {
                                    Warning(err);
                                }
                            }
                            catch (Exception ex)
                            {
                                AllLog.Instance.E(ex);
                            }
                        }
                        else
                        {
                            isok = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        AllLog.Instance.E(ex);
                        Warning("会员积分，结算失败!");
                    }
                }
                else
                {
                    isok = true;
                }
            }

            if (isok)
            {
                //下单
                var result = bookorder(ordertype);
                if (!string.IsNullOrEmpty(result))
                {
                    RestClient.cancelOrder(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, currtableno);
                    Warning(result);
                    return false;
                }

                if (!RestClient.settleorder(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserID, getPayTypeJsonArray()).Equals("0"))
                {
                    Warning("结算失败...");
                    return false;
                }

                RestClient.caleTableAmount(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid); //计算账单总金额。
                try
                {
                    RestClient.debitamout(Globals.CurrOrderInfo.orderid);
                }
                catch (Exception ex)
                {
                    AllLog.Instance.E("计算实收接口异常。" + ex.Message);
                }
            }
            else
            {
                RestClient.cancelOrder(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, currtableno);
            }
            return isok;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvjs.RowCount <= 0)
                    return;
                DataRow dr = (this.dgvjs.SelectedRows[0].DataBoundItem as DataRowView).Row;
                int num = int.Parse(dr["num"].ToString());
                amountgz2 = (float)Math.Round(amountgz2 - (float)(float.Parse(dr["debitamount"].ToString())), 2);// * num
                amountym = (float)Math.Round(amountym - (float)(float.Parse(dr["freeamount"].ToString())), 2);// * num
                tbyh.Rows.Remove(dr);
                JArray ja = Globals.GetTableJson(tbyh);
                string str = ja.ToString();
                RestClient.saveOrderPreferential(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, str);
                CheckGzYm();
            }
            catch { }
            getAmount();
        }

        private void btndown_Click_1(object sender, EventArgs e)
        {
            checkjarrTables();
            try
            {
                if (jarrTables.Count <= 0)
                {
                    getCoupon_rule();
                }
            }
            catch { }
            if (pagecurr < pagecount)
            {
                clearBtn();
                pagecurr = pagecurr + 1;
                FillBtn(pagecurr);
                setupdownbtn();
            }
        }
        public void clearBtn()
        {
            for (int i = 0; i <= btncount - 1; i++)
            {
                string btnname = "btnyh" + (i + 1).ToString();
                Button btn = getbtn(btnname);
                if (btn != null)
                {
                    btn.Tag = 0;
                    btn.Text = "";
                }
            }
        }
        private void btnup_Click_1(object sender, EventArgs e)
        {
            checkjarrTables();
            if (pagecurr > 1)
            {
                clearBtn();
                pagecurr = pagecurr - 1;
                FillBtn(pagecurr);
                setupdownbtn();
            }
        }

        private void tmrOpenTable_Tick(object sender, EventArgs e)
        {
            try
            {
                tmrOpenTable.Enabled = false;
                showOpenTable();
            }
            catch { }
        }

        private void pnlMore_Click(object sender, EventArgs e)
        {
            pnlMore.Visible = false;
        }

        private void btnMore_Click(object sender, EventArgs e)
        {
            try
            {
                pnlMore.Visible = !pnlMore.Visible;
                //dgvjs.Visible = !pnlMore.Visible;
                pnlMore.BringToFront();
                pnlMore.Refresh();
                btnRePrint.BringToFront();
                btnRePrintCust.BringToFront();
                btnPrintMember1.BringToFront();
                btnCancelOrder.BringToFront();
                pnlMore.BringToFront();
                pnlMore.Refresh();
            }
            finally { }
        }

        private void btnOrder2_Click(object sender, EventArgs e)
        {

            if (iswm)
                ShowWm();
            else
            {
                //获取餐台最新的下单IDgetOrderSequence
                string serverSequence = "";
                if (RestClient.getOrderSequence(Globals.CurrTableInfo.tableNo, out serverSequence))
                {
                    try
                    {
                        int localSequence = int.Parse(IniPos.getPosIniValue(Application.StartupPath, "ORDER", Globals.CurrTableInfo.tableNo, "0"));
                        if (localSequence < int.Parse(serverSequence))
                        {
                            IniPos.setPosIniVlaue(Application.StartupPath, "ORDER", Globals.CurrTableInfo.tableNo, serverSequence);
                        }
                    }
                    catch { if (!serverSequence.Equals("")) { IniPos.setPosIniVlaue(Application.StartupPath, "ORDER", Globals.CurrTableInfo.tableNo, serverSequence); } }
                }
                ShowOrder();
            }
        }
        private void ShowOrder()
        {
            if (frmorder == null)
            {
                frmorder = new frmOrder();
                frmorder.shoppingChange += new frmOrder.ShoppingChange(ShoppingChange);
                frmorder.accounts += new frmOrder.Accounts(ShowAccounts);
                frmorder.OrderRemarkChanged += FrmorderOnOrderRemarkChanged;
                this.IsMdiContainer = true;
                frmorder.MdiParent = this;
                frmorder.Parent = pnlCash;
            }
            frmorder.hideGz();
            xtraTabControl1.SelectedTabPageIndex = 0;
            pnlCash.Enabled = true;
            BtnMark.Visible = true;
            xtraCoupon.Visible = false;
            btnOrder.Visible = false;
            xtraTabControl1.Visible = false;
            pnlAmount.Visible = false;
            LbOrderMark.Visible = true;
            Globals.OrderRemark = "";//重置全单备注。
            SetRemarkOrder("");
            panel7.Visible = false;
            btnOpen.Visible = false;
            SetShowOrderFrm(true);
            if (Globals.ShoppTable == null)
            {
                //创建购物车表
                t_shopping.createShoppTable(ref Globals.ShoppTable);
            }
            else
            {
                Globals.ShoppTable.Clear();
            }
            DataView dv = new DataView(Globals.ShoppTable);
            dv.AllowNew = false;
            this.dgvBill.AutoGenerateColumns = false;
            this.dgvBill.DataSource = dv;
            this.dgvBill.Tag = 1;
            ShowTotal();
            frmorder.showbtnOrderText();
            Globals.OrderRemark = "";
            frmorder.Show();
        }
        private void SetShowOrderFrm(bool isshow)
        {
            if (isshow)
            {
                lblAmountWm.Visible = true;
                lblAmountWm.Top = 370;
                try
                {
                    frmorder.showTypeNum();
                }
                catch { }
            }
            else
            {
                lblAmountWm.Visible = false;
            }
        }
        private void pnlAmount_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnZd_Click(object sender, EventArgs e)
        {
            //赠送下单
            if (iswm)
                ShowWm();
            else
                ShowOrder();
        }

        private void btnOrderML_Click(object sender, EventArgs e)
        {
            //暂时只处理本单，
            maling = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                getAmount();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                pnlMore.Visible = false;
            }
        }

        private void frmPosMainV3_Activated(object sender, EventArgs e)
        {
            pnlMore.Visible = false;
        }

        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            //cleantable
            try
            {
                Opentable2();
            }
            catch { }
            if (dgvBill.Rows.Count <= 0)
            {
                if (!CheckCallBill())
                {
                    if (!AskQuestion("确定要取消帐单吗?"))
                        return;
                    if (RestClient.cleantable(Globals.CurrTableInfo.tableNo))
                    {
                        try
                        {
                            RestClient.broadcastmsg(1005, Globals.CurrOrderInfo.orderid); //这里是发清帐单指令1005
                        }
                        catch { }
                        Warning("取消帐单完成!");
                        Close();
                    }
                    else
                    {
                        Warning("取消帐单失败!");
                    }
                }
            }
            else
            {
                Warning("只能取消空帐单！");
            }
        }
        private void showGridWeight()
        {
            DataView dv = (DataView)this.dgvBill.DataSource;
            for (int i = 0; i <= dgvBill.Rows.Count - 1; i++)
            {
                DataRowView mydrv = dv[i];
                DataRow dr = ((DataRowView)dgvBill.Rows[i].DataBoundItem).Row;
                string dishstatus = dr["dishstatus"].ToString();
                //string dishstatus = Convert.ToString(mydrv["dishstatus"]);//要判断的字段
                if (dishstatus.Equals("1"))
                {
                    dgvBill.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    dgvBill.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    //绘制待称重图标
                    //dgvBill.Rows[i].Cells[4].Value = "";//图片
                    //this.dgvBill.Rows[i].Cells[5].Value = iltDbgBill.Images[1];
                }
                else
                {
                    //取消绘制待称重图标

                }
            }
        }

        private void dgvBill_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void dgvBill_Paint(object sender, PaintEventArgs e)
        {

        }
        private bool CheckBillCanPay()
        {
            bool ret = true;
            foreach (DataRow dr in Globals.OrderTable.Rows)
            {
                if (dr["dishstatus"].ToString().Equals("1"))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }

        private void xtraTabPage1_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(Color.White);
            e.Graphics.FillRectangle(b, new Rectangle(0, 0, xtraTabPage1.Width, xtraTabPage1.Height));
        }

        private void xtp1_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(Color.White);
            e.Graphics.FillRectangle(b, new Rectangle(0, 0, xtp1.Width, xtp1.Height));
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }


        /// <summary>
        /// 初始化会员调用功能  显示的字和按钮点击事件
        /// </summary>
        private void InitMemberFun()
        {
            int membersystem = RestClient.getMemberSystem();
            if (membersystem == 1)
            {
                btnMemberReg.Text = "会员注册";
                try
                {
                    btnMemberReg.Click -= new EventHandler(button5_Click);
                    btnMemberQuery.Click -= new EventHandler(button3_Click);
                    btnMemberStore.Click -= new EventHandler(button4_Click);
                }
                catch { }
                btnMemberReg.Click -= new EventHandler(MemberReg_Candao);
                btnMemberQuery.Click -= new EventHandler(MemberQuery_Candao);
                btnMemberStore.Click -= new EventHandler(MemberStore_Candao);
                btnMemberReg.Click += new EventHandler(MemberReg_Candao);
                btnMemberQuery.Click += new EventHandler(MemberQuery_Candao);
                btnMemberStore.Click += new EventHandler(MemberStore_Candao);
            }
        }
        /// <summary>
        /// 会员注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberReg_Candao(object sender, EventArgs e)
        {
            //frmMemberReg.ShowMemberReg();

            var veModel = new UcVipRegViewModel();
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberStore_Candao(object sender, EventArgs e)
        {
            //frmMemberQuery.ShowMemberQuery();
            var veModel = new UcVipRechargeViewModel();
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }
        /// <summary>
        ///  会员查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MemberQuery_Candao(object sender, EventArgs e)
        {
            //frmMemberQuery.ShowMemberQuery();

            var veModel = new UcVipSelectViewModel();
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }
        public bool MemberSale(string aUserid, string orderid, string pszInput, string pszSerial, float pszCash, float pszPoint, int psTransType, float pszStore, string pszTicketList, string pszPwd, float memberyhqamount)
        {
            int membersystem = RestClient.getMemberSystem();
            if (membersystem == 0)
            {
                JObject json = (JObject)RestClient.MemberSale(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, membercard, Globals.CurrOrderInfo.orderid, pszCash, pszPoint, 1, amounthyk, pszTicketList, pszPwd, (float)Math.Round(memberyhqamount, 2));
                string data = json["Data"].ToString();
                if (data == "0")
                {
                    //检查调用接口有没有成功
                    try
                    {
                        string err = json["Info"].ToString();

                        if (err.IndexOf("密码不正确") > 0)
                        {
                            Warning("卡号:" + cardno + err);
                            edtPwd.Focus();
                            edtPwd.SelectAll();
                        }
                        else
                        {
                            Warning(err);
                        }
                    }
                    catch { }
                    return false;
                    //反结算
                    //RestClient.rebacksettleorder(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserID);//反结算放到会员接口
                }
                return true;
            }
            else
                if (membersystem == 1)
                {
                    //餐道会员
                    if (cardno.Trim().Equals(""))
                    {
                        //重新查询会员获取卡号
                        //餐道会员
                        string msg = "";
                        bool qret = QueryMemberCard2_CanDao(out msg);
                        if (!qret)
                        {
                            string weChatMsg;
                            RestClient.rebacksettleorder(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName, "会员结算失败,系统自动反结", out weChatMsg);
                            Warning("获取会员卡号失败:" + msg);
                            return false;
                        }
                        if (cardno.Trim().Equals(""))
                        {
                            //RestClient.posrebacksettleorder(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid);
                            string weChatMsg;
                            RestClient.rebacksettleorder(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName, "会员结算失败,系统自动反结", out weChatMsg);
                            Warning("获取会员卡号失败:" + msg);
                            return false;
                        }
                    }
                    TCandaoMemberSale membersale = new TCandaoMemberSale();
                    membersale.Branch_id = Globals.branch_id;
                    membersale.Cardno = cardno;
                    membersale.Securitycode = "";
                    membersale.Password = pszPwd;
                    membersale.Serial = pszSerial;
                    membersale.Fcash = (decimal)pszCash;
                    membersale.Fintegral = (decimal)pszPoint;
                    membersale.Fstore = (decimal)amounthyk;
                    membersale.Fticketlist = "";
                    TCandaoRet_Sale ret = CanDaoMemberClient.MemberSale(membersale);
                    if (!ret.Ret)
                    {
                        string weChatMsg;
                        RestClient.rebacksettleorder(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName, "会员结算失败,系统自动反结", out weChatMsg);
                        Warning(ret.Retinfo);
                        return false;
                    }
                    else
                    {
                        //加入会员消费记录到门店
                        TCandaoOrderMemberInfo ordermemberinfo = new TCandaoOrderMemberInfo();
                        ordermemberinfo.Cardno = cardno;
                        ordermemberinfo.Orderid = Globals.CurrOrderInfo.orderid;
                        ordermemberinfo.Userid = Globals.UserInfo.UserID;
                        ordermemberinfo.Business = RestClient.getbranch_id();
                        ordermemberinfo.Terminal = RestClient.getPosID();
                        ordermemberinfo.Serial = ret.Tracecode;
                        ordermemberinfo.Businessname = WebServiceReference.WebServiceReference.Report_title;
                        ordermemberinfo.Score = (decimal)pszPoint;
                        ordermemberinfo.Scorebalance = ret.Integraloverall;
                        ordermemberinfo.Couponsbalance = "0";
                        ordermemberinfo.Storedbalance = ret.Storecardbalance;
                        ordermemberinfo.Psexpansivity = 0;
                        ordermemberinfo.Netvalue = (decimal)amounthyk;
                        ordermemberinfo.Inflated1 = 0;
                        ordermemberinfo.Coupons = 0;
                        ordermemberinfo.Stored = (decimal)amounthyk;

                        if (PrintService.CardCheck()) //判断复写卡是否在位
                        {
                            CanDaoMemberClient.AddOrderMember(ordermemberinfo);

                        }
                    }
                    return true;
                }
                else
                {
                    Warning("没有配置会员系统");
                    return false;
                }
        }
        private bool QueryMemberCard2_CanDao(out string msg)
        {
            label15.Text = string.Format("储值余额：{0}", 0);
            label9.Text = string.Format("积分余额：{0}", 0);
            membercard = "";
            //会员查询,
            string tmpmember = edtMemberCard.Text;
            if (tmpmember.IndexOf('=') > 0)
            {
                tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                edtMemberCard.Text = tmpmember;
            }
            JObject json = null;
            TCandaoMemberInfo ret = null;
            try
            {
                ret = CanDaoMemberClient.QueryBalance(Globals.branch_id, "", edtMemberCard.Text, "");//edtPwd.Text
                if (!ret.Ret)
                {
                    msg = ret.Retinfo;
                    return false;
                }
            }
            catch (Exception ex) { msg = "餐道会员查询失败，请检查外网是否稳定，并重试!"; return false; }
            string data = ret.Retcode;
            textEdit1.Text = "";
            if (!data.Equals("0"))
            {

                msg = ret.Retinfo;
                return false;
            }
            else
            {
                string str = ret.Storecardbalance.ToString();
                try
                {
                    psStoredCardsBalance = float.Parse(str);

                }
                catch
                {
                    msg = "获取余额失败,请重试...";
                    return false;
                }
                str = ret.Integraloverall.ToString();
                try
                {
                    psIntegralOverall = float.Parse(str);

                }
                catch
                {
                    msg = "获取余额失败,请重试...";
                    return false;
                }
                //getTicketList(json["psTicketInfo"].ToString());
                //if (xtraTabControl2.SelectedTabPageIndex != 5)
                //{
                //    xtraTabControl2.SelectedTabPageIndex = 5;
                //}
                //else
                //{
                //pnlz
                //     xtraTabControl2_SelectedPageChanged(xtraTabControl2, null);
                //}
                label15.Text = string.Format("储值余额：{0}", psStoredCardsBalance);
                label9.Text = string.Format("积分余额：{0}", psIntegralOverall);
                try
                {
                    if (!ret.Cardlevel.Equals("0"))
                    {
                        if (!ret.Cardlevel.Equals(""))
                        {
                            label15.Text = string.Format("储值余额：{0}({1})", psStoredCardsBalance, ret.Cardlevel);
                        }
                    }
                }
                catch { }
                membercard = edtMemberCard.Text.Trim().ToString();
                Globals.CurrOrderInfo.memberno = membercard;
                try
                {
                    cardno = ret.Cardno.ToString();
                }
                catch { }
                //调用会员价接口
                try
                {
                    RestClient.setMemberPrice(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, membercard);
                    Opentable2();
                }
                catch { }
                if (iswm)
                {
                    ///如果是外卖把本地的价格变成会员价;
                    t_shopping.setMemberPrice(ref Globals.ShoppTable);
                    ShowTotal();
                }
                //如果是一号多卡，弹出框选择一张卡号再根据卡号查询
            }
            edtMember.Focus();
            edtMember.SelectAll();
            msg = "ok";
            return true;
        }
        private bool VoidSale_CanDao(out string msg, TCandaoRetBase saleInfo)
        {
            bool ret = false;
            msg = "";
            TCandaoRet_VoidSale candaoret = null;
            try
            {
                string info = "";
                Globals.superpwd = RestClient.getManagerPwd();
                TCandaoMemberVoidSale voidsale = new TCandaoMemberVoidSale();
                voidsale.Branch_id = Globals.branch_id;
                voidsale.Securitycode = "";
                voidsale.Cardno = saleInfo.Cardno;
                voidsale.Password = edtPwd.Text;
                voidsale.Serial = Globals.CurrOrderInfo.orderid;
                voidsale.Tracecode = saleInfo.Tracecode2;
                voidsale.Superpwd = "";
                candaoret = CanDaoMemberClient.VoidSale(voidsale);
                if (!candaoret.Ret)
                {
                    msg = info;
                    return ret;
                }
            }
            catch
            {
                msg = "会员消费撤消失败...,请重试";
                return ret;
            }
            return candaoret.Ret;
        }
        private bool QueryMemberCard3_CanDao(out string msg)
        {
            //会员查询,
            string tmpmember = edtMemberCard.Text;
            if (tmpmember.IndexOf('=') > 0)
            {
                tmpmember = tmpmember.Substring(0, tmpmember.IndexOf('='));
                edtMemberCard.Text = tmpmember;
            }
            JObject json = null;
            TCandaoMemberInfo ret = null;
            try
            {
                ret = CanDaoMemberClient.QueryBalance(Globals.branch_id, "", edtMemberCard.Text, edtPwd.Text);
            }
            catch (Exception ex) { msg = "餐道会员查询失败，请检查外网是否稳定，并重试!"; return false; }
            string data = ret.Retcode;
            textEdit1.Text = "";
            if (!data.Equals("0"))
            {

                msg = ret.Retinfo;
                return false;
            }
            else
            {
                string str = ret.Storecardbalance.ToString();
                try
                {
                    psStoredCardsBalance = float.Parse(str);

                }
                catch
                {
                    msg = "获取余额失败,请重试...";
                    return false;
                }
                str = ret.Integraloverall.ToString();
                try
                {
                    psIntegralOverall = float.Parse(str);

                }
                catch
                {
                    msg = "获取余额失败,请重试...";
                    return false;
                }
                //getTicketList(json["psTicketInfo"].ToString());

            }
            msg = "ok";
            return true;
        }
        private void getOrderInvoiceTitle()
        {
            if (Globals.CurrOrderInfo.orderid == null)
                return;
            if (Globals.CurrOrderInfo.orderid.Equals(""))
                return;
            string invoice_title = "";
            if (MessageCenter.findInvoiceByOrderid(Globals.CurrOrderInfo.orderid, ref invoice_title))
            {
                Globals.CurrOrderInfo.Invoicetitle = invoice_title;
            }
            if (!Globals.CurrOrderInfo.Invoicetitle.Equals(""))
            {
                lblInvoice.Text = String.Format("开发票：{0}", Globals.CurrOrderInfo.Invoicetitle);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            LoginVIP();
        }

        /// <summary>
        /// 登录会员
        /// </summary>
        private void LoginVIP()
        {
            if (btnFind.Tag.ToString().Equals("0"))
            {
                querymember();
            }
            else
            {
                edtMemberCard.Text = "";
                label15.Text = string.Format("储值余额：{0}", 0);
                label9.Text = string.Format("积分余额：{0}", 0);
                querymember();
            }
        }

        private void dgvjs_MouseClick(object sender, MouseEventArgs e)
        {
            pnlMore.Visible = false;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = tbyh.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = tbyh.Rows[i];
                    if (!dr["banktype"].ToString().Equals("101"))
                    {
                        amountym = amountym - float.Parse(dr["freeamount"].ToString());
                        amountgz2 = amountgz2 - float.Parse(dr["debitamount"].ToString());
                        dr.Delete();
                    }
                }
                tbyh.AcceptChanges();
            }
            catch
            {
                // ignored
            }

            CheckGzYm();
            getAmount();

            try
            {
                JArray ja = Globals.GetTableJson(tbyh);
                string str = ja.ToString();
                RestClient.saveOrderPreferential(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, str);
                getAmount();
            }
            catch
            {
                // ignored
            }
        }

        private void btnPrintAmount_Click(object sender, EventArgs e)
        {
            /*if(string2float(edtWxAmount.Text)<=0)
            {
                Warning("请输入正确的金额！");
                edtWxAmount.Focus();
                return;
            }
            ReportPrint.PrintPayAmount((decimal)string2float(edtWxAmount.Text));*/
        }

        private void edtZfbAmount_EditValueChanged(object sender, EventArgs e)
        {
            getAmount();
        }

        private void edtWxAmount_EditValueChanged(object sender, EventArgs e)
        {
            getAmount();
        }

        private void edtZfbAmount_Enter(object sender, EventArgs e)
        {
            focusedt = edtZfbAmount;
        }

        private void edtWxAmount_Enter(object sender, EventArgs e)
        {
            focusedt = edtWxAmount;
        }


        /////现付功能
        private void setFormToPayType1()
        {
            if (iswm)
                return;
            if (WebServiceReference.WebServiceReference.Paytype.Equals(1))
            {
                //现付模式
                btnPay.Text = "收款";
                btnPay.Height = button19.Height;
                btnClear.Visible = true;
                btnPay2.Text = "收款";
                btnPay2.Height = button19.Height;
                btnClear2.Visible = true;
                btnPay.Click -= new EventHandler(button27_Click_1);
                btnPay2.Click -= new EventHandler(button27_Click_1);
                btnPay.Click -= new EventHandler(Pay_Click);
                btnPay2.Click -= new EventHandler(Pay_Click);
                btnPay.Click += new EventHandler(Pay_Click);
                btnPay2.Click += new EventHandler(Pay_Click);
            }
        }
        private void Pay_Click(object sender, EventArgs e)
        {
            //收款
            //
            bool isok = false;
            decimal invoiceAmount = 0;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (edtYHCard == focusedt)
                {
                    edtCard.SelectAll();
                    edtCard.Focus();
                    return;
                }
                if (edtMemberCard == focusedt)
                {
                    //QueryMemberCard();
                    querymember();
                    return;
                }
                Opentable2();
                getAmount();
                if (Math.Round(payamount - getamount + amountroundtz, 2) > 0)
                {
                    Warning("还有未收金额...");
                    return;
                }
                if (returnamount >= 100)
                {
                    Warning("找零不能大于100...");
                    return;
                }
                if (amountrmb <= 0)
                {
                    if (amountyhk > payamount)
                    {
                        Warning("请输入正确的刷卡金额...");
                        return;
                    }
                    if (amounthyk > payamount)
                    {
                        Warning("请输入正确的会员卡金额...");
                        return;
                    }
                    if (amountzfb > payamount)
                    {
                        Warning("请输入正确的支付宝金额...");
                        return;
                    }
                    if (amountwx > payamount)
                    {
                        Warning("请输入正确的微信金额...");
                        return;
                    }

                }
                if (amountrmb < returnamount)
                {
                    Warning("找零金额错误...");
                    return;
                }
                if (amounthyk > 0)
                {
                    if (membercard.Length <= 0)
                    {
                        Warning("请刷会员卡...");
                        return;
                    }
                    if (amounthyk > psStoredCardsBalance)
                    {
                        Warning("会员卡余额不足...");
                        return;
                    }
                    if (amounthyk > payamount)
                    {
                        Warning("会员卡使用金额不能大于应付额...");
                        return;
                    }
                }
                if ((getamountsy > 0) && (ysamount > 0) && (amountrmb <= 0))
                {
                    if (getamountsy > (ysamount))
                    {
                        Warning("实际输入金额大于应收,请确认...");
                    }
                }
                ///检查有没有未称重的，有就不能结帐
                if (!CheckBillCanPay())
                {
                    Warning("还有未称重菜品,不能收款...");
                    return;
                }
                invoiceAmount = (decimal)Globals.CurrTableInfo.amount;
                string settleorderorderid = Globals.CurrOrderInfo.orderid;
                bool ismember = false;
                if (AskQuestion("台号：" + Globals.CurrTableInfo.tableName + "确定收款吗?"))
                {
                    //保存台的本次收款记录

                    //现金金额，用于积分
                    if (membercard.Length > 0)
                    {
                        float psccash = amountrmb + amountyhk + amountgz + amountzfb + amountwx;//现金 用于会员积分
                        float pscpoint = amountjf; //使用积分付款
                        float pszStore = amounthyk;//使用储值余额付款
                        float tmppsccash = psccash - returnamount;
                        if (tmppsccash < 0)
                        {
                            tmppsccash = 0;
                        }
                        //使用优惠券
                        String tickstrs = getTicklistStr();
                        if (tmppsccash > 0 || pscpoint > 0 || tickstrs.Length > 0 || amounthyk > 0)
                        {
                            ismember = true;
                            try
                            {
                                string pwd = "0";
                                if (edtPwd.Text.Trim().ToString().Length > 0)
                                {
                                    pwd = edtPwd.Text.Substring(0, 6);
                                }
                                bool data = MemberSale(Globals.UserInfo.UserID, Globals.CurrOrderInfo.orderid, membercard, Globals.CurrOrderInfo.orderid, tmppsccash, pscpoint, 1, amounthyk, tickstrs, pwd, (float)Math.Round(memberyhqamount, 2));
                                if (!data)
                                {

                                }
                                else
                                {
                                    try
                                    {
                                        ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
                                    }
                                    catch { }
                                    isok = true;
                                }
                            }
                            catch
                            {
                                Warning("会员积分，结算失败!");
                            }
                        }
                        else
                        {
                            try
                            {
                                ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
                            }
                            catch { }
                            isok = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            RestClient.OpenCash();
                        }
                        catch { }
                        isok = true;
                    }
                }
                Opentable2();
                if (isok)
                {
                    if (ismember)
                    {
                        try
                        {
                            ReportPrint.PrintMemberPay1(Globals.CurrOrderInfo.orderid, Globals.UserInfo.UserName, psIntegralOverall);
                        }
                        catch { }
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private BankInfo _selectedBankInfo;

        public BankInfo SelectedBankInfo
        {
            get { return _selectedBankInfo; }
            set
            {
                _selectedBankInfo = value;
                TbBankName.Text = _selectedBankInfo != null ? _selectedBankInfo.Name : "";
            }
        }

        private void BtnSelectBank_Click_1(object sender, EventArgs e)
        {
            SelectBankWindow wnd = new SelectBankWindow(_selectedBankInfo);
            if (wnd.ShowDialog() == true)
                SelectedBankInfo = wnd.SelectedBank;
        }

        /// <summary>
        /// 优惠券按钮点击时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnyh_MouseDown(object sender, MouseEventArgs e)
        {
            _isLongPressModel = false;
            _curCoupon = ((Button)sender).Tag as VCouponRule;
            if (_curCoupon == null)
                return;

            _longPressTimer.Start();
        }

        private void BtnRemark_Click(object sender, EventArgs e)
        {
            if (dgvBill.SelectedRows.Count < 1)
            {
                Warning("请先选择一个菜品。");
                return;
            }

            DataRow dr = (this.dgvBill.SelectedRows[0].DataBoundItem as DataRowView).Row;

            string groupid = dr["Groupid"].ToString();
            string orderstatus = dr["Orderstatus"].ToString();
            string primarydishtype = dr["primarydishtype"].ToString();
            bool allowInputDishNum = true;
            if (!groupid.Equals(""))
            {
                allowInputDishNum = false;
                if (primarydishtype.Equals("2") && orderstatus != "0")//orderstatus=0是套餐主体
                {
                    Warning("请选择套餐主体设置备注。");
                    return;
                }
                if (primarydishtype.Equals("1") && orderstatus != "0")//orderstatus=0是鱼锅主体
                {
                    Warning("请选择鱼锅主体设置备注。");
                    return;
                }
            }

            string dishid = dr["dishid"].ToString();
            string dishunit = dr["dishunit"].ToString();
            int dishStatus = RestClient.getFoodStatus(dishid, dishunit);
            if (dishStatus == 1)
            {
                Warning("选择的菜品已沽清！");
                return;
            }

            var dishName = dr["dishName"].ToString();
            decimal price = decimal.Parse(dr["price"].ToString());
            decimal dishNum = decimal.Parse(dr["dishnum"].ToString());
            var dishSimpleInfo = new DishSimpleInfo()
            {
                DishName = dishName,
                DishPrice = price,
                DishUnit = dishunit,
                DishNum = dishNum,
            };

            var wnd = new SetDishTasteAndDietWindow(null, dishSimpleInfo, allowInputDishNum);
            if (wnd.ShowDialog() == true)
            {
                try
                {
                    t_shopping.EditDishDietAndNum(dr, wnd.DishNum, wnd.Diet);

                    if (!string.IsNullOrEmpty(groupid) && primarydishtype.Equals("2"))//套餐需要设置内部所有菜品的忌口。
                    {
                        foreach (DataRow dataRow in Globals.ShoppTable.Rows)
                        {
                            if (!groupid.Equals(dataRow["groupid"].ToString()))
                                continue;

                            t_shopping.EditDishDietAndNum(dataRow, wnd.DishNum, wnd.Diet);
                        }
                    }
                    ShowTotal();
                    frmorder.shoppingchange();
                }
                catch (Exception ex)
                {
                    AllLog.Instance.E(ex.Message);
                }
            }
        }

        private void BtnBackAll_Click(object sender, EventArgs e)
        {
            var wnd = new BackDishReasonSelectWindow();
            if (wnd.ShowDialog() != true)
                return;

            string discardUserId;
            if (!frmAuthorize.ShowAuthorize("退菜权限验证", Globals.UserInfo.UserID, "030102", out discardUserId))
                return;

            var service = new RestaurantServiceImpl();
            var msg = service.BackAllDish(Globals.CurrTableInfo.tableNo, Globals.CurrOrderInfo.orderid, discardUserId, wnd.SelectedReason);
            if (!string.IsNullOrEmpty(msg))
            {
                Warning(msg);
                return;
            }

            try
            {
                RestClient.broadcastmsg(2201, Globals.CurrOrderInfo.orderid); //这里是发清帐单指令1005
            }
            catch (Exception ex)
            {
                AllLog.Instance.E(ex);
            }

            Warning("整单退菜成功。");
            Opentable2();
        }

        /// <summary>
        /// 加入密码输入回车结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edtPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (getamount >= payamount)
                {
                    button27_Click_1(btnPay, e);
                }
            }
        }
    }
}
