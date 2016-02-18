using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Common;
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

        private DateTime _endWorkTime;
        private DateTime _openTime;
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
            GetTradeTime();
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
            var idleTableControls = _tableControls.Where(t => ((TableInfo)t.Tag).TableStatus == EnumTableStatus.Idle).ToList();
            idleTableControls.ForEach(t => t.Enabled = false);
            btnShapping.Enabled = false;
            button3.Enabled = false;
            _isForcedEndWorkModel = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                timer2.Enabled = false;

                //if (DateTime.Now > _openTime) //当前时间大于开业时间，强制结业
                //{
                //    SetInForcedEndWorkModel();
                //}
                //else if (DateTime.Now > _endWorkTime)//当前时间大于结业时间，提示结业
                //{
                //    _endWorkTime = _endWorkTime.AddHours(24);//提示一次以后就要把结业时间计算成下一次结业，不然每一秒都会提示该信息。
                //    Warning("结业时间到了，请及时结业。");
                //}

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
            var wnd = new SelectClearMachineStepWindow(!hadDinnerTable);
            if (wnd.ShowDialog() == true)
            {
                if (wnd.DoEndWork)
                {

                    IRestaurantService service = new RestaurantServiceImpl();
                    var result = service.GetUnclearnPosInfo();
                    if (!string.IsNullOrEmpty(result.Item1))
                    {
                        Warning(result.Item1);
                        return;
                    }

                    var noClearnMachineList = result.Item2;
                    if (noClearnMachineList.Any())
                    {
                        var localMac = RestClient.GetMacAddr();
                        var thisMachineNoClearList = noClearnMachineList.Where(t => t.MachineFlag.Equals(localMac)).ToList();
                        if (thisMachineNoClearList.Any())
                        {
                            if (thisMachineNoClearList.Any(t => !ClearMachine(t.UserName)))//任何一个本机收银全清机失败就返回。
                                return;
                        }

                        if (noClearnMachineList.Any(t => !t.MachineFlag.Equals(localMac)))//有其他POS的收银机。
                        {
                            OtherMachineNoClearnWarningWindow warningWnd = new OtherMachineNoClearnWarningWindow();
                            if (warningWnd.ShowDialog() != true)
                                return;
                        }
                    }

                    if (!EndWork())
                        ForcedLogin();
                }
                else //选择倒班。
                {
                    if (ClearMachine())
                        ForcedLogin();
                }
            }
        }

        /// <summary>
        /// 清机业务组合。
        /// </summary>
        /// <param name="userName">收银员账号。</param>
        /// <returns>清机成功返回true，否则返回false。</returns>
        private static bool ClearMachine(string userName = null)
        {
            if (!frmPermission2.ShowPermission2("收银员清机", EnumRightType.ClearMachine, userName))
                return false;

            ReportPrint.PrintClearMachine(); //打印清机报表
            ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
            Warning("清机成功!");
            return true;
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
            if (!Globals.userRight.getJyRight())
            {
                Warning("您没有结业权限！");
                return;
            }
            if (TableInfos.Any(t => t.TableStatus == EnumTableStatus.Dinner))
            {
                Warning("还有未结账的餐台，不能结业。");
                return;
            }
            if (AskQuestion("确定要现在结业吗？"))
                EndWork();
        }

        /// <summary>
        /// 结业处理。
        /// </summary>
        private bool EndWork()
        {
            if (!frmPermission2.ShowPermission2("结业经理授权", EnumRightType.EndWork))
                return false;

            try
            {
                JObject ja = RestClient.endWork(Globals.UserInfo.UserID);
                string data = ja["Data"].ToString();
                if (data.Equals("1"))
                {
                    do
                    {
                        try
                        {
                            if (RestClient.jdesyndata())//调用上传回调
                                break;
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        if (!AskQuestion("发生异常，上传失败，是否重新上传？"))
                            break;
                    } while (true);

                    Warning("结业成功!");
                    Application.Exit();
                    Close();
                    return true;
                }

                var msg = ja["Info"].ToString();
                if (string.IsNullOrEmpty(msg))
                    msg = "结业失败！";
                Warning(msg);
                return false;
            }
            catch (Exception ex)
            {
                Warning("结业失败。" + ex.Message);
                return false;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            frmQueryBill.ShowQueryBill(this);
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

        private void GetTradeTime()
        {
            _endWorkTime = DateTime.Now.AddSeconds(5);//DateTime.Today.AddHours(20);
            _openTime = DateTime.Now.AddSeconds(10); //DateTime.Today.AddHours(31);//第二天7点
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