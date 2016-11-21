using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 现金付款方式。
    /// </summary>
    public class PayWayCashInfo : PayWayInfo
    {
        public PayWayCashInfo()
        {
            IsCashPayWay = true;
        }

        /// <summary>
        /// 找零金额。
        /// </summary>
        private decimal _chargeAmount;
        /// <summary>
        /// 找零金额。
        /// </summary>
        public decimal ChargeAmount
        {
            get { return _chargeAmount; }
            set
            {
                _chargeAmount = value;
                RaisePropertyChanged("ChargeAmount");
            }
        }

        /// <summary>
        /// 小费支付金额。
        /// </summary>
        public decimal TipPaymentAmount { get; set; }

        public override List<string> GenerateSettlementInfo()
        {
            var info = new List<string>();
            if (Amount > 0)
                info.Add(string.Format("现金：{0:f2}", Amount));
            if (ChargeAmount > 0)
                info.Add(string.Format("找零：{0:f2}", ChargeAmount));
            if (TipPaymentAmount > 0)
                info.Add(string.Format("小费：{0:f2}", TipPaymentAmount));
            return info;
        }

        public override List<BillPayInfo> GenerateBillPayInfo()
        {
            var list = new List<BillPayInfo>();
            var realCashAmount = Amount - ChargeAmount - TipPaymentAmount;//真实的现金消费金额为输入的现金-找零的现金-小费金额
            list.Add(new BillPayInfo(realCashAmount, ItemId));
            return list;
        }

        public override string CheckTheBillAllowPay()
        {
            return ChargeAmount > 100 ? "找零金额不能大于100。" : null;
        }
    }
}