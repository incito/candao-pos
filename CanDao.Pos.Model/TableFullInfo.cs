using System;
using System.Collections.ObjectModel;
using System.Linq;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 餐台信息全。
    /// </summary>
    public class TableFullInfo : TableInfo
    {
        public TableFullInfo()
        {
            DishInfos = new ObservableCollection<OrderDishInfo>();
            UsedCouponInfos = new ObservableCollection<UsedCouponInfo>();
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
        /// 已付金额总和。
        /// </summary>
        public decimal TotalAlreadyPayment { get; set; }

        /// <summary>
        /// 优免调整金额
        /// </summary>
        public decimal AdjustmentAmount { get; set; }

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
        private decimal _couponAmount;
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
        /// 总挂账金额。
        /// </summary>
        public decimal TotalDebitAmount { get; set; }
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
        /// 会员号。
        /// </summary>
        private string _memberNo;
        /// <summary>
        /// 会员号。
        /// </summary>
        public string MemberNo
        {
            get { return _memberNo; }
            set
            {
                _memberNo = value;
                RaisePropertyChanged("MemberNo");
            }
        }

        /// <summary>
        /// 订单发票抬头。如果为null则不开发票。
        /// </summary>
        private string _orderInvoiceTitle;
        /// <summary>
        /// 订单发票抬头。如果为null则不开发票。
        /// </summary>
        public string OrderInvoiceTitle
        {
            get { return _orderInvoiceTitle; }
            set
            {
                _orderInvoiceTitle = value;
                RaisePropertyChanged("OrderInvoiceTitle");
            }
        }

        /// <summary>
        /// 顾客人数。
        /// </summary>
        private int _customerNumber;
        /// <summary>
        /// 顾客人数。
        /// </summary>
        public int CustomerNumber
        {
            get { return _customerNumber; }
            set
            {
                _customerNumber = value;
                RaisePropertyChanged("CustomerNumber");
            }
        }

        /// <summary>
        /// 订单状态。
        /// </summary>
        public EnumOrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 是否已经结账。
        /// </summary>
        public bool HasBeenPaied
        {
            get
            {
                return OrderStatus == EnumOrderStatus.InternalSettle ||
                    OrderStatus == EnumOrderStatus.SingleTableSettle ||
                    OrderStatus == EnumOrderStatus.RelatedTableSettle;
            }
        }

        /// <summary>
        /// 订单明细。
        /// </summary>
        public ObservableCollection<OrderDishInfo> DishInfos { get; set; }

        /// <summary>
        /// 使用的优惠券集合。
        /// </summary>
        public ObservableCollection<UsedCouponInfo> UsedCouponInfos { get; private set; }

        /// <summary>
        /// 会员信息。
        /// </summary>
        private MemberInfo _memberInfo;

        /// <summary>
        /// 会员信息。
        /// </summary>
        public MemberInfo MemberInfo
        {
            get { return _memberInfo; }
            set
            {
                _memberInfo = value;
                RaisePropertyChanged("MemberInfo");
            }
        }

        public void CloneDataFromTableInfo(TableInfo tableInfo)
        {
            TableNo = tableInfo.TableNo;
            TableName = tableInfo.TableName;
            TableStatus = tableInfo.TableStatus;
            OrderId = tableInfo.OrderId;
            BeginTime = tableInfo.BeginTime;
            TableType = tableInfo.TableType;
            IsHangOrder = tableInfo.IsHangOrder;
        }

        public void CloneData(TableFullInfo info)
        {
            OrderId = info.OrderId;
            MemberInfo = info.MemberInfo;
            MemberNo = info.MemberNo;
            OrderInvoiceTitle = info.OrderInvoiceTitle;
            OrderStatus = info.OrderStatus;
            PaymentAmount = info.PaymentAmount;
            TipAmount = info.TipAmount;
            TotalAmount = info.TotalAmount;
            TableStatus = info.TableStatus;
            CustomerNumber = info.CustomerNumber;

            DishInfos.Clear();
            if (info.DishInfos != null && info.DishInfos.Any())
            {
                foreach (var dishInfo in info.DishInfos)
                {
                    DishInfos.Add(dishInfo);
                }
            }
        }

        public void CloneOrderData(TableFullInfo info)
        {
            OrderId = info.OrderId;
            MemberNo = info.MemberNo;
            TableStatus = info.TableStatus;
            CustomerNumber = info.CustomerNumber;
            OrderInvoiceTitle = info.OrderInvoiceTitle;
            OrderStatus = info.OrderStatus;

            PaymentAmount = info.PaymentAmount;
            TipAmount = info.TipAmount;
            TotalAmount = info.TotalAmount;
            TotalFreeAmount = info.TotalFreeAmount;
            CouponAmount = info.CouponAmount;
            TotalDebitAmount = info.TotalDebitAmount;
            RemovezeroAmount = info.RemovezeroAmount;
            RoundAmount = info.RoundAmount;
            AdjustmentAmount = info.AdjustmentAmount;
            ToalDebitAmountMany = info.ToalDebitAmountMany;

            DishInfos.Clear();
            if (info.DishInfos != null && info.DishInfos.Any())
            {
                foreach (var dishInfo in info.DishInfos)
                {
                    DishInfos.Add(dishInfo);
                }
            }
            UsedCouponInfos.Clear();
            if (info.UsedCouponInfos != null && info.UsedCouponInfos.Any())
            {
                foreach (var couponInfo in info.UsedCouponInfos)
                {
                    UsedCouponInfos.Add(couponInfo);
                }
            }
        }

        public void ClonePreferentialInfo(preferentialInfoResponse preferential)
        {
            TotalFreeAmount = preferential.toalFreeAmount;
            CouponAmount = preferential.amount;
            PaymentAmount = preferential.payamount;
            TipAmount = preferential.tipAmount;
            TotalAmount = preferential.menuAmount;
            TotalDebitAmount = preferential.toalDebitAmount;
            AdjustmentAmount = preferential.adjAmout;
            ToalDebitAmountMany = preferential.toalDebitAmountMany;
            switch (preferential.moneyDisType)
            {
                case 0: //未设置
                    {
                        RoundAmount = 0;
                        RemovezeroAmount = 0;
                        break;
                    }
                case 1: //四舍五入
                    {
                        RoundAmount = preferential.moneyWipeAmount;
                        RemovezeroAmount = 0;
                        break;
                    }
                case 2:
                    {
                        RoundAmount = 0;
                        RemovezeroAmount = preferential.moneyWipeAmount;
                        break;
                    }
            }



            if (preferential.detailPreferentials == null)
            { return; }

            foreach (var info in preferential.detailPreferentials)
            {
                var coupon = new UsedCouponInfo();
                coupon.CouponInfo = new CouponInfo();
                coupon.CouponInfo.RuleId = info.coupondetailid;
                coupon.CouponInfo.CouponId = info.preferential;


                coupon.Count = 1;//默认单张
                coupon.RelationId = info.id;
                coupon.BillAmount = info.deAmount;
                coupon.DebitAmount = info.toalDebitAmount;
                coupon.FreeAmount = info.toalFreeAmount;
                coupon.Name = info.activity.name;
                UsedCouponInfos.Add(coupon);
            }
        }
    }
}