using System.Windows;
using DevExpress.Xpf.Core;

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
