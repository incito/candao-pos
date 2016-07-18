using System.Linq;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.Controls;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 鱼锅选择窗口。
    /// </summary>
    public partial class MenuFishPotSelectWindow
    {
        public MenuFishPotSelectWindow(MenuFishPotFullInfo info)
        {
            InitializeComponent();
            DataContext = new MenuFishPotSelectWndVm(info) { OwnerWindow = this };
            TasteSetCtrl.TasteInfos = info.FishPotSelfInfo.Tastes.Select(t => new AllowSelectInfo(t)).ToList();
        }

        public MenuFishPotFullInfo FishPotFullInfo
        {
            get { return ((MenuFishPotSelectWndVm)DataContext).Data; }
        }

        /// <summary>
        /// 选择的口味信息。
        /// </summary>
        public string SelectedTaste
        {
            get { return TasteSetCtrl.SelectedTaste; }
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
