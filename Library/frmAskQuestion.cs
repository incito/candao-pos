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

namespace Library
{
    public partial class frmAskQuestion : frmBase
    {
        public string inputNo="";
        public static bool ShowAskQuestion(string msg)
        {
            frmAskQuestion frm = new frmAskQuestion();
            if(msg.Length>26)
            {
                frm.lblMsg.Text = msg.Substring(0, 21);
                frm.lblmsg2.Text = msg.Substring(21, msg.Length-21);
            }
            else
            {
                frm.lblMsg.Text = msg;
                frm.lblmsg2.Text = "";
            }

            frm.ShowDialog();

            return frm.DialogResult == DialogResult.OK ;
        }
        public frmAskQuestion()
        {
            InitializeComponent();
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
            Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
