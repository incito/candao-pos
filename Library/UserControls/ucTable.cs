using System;
using System.Drawing;
using System.Windows.Forms;
using Models;
using Models.Enum;

namespace Library.UserControls
{
    public sealed partial class UcTable : UserControl
    {
        public UcTable()
        {
            InitializeComponent();
        }

        public UcTable(TableInfo tableInfo)
            : this()
        {
            TableInfo = tableInfo;
            lbTableName.Text = tableInfo.TableNo;
            lbCusNum.Text = string.Format("{0}人桌", tableInfo.PeopleNumber);
            switch (tableInfo.TableStatus)
            {
                case EnumTableStatus.Idle:
                    BackColor = Color.White;
                    plInfo.Visible = false;
                    break;
                case EnumTableStatus.Dinner:
                    BackColor = Color.LightSalmon;
                    plInfo.Visible = true;
                    UpdateTimeView();
                    lbAmount.Text = tableInfo.Amount.HasValue ? tableInfo.Amount.Value.ToString("F0") : "0";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TableInfo TableInfo { get; set; }

        public void UpdateTimeView()
        {
            if (TableInfo.TableStatus != EnumTableStatus.Dinner)
                return;

            var time = DateTime.Now - TableInfo.BeginTime;
            if (time.HasValue)
                lbTime.Text = string.Format("{0:00}:{1:00}", time.Value.Hours, time.Value.Minutes);
        }

        private void lable_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }
    }
}
