using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Common;
using Models;

namespace Library
{
    /// <summary>
    /// 选择银行窗口。
    /// </summary>
    public partial class SelectBankWindow
    {
        public SelectBankWindow(BankInfo bankInfo)
        {
            InitializeComponent();
            BankInfos = new ObservableCollection<BankInfo>();
            if (Globals.BankInfos != null)
            {
                Globals.BankInfos.ForEach(BankInfos.Add);
            }
            SelectedBank = bankInfo;

            DataContext = this;
        }

        public ObservableCollection<BankInfo> BankInfos { get; private set; }

        public BankInfo SelectedBank { get; set; }

        private void ButtonConfirm_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
