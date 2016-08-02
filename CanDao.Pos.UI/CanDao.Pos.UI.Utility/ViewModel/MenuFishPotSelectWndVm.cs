using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 鱼锅选择窗口VM。
    /// </summary>
    public class MenuFishPotSelectWndVm : NormalWindowViewModel
    {
        #region Constructor

        public MenuFishPotSelectWndVm(MenuFishPotFullInfo info)
        {
            Data = info;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 鱼锅数据。
        /// </summary>
        public MenuFishPotFullInfo Data { get; set; }

        #endregion

        #region Protected Methods

        protected override void Confirm(object param)
        {
            Data.FishPotSelfInfo.SelectedCount = 1;//设定鱼锅本身数量为1
            Data.PotInfo.SelectedCount = 1;//设定锅底数量为1

            base.Confirm(param);
        }

        protected override bool CanConfirm(object param)
        {
            var tasteSetCtrl = ((MenuFishPotSelectWindow) OwnerWindow).TasteSetCtrl;
            return Data.FishDishes.All(t => t.SelectedCount > 0) && (tasteSetCtrl.TasteInfos == null || !string.IsNullOrEmpty(tasteSetCtrl.SelectedTaste));
        }

        #endregion
    }
}