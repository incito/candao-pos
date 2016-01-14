using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Library;
using Common;

namespace Main
{
    public partial class frmProgress : frmBase
    {
        public  string inputNo="";
        public static frmProgress frm;
        public static void ShowProgress(string acaption)
        {
            frm = new frmProgress();
            frm.Text = acaption;
            frm.Show();
        }
        public frmProgress()
        {
            InitializeComponent();
        }
        public void SetProgress(string acaption, int count, int step)
        {
            progressBar1.Maximum = count;
            progressBar1.Value = step;
            Text = acaption;
        }
        private void frmInputText_Load(object sender, EventArgs e)
        {

        }

        private void frmInputText_Activated(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {

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

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
