using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// JDE重新上传数据窗口。
    /// </summary>
    public partial class JdeReloadWindow
    {
        public JdeReloadWindow()
        {
            InitializeComponent();
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
        }
    }
}
