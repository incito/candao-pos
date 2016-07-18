using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.Controls;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 退菜原因选择窗口。
    /// </summary>
    public partial class BackDishReasonSelectWindow
    {
        public BackDishReasonSelectWindow()
        {
            InitializeComponent();
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
            if (SystemConfigCache.BackDishReasonList != null && SystemConfigCache.BackDishReasonList.Any())
                SelectorCtrl.ItemsSource = SystemConfigCache.BackDishReasonList.Select(t => new AllowSelectInfo(t)).ToList();
        }
    }
}
