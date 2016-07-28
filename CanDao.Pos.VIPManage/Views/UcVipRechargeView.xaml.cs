using CanDao.Pos.Common.Controls.CSystem;
using CanDao.Pos.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CanDao.Pos.VIPManage.Views
{
    /// <summary>
    /// UcVipRechargeView.xaml 的交互逻辑
    /// </summary>
    public partial class UcVipRechargeView 
    {
        public Action<MListBoxInfo> SelectAction;
        public Action<string> TexRechargeAction;

        public UcVipRechargeView()
        {
            InitializeComponent();
            LbData.SelectionChanged += LbData_SelectionChanged;
            //LbData.SelectedIndex = 0;
        }

        void LbData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbData.SelectedItem != null)
            {
                var item = LbData.SelectedItem as MListBoxInfo;
                if (item != null)
                {
                    TexSelect.Text = item.Title;
                    if (SelectAction != null)
                    {
                        SelectAction(item);
                    }
                }
            }
            else
            {
                TexSelect.Text = string.Empty;
                if (SelectAction != null)
                {
                    SelectAction(null);
                }
            }
        }

        #region 虚拟键盘绑定

      
        #endregion

        private void TexRecharge_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            decimal outVal = 0;
            if (decimal.TryParse(TexRecharge.Text, out outVal))
            {
                if (TexRechargeAction != null)
                {
                    TexRechargeAction(TexRecharge.Text);
                }
            }

        }
    }
}
