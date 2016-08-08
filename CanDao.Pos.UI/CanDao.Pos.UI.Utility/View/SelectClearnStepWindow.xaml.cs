using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 选择清机步骤的窗口。
    /// </summary>
    public partial class SelectClearnStepWindow
    {
        /// <summary>
        /// 实例化一个选择清机步骤的窗口。
        /// </summary>
        /// <param name="allowEndWork">是否允许结业。</param>
        /// <param name="allowClearn">是否允许清机。</param>
        public SelectClearnStepWindow(bool allowEndWork, bool allowClearn)
        {
            InitializeComponent();
            DataContext = new SelectClearnStepWndVm(allowEndWork, allowClearn) { OwnerWindow = this };
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
