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
    /// 支付方式设置控件VM。
    /// </summary>
    public class PayWaySetControlVm : BaseViewModel
    {
        public PayWaySetControlVm()
        {
            InitCommand();

            PayWays = new ObservableCollection<PayWayInfo>();
            TaskService.Start(null, GetPayWayInfoProcess, GetPayWayInfoComplete, null);
        }

        /// <summary>
        /// 结算方式集合。
        /// </summary>
        public ObservableCollection<PayWayInfo> PayWays { get; set; }

        /// <summary>
        /// 选择的结算方式。
        /// </summary>
        private PayWayInfo _selectedPayWay;
        /// <summary>
        /// 选择的结算方式。
        /// </summary>
        public PayWayInfo SelectedPayWay
        {
            get { return _selectedPayWay; }
            set
            {
                if (_selectedPayWay != null)
                    _selectedPayWay.IsSelected = false;

                _selectedPayWay = value;
                if (_selectedPayWay != null)
                    _selectedPayWay.IsSelected = true;
                RaisePropertyChanged("SelectedPayWay");
            }
        }

        public PayWaySetControl SetControl { get; set; }

        /// <summary>
        /// 操作命令。
        /// </summary>
        public ICommand OperCmd { get; private set; }

        /// <summary>
        /// 分组命令。
        /// </summary>
        public ICommand GroupCmd { get; set; }

        /// <summary>
        /// 操作命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void OperMethod(object arg)
        {
            if (SelectedPayWay == null)
                return;

            var item = SelectedPayWay;
            var index = PayWays.IndexOf(SelectedPayWay);
            switch (arg as string)
            {
                case "Enabled":
                    if (!MessageDialog.Quest("确认启用后，在结算方式区域里显示出该结算方式！"))
                        return;
                    item.IsVisible = true;
                    break;
                case "Disabled":
                    if (!MessageDialog.Quest("确认禁用后，在结算方式区域里不再显示该结算方式！"))
                        return;
                    item.IsVisible = false;
                    break;
                case "Up":
                    if (index <= 0)
                        return;
                    PayWays.Remove(item);
                    PayWays.Insert(index - 1, item);

                    SetControl.PayWaySelector.SelectedGroupIndex = PayWays.IndexOf(item) / SetControl.PayWaySelector.ItemCountEachGroup;
                    SelectedPayWay = item;
                    break;
                case "Down":
                    if (index >= PayWays.Count - 1)
                        return;
                    PayWays.Remove(item);
                    PayWays.Insert(index + 1, item);

                    SetControl.PayWaySelector.SelectedGroupIndex = PayWays.IndexOf(item) / SetControl.PayWaySelector.ItemCountEachGroup;
                    SelectedPayWay = item;
                    break;
                default:
                    return;
            }
            SavePayWaySettingAsync();
        }

        /// <summary>
        /// 操作命令是否可用的判断方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanOperMethod(object param)
        {
            if (SelectedPayWay == null)
                return true;

            switch (param as string)
            {
                default:
                    return true;
            }
        }

        /// <summary>
        /// 分组命令的 执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void GroupMethod(object arg)
        {
            switch (arg as string)
            {
                case "PreGroup":
                    if (SetControl != null)
                        SetControl.PayWaySelector.PreviousGroup();
                    break;
                case "NextGroup":
                    if (SetControl != null)
                        SetControl.PayWaySelector.NextGroup();
                    break;
            }
        }

        /// <summary>
        /// 分组命令是否可用的判断你方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanGroupMethod(object arg)
        {
            switch (arg as string)
            {
                case "PreGroup":
                    return SetControl.PayWaySelector.CanPreviousGroup;
                case "NextGroup":
                    return SetControl.PayWaySelector.CanNextGruop;
            }
            return true;
        }

        /// <summary>
        /// 初始化命令。
        /// </summary>
        private void InitCommand()
        {
            OperCmd = CreateDelegateCommand(OperMethod, CanOperMethod);
            GroupCmd = CreateDelegateCommand(GroupMethod, CanGroupMethod);
        }

        /// <summary>
        /// 异步保存结算方式设置。
        /// </summary>
        private void SavePayWaySettingAsync()
        {
            TaskService.Start(null, SavePayWaySettingProcess, SavePayWaySettingComplete, "保存结算方式设置中...");
        }

        /// <summary>
        /// 保存结算设置执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object SavePayWaySettingProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return "创建IRestaurantService服务失败。";

            var temp = PayWays.ToList();
            return service.SavePayWayInfo(temp);
        }

        /// <summary>
        /// 保存结算设置执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void SavePayWaySettingComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result);
                return;
            }

            NotifyDialog.Notify("修改结算方式成功。");
        }


        /// <summary>
        /// 获取结算方式执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object GetPayWayInfoProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, List<PayWayInfo>>("创建IRestaurantService服务失败。", null);

            return service.GetPayWayInfo();
        }

        /// <summary>
        /// 获取结算方式完成时执行。
        /// </summary>
        /// <param name="obj"></param>
        private void GetPayWayInfoComplete(object obj)
        {
            var result = (Tuple<string, List<PayWayInfo>>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var errMsg = string.Format("获取结算方式失败：{0}", result.Item1);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg);
            }

            if (result.Item2 != null)
            {
                result.Item2.ForEach(PayWays.Add);
                SelectedPayWay = PayWays.FirstOrDefault();
            }
        }

    }
}