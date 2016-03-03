using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
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

namespace Main
{

    public partial class frmAllTable : frmBase
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hwnd);

        private List<ucTable> _tableControls;
        private const int Rowcount = 10;
        private int btnWidth = 88;
        private int btnHeight = 58;
        private int btnSpace = 10;

        private bool _isForcedEndWorkModel;//是否是强制结业模式。

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

        private void ucTable1_Load(object sender, EventArgs e)
        {
            ((ucTable)sender).lblNo.Click += new EventHandler(ucTable1_Click);
            ((ucTable)sender).lbl2.Click += new EventHandler(ucTable1_Click);
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
            timer2.Enabled = true;
            timer2.Interval = 1000;
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
            ///获取分店ID
            try
            {
                String branch_id = "";
                CanDaoMemberClient.findBranchid(out branch_id, "");
                if (branch_id.Equals(""))
                    branch_id = "0013";
                Globals.branch_id = branch_id;
            }
            catch { }
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
            ucTable uctable = ((ucTable)((Label)sender).Tag);
            string tableno = uctable.lblNo.Text;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                timer2.Enabled = false;
                //frmPosMain.ShowPosMain(tableno, uctable.status);
                frmpos.ShowFrm(tableno, uctable.status);

                if (_isForcedEndWorkModel)//如果已经是强制结业模式，就继续设定成强制结业（结算是在另外的弹出窗口）
                    SetInForcedEndWorkModel();
            }
            finally
            {
                btnRefresh.Tag = 0;
                this.Cursor = Cursors.Default;
                timer2.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //
            try
            {
                UpdateRefreshBtnStatus(5);
                this.Cursor = Cursors.WaitCursor;
                btnRefresh.Enabled = false;
                timer2.Stop();
                this.Update();//必须

                IRestaurantService service = new RestaurantServiceImpl();
                var result = service.GetAllTableInfoes();
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    Warning(result.Item1);
                    return;
                }

                TableInfos = result.Item2;
                CreateTableControls();

                lblState0.Text = string.Format("空闲({0})", TableInfos.Count(t => t.TableStatus == EnumTableStatus.Idle));
                lblState1.Text = string.Format("就餐({0})", TableInfos.Count(t => t.TableStatus == EnumTableStatus.Dinner));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnRefresh.Enabled = true;
                timer2.Start();
            }
        }

        /// <summary>
        /// 创建餐台控件。
        /// </summary>
        private void CreateTableControls()
        {
            try
            {
                //frmProgress.ShowProgress("正在加载桌台资料...");
                LockWindowUpdate(Handle);
                //frmProgress.frm.SetProgress("正在加载桌台资料...", TableInfos.Count, 0);

                int idx = 0;
                if (_tableControls != null)
                {
                    foreach (var tableControl in _tableControls)
                    {
                        tableControl.lblNo.Click -= ucTable1_Click;
                        tableControl.lbl2.Click -= ucTable1_Click;
                        tableControl.Parent = null;
                    }
                }
                _tableControls = new List<ucTable>();

                foreach (var tableInfo in TableInfos)
                {
                    var colindex = (idx % Rowcount);
                    var rowindex = idx / Rowcount;
                    ucTable table = new ucTable(tableInfo)
                    {
                        Parent = pnlMain,
                        Width = btnWidth,
                        Height = btnHeight,
                        Left = colindex * btnWidth + ucTable1.Left + (colindex * btnSpace),
                        Top = btnHeight * rowindex + ucTable1.Top + (rowindex * btnSpace),
                    };
                    table.lblNo.Click += ucTable1_Click;
                    table.lbl2.Click += ucTable1_Click;
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
                var idleTableControls = _tableControls.Where(t => ((TableInfo)t.Tag).TableStatus == EnumTableStatus.Idle).ToList();
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

                int inttime = int.Parse(btnRefresh.Tag.ToString());
                if (inttime > 0)
                    UpdateRefreshBtnStatus(--inttime);
                else
                    button3_Click(null, null);
            }
            finally
            {
                timer2.Enabled = true;
            }
        }

        private void UpdateRefreshBtnStatus(int inttime)
        {
            btnRefresh.Tag = inttime;
            btnRefresh.Text = String.Format("刷新[{0}]", inttime);
            btnRefresh.Update();
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
            if (AskQuestion("确定要现在结业吗？"))
                CommonHelper.EndWork();
        }

        private object EndWorkSyncDataProcess(object param)
        {
            try
            {
                return RestClient.jdesyndata();
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void EndWorkSyncDataComplete(object param)
        {
            if (!(bool)param)
            {
                if (AskQuestion("发生异常，上传失败，是否重新上传？"))
                {
                    TaskService.Start(null, EndWorkSyncDataProcess, EndWorkSyncDataComplete, "数据正在上传...");
                    return;
                }

                Warning("结业成功，但数据上传失败。");
            }
            else
            {
                Warning("结业成功!");
            }
            Application.Exit();
            Close();
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
            string tableno = RestClient.getTakeOutTable();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                timer2.Enabled = false;
                frmposwm.showFrmWm(tableno);
            }
            finally
            {
                btnRefresh.Tag = 0;
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
    }
}