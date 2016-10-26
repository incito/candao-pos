namespace CanDao.Pos.Model
{
    /// <summary>
    /// 付款方式信息。
    /// </summary>
    public class PayWayInfo : BaseNotifyObject
    {
        /// <summary>
        /// 付款方式的ID。
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 付款方式名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否可见。
        /// </summary>
        private bool _isVisible;
        /// <summary>
        /// 是否可见。
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        /// <summary>
        /// 付款金额。
        /// </summary>
        private decimal _amount;
        /// <summary>
        /// 付款金额。
        /// </summary>
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaisePropertyChanged("Amount");
            }
        }

        /// <summary>
        /// 备注信息。即卡号等信息。
        /// </summary>
        private string _remark;
        /// <summary>
        /// 备注信息。即卡号等信息。
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set
            {
                _remark = value;
                RaisePropertyChanged("Remark");
            }
        }

        /// <summary>
        /// 克隆数据。
        /// </summary>
        /// <returns></returns>
        public PayWayInfo CloneObject()
        {
            return (PayWayInfo) MemberwiseClone();
        }
    }
}