using System.Collections.Generic;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 赠菜选择窗口。
    /// </summary>
    public partial class SelectGiftDishWindow
    {
        public SelectGiftDishWindow(TableFullInfo tableFullInfo)
        {
            InitializeComponent();
            DataContext = new SelectGiftDishWndVm(tableFullInfo) { OwnerWindow = this };
        }

        /// <summary>
        /// 选择的赠菜集合。
        /// </summary>
        public List<GiftDishInfo> SelectedGiftDishInfos
        {
            get { return ((SelectGiftDishWndVm) DataContext).SelectedGiftDishInfos; }
        }
    }
}
