using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
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

        #region Command

        public ICommand WndLoadCmd { get; private set; }

        #endregion

        #region Command Methods

        private void WindowLoad(object arg)
        {

        }

        #endregion

        #region Protected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            WndLoadCmd = CreateDelegateCommand(WindowLoad);
        }

        protected override void Confirm(object param)
        {
            Data.FishPotSelfInfo.SelectedCount = 1;//设定鱼锅本身数量为1
            Data.PotInfo.SelectedCount = 1;//设定锅底数量为1

            base.Confirm(param);
        }

        protected override bool CanConfirm(object param)
        {
            return Data.FishDishes.All(t => t.SelectedCount > 0) && !string.IsNullOrEmpty(((MenuFishPotSelectWindow)OwnerWindow).TasteSetCtrl.SelectedTaste);
        }

        #endregion
    }
}