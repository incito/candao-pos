using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 选择清机步骤的窗口。
    /// </summary>
    public partial class SelectClearnStepWindow
    {
        public SelectClearnStepWindow(bool allowEndWork, bool isForcedEndWorkModel)
        {
            InitializeComponent();
            DataContext = new SelectClearnStepWndVm(allowEndWork, isForcedEndWorkModel) { OwnerWindow = this };
        }

        /// <summary>
        /// 是否选择结业。
        /// </summary>
        public bool IsEndWork
        {
            get { return ((SelectClearnStepWndVm) DataContext).IsEndWork; }
        }
    }
}
