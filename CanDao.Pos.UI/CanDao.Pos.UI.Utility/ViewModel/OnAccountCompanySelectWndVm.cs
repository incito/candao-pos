using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 挂账单位选择窗口Vm。
    /// </summary>
    public class OnAccountCompanySelectWndVm : NormalWindowViewModel<OnAccountCompanySelectWindow>
    {
        private List<OnCompanyAccountInfo> _srcCmpInfos;

        public OnAccountCompanySelectWndVm()
        {
            CompanyInfos = new ObservableCollection<OnCompanyAccountInfo>();
        }

        /// <summary>
        /// 挂账单位集合。
        /// </summary>
        public ObservableCollection<OnCompanyAccountInfo> CompanyInfos { get; private set; }

        /// <summary>
        /// 选择的挂账单位。
        /// </summary>
        public OnCompanyAccountInfo SelectedCompany { get; set; }

        /// <summary>
        /// 过滤字母。
        /// </summary>
        private string _filterLetter;
        /// <summary>
        /// 过滤字母。
        /// </summary>
        public string FilterLetter
        {
            get { return _filterLetter; }
            set
            {
                _filterLetter = value;
                RaisePropertiesChanged("FilterLetter");

                FilterCompany();
            }
        }

        /// <summary>
        /// 清除过滤字母命令。
        /// </summary>
        public ICommand ClearFilterLetterCmd { get; private set; }

        /// <summary>
        /// 清除过滤字母。
        /// </summary>
        /// <param name="arg"></param>
        private void ClearFilterLetter(object arg)
        {
            FilterLetter = "";
        }

        protected override void InitCommand()
        {
            base.InitCommand();
            ClearFilterLetterCmd = CreateDelegateCommand(ClearFilterLetter);
        }

        protected override void OnWindowLoaded(object param)
        {
            TaskService.Start(null, GetAllOnAccountCompanyProcess, GetAllOnAccountCompanyComplete, "获取挂账单位...");
        }

        protected override bool CanConfirm(object param)
        {
            return SelectedCompany != null;
        }

        protected override void OperMethod(object param)
        {
            switch (param as string)
            {
                case "PreGroup":
                    ((OnAccountCompanySelectWindow)OwnerWindow).GsCpys.PreviousGroup();
                    break;
                case "NextGroup":
                    ((OnAccountCompanySelectWindow)OwnerWindow).GsCpys.NextGroup();
                    break;
            }
        }

        protected override bool CanOperMethod(object param)
        {
            switch (param as string)
            {
                case "PreGroup":
                    return ((OnAccountCompanySelectWindow)OwnerWindow).GsCpys.CanPreviousGroup;
                case "NextGroup":
                    return ((OnAccountCompanySelectWindow)OwnerWindow).GsCpys.CanNextGruop;
                default:
                    return true;
            }
        }

        /// <summary>
        /// 获取所有挂账单位的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object GetAllOnAccountCompanyProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, List<BackDishInfo>>("创建IRestaurantService服务失败。", null);

            return service.GetAllOnAccountCompany();
        }

        /// <summary>
        /// 获取所有挂账单位执行完成。
        /// </summary>
        /// <param name="obj"></param>
        private void GetAllOnAccountCompanyComplete(object obj)
        {
            InfoLog.Instance.I("结束获取挂账单位。");
            var result = (Tuple<string, List<OnCompanyAccountInfo>>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取挂账单位失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            _srcCmpInfos = result.Item2;
            FilterCompany();
        }

        /// <summary>
        /// 过滤可选的挂账单位。
        /// </summary>
        private void FilterCompany()
        {
            if (_srcCmpInfos == null)
                return;

            CompanyInfos.Clear();
            if (string.IsNullOrEmpty(FilterLetter))
                _srcCmpInfos.ForEach(CompanyInfos.Add);
            else
                _srcCmpInfos.Where(t => t.NameFirstLetter.ToLower().Contains(FilterLetter.ToLower())).ToList().ForEach(CompanyInfos.Add);
        }
    }
}