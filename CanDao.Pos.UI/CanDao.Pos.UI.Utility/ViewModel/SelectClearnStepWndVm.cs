using System.Windows.Input;
using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class SelectClearnStepWndVm : NormalWindowViewModel
    {
        #region Constructor

        public SelectClearnStepWndVm(bool allowEndWork, bool isForcedEndWorkModel)
        {
            AllowEndWork = allowEndWork;
            AllowChangeShift = !isForcedEndWorkModel;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// 是否允许结业。
        /// </summary>
        public bool AllowEndWork { get; set; }

        /// <summary>
        /// 是否允许换班。
        /// </summary>
        public bool AllowChangeShift { get; set; }

        /// <summary>
        /// 是否选择结业。
        /// </summary>
        public bool IsEndWork { get; set; }

        #endregion

        #region Command

        public ICommand ChangeShiftCmd { get; private set; }

        public ICommand EndWorkCmd { get; set; }

        #endregion

        #region Command Methods

        private void ChangeShift(object arg)
        {
            IsEndWork = false;
            CloseWindow(true);
        }

        private bool CanChangeShift(object arg)
        {
            return AllowChangeShift;
        }

        private void EndWork(object arg)
        {
            IsEndWork = true;
            CloseWindow(true);
        }

        private bool CanEndWork(object arg)
        {
            return AllowEndWork;
        }

        #endregion

        #region Protected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            ChangeShiftCmd = CreateDelegateCommand(ChangeShift, CanChangeShift);
            EndWorkCmd = CreateDelegateCommand(EndWork, CanEndWork);
        } 

        #endregion
    }
}