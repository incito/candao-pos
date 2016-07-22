using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 菜品称重窗口。
    /// </summary>
    public partial class DishWeightWindow
    {
        public DishWeightWindow()
        {
            InitializeComponent();
            DataContext = new DishWeightWndVm { OwnerWindow = this };
            CloseBtnEnable = false;
        }

        /// <summary>
        /// 菜品称重数量。
        /// </summary>
        public decimal DishWeightNum
        {
            get { return ((DishWeightWndVm) DataContext).DishWeight; }
        }
    }
}
