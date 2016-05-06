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
        #region Fields

        private static ReportViewWindow _instance;
        private static readonly object LockObj = new object();

        /// <summary>
        /// 当前选中的周期。
        /// </summary>
        private ToggleButton _curSelectTbBtn;

        /// <summary>
        /// 品项全信息。
        /// </summary>
        private DishSaleFullInfo _dishSaleFullInfo;

        /// <summary>
        /// 当前视图序号。
        /// </summary>
        private int _curViewIndex;

        private const int PageSize = 11;

        #endregion

        #region Constructor

        private ReportViewWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public static ReportViewWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ReportViewWindow();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Event Methods

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void ReportViewWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.IsInDesignTool())
                return;

            DxTiDish.IsSelected = true;
        }

        #endregion

        public void Init()
        {
            
        }

        private void ReportViewWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
