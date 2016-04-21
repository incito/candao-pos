using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Common;
using DevExpress.Xpf.Core;
using Library;
using Models;
using Models.Enum;
using ReportsFastReport;
using WebServiceReference.IService;
using WebServiceReference.ServiceImpl;
using Keyboard = Library.Keyboard;

namespace KYPOS
{
    /// <summary>
    /// 报表显示窗口。
    /// </summary>
    public partial class ReportViewWindow
    {
        #region Constructor

        public ReportViewWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Methods

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ReportViewWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.IsInDesignTool())
                return;

            DxTiDish.IsSelected = true;
        }

        #endregion
    }
}
