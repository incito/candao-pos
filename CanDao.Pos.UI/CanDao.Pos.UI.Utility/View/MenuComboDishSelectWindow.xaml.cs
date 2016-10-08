using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 套餐选择窗口。
    /// </summary>
    public partial class MenuComboDishSelectWindow
    {
        public MenuComboDishSelectWindow()
        {
            InitializeComponent();
        }

        public MenuComboFullInfo ComboFullInfo
        {
            get { return ((MenuComboDishSelectWndVm) DataContext).Data; }
        }

        /// <summary>
        /// 选择的忌口信息。
        /// </summary>
        public string SelectedDiet
        {
            get { return DietSetCtrl.SelectedInfo; }
        }
    }
}
