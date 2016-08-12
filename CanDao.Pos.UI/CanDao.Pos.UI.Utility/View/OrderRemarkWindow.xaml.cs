using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 全单备注设置窗口。
    /// </summary>
    public partial class OrderRemarkWindow
    {
        /// <summary>
        /// 实例化一个全单备注设置窗口。
        /// </summary>
        public OrderRemarkWindow(string diet)
        {
            InitializeComponent();
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
            DietSetCtrl.SetInitValue(diet);
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
