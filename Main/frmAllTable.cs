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

        private bool _isForcedEndWorkModel;//�Ƿ���ǿ�ƽ�ҵģʽ��

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            lblTime.Text = "��ǰʱ�䣺" + dt.ToLocalTime().ToString();//2005-11-5 21:21:25
        }

        private void btnRBill_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (AskQuestion("ȷ��Ҫ�˳�ϵͳ��")) Application.Exit();
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

                if (_isForcedEndWorkModel)//����Ѿ���ǿ�ƽ�ҵģʽ���ͼ����趨��ǿ�ƽ�ҵ��������������ĵ������ڣ�
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
                    if (_isForcedEndWorkModel)//�����ǿ�ƽ�ҵģʽ����ֻ��������ͲͲ�̨��
                        table.Enabled = tableInfo.TableStatus == EnumTableStatus.Dinner;
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
        /// ����ǿ�ƽ�ҵģʽ��ֻ�ܽ��ˡ�����ͽ�ҵ��
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

                if (DateTime.Now > Globals.TradeTime.BeginTime) //��ǰʱ����ڿ�ҵʱ�䣬ǿ�ƽ�ҵ
                {
                    Globals.TradeTime.BeginTime = Globals.TradeTime.BeginTime.AddDays(1);
                    Warning("���컹δ��ҵ�����Ƚ�ҵ��");
                    SetInForcedEndWorkModel();
                }
                else if (DateTime.Now > Globals.TradeTime.EndTime.AddSeconds(10))//��ǰʱ����ڽ�ҵʱ�䣬��ʾ��ҵ������ʵ��ʱ����ǰ10�롣
                {
                    Globals.TradeTime.EndTime = Globals.TradeTime.EndTime.AddDays(1);//��ʾһ���Ժ��Ҫ�ѽ�ҵʱ��������һ�ν�ҵ����Ȼÿһ�붼����ʾ����Ϣ��
                    Warning("��ҵʱ�䵽�ˣ��뼰ʱ��ҵ��");
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
                else //ѡ�񵹰ࡣ
                {
                    if (CommonHelper.ClearMachine())
                        ForcedLogin();
                }
            }
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
            if (TableInfos.Any(t => t.TableStatus == EnumTableStatus.Dinner))
            {
                Warning("����δ���˵Ĳ�̨�����ܽ�ҵ��");
                return;
            }
            if (AskQuestion("ȷ��Ҫ���ڽ�ҵ��"))
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
                if (AskQuestion("�����쳣���ϴ�ʧ�ܣ��Ƿ������ϴ���"))
                {
                    TaskService.Start(null, EndWorkSyncDataProcess, EndWorkSyncDataComplete, "���������ϴ�...");
                    return;
                }

                Warning("��ҵ�ɹ����������ϴ�ʧ�ܡ�");
            }
            else
            {
                Warning("��ҵ�ɹ�!");
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