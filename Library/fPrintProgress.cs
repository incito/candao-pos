using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Library;
using Common;

namespace Library
{
    public partial class frmPrintProgress : frmBase
    {
        public string inputNo="";
        public int maxnum = 0;
        private delegate void ThreadWork(int o);
        Thread thread;


        public static bool ShowInputNum()
        {
            frmPrintProgress frm = new frmPrintProgress();
            frm.ShowDialog();
            bool ret=frm.DialogResult == DialogResult.OK ;
            return ret; 
        }
        public frmPrintProgress()
        {
            InitializeComponent();
        }

        private void frmInputText_Load(object sender, EventArgs e)
        {
            setBtnFocus();

            
        }
        public void hideFrm()
        {
            //tmrProgress.Enabled = false;
            //thread.Abort();
            tmrProgress.Tag = 0;
            Hide();
        }
        public void showFrm()
        {
            tmrProgress.Tag = 0;
            Show();
            //tmrProgress.Enabled = true;
            //UpdateText();
        }
        public void Progress(string msg)
        {
            lblMsg.Text = msg;
            Application.DoEvents();
        }
        private void frmInputText_Activated(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {

        }
        public void UpdateText()
        {
            thread = new Thread(new ThreadStart(CrossThreadFlush));
            thread.IsBackground = true;
            thread.Start();
        }
        private void CrossThreadFlush()
        {
            while (true)
            { //将sleep和无限循环放在等待异步的外面 

                for (int i = 1; i < 100; i++)
                {
                    ThreadFunction(i);
                    Thread.Sleep(100);
                }
            }
        }

        private void ThreadFunction(int o)
        {
            if (this.lblMsg.InvokeRequired)//等待异步 
            {
                ThreadWork fc = new ThreadWork(ThreadFunction);
                // this.Invoke(fc);//通过代理调用刷新方法 
                this.Invoke(fc, new object[1] { o });

            }
            else
            {
                string diang = ".";
                int i = int.Parse(this.lblMsg.Tag.ToString());
                if (i <= 2)
                    i = i + 1;
                else
                    i = 1;
                diang = diang.PadRight(i, '.');
                this.lblMsg.Tag = i.ToString();
                this.lblMsg.Text = String.Format("正在打印{0}", diang);
                this.lblMsg.Refresh();
                Application.DoEvents();
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
 
        }

        private void button27_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
  
        }
        private void setBtnFocus()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void tmrProgress_Tick(object sender, EventArgs e)
        {
            try
            {
                    tmrProgress.Enabled = false;
                    string diang = ".";
                    int i = int.Parse(tmrProgress.Tag.ToString());
                    if (i <= 2)
                        i = i + 1;
                    else
                        i = 1;
                    diang=diang.PadRight(i, '.');
                    tmrProgress.Tag = i.ToString();
                    lblMsg.Text = String.Format("正在打印{0}", diang);
                    Application.DoEvents();


            }
            finally
            {
                tmrProgress.Enabled = true;
            }
        }
    }
}
