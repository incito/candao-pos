namespace CanDao.Pos.Model
{
    /// <summary>
    /// 餐台服务费部分信息。
    /// </summary>
    public class TableServiceChargePartInfo : TableAmountPartInfo
    {
        /// <summary>
        /// 服务费信息。
        /// </summary>
        private ServiceChargeInfo _serviceChargeInfo;
        /// <summary>
        /// 获取或设置服务费信息。
        /// </summary>
        public ServiceChargeInfo ServiceChargeInfo
        {
            get { return _serviceChargeInfo; }
            set
            {
                _serviceChargeInfo = value;
                RaisePropertyChanged("ServiceChargeInfo");

                HasServiceCharge = value != null;
            }
        }

        /// <summary>
        /// 是否有服务费。
        /// </summary>
        private bool _hasServiceCharge;
        /// <summary>
        /// 获取或设置是否有服务费。
        /// </summary>
        public bool HasServiceCharge
        {
            get { return _hasServiceCharge; }
            private set
            {
                _hasServiceCharge = value;
                RaisePropertyChanged("HasServiceCharge");
            }
        }

    }
}