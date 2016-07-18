using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;
using DevExpress.Xpf.Editors.Helpers;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 挂账单位选择窗口Vm。
    /// </summary>
    public class OnAccountCompanySelectWndVm : NormalWindowViewModel
    {
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
        /// 窗口加载时命令。
        /// </summary>
        public ICommand WindowLoadCmd { get; private set; }

        /// <summary>
        /// 清除过滤字母。
        /// </summary>
        /// <param name="arg"></param>
        private void ClearFilterLetter(object arg)
        {
            FilterLetter = "";
        }

        /// <summary>
        /// 窗口加载。
        /// </summary>
        /// <param name="arg"></param>
        private void WindowLoad(object arg)
        {
            if (Globals.OnCompanyInfos == null || !Globals.OnCompanyInfos.Any())
                TaskService.Start(null, GetAllOnAccountCompanyProcess, GetAllOnAccountCompanyComplete, "获取挂账单位...");
            else
                FilterCompany();
        }

        protected override void InitCommand()
        {
            base.InitCommand();
            ClearFilterLetterCmd = CreateDelegateCommand(ClearFilterLetter);
            WindowLoadCmd = CreateDelegateCommand(WindowLoad);
        }

        protected override bool CanConfirm(object param)
        {
            return SelectedCompany != null;
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

            result.Item2.Add(new OnCompanyAccountInfo() { Id = "1", Name = "ABCDEFG", NameFirstLetter = "abcdefg" });
            result.Item2.Add(new OnCompanyAccountInfo() { Id = "2", Name = "你好", NameFirstLetter = "nh" });
            result.Item2.Add(new OnCompanyAccountInfo() { Id = "3", Name = "我爱你", NameFirstLetter = "wai" });
            Globals.OnCompanyInfos = result.Item2;
            FilterCompany();
        }

        /// <summary>
        /// 过滤可选的挂账单位。
        /// </summary>
        private void FilterCompany()
        {
            if (Globals.OnCompanyInfos == null)
                return;

            CompanyInfos.Clear();
            if (string.IsNullOrEmpty(FilterLetter))
                Globals.OnCompanyInfos.ForEach(CompanyInfos.Add);
            else
                Globals.OnCompanyInfos.Where(t => t.NameFirstLetter.ToLower().Contains(FilterLetter.ToLower())).ForEach(CompanyInfos.Add);
        }
    }
}