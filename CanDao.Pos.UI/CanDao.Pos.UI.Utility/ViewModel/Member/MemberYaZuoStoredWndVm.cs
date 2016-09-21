using CanDao.Pos.Common;
using CanDao.Pos.IService;
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

        protected override bool CanConfirm(object param)
        {
            return string.IsNullOrEmpty(MemberNo) && StoredValue > 0;
        }

        #endregion

        #region Private Methods

        private object StorageProcess(object param)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return "创建IRestaurantService服务失败。";

            return null;
        }

        private void StorageComplete(object param)
        {

        } 

        #endregion
    }
}