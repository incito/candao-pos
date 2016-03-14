using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            ReportPrint.Init();
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
                this.Cursor = Cursors.WaitCursor;
                btnRefresh.Enabled = false;
                this.Update();//����
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
            //���û�д���̨��ô����
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
                {
                    inttime = inttime - 1;
                    btnRefresh.Tag = inttime;
                    btnRefresh.Text = String.Format("ˢ��[{0}]", inttime);
                    btnRefresh.Update();
                    return;
                }
                inttime = 5;
                btnRefresh.Tag = inttime;
                btnRefresh.Text = String.Format("ˢ��[{0}]", inttime);
                btnRefresh.Update();
                button3_Click(btnRefresh, e);
            }
            finally
            { timer2.Enabled = true; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //��� ��ӡ����������������Ϊ�����״̬��������һ������Ա ¼�����ҽ�
            //����Ȩ��
            if (!Globals.userRight.getSyRigth())
            {
                Warning("��û�����Ȩ�ޣ�");
                return;
            }
            if (!AskQuestion("ȷ��Ҫ���������"))
            {
                return;
            }
            if (!frmPermission2.ShowPermission2("����Ա���", eRightType.right4))
            {
                return;
            }
            try
            {
                string authorizer = Globals.authorizer;
                JObject ja = RestClient.clearMachine(Globals.UserInfo.UserID, Globals.UserInfo.UserName, authorizer);
                string data = ja["Data"].ToString();
                if (data.Equals("1"))
                {
                    //��ӡ�������
                    ReportPrint.PrintClearMachine();
                    RestClient.OpenCash();
                    Warning("����ɹ�!");
                    //����������
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
                        lblUser.Text = String.Format("��¼Ա��:{0}", Globals.UserInfo.UserName);
                    }
                    else//��¼ʧ��,�˳�����
                        Application.Exit();

                    return;
                }
                else
                {
                    Warning(ja["Info"].ToString());
                }

            }
            catch { }

        }

        private void btnend_Click(object sender, EventArgs e)
        {
            if (!Globals.userRight.getJyRight())
            {
                Warning("��û�н�ҵȨ�ޣ�");
                return;
            }
            if (!AskQuestion("ȷ��Ҫ���ڽ�ҵ��"))
            {
                return;
            }
            //��� ��ӡ����������������Ϊ�����״̬��������һ������Ա ¼�����ҽ�
            //����Ȩ��
            if (!frmPermission2.ShowPermission2("��ҵ������Ȩ", eRightType.right5))
            {
                return;
            }
            try
            {
                JObject ja = RestClient.endWork(Globals.UserInfo.UserID);
                string data = ja["Data"].ToString();
                if (data.Equals("1"))
                {
                    TaskService.Start(null, EndWorkSyncDataProcess, EndWorkSyncDataComplete, "���������ϴ�...");
                }
                else
                {
                    var msg = ja["Info"].ToString();
                    if (string.IsNullOrEmpty(msg))
                        msg = "��ҵʧ�ܣ�";
                    Warning(msg);
                }

            }
            catch (Exception ex)
            {
                Warning("��ҵʧ�ܡ�" + ex.Message);
            }

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