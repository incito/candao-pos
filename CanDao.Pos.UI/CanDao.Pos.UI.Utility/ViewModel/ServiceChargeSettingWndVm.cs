using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 服务费设置窗口的VM。
    /// </summary>
    public class ServiceChargeSettingWndVm : NormalWindowViewModel<ServiceChargeSettingWindow>
    {
        /// <summary>
        /// 订单号。
        /// </summary>
        private readonly string _orderId;

        public ServiceChargeSettingWndVm(ServiceChargeInfo data)
        {
            IsFree = true;//默认免费，当收费时会设置成false。
            _orderId = data.OrderId;
            IsCharge = data.IsChargeOn;
            SrcChargeAmount = data.ServiceAmount;
            CusChargeAmount = SrcChargeAmount;
        }

        /// <summary>
        /// 是否收费。
        /// </summary>
        private bool _isCharge;
        /// <summary>
        /// 获取或设置是否收费。
        /// </summary>
        public bool IsCharge
        {
            get { return _isCharge; }
            set
            {
                if (_isCharge == value)
                    return;

                _isCharge = value;
                RaisePropertyChanged("IsCharge");

                IsFree = !value;
            }
        }

        /// <summary>
        /// 是否免费。
        /// </summary>
        private bool _isFree;
        /// <summary>
        /// 获取或设置是否免费。
        /// </summary>
        public bool IsFree
        {
            get { return _isFree; }
            set
            {
                if (_isFree == value)
                    return;

                _isFree = value;
                RaisePropertyChanged("IsFree");

                IsCharge = !value;
                if (value)
                    CusChargeAmount = SrcChargeAmount;
            }
        }

        /// <summary>
        /// 当前服务费。
        /// </summary>
        private decimal _srcChargeAmount;
        /// <summary>
        /// 获取或设置当前服务费。
        /// </summary>
        public decimal SrcChargeAmount
        {
            get { return _srcChargeAmount; }
            set
            {
                _srcChargeAmount = value;
                RaisePropertyChanged("SrcChargeAmount");
            }
        }

        /// <summary>
        /// 实收服务费。
        /// </summary>
        private decimal _cusChargeAmount;
        /// <summary>
        /// 获取或设置实收服务费。
        /// </summary>
        public decimal CusChargeAmount
        {
            get { return _cusChargeAmount; }
            set
            {
                _cusChargeAmount = value;
                RaisePropertyChanged("CusChargeAmount");
            }
        }

        protected override void Confirm(object param)
        {
            TaskService.Start(null, SaveServiceChargeProcess, SaveServiceChargeComplete, "保存服务费中...");
        }

        /// <summary>
        /// 保存优惠券信息执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object SaveServiceChargeProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var isCustomChange = CusChargeAmount != SrcChargeAmount;//根据服务费是否改变来判断是否是服务员设置了服务费。
            return service.SaveServiceCharge(_orderId, Globals.Authorizer.UserName, IsCharge, isCustomChange, CusChargeAmount);
        }

        /// <summary>
        /// 保存优惠券信息执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void SaveServiceChargeComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                var errMsg = string.Format("保存服务费失败：{0}", result);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(result);
                return;
            }

            NotifyDialog.Notify("保存服务费成功。");
            CloseWindow(true);
        }
    }
}