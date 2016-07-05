using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using CanDao.Pos.SystemConfig.Views;
using CanDao.Pos.UI.Library.View;
using CanDaoCD.Pos.Common.Operates;
using Common;
using KYPOS;
using Library;
using Library.UserControls;
using Models;
using Models.Enum;
using Newtonsoft.Json.Linq;
using ReportsFastReport;
using WebServiceReference;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;
using Color = System.Drawing.Color;

namespace Main
{
    public partial class frmAllTable : frmBase
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hwnd);

        private List<UcTable> _tableControls;        /// <summary>
        /// 列个数。
        /// </summary>
        private const int Columncount = 8;
        /// <summary>
        /// 行个数。
        /// </summary>
        private const int RowCount = 6;
        /// <summary>
        /// 餐台总页数。
        /// </summary>
        private int totalTablePageCount = 1;
        /// <summary>
        /// 当前餐台页数。
        /// </summary>
        private int curTablePage = 1;
        private int btnWidth = 115;
        private int btnHeight = 78;
        private int btnSpace = 9;

        private bool _isForcedEndWorkModel;//是否是强制结业模式。

        /// <summary>
        /// 不再警告间隔时间（秒），默认10分钟。
        /// </summary>
        private const int _noWarningIntervalTime = 600;

        /// <summary>
        /// 是否启用了不再警告时间。
        /// </summary>
        private bool _isEnableNoWarning;

        /// <summary>
        /// 打印机错误警告时间。
        /// </summary>
        private DateTime _printErrorWarningTime;

        public frmPosMainV3 frmpos = new frmPosMainV3();
        public frmPosMainV3 frmposwm = new frmPosMainV3();

        /// <summary>
        /// 餐桌集合。
        /// </summary>
        public List<TableInfo> TableInfos { get; set; }

        public frmAllTable()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lblTime.Text = "当前时间：" + dt.ToLocalTime().ToString();//2005-11-5 21:21:25
        }

        private void btnRBill_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (AskQuestion("确定要退出系统吗？")) Application.Exit();
        }

        private void frmAllTable_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(t =>
            {
                if (Globals.BankInfos == null)
                    Globals.BankInfos = RestClient.GetAllBankInfos();

                if (Globals.BankInfos != null)
                {
                    foreach (var bankInfo in Globals.BankInfos)
                    {
                        var imgTempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", bankInfo.Id.ToString() + ".png");
                        bankInfo.ImageSource = imgTempPath;
                    }
                }
            });
            lblUser.Text = String.Format("登录员工:{0}", Globals.UserInfo.UserName);
            lblbranchid.Text = String.Format("店铺编号：{0}", RestClient.getbranch_id());
            timer2.Start();
            timerPrint.Start();
            lblVer.Text = String.Format("版本:{0}", Globals.ProductVersion);
            try
            {
                RestClient.getSystemSetData(out Globals.roundinfo);
            }
            catch { }
            try
            {
                RestClient.getSystemSetData("DISHES", out Globals.cjSetting);
            }
            catch { }
            try
            {
                getCJFood();
            }
            catch { }
            RestClient.openCashCom();
            Globals.branch_id = RestClient.getbranch_id();
        }

        private void ucTable1_Click(object sender, EventArgs e)
        {
            if (!Globals.userRight.getSyRigth())
            {
                Warning("您没有收银权限！");
                return;
            }
            if (Globals.roundinfo.Type.Equals(""))
            {
                try
                {
                    RestClient.getSystemSetData(out Globals.roundinfo);
                }
                catch { }
            }
            if (Globals.cjSetting.Type.Equals(""))
            {
                try
                {
                    RestClient.getSystemSetData("DISHES", out Globals.cjSetting);
                }
                catch { }
            }
            if (Globals.cjFood == null)
            {
                try
                {
                    getCJFood();
                }
                catch
                {
                    // ignored
                }
            }
            var tableInfo = ((UcTable)sender).TableInfo;
            string tableno = tableInfo.TableNo;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                timer2.Enabled = false;
                //frmPosMain.ShowPosMain(tableno, uctable.status);
                frmpos.ShowFrm(tableno, (int)tableInfo.TableStatus);

                if (_isForcedEndWorkModel)//如果已经是强制结业模式，就继续设定成强制结业（结算是在另外的弹出窗口）
                    SetInForcedEndWorkModel();
            }
            finally
            {
                btnReport.Tag = 0;
                this.Cursor = Cursors.Default;
                timer2.Enabled = true;
            }
        }

        private void RefreshAllTableStatus()
        {
            //
            try
            {
                btnReport.Tag = 5;
                this.Cursor = Cursors.WaitCursor;
                timer2.Stop();
                this.Update();//必须

                IRestaurantService service = new RestaurantServiceImpl();
                var result = service.GetAllTableInfoes();
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    Warning(result.Item1);
                    return;
                }

                TableInfos = result.Item2.Where(t => t.TableType != EnumTableType.Takeout).ToList();//不显示外卖台。
                totalTablePageCount = (TableInfos.Count + Columncount * RowCount - 1) / (Columncount * RowCount);
                panelPage.Visible = totalTablePageCount > 1;
                UpdatePageButtonEnableStatus();
                CreateTableControls();

                lblState0.Text = string.Format("空闲({0})", TableInfos.Count(t => t.TableStatus == EnumTableStatus.Idle));
                lblState1.Text = string.Format("就餐({0})", TableInfos.Count(t => t.TableStatus == EnumTableStatus.Dinner));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                timer2.Start();
            }
        }

        private void UpdatePageButtonEnableStatus()
        {
            btnUp.Enabled = curTablePage > 1;
            btnDown.Enabled = curTablePage < totalTablePageCount;
        }

        /// <summary>
        /// 创建餐台控件。
        /// </summary>
        private void CreateTableControls()
        {
            try
            {
                LockWindowUpdate(Handle);

                int idx = 0;
                if (_tableControls != null)
                {
                    foreach (var tableControl in _tableControls)
                    {
                        tableControl.Click -= ucTable1_Click;
                        tableControl.Parent = null;
                        tableControl.Parent = null;
                    }
                }
                _tableControls = new List<UcTable>();

                var temp = TableInfos.Skip((curTablePage - 1) * Columncount * RowCount);
                temp = temp.Take(Columncount * RowCount);
                foreach (var tableInfo in temp)
                {
                    var colindex = (idx % Columncount);
                    var rowindex = idx / Columncount;
                    var table = new UcTable(tableInfo)
                    {
                        Parent = pnlMain,
                        Width = btnWidth,
                        Height = btnHeight,
                        Left = colindex * btnWidth + btnSpace + (colindex * btnSpace),
                        Top = btnHeight * rowindex + 75 + btnSpace + (rowindex * btnSpace),
                    };
                    table.Click += ucTable1_Click;
                    _tableControls.Add(table);
                    if (_isForcedEndWorkModel)//如果是强制结业模式，则只允许操作就餐餐台。
                        table.Enabled = tableInfo.TableStatus == EnumTableStatus.Dinner;
                    //frmProgress.frm.SetProgress("正在加载桌台资料..." + tableInfo.TableNo, TableInfos.Count, idx);
                    idx++;
                }
            }
            finally
            {
                LockWindowUpdate(IntPtr.Zero);
                //frmProgress.frm.Close();
            }
        }

        /// <summary>
        /// 进入强制结业模式，只能结账、清机和结业。
        /// </summary>
        public void SetInForcedEndWorkModel()
        {
            if (_tableControls != null)
            {
                var idleTableControls = _tableControls.Where(t => t.TableInfo.TableStatus == EnumTableStatus.Idle).ToList();
                idleTableControls.ForEach(t => t.Enabled = false);
            }
            btnShapping.Enabled = false;
            _isForcedEndWorkModel = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                timer2.Enabled = false;

                if (DateTime.Now > Globals.TradeTime.BeginTime) //当前时间大于开业时间，强制结业
                {
                    Globals.TradeTime.BeginTime = Globals.TradeTime.BeginTime.AddDays(1);
                    Warning("昨天还未结业，请先结业。");
                    SetInForcedEndWorkModel();
                }
                else if (DateTime.Now > Globals.TradeTime.EndTime.AddSeconds(10))//当前时间大于结业时间，提示结业，与真实的时间提前10秒。
                {
                    Globals.TradeTime.EndTime = Globals.TradeTime.EndTime.AddDays(1);//提示一次以后就要把结业时间计算成下一次结业，不然每一秒都会提示该信息。
                    Warning("结业时间到了，请及时结业。");
                }

                int inttime = int.Parse(btnReport.Tag.ToString());
                if (inttime > 0)
                {
                    btnReport.Tag = --inttime;
                    if (_tableControls != null)
                        _tableControls.ForEach(t => t.UpdateTimeView());
                }
                else
                    RefreshAllTableStatus();
            }
            finally
            {
                timer2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Globals.userRight.getSyRigth())
            {
                Warning("您没有清机权限！");
                return;
            }

            var hadDinnerTable = TableInfos.Any(t => t.TableStatus == EnumTableStatus.Dinner);
            var wnd = new SelectClearMachineStepWindow(!hadDinnerTable, _isForcedEndWorkModel);
            if (wnd.ShowDialog() == true)
            {
                if (wnd.DoEndWork)
                {
                    if (!CommonHelper.ClearAllMachine(false))
                        return;

                    if (!CommonHelper.EndWork())
                        ForcedLogin();
                }
                else //选择倒班。
                {
                    if (CommonHelper.ClearMachine())
                        ForcedLogin();
                }
            }
        }

        /// <summary>
        /// 强制登录。
        /// </summary>
        private void ForcedLogin()
        {
            if (frmLogin.Login()) //返回主界面
            {
                if (Globals.userRight.getSyRigth())
                {
                    if (!frmPosMainV3.checkInputTellerCash())
                    {
                        Application.Exit();
                        return;
                    }
                }
                lblUser.Text = String.Format("登录员工:{0}", Globals.UserInfo.UserName);
            }
            else //登录失败,退出程序
                Application.Exit();
        }

        private void btnend_Click(object sender, EventArgs e)
        {
            if (TableInfos.Any(t => t.TableStatus == EnumTableStatus.Dinner))
            {
                Warning("还有未结账的餐台，不能结业。");
                return;
            }
            timer2.Enabled = false;
            if (AskQuestion("确定要现在结业吗？"))
                CommonHelper.EndWork();
            timer2.Enabled = true;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            frmQueryBill.ShowQueryBill(this, _isForcedEndWorkModel);
        }

        private void btnShapping_Click(object sender, EventArgs e)
        {
            if (!Globals.userRight.getSyRigth())
            {
                Warning("您没有收银权限！");
                return;
            }

            //获取咖啡外卖台
            var service = new RestaurantServiceImpl();
            var result = service.GetTableInfoByType(new List<EnumTableType>() { EnumTableType.CFTakeout });//查询咖啡模式外卖台
            if (!string.IsNullOrEmpty(result.Item1))
            {
                Warning(result.Item1);
                return;
            }

            string tableno = RestClient.getTakeOutTable();//默认选中配置文件的外卖台。
            if (result.Item2 != null)
            {
                var selectTableWnd = new SelectCoffeeTakeoutTableWindow(result.Item2);
                if (selectTableWnd.ShowDialog() == true && selectTableWnd.SelectedTable != null)
                {
                    tableno = selectTableWnd.SelectedTable.TableNo;
                }
            }
            try
            {
                this.Cursor = Cursors.WaitCursor;
                timer2.Enabled = false;
                frmposwm.showFrmWm(tableno);
            }
            finally
            {
                btnReport.Tag = 0;
                this.Cursor = Cursors.Default;
                timer2.Enabled = true;
            }
        }

        private void getCJFood()
        {
            try
            {
                JArray jrOrder;
                if (RestClient.getCJFood(Globals.UserInfo.UserName, out jrOrder))
                    Globals.cjFood = jrOrder;
            }
            catch
            {
                // ignored
            }
        }

        private void frmAllTable_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                RestClient.sc.closePort();
            }
            catch { }
            Application.Exit();
        }

        /// <summary>
        /// 报表。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReport_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            (new ReportViewWindow()).ShowDialog();
            RefreshAllTableStatus();
            timer2.Start();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            curTablePage--;
            CreateTableControls();
            UpdatePageButtonEnableStatus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            curTablePage++;
            CreateTableControls();
            UpdatePageButtonEnableStatus();
        }

        private void BtnSysCfg_Click(object sender, EventArgs e)
        {
            timerPrint.Stop();
            OWindowManage.ShowPopupWindow(new UcSystemSettingView(), Brushes.AntiqueWhite);
            timerPrint.Start();
        }

        /// <summary>
        /// 检测打印机状态。
        /// </summary>
        private void CheckPrinterStatus()
        {
            var service = new RestaurantServiceImpl();
            var result = service.GetPrinterStatusInfo();
            if (!string.IsNullOrEmpty(result.Item1))
            {
                Invoke((Action)delegate { Warning(string.Format("获取打印机状态列表错误：{0}", result.Item1)); });
                return;
            }

            var errPrintCount = result.Item2.Count(t => t.PrintStatus != EnumPrintStatus.Normal);
            if (errPrintCount > 0)
            {
                Invoke((Action)delegate
                {
                    BtnSysCfg.FlatAppearance.BorderColor = Color.Red;
                    BtnSysCfg.FlatAppearance.BorderSize = 2;
                    if (!_isEnableNoWarning || (DateTime.Now - _printErrorWarningTime).TotalSeconds > _noWarningIntervalTime)
                    {
                        var errMsg = string.Format("检测到{0}个打印机异常，请到\"系统\">\"打印机列表\"查看并修复。", errPrintCount);
                        var warningDialog = new FrmWarningPrintError(errMsg) { Owner = this };
                        warningDialog.ShowDialog();//这种提示窗口不考虑取消状态。
                        _isEnableNoWarning = warningDialog.IsCheckedNoWarning;
                        if (_isEnableNoWarning)
                            _printErrorWarningTime = DateTime.Now;
                    }
                });
            }
            else
            {
                _isEnableNoWarning = false;//当全部成功的话，就重置不警告的字段。
                Invoke((Action)delegate
                {
                    BtnSysCfg.FlatAppearance.BorderColor = Color.Empty;
                    BtnSysCfg.FlatAppearance.BorderSize = 1;
                });
            }
        }

        private void timerPrint_Tick(object sender, EventArgs e)
        {
            timerPrint.Stop();
            CheckPrinterStatus();
            timerPrint.Start();
        }
    }
}