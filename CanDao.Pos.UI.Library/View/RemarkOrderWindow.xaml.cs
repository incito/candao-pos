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
using System.Windows.Shapes;
using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// 全单备注设置窗口。
    /// </summary>
    public partial class RemarkOrderWindow
    {
        public RemarkOrderWindow()
        {
            InitializeComponent();
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
        }

        public string Diet
        {
            get { return DietSetCtrl.Diet; }
        }
    }
}
