using System;
using System.Collections.Generic;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Event;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 付款方式信息。
    /// </summary>
    public class PayWayInfo : BaseNotifyObject
    {
        /// <summary>
        /// 是否是现金结算方式。
        /// </summary>
        protected bool IsCashPayWay = false;

        /// <summary>
        /// 金额变化时触发。
        /// </summary>
        public EventHandler<AmountChangedEventArgs> AmountChanged;

        /// <summary>
        /// 触发金额变化事件。
        /// </summary>
        /// <param name="isCashAmountChanged">是否是现金金额改变。</param>
        protected void OnAmountChanged(bool isCashAmountChanged = false)
        {
            if (AmountChanged != null)
                AmountChanged(this, new AmountChangedEventArgs(isCashAmountChanged));
        }

        /// <summary>
        /// 付款方式的ID。
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 结算方式枚举。
        /// </summary>
        public EnumPayWayType PayWayType { get; set; }

        /// <summary>
        /// 付款方式名称。
        /// </summary>
        private string _name;
        /// <summary>
        /// 付款方式名称。
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

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
                var temp = _amount;
                _amount = value;
                if (temp != _amount)
                {
                    RaisePropertyChanged("Amount");
                    OnAmountChanged(IsCashPayWay);
                }
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
            return (PayWayInfo)MemberwiseClone();
        }

        public virtual void CloneData(PayWayInfo info)
        {
            ItemId = info.ItemId;
            Name = info.Name;
            IsVisible = info.IsVisible;
            PayWayType = info.PayWayType;
        }

        /// <summary>
        /// 生成结算信息。
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GenerateSettlementInfo()
        {
            var list = new List<string>();
            if (Amount > 0)
                list.Add(string.Format("{0}：{1:f2}", Name, Amount));
            return list;
        }

        /// <summary>
        /// 生成结算信息。
        /// </summary>
        /// <returns></returns>
        public virtual List<BillPayInfo> GenerateBillPayInfo()
        {
            return Amount > 0 ? new List<BillPayInfo> { new BillPayInfo(Amount, ItemId, Remark) } : null;
        }

        public virtual string CheckTheBillAllowPay()
        {
            return null;
        }
    }
}