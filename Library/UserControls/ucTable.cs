using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Library.UserControls
{
    public partial class ucTable : UserControl
    {
        public int status = 0;
        public string tableno = "";
        public string tablename = "";
        public string memo = "";
        public ucTable()
        {
            InitializeComponent();
        }

        private void lblNo_Click(object sender, EventArgs e)
        {

        }
        
    }
}
