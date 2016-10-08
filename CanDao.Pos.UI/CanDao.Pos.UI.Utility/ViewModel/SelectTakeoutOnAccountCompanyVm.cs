using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 选择外卖挂账单位窗口Vm。
    /// </summary>
    public class SelectTakeoutOnAccountCompanyVm : NormalWindowViewModel<SelectTakeoutOnAccountCompany>
    {
        #region Properties

        /// <summary>
        /// 挂账单位信息。
        /// </summary>
        private OnCompanyAccountInfo _onAccountInfo;

        /// <summary>
        /// 挂账单位信息。
        /// </summary>
        public OnCompanyAccountInfo OnAccountInfo
        {
            get { return _onAccountInfo; }
            set
            {
                _onAccountInfo = value;
                RaisePropertyChanged("OnAccountInfo");
            }
        }

        /// <summary>
        /// 联系人手机。
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 联系人姓名。
        /// </summary>
        public string ContactName { get; set; }

        #endregion

        #region Command

        /// <summary>
        /// 选择挂账单位命令。
        /// </summary>
        public ICommand SelectOnAccountCmd { get; private set; }
        
        #endregion

        #region Command Methods

        /// <summary>
        /// 选择挂账单位命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void SelectOnAccount(object arg)
        {
            var companyWnd = new OnAccountCompanySelectWndVm();
            if (WindowHelper.ShowDialog(companyWnd, OwnerWindow))
                OnAccountInfo = companyWnd.SelectedCompany;
        }
        
        #endregion

        #region Protected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            SelectOnAccountCmd = CreateDelegateCommand(SelectOnAccount);
        }

        protected override bool CanConfirm(object param)
        {
            return OnAccountInfo != null;
        }

        #endregion
    }
}