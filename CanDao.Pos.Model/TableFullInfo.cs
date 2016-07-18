using System;
using System.Collections.ObjectModel;
using System.Linq;
using CanDao.Pos.Model.Enum;

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
        /// 调整的金额。（如果抹零则是零头金额，如果四舍五入，舍去的或者舍入的金额。当舍入是为正数，否则会负数。）
        /// </summary>
        public decimal AdjustmentAmount { get; set; }

        /// <summary>
        /// 优免总金额。
        /// </summary>
        public decimal TotalFreeAmount { get; set; }

        /// <summary>
        /// 总挂账金额。
        /// </summary>
        public decimal TotalDebitAmount { get; set; }

        /// <summary>
        /// 小费金额。
        /// </summary>
        public decimal TipAmount { get; set; }

        /// <summary>
        /// 会员号。
        /// </summary>
        public string MemberNo { get; set; }

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
        public int CustomerNumber { get; set; }

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
        }
    }
}