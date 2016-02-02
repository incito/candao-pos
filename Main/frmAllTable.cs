using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common;
using Models;
using WebServiceReference;
using ReportsFastReport;
using System.Runtime.InteropServices;
using System.Threading;

namespace Main
{

    public partial class frmAllTable : frmBase
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hwnd);
        private JArray jarrTables = null;
        private Library.UserControls.ucTable[] btntables;
        private const int rowcount = 10;
        private int btnWidth = 88;
        private int btnHeight = 58;
        private int btnSpace = 10;

        private int statusnum0 = 0;
        private int statusnum1 = 0;
        private int statusnum3 = 0;
        private int statusnum4 = 0;
        private int statusnum5 = 0;
        //public frmPosMain frmpos = new frmPosMain();
        //public frmPosMain frmposwm = new frmPosMain();
        //public frmPosMain frmpos = new frmPosMain();
        public frmPosMainV3 frmpos = new frmPosMainV3();
        public frmPosMainV3 frmposwm = new frmPosMainV3();
        public frmAllTable()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lblTime.Text = "当前时间：" + dt.ToLocalTime().ToString();//2005-11-5 21:21:25
        }

        private void btnRBill_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (AskQuestion("确定要退出系统吗？")) Application.Exit();
        }

        private void ucTable1_Load(object sender, EventArgs e)
        {
            ((Library.UserControls.ucTable)sender).lblNo.Click += new EventHandler(ucTable1_Click);
            ((Library.UserControls.ucTable)sender).lbl2.Click += new EventHandler(ucTable1_Click);
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
                catch { }
            }
            object obj = ((Label)sender).Tag;
            Library.UserControls.ucTable uctable = ((Library.UserControls.ucTable)obj);
            string tableno = uctable.lblNo.Text;
            //if (uctable.status == 0)
            //    return;
            if (uctable.status == 2)
                return;
            if (uctable.status == 4)
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                timer2.Enabled = false;
                //frmPosMain.ShowPosMain(tableno, uctable.status);
                frmpos.showFrm(tableno, uctable.status);
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
                this.Cursor = Cursors.WaitCursor;
                btnRefresh.Enabled = false;
                this.Update();//必须
                jarrTables = RestClient.querytables();
                if (jarrTables == null)
                    return;
                CreateTableArr();
                UpdateTableStatus();
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnRefresh.Enabled = true;
            }
        }
        private void CreateBtnArr()
        {
            btntables = new Library.UserControls.ucTable[jarrTables.Count];
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
            frmProgress.frm.SetProgress("正在加载桌台资料...", btntables.Length, 0);
            for (int i = 0; i <= btntables.Length - 1; i++)
            {
                btntables[i] = new Library.UserControls.ucTable();
                btntables[i].lblNo.Click += new EventHandler(ucTable1_Click);
                btntables[i].lbl2.Click += new EventHandler(ucTable1_Click);
                ja = (JObject)jarrTables[i];
                tableid = ja["tableid"].ToString();
                tableName = ja["tableName"].ToString();
                tableNo = ja["tableNo"].ToString();
                tabletype = ja["tabletype"].ToString();
                try
                {
                    personnum = int.Parse(ja["personNum"].ToString());
                }
                catch { personnum = 0; }
                orderstatus = int.Parse(ja["status"].ToString());
                btntables[i].lblNo.Text = tableNo;
                btntables[i].lbl2.Text = string.Format("{0}人桌", personnum);
                if (tableName.IndexOf("外") >= 0)
                {
                    btntables[i].lbl2.Text = tableName;
                }
                btntables[i].status = orderstatus;


                btntables[i].lblNo.Tag = btntables[i];
                btntables[i].lbl2.Tag = btntables[i];

                setbtnColor(btntables[i], orderstatus);
                //位置
                btntables[i].Parent = pnlMain;
                btntables[i].Width = btnWidth;
                btntables[i].Height = btnHeight;
                colindex = (i % rowcount);
                btnleft = colindex * btnWidth + ucTable1.Left + (colindex * btnSpace);
                btntables[i].Left = btnleft;
                rowindex = i / rowcount;
                btntop = btnHeight * rowindex + ucTable1.Top + (rowindex * btnSpace);
                btntables[i].Top = btntop;
                frmProgress.frm.SetProgress("正在加载桌台资料..." + tableNo, btntables.Length, i);

            }
        }
        private void setbtnColor(Library.UserControls.ucTable btn, int orderstatus)
        {
            switch (orderstatus)
            {
                case 0: //空闲
                    btn.BackColor = lblState0.BackColor;
                    statusnum0 = statusnum0 + 1;
                    break;
                case 1: //就餐
                    btn.BackColor = lblState1.BackColor;
                    statusnum1 = statusnum1 + 1;
                    break;
                case 3: //已结账
                    statusnum3 = statusnum3 + 1;
                    btn.BackColor = lblState3.BackColor;
                    break;
                case 4: //预定
                    statusnum4 = statusnum4 + 1;
                    btn.BackColor = lblState4.BackColor;
                    break;
                case 5: //已撤销
                    statusnum5 = statusnum5 + 1;
                    btn.BackColor = lblState5.BackColor;
                    break;
            }
        }
        private JObject getTableJson(string tableno)
        {
            JObject ja = null;
            for (int i = 0; i <= jarrTables.Count - 1; i++)
            {
                ja = (JObject)jarrTables[i];
                if (tableno.Equals(ja["tableNo"].ToString()))
                {
                    //return ja;
                    break;
                }
            }
            return ja;
        }
        private void CreateTableArr()
        {
            //如果没有创建台那么创建
            if (btntables != null)
                return;
            try
            {
                frmProgress.ShowProgress("正在加载桌台资料...");
                frmAllTable.LockWindowUpdate(this.Handle);
                CreateBtnArr();
            }
            finally
            {
                frmAllTable.LockWindowUpdate(IntPtr.Zero);
                frmProgress.frm.Close();
            }

        }
        private void UpdateTableStatus()
        {
            //如果没有创建台那么创建
            try
            {
                //刷新最新状态
                statusnum0 = 0;
                statusnum1 = 0;
                statusnum3 = 0;
                statusnum4 = 0;
                statusnum5 = 0;
                JObject ja;
                for (int i = 0; i <= btntables.Length - 1; i++)
                {
                    ja = getTableJson(btntables[i].lblNo.Text);
                    if (ja == null)
                    {
                        btntables[i].Enabled = false;
                    }
                    else
                    {
                        btntables[i].Enabled = true;
                        int orderstatus = int.Parse(ja["status"].ToString());
                        setbtnColor(btntables[i], orderstatus);
                        btntables[i].status = orderstatus;
                    }
                }
                lblState0.Text = string.Format("空闲({0})", statusnum0);
                lblState1.Text = string.Format("就餐({0})", statusnum1);
                lblState4.Text = string.Format("预定({0})", statusnum4);
                lblState3.Text = string.Format("(结帐{0})", statusnum3);
                lblState5.Text = string.Format("撤销({0})", statusnum5);
            }
            finally
            {
            }

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                timer2.Enabled = false;
                int inttime = int.Parse(btnRefresh.Tag.ToString());
                if (inttime > 0)
                {
                    inttime = inttime - 1;
                    btnRefresh.Tag = inttime;
                    btnRefresh.Text = String.Format("刷新[{0}]", inttime);
                    btnRefresh.Update();
                    return;
                }
                inttime = 5;
                btnRefresh.Tag = inttime;
                btnRefresh.Text = String.Format("刷新[{0}]", inttime);
                btnRefresh.Update();
                button3_Click(btnRefresh, e);
            }
            finally
            { timer2.Enabled = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //清机 打印清机报表，标记收银机为已清机状态，可以下一个收银员 录入零找金
            //经理权限
            if (!Globals.userRight.getSyRigth())
            {
                Warning("您没有清机权限！");
                return;
            }

            var wnd = new SelectClearMachineStepWindow(true);
            if (wnd.ShowDialog() == true)
            {
                if (wnd.DoEndWork)
                {
                    var noClearnMachineList = RestClient.GetNoClearMachineInfos();
                    if (noClearnMachineList.Any())
                    {
                        var thisMachineNoClearList = noClearnMachineList.Where(t=>t.MachineFlag.Equals(RestClient.GetMacAddr())).ToList();
                        if (thisMachineNoClearList.Any())
                        {
                            foreach (var noClearMachineInfo in thisMachineNoClearList)
                            {
                                
                            }
                        }
                    }
                }
                else //选择倒班。
                {
                    if (!frmPermission2.ShowPermission2("收银员清机", eRightType.right4))
                        return;

                    try
                    {
                        JObject ja = RestClient.clearMachine(Globals.UserInfo.UserID, Globals.UserInfo.UserName,
                            Globals.authorizer);
                        string data = ja["Data"].ToString();
                        if (data.Equals("1"))
                        {
                            //打印清机报表
                            ReportPrint.PrintClearMachine();
                            RestClient.OpenCash();
                            Warning("清机成功!");
                            //返回主界面
                            if (frmLogin.Login())
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
                        else
                        {
                            Warning(!string.IsNullOrEmpty(ja["Info"].ToString()) ? ja["Info"].ToString() : "清机失败。");
                        }
                    }
                    catch(Exception ex)
                    {
                        Warning("清机时错误：" + ex.Message);
                    }
                }
            }
        }

        private void btnend_Click(object sender, EventArgs e)
        {
            if (!Globals.userRight.getJyRight())
            {
                Warning("您没有结业权限！");
                return;
            }
            if (!AskQuestion("确定要现在结业吗？"))
            {
                return;
            }
            //清机 打印清机报表，标记收银机为已清机状态，可以下一个收银员 录入零找金
            //经理权限
            if (!frmPermission2.ShowPermission2("结业经理授权", eRightType.right5))
            {
                return;
            }
            try
            {
                JObject ja = RestClient.endWork(Globals.UserInfo.UserID);
                string data = ja["Data"].ToString();
                if (data.Equals("1"))
                {
                    do
                    {
                        bool result = false;
                        try
                        {
                            result = RestClient.jdesyndata();//调用上传回调//http://localhost:8080/newspicyway/padinterface/jdesyndata.json
                        }
                        catch (Exception)
                        {
                            result = false;
                            //Warning("上传数据失败！");
                        }

                        if (result)
                            break;

                        if (!AskQuestion("上传数据失败，重新上传？" + Environment.NewLine + "\"确定\"重新上传，\"取消\"放弃。"))
                            break;
                    } while (true);
                    //打印清机报表
                    Warning("结业成功!");
                    //返回主界面
                    Application.Exit();
                    Close();
                    return;
                }
                else
                {
                    var msg = ja["Info"].ToString();
                    if (string.IsNullOrEmpty(msg))
                        msg = "结业失败！";
                    Warning(msg);
                }

            }
            catch (Exception ex)
            {
                Warning("结业失败。" + ex.Message);
            }

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //
            frmQueryBill.ShowQueryBill(this);
        }

        private void pnlState3_Paint(object sender, PaintEventArgs e)
        {

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

        private void panel1_DoubleClick(object sender, EventArgs e)
        {

        }
        private void getCJFood()
        {

            JArray jrOrder = null;
            try
            {
                if (!RestClient.getCJFood(Globals.UserInfo.UserName, out jrOrder))
                {
                    return;
                }
            }
            catch { }
            Globals.cjFood = jrOrder;
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