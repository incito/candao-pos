using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 选择外卖挂账单位窗口Vm。
    /// </summary>
    public class SelectTakeoutOnAccountCompanyVm : NormalWindowViewModel
    {
        public SelectTakeoutOnAccountCompanyVm(TableInfo tableInfo)
        {
            TableInfo = tableInfo;
        }

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

        /// <summary>
        /// 餐桌信息。
        /// </summary>
        public TableInfo TableInfo { get; set; }

        /// <summary>
        /// 选择挂账单位命令。
        /// </summary>
        public ICommand SelectOnAccountCmd { get; private set; }

        /// <summary>
        /// 选择挂账单位命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void SelectOnAccount(object arg)
        {
            var companyWnd = new OnAccountCompanySelectWindow();
            if (WindowHelper.ShowDialog(companyWnd, OwnerWindow))
            {
                OnAccountInfo = companyWnd.SelectedCompany;
            }
        }

        #region Protected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            SelectOnAccountCmd = CreateDelegateCommand(SelectOnAccount);
        }

        protected override void Confirm(object param)
        {
            TaskService.Start(null, SetTakeoutOrderOnAccountProcess, SetTakeoutOrderOnAccountComplete, "设置外卖挂单中...");
        }

        protected override bool CanConfirm(object param)
        {
            return OnAccountInfo != null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 设置外卖挂单执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object SetTakeoutOrderOnAccountProcess(object param)
        {
            InfoLog.Instance.I("开始设置外卖挂单...");
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return "创建IRestaurantService服务失败。";

            var request = new SetTakeoutOrderOnAccountRequest
                {
                    CmpCode = OnAccountInfo.Id,
                    CmpName = OnAccountInfo.Name,
                    ContactMobile = ContactMobile,
                    ContactName = ContactName,
                };
            return service.SetTakeoutOrderOnAccount(TableInfo.TableNo, TableInfo.OrderId, request);
        }

        /// <summary>
        /// 设置外卖挂单执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void SetTakeoutOrderOnAccountComplete(object param)
        {
            var result = (string)param;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result);
                return;
            }

            InfoLog.Instance.I("设置外卖挂单成功。");
            MessageDialog.Warning(string.Format("设置外卖挂单成功，挂单单号：{0}", TableInfo.OrderId));
            CloseWindow(true);
        }

        #endregion
    }
}