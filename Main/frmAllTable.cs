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
using WebServiceReference;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;

namespace Main
{
    public partial class frmAllTable : frmBase
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hwnd);

        private List<UcTable> _tableControls;
        /// <summary>
        /// �и�����
        /// </summary>
        private const int Columncount = 8;
        /// <summary>
        /// �и�����
        /// </summary>
        private const int RowCount = 6;
        /// <summary>
        /// ��̨��ҳ����
        /// </summary>
        private int totalTablePageCount = 1;
        /// <summary>
        /// ��ǰ��̨ҳ����
        /// </summary>
        private int curTablePage = 1;

        private int btnWidth = 115;
        private int btnHeight = 78;
        private int btnSpace = 9;

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
            lblbranchid.Text = String.Format("���̱�ţ�{0}", Globals.BranchInfo.BranchId);
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
            RestClient.OpenCashCom();
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
            var tableInfo = ((UcTable)sender).TableInfo;
            string tableno = tableInfo.TableNo;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                timer2.Enabled = false;
                //frmPosMain.ShowPosMain(tableno, uctable.status);
                frmpos.ShowFrm(tableno, (int)tableInfo.TableStatus);

                if (_isForcedEndWorkModel)//����Ѿ���ǿ�ƽ�ҵģʽ���ͼ����趨��ǿ�ƽ�ҵ��������������ĵ������ڣ�
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
                this.Update();//����

                IRestaurantService service = new RestaurantServiceImpl();
                var result = service.GetAllTableInfoes();
                if (!string.IsNullOrEmpty(result.Item1))
                {
                    Warning(result.Item1);
                    return;
                }

                TableInfos = result.Item2.Where(t => t.TableType != EnumTableType.Takeout).ToList();//����ʾ����̨��
                totalTablePageCount = (TableInfos.Count + Columncount * RowCount - 1) / (Columncount * RowCount);
                panelPage.Visible = totalTablePageCount > 1;
                UpdatePageButtonEnableStatus();
                CreateTableControls();

                lblState0.Text = string.Format("����({0})", TableInfos.Count(t => t.TableStatus == EnumTableStatus.Idle));
                lblState1.Text = string.Format("�Ͳ�({0})", TableInfos.Count(t => t.TableStatus == EnumTableStatus.Dinner));
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
        /// ������̨�ؼ���
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
                    if (_isForcedEndWorkModel)//�����ǿ�ƽ�ҵģʽ����ֻ��������ͲͲ�̨��
                        table.Enabled = tableInfo.TableStatus == EnumTableStatus.Dinner;
                    idx++;
                }
            }
            finally
            {
                LockWindowUpdate(IntPtr.Zero);
            }
        }

        /// <summary>
        /// ����ǿ�ƽ�ҵģʽ��ֻ�ܽ��ˡ�����ͽ�ҵ��
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
            timer2.Enabled = false;
            if (AskQuestion("ȷ��Ҫ���ڽ�ҵ��"))
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
                Warning("��û������Ȩ�ޣ�");
                return;
            }
            try
            {
                Cursor = Cursors.WaitCursor;
                timer2.Enabled = false;
                frmposwm.showFrmWm(RestClient.TakeOutTable);
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
        /// ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReport_Click(object sender, EventArgs e)
        {
            BigDataHelper.DeviceActionAsync(EnumDeviceAction.ReportClicking);
            timer2.Stop();
            ReportViewWindow.Instance.ShowDialog();
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
    }
}