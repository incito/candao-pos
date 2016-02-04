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
using Library.UserControls;
using Models.Enum;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;

namespace Main
{

    public partial class frmAllTable : frmBase
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hwnd);
        private JArray jarrTables = null;
        private Library.UserControls.ucTable[] btntables;
        private List<ucTable> _tableControls;
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

        /// <summary>
        /// �������ϡ�
        /// </summary>
        public List<TableInfo> TableInfos { get; set; }

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
            lblTime.Text = "��ǰʱ�䣺" + dt.ToLocalTime().ToString();//2005-11-5 21:21:25
        }

        private void btnRBill_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (AskQuestion("ȷ��Ҫ�˳�ϵͳ��")) Application.Exit();
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
            lblUser.Text = String.Format("��¼Ա��:{0}", Globals.UserInfo.UserName);
            lblbranchid.Text = String.Format("���̱�ţ�{0}", RestClient.getbranch_id());
            timer2.Enabled = true;
            timer2.Interval = 1000;
            lblVer.Text = String.Format("�汾:{0}", Globals.ProductVersion);
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
            ///��ȡ�ֵ�ID
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
                Warning("��û������Ȩ�ޣ�");
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
                UpdateRefreshBtnStatus(5);
                this.Cursor = Cursors.WaitCursor;
                btnRefresh.Enabled = false;
                timer2.Stop();
                this.Update();//����

                IRestaurantService service = new RestaurantServiceImpl();
                var result = service.GetAllTableInfoes();
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    Warning(result.Item1);
                    return;
                }

                TableInfos = result.Item2;
                CreateTableControls();

                lblState0.Text = string.Format("����({0})", TableInfos.Count(t => t.TableStatus == EnumTableStatus.Idle));
                lblState1.Text = string.Format("�Ͳ�({0})", TableInfos.Count(t => t.TableStatus == EnumTableStatus.Dinner));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnRefresh.Enabled = true;
                timer2.Start();
            }
        }

        /// <summary>
        /// ������̨�ؼ���
        /// </summary>
        private void CreateTableControls()
        {
            try
            {
                //frmProgress.ShowProgress("���ڼ�����̨����...");
                LockWindowUpdate(Handle);
                //frmProgress.frm.SetProgress("���ڼ�����̨����...", TableInfos.Count, 0);

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
                    var colindex = (idx % rowcount);
                    var rowindex = idx / rowcount;
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
                    //frmProgress.frm.SetProgress("���ڼ�����̨����..." + tableInfo.TableNo, TableInfos.Count, idx);
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
        /// ����ǿ�ƽ�ҵģʽ��ֻ�ܽ��˺������
        /// </summary>
        public void SetInForcedEndWorkModel()
        {
            var idleTableControls = _tableControls.Where(t => ((TableInfo)t.Tag).TableStatus == EnumTableStatus.Idle).ToList();
            idleTableControls.ForEach(t => t.Enabled = false);
            btnShapping.Enabled = false;
            button3.Enabled = false;
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
            frmProgress.frm.SetProgress("���ڼ�����̨����...", btntables.Length, 0);
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
                btntables[i].lbl2.Text = string.Format("{0}����", personnum);
                if (tableName.IndexOf("��") >= 0)
                {
                    btntables[i].lbl2.Text = tableName;
                }
                btntables[i].status = orderstatus;


                btntables[i].lblNo.Tag = btntables[i];
                btntables[i].lbl2.Tag = btntables[i];

                setbtnColor(btntables[i], orderstatus);
                //λ��
                btntables[i].Parent = pnlMain;
                btntables[i].Width = btnWidth;
                btntables[i].Height = btnHeight;
                colindex = (i % rowcount);
                btnleft = colindex * btnWidth + ucTable1.Left + (colindex * btnSpace);
                btntables[i].Left = btnleft;
                rowindex = i / rowcount;
                btntop = btnHeight * rowindex + ucTable1.Top + (rowindex * btnSpace);
                btntables[i].Top = btntop;
                frmProgress.frm.SetProgress("���ڼ�����̨����..." + tableNo, btntables.Length, i);

            }
        }
        private void setbtnColor(Library.UserControls.ucTable btn, int orderstatus)
        {
            switch (orderstatus)
            {
                case 0: //����
                    btn.BackColor = lblState0.BackColor;
                    statusnum0 = statusnum0 + 1;
                    break;
                case 1: //�Ͳ�
                    btn.BackColor = lblState1.BackColor;
                    statusnum1 = statusnum1 + 1;
                    break;
                case 3: //�ѽ���
                    statusnum3 = statusnum3 + 1;
                    btn.BackColor = lblState3.BackColor;
                    break;
                case 4: //Ԥ��
                    statusnum4 = statusnum4 + 1;
                    btn.BackColor = lblState4.BackColor;
                    break;
                case 5: //�ѳ���
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
            //���û�д���̨��ô����
            if (btntables != null)
                return;
            try
            {
                frmProgress.ShowProgress("���ڼ�����̨����...");
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
            try
            {
                //ˢ������״̬
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
                lblState0.Text = string.Format("����({0})", statusnum0);
                lblState1.Text = string.Format("�Ͳ�({0})", statusnum1);
                lblState4.Text = string.Format("Ԥ��({0})", statusnum4);
                lblState3.Text = string.Format("(����{0})", statusnum3);
                lblState5.Text = string.Format("����({0})", statusnum5);
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
            btnRefresh.Text = String.Format("ˢ��[{0}]", inttime);
            btnRefresh.Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Globals.userRight.getSyRigth())
            {
                Warning("��û�����Ȩ�ޣ�");
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
                            if (thisMachineNoClearList.Any(t => ClearMachine(t.UserName)))
                                return;
                        }

                        if (noClearnMachineList.Any(t => !t.MachineFlag.Equals(localMac)))//������POS����������
                        {
                            OtherMachineNoClearnWarningWindow warningWnd = new OtherMachineNoClearnWarningWindow();
                            if (warningWnd.ShowDialog() != true)
                                return;
                        }
                    }

                    if (!EndWork())
                        ForcedLogin();
                }
                else //ѡ�񵹰ࡣ
                {
                    ClearMachine();
                    ForcedLogin();
                }
            }
        }

        /// <summary>
        /// ���ҵ����ϡ�
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static bool ClearMachine(string userName = null)
        {
            if (!frmPermission2.ShowPermission2("����Ա���", EnumRightType.ClearMachine, userName))
                //�κ�һ����������ȫ���ʧ�ܾͷ��ء�
                return true;

            ReportPrint.PrintClearMachine(); //��ӡ�������
            ThreadPool.QueueUserWorkItem(t => { RestClient.OpenCash(); });
            Warning("����ɹ�!");
            return false;
        }

        /// <summary>
        /// ǿ�Ƶ�¼��
        /// </summary>
        private void ForcedLogin()
        {
            if (frmLogin.Login()) //����������
            {
                if (Globals.userRight.getSyRigth())
                {
                    if (!frmPosMainV3.checkInputTellerCash())
                    {
                        Application.Exit();
                        return;
                    }
                }
                lblUser.Text = String.Format("��¼Ա��:{0}", Globals.UserInfo.UserName);
            }
            else //��¼ʧ��,�˳�����
                Application.Exit();
        }

        private void btnend_Click(object sender, EventArgs e)
        {
            if (!Globals.userRight.getJyRight())
            {
                Warning("��û�н�ҵȨ�ޣ�");
                return;
            }
            if (TableInfos.Any(t => t.TableStatus == EnumTableStatus.Dinner))
            {
                Warning("����δ���˵Ĳ�̨�����ܽ�ҵ��");
                return;
            }
            if (AskQuestion("ȷ��Ҫ���ڽ�ҵ��"))
                EndWork();
        }

        /// <summary>
        /// ��ҵ����
        /// </summary>
        private bool EndWork()
        {
            if (!frmPermission2.ShowPermission2("��ҵ������Ȩ", EnumRightType.EndWork))
                return false;

            try
            {
                JObject ja = RestClient.endWork(Globals.UserInfo.UserID);
                string data = ja["Data"].ToString();
                if (data.Equals("1"))
                {
                    do
                    {
                        bool result;
                        try
                        {
                            result = RestClient.jdesyndata();//�����ϴ��ص�//http://localhost:8080/newspicyway/padinterface/jdesyndata.json
                        }
                        catch (Exception)
                        {
                            result = false;
                        }

                        if (result)
                            break;

                        if (!AskQuestion("�ϴ�����ʧ�ܣ������ϴ���" + Environment.NewLine + "\"ȷ��\"�����ϴ���\"ȡ��\"������"))
                            break;
                    } while (true);

                    Warning("��ҵ�ɹ�!");
                    Application.Exit();
                    Close();
                    return true;
                }

                var msg = ja["Info"].ToString();
                if (string.IsNullOrEmpty(msg))
                    msg = "��ҵʧ�ܣ�";
                Warning(msg);
                return false;
            }
            catch (Exception ex)
            {
                Warning("��ҵʧ�ܡ�" + ex.Message);
                return false;
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
                Warning("��û������Ȩ�ޣ�");
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