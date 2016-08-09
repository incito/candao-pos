using System.Windows.Input;
using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class SelectClearnStepWndVm : NormalWindowViewModel
    {
        #region Constructor

        public SelectClearnStepWndVm(bool allowEndWork, bool allowClearn)
        {
            AllowEndWork = allowEndWork;
            AllowChangeShift = allowClearn;
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

        #region Protected Methods

        protected override void OperMethod(object param)
        {
            switch ((string)param)
            {
                case "ChangeShift":
                    IsEndWork = false;
                    break;
                case "EndWork":
                    IsEndWork = true;
                    break;
            }
            CloseWindow(true);
        }

        protected override bool CanOperMethod(object param)
        {
            switch ((string)param)
            {
                case "ChangeShift":
                    return AllowChangeShift;
                case "EndWork":
                    return AllowEndWork;
                default:
                    return true;
            }
        }

        #endregion
    }
}