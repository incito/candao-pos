using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.Controls;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 赠菜原因选择窗口。
    /// </summary>
    public partial class DishGiftReasonSelectWindow
    {
        public DishGiftReasonSelectWindow()
        {
            InitializeComponent();
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
            if (SystemConfigCache.DishGiftReasonList != null && SystemConfigCache.DishGiftReasonList.Any())
                SelectorCtrl.ItemsSource = SystemConfigCache.DishGiftReasonList.Select(t => new AllowSelectInfo(t)).ToList();
        }

        /// <summary>
        /// 选择的原因。
        /// </summary>
        public string SelectedReason
        {
            get { return SelectorCtrl.SelectedInfo; }
        }
    }
}
