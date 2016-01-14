using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Library;
using Common;
using System.IO;
using Models;
using WebServiceReference;
using ReportsFastReport;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Main
{
    public partial class frmSelectGz : frmBase
    {
        private int downx = 0;
        private int downy = 0;
        private DataView dv = null;
        public static bool ShowSelectGz(out string gzname,out string id)
        {
            frmSelectGz frm = new frmSelectGz();
            frm.ShowDialog();
            bool ret=frm.DialogResult == DialogResult.OK;
            gzname="";
            id = "";
            if(ret)
            {
                gzname = frm.dgvBill.SelectedRows[0].Cells["gzName"].Value.ToString();
                id = frm.dgvBill.SelectedRows[0].Cells["parternerid"].Value.ToString(); 
            }
            return ret;
        }
        public static bool ShowSelectGz(out string gzname, out string id, out string telephone, out string relaperson)
        {
            frmSelectGz frm = new frmSelectGz();
            frm.ShowDialog();
            bool ret = frm.DialogResult == DialogResult.OK;
            gzname = "";
            id = "";
            telephone = "";
            relaperson = "";
            if (ret)
            {
                gzname = frm.dgvBill.SelectedRows[0].Cells["gzName"].Value.ToString();
                id = frm.dgvBill.SelectedRows[0].Cells["code"].Value.ToString();
                telephone = frm.dgvBill.SelectedRows[0].Cells["telephone"].Value.ToString();
                relaperson = frm.dgvBill.SelectedRows[0].Cells["relaperson"].Value.ToString();
            }
            return ret;
        }
        public frmSelectGz()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void frmPettyCash_Activated(object sender, EventArgs e)
        {
            edtTableNo.Focus();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //edtRoom.Focus();
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void edtRoom_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void frmPettyCash_Load(object sender, EventArgs e)
        {
            setBtnFocus();
            btnRefresh_Click(btnRefresh, e);
            Left = 0;
            Top = 0;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                getAllData();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }
        public void getAllData()
        {
            //
            JArray jrOrder = null;
            try
            {
                if (!RestClient.getAllGZDW(Globals.UserInfo.UserName, out jrOrder))
                {
                    return;
                }
            }
            catch { }
            DataTable dtOrder = null;
            dtOrder = Models.Bill_Order.getGz_List(jrOrder);
            dv = new DataView(dtOrder);
            dv.AllowNew = false;
            this.dgvBill.AutoGenerateColumns = false;
            this.dgvBill.DataSource = dv;
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            edtTableNo.Focus();
            SendKeys.Send(((Button)sender).Text);
            //System.Threading.Thread.Sleep(100);
            SendKeys.Flush();
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            edtTableNo.Text = "";
            edtTableNo.Focus();
        }
        private void setBtnFocus()
        {
            Common.Globals.SetButton(button12);
            Common.Globals.SetButton(button11);
            Common.Globals.SetButton(button10);
            Common.Globals.SetButton(button9);
            Common.Globals.SetButton(button8);
            Common.Globals.SetButton(button7);
            Common.Globals.SetButton(button6);
            Common.Globals.SetButton(button5);
            Common.Globals.SetButton(button4);
            Common.Globals.SetButton(button3);
            Common.Globals.SetButton(button2);
            Common.Globals.SetButton(button13);
            Common.Globals.SetButton(button14);
            Common.Globals.SetButton(button15);
            Common.Globals.SetButton(button16);
            Common.Globals.SetButton(button17);
            Common.Globals.SetButton(button18);
            Common.Globals.SetButton(button19);
            Common.Globals.SetButton(button20);
            Common.Globals.SetButton(button22);
            Common.Globals.SetButton(button23);
            Common.Globals.SetButton(button24);
            Common.Globals.SetButton(button25);
            Common.Globals.SetButton(button26);
            Common.Globals.SetButton(button27);
            Common.Globals.SetButton(button28);
            Common.Globals.SetButton(button29);
            Common.Globals.SetButton(button30);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button27_Click(object sender, EventArgs e)
        {

            try
            {
                string select = dgvBill.SelectedRows[0].Cells["gzName"].Value.ToString();
                if (select.Length <= 0)
                    return;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch { }
        }

        private void edtTableNo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (edtTableNo.Text.Length <= 0)
                {
                    dv.RowFilter = "";
                }
                else
                {
                    dv.RowFilter = "py like '%" + edtTableNo.Text + "%'";
                }

            }
            catch
            { }
        }

    }
}