using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;

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
            InitPayWayInfos();
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
                _selectedPayWay = value;
                RaisePropertyChanged("SelectedPayWay");
            }
        }

        /// <summary>
        /// 操作命令。
        /// </summary>
        public ICommand OperCmd { get; private set; }

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
                    SelectedPayWay = item;
                    break;
                case "Down":
                    if (index >= PayWays.Count - 1)
                        return;
                    PayWays.Remove(item);
                    PayWays.Insert(index + 1, item);
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
        /// 初始化命令。
        /// </summary>
        private void InitCommand()
        {
            OperCmd = CreateDelegateCommand(OperMethod, CanOperMethod);
        }

        /// <summary>
        /// 初始化支付方式集合。
        /// </summary>
        private void InitPayWayInfos()
        {
            PayWays.Clear();
            if (Globals.PayWayInfos != null)
                Globals.PayWayInfos.ForEach(t => PayWays.Add(t.CloneObject()));
        }

        /// <summary>
        /// 异步保存结算方式设置。
        /// </summary>
        private void SavePayWaySettingAsync()
        {
            TaskService.Start(null, SavePayWaySettingProcess, SavePayWaySettingComplete, "保存结算方式设置中...");
        }

        private object SavePayWaySettingProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return "创建IRestaurantService服务失败。";

            return service.SavePayWayInfo(PayWays.ToList());
        }

        private void SavePayWaySettingComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result);
                InitPayWayInfos();
                return;
            }

            NotifyDialog.Notify("修改结算方式成功。");
            Globals.PayWayInfos = PayWays.ToList();
        }
    }
}