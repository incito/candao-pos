using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 雅座会员储值窗口的VM。
    /// </summary>
    public class MemberYaZuoStoredWndVm : NormalWindowViewModel
    {
        #region Properties

        /// <summary>
        /// 会员号。
        /// </summary>
        public string MemberNo { get; set; }

        /// <summary>
        /// 储值余额。
        /// </summary>
        private decimal _storedValue;
        /// <summary>
        /// 储值余额。
        /// </summary>
        public decimal StoredValue
        {
            get { return _storedValue; }
            set
            {
                _storedValue = value;
                RaisePropertyChanged("StoredValue");
            }
        }

        /// <summary>
        /// 充值付款类型。
        /// </summary>
        private EnumStoragePayType _storagePayType;
        /// <summary>
        /// 充值付款类型。
        /// </summary>
        public EnumStoragePayType StoragePayType
        {
            get { return _storagePayType; }
            set
            {
                _storagePayType = value;
                RaisePropertyChanged("StoragePayType");
            }
        }

        #endregion

        #region Protected Methods

        protected override void OperMethod(object param)
        {
            switch (param as string)
            {
                case "Cash":
                    StoragePayType = EnumStoragePayType.Cash;
                    break;
                case "Bank":
                    StoragePayType = EnumStoragePayType.Bank;
                    break;
            }
        }

        protected override void Confirm(object param)
        {
            if (MessageDialog.Quest(string.Format("确定给会员号：\"{0}\"储值：\"{1}\"吗？", MemberNo, StoredValue)))
                TaskService.Start(null, StorageProcess, StorageComplete, "会员储值处理中...");
        }

        protected override bool CanConfirm(object param)
        {
            return !string.IsNullOrEmpty(MemberNo) && StoredValue > 0;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 会员储值执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object StorageProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            return service.StorageYaZuo(MemberNo, StoredValue, StoragePayType);
        }

        /// <summary>
        /// 会员储值执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void StorageComplete(object param)
        {
            var result = (Tuple<string, YaZuoStorageInfo>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                var msg = string.Format("会员储值失败：{0}", result.Item1);
                ErrLog.Instance.E(msg);
                MessageDialog.Warning(msg, OwnerWindow);
                return;
            }

            CloseWindow(true);
            NotifyDialog.Notify(string.Format("会员储值成功。{0}交易流水号：{1}", Environment.NewLine, result.Item2.TradeCode), OwnerWindow.Owner);
        }

        #endregion
    }
}