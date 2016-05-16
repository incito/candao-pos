using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Library;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Common;
using Models;
using WebServiceReference;
using System.Runtime.InteropServices;
using ReportsFastReport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Main
{
    
    public partial class frmQueryBill : frmBase
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool LockWindowUpdate(IntPtr hwnd);
        private int downx = 0;
        private int downy = 0;
        private DataTable dtOrder = null;
        private DataView dv=null;
        private frmAllTable frmtable=null;

        public static void ShowQueryBill(frmAllTable frmtable, bool isForcedEndWorkModel)
        {
            frmQueryBill frm = new frmQueryBill();
            frm.frmtable = frmtable;
            if (isForcedEndWorkModel)
            {
                frm.button2.Enabled = false;
                frm.button3.Enabled = false;
                frm.btnRePrintClear.Enabled = false;
            }
            frm.ShowDialog();
        }

        public frmQueryBill()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button26_Click(object sender, EventArgs e)
        {
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            tbxBill.Text = "";
            tbxBill.Focus();
            edtTableNo.Text = "";
            edtTableNo.Focus();
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
            Common.Globals.SetButton(button1);
            Common.Globals.SetButton(button16);
        }

        private void frmQueryBill_Load(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                setBtnFocus();
                getAllBill();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            Left = 0;
            Top = 0;
        }

        private void frmQueryBill_Activated(object sender, EventArgs e)
        {
            tbxBill.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        public  void getAllBill()
        {
            //
            JArray jrOrder = null;
            try
            {
                if (!RestClient.getAllOrderInfo2(Globals.UserInfo.UserName, out jrOrder))
                {
                    return;
                }
            }
            catch { }

            dtOrder = Models.Bill_Order.getOrder(jrOrder);
            dv = new DataView(dtOrder);
            dv.AllowNew = false;
            this.dgvBill.AutoGenerateColumns = false;
            this.dgvBill.DataSource = dv;
            for (int i = 0; i <= dgvBill.Rows.Count - 1; i++)
            {
                DataRowView mydrv = dv[i];
                string orderstatus = Convert.ToString(mydrv["orderstatus"]);//要判断的字段
                if (orderstatus.Equals("3") || orderstatus.Equals("2"))
                {
                    mydrv["orderstatus"] = "已结";
                }
                else
                {
                    dgvBill.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    dgvBill.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    mydrv["orderstatus"] = "未结";
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                getAllBill();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void dgvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            dgvBill.Select();
            //SendKeys.Send("{PGDN}");
            SendKeys.Send("{PGUP}");
        }

        private void pageDown()
        {
            dgvBill.Select();
            SendKeys.Send("{PGDN}");
            //SendKeys.Send("{PGUP}");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            pageUp();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pageDown();
        }
        private void tbxBill_EditValueChanged(object sender, EventArgs e)
        {
            //
            try
            {
                if (tbxBill.Text.Length <= 0)
                {
                    dv.RowFilter = "";
                }
                else
                {
                    dv.RowFilter = "orderid like '%" + tbxBill.Text + "%'";
                }
                
            }catch
            { }
        }
        private void edtTableNo_EditValueChanged(object sender, EventArgs e)
        {
            //
            try
            {
                if (edtTableNo.Text.Length <= 0)
                {
                    dv.RowFilter = "";
                }
                else
                {
                    dv.RowFilter = "tableName like '%" + edtTableNo.Text + "%'";
                }

            }
            catch
            { }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                if(dgvBill.RowCount<=0)
                {
                    return;
                }
                string orderid = dgvBill.SelectedRows[0].Cells["orderid"].Value.ToString();
                string orderstatus = dgvBill.SelectedRows[0].Cells["orderstatus"].Value.ToString();
                if (orderstatus.Equals("未结"))
                {
                    Warning("选择的单据未结算！");
                    return;
                }
                if (!AskQuestion(orderid + "确定要重印吗?"))
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                PrintBill2(orderid);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void PrintBill2(string orderid)
        {
            ReportPrint.PrintPayBill2(orderid, Globals.UserInfo.UserName);
        }

        private void btnRePrintClear_Click(object sender, EventArgs e)
        {
           //
            ReportPrint.PrintClearMachine();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBill.RowCount <= 0)
                {
                    return;
                }
                if (!Globals.userRight.getSyRigth())
                {
                    Warning("您没有收银权限！");
                    return;
                }
                try
                {
                    string orderid = dgvBill.SelectedRows[0].Cells["orderid"].Value.ToString();
                    string orderstatus = dgvBill.SelectedRows[0].Cells["orderstatus"].Value.ToString();
                    if (orderstatus.Equals("未结"))
                    {
                        Warning("选择的单据未结算！");
                        return;
                    }
                    if (!AskQuestion(orderid + "确定反结吗?"))
                    {
                        return;
                    }
                    string errStr = "";
                    if (RestClient.rebackorder(Globals.UserInfo.UserID, orderid, ref errStr))
                    {
                        //frmPosMain.ShowPosMain(errStr, 9);
                        frmtable.frmpos.ShowFrm(errStr, 9);
                    }
                    else
                    {
                        Warning(errStr);
                    }
                }
                catch { }
                this.Cursor = Cursors.WaitCursor;
                //PrintBill2(orderid);
                //反结算，逻辑服务端判断，如果要保存原来的帐单，加两个结构一样的表，

            }
            finally
            {
                btnRefresh_Click(sender, e);
                this.Cursor = Cursors.Default;
            }
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBill.RowCount <= 0)
                {
                    return;
                }
                if (!Globals.userRight.getSyRigth())
                {
                    Warning("您没有收银权限！");
                    return;
                }
                try
                {
                    string orderid = dgvBill.SelectedRows[0].Cells["orderid"].Value.ToString();
                    string orderstatus = dgvBill.SelectedRows[0].Cells["orderstatus"].Value.ToString();
                    if (orderstatus.Equals("已结"))
                    {
                        Warning("选择的单据已经结算！");
                        return;
                    }

                    if (!AskQuestion(orderid + "确定结算吗?"))
                    {
                        return;
                    }
                    string errStr = "";
                    if (RestClient.accountsorder(Globals.UserInfo.UserID, orderid, ref errStr))
                    {
                        //frmPosMain.ShowPosMain(errStr, 9);
                        frmtable.frmpos.ShowFrm(errStr, 8);
                    }
                    else
                    {
                        Warning(errStr);
                    }
                }
                catch { }
                this.Cursor = Cursors.WaitCursor;
                //PrintBill2(orderid);
                //反结算，逻辑服务端判断，如果要保存原来的帐单，加两个结构一样的表，

            }
            finally
            {
                btnRefresh_Click(sender, e);
                this.Cursor = Cursors.Default;
            }
        }

        private void bthCheck1_Click(object sender, EventArgs e)
        {

        }

        private void btnCheck1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnCheck1_Click(object sender, EventArgs e)
        {
            string tag=((DevExpress.XtraEditors.CheckButton)sender).Tag.ToString();
            string filter = "";
            try
            {
                if (tag == "1")
                {
                    btnCheck2.Checked = false;
                    btnCheck3.Checked = false;
                    filter = "";
                    return;
                }
                else
                    if (tag == "2")
                    {
                        btnCheck1.Checked = false;
                        btnCheck3.Checked = false;
                        filter = " orderstatus='已结' or orderstatus='3' ";
                    }
                    else
                        if (tag == "3")
                        {
                            btnCheck1.Checked = false;
                            btnCheck2.Checked = false;
                            filter = " orderstatus='未结' or  orderstatus='0'";
                        }
            }
            finally
            {
                if (dgvBill.RowCount>0)
                  dgvBill.Rows[dgvBill.RowCount-1].Selected = true; 
                dv = null;
                dv = new DataView(dtOrder);
                dv.RowFilter = filter;
                dv.AllowNew = false;
                this.dgvBill.DataSource = dv;
                ((DevExpress.XtraEditors.CheckButton)sender).Checked = false;
                setGridColor();
            }
        }
        private void setGridColor()
        {
            for (int i = 0; i <= dgvBill.Rows.Count - 1; i++)
            {
                DataRowView mydrv = dv[i];
                string orderstatus = Convert.ToString(mydrv["orderstatus"]);//要判断的字段
                if ((orderstatus.Equals("3")) || (orderstatus.Equals("2")))
                {
                    mydrv["orderstatus"] = "已结";
                }
                else
                {
                    dgvBill.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    mydrv["orderstatus"] = "未结";
                }
            }
        }

        private void dgvBill_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void dgvBill_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
        {

        }

        private void dgvBill_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            setGridColor();
        }
    }

}