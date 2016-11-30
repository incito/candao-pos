using System;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 餐台各项金额的数据。
    /// </summary>
    public class TableAmountPartInfo : TableInfo
    {
        /// <summary>
        /// 已付金额总和。
        /// </summary>
        public decimal TotalAlreadyPayment { get; set; }

        /// <summary>
        /// 剩余未付金额。
        /// </summary>
        public decimal RemainderAmount { get; set; }

        /// <summary>
        /// 优免总金额。
        /// </summary>
        private decimal _totalFreeAmount;
        /// <summary>
        /// 优免总金额。
        /// </summary>
        public decimal TotalFreeAmount
        {
            get { return _totalFreeAmount; }
            set
            {
                _totalFreeAmount = Math.Round(value, 2);
                RaisePropertyChanged("TotalFreeAmount");
            }
        }

        /// <summary>
        /// 优惠总额。
        /// </summary>
        private decimal _couponAmount;
        /// <summary>
        /// 优惠总额。
        /// </summary>
        public decimal CouponAmount
        {
            get { return _couponAmount; }
            set
            {
                _couponAmount = Math.Round(value, 2);
                RaisePropertyChanged("CouponAmount");
            }
        }

        /// <summary>
        /// 应付金额。
        /// </summary>
        private decimal _paymentAmount;
        /// <summary>
        /// 应付金额。
        /// </summary>
        public decimal PaymentAmount
        {
            get { return _paymentAmount; }
            set
            {
                _paymentAmount = value;
                RaisePropertyChanged("PaymentAmount");
            }
        }

        /// <summary>
        /// 小费金额。
        /// </summary>
        private decimal _tipAmount;
        /// <summary>
        /// 小费金额。
        /// </summary>
        public decimal TipAmount
        {
            get { return _tipAmount; }
            set
            {
                _tipAmount = value;
                RaisePropertyChanged("TipAmount");
            }
        }

        /// <summary>
        /// 总额。
        /// </summary>
        private decimal _totalAmount;
        /// <summary>
        /// 总额。
        /// </summary>
        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                RaisePropertyChanged("TotalAmount");
            }
        }

        /// <summary>
        /// 总挂账金额。
        /// </summary>
        public decimal TotalDebitAmount { get; set; }

        /// <summary>
        /// 优免调整金额
        /// </summary>
        public decimal AdjustmentAmount { get; set; }

        /// <summary>
        /// 总挂账多收
        /// </summary>
        public decimal ToalDebitAmountMany { set; get; }

        /// <summary>
        /// 四舍五入金额
        /// </summary>
        public decimal RoundAmount { get; set; }

        /// <summary>
        /// 抹零金额
        /// </summary>
        public decimal RemovezeroAmount { get; set; }

    }
}