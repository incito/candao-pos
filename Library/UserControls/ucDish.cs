using System;
using System.Drawing;
using System.Windows.Forms;
using Models;
using Models.Enum;

namespace Library.UserControls
{
    public partial class ucDish : UserControl
    {
        public int status = 0;
        public string tableno = "";
        public string tablename = "";
        public string memo = "";
        public ucDish()
        {
            InitializeComponent();
        }

        public ucDish(TableInfo tableInfo)
        {
            InitializeComponent();
            Tag = tableInfo;
            lblNo.Tag = this;
            lbl2.Tag = this;
            lblNo.Text = tableInfo.TableNo;
            lbl2.Text = string.Format("{0}人桌", tableInfo.PeopleNumber);
            status = (int) tableInfo.TableStatus;
            switch (tableInfo.TableStatus)
            {
                case EnumTableStatus.Idle:
                    BackColor = Color.White;
                    break;
                case EnumTableStatus.Dinner:
                    BackColor = Color.LightSalmon;
                    break;
            }
        }

        private void lblNo_Click(object sender, EventArgs e)
        {

        }
        
    }
}
