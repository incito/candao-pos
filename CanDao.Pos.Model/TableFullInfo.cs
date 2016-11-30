using System.Collections.ObjectModel;
using System.Linq;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 餐台信息全。
    /// </summary>
    public class TableFullInfo : TableServiceChargePartInfo
    {
        public TableFullInfo()
        {
            DishInfos = new ObservableCollection<OrderDishInfo>();
            UsedCouponInfos = new ObservableCollection<UsedCouponInfo>();
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
        /// 餐具是否免费。
        /// </summary>
        public bool IsDinnerWareFree { get; set; }

        /// <summary>
        /// 餐具个数。
        /// </summary>
        public int DinnerWareCount { get; set; }

        /// <summary>
        /// 订单状态。
        /// </summary>
        public EnumOrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 是否已经结账。
        /// </summary>
        public bool HasBeenPaied
        {
            get { return OrderStatus == EnumOrderStatus.InternalSettle; }
        }

        /// <summary>
        /// 订单明细。
        /// </summary>
        public ObservableCollection<OrderDishInfo> DishInfos { get; set; }


        /// <summary>
        /// 使用的优惠券集合。
        /// </summary>
        public ObservableCollection<UsedCouponInfo> UsedCouponInfos { get; protected set; }

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

        /// <summary>
        /// 复制简单信息。
        /// </summary>
        /// <param name="info"></param>
        public void CloneSimpleData(TableFullInfo info)
        {
            OrderId = info.OrderId;
            MemberNo = info.MemberNo;
            OrderInvoiceTitle = info.OrderInvoiceTitle;
            OrderStatus = info.OrderStatus;
            PaymentAmount = info.PaymentAmount;
            TipAmount = info.TipAmount;
            TotalAmount = info.TotalAmount;
            TotalFreeAmount = info.TotalFreeAmount;
            TableStatus = info.TableStatus;
            CustomerNumber = info.CustomerNumber;
            ServiceChargeInfo = info.ServiceChargeInfo;
        }

        /// <summary>
        /// 复制订单信息。
        /// </summary>
        /// <param name="info"></param>
        public void CloneOrderData(TableFullInfo info)
        {
            OrderId = info.OrderId;
            MemberNo = info.MemberNo;
            TableStatus = info.TableStatus;
            CustomerNumber = info.CustomerNumber;
            IsDinnerWareFree = info.IsDinnerWareFree;
            DinnerWareCount = info.DinnerWareCount;
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
            ServiceChargeInfo = info.ServiceChargeInfo;

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

        /// <summary>
        /// 复制数据。
        /// </summary>
        /// <param name="info"></param>
        public void CloneFromTableServiceChargePart(TableAfterUseCouponInfo info)
        {
            TotalFreeAmount = info.TotalFreeAmount;
            CouponAmount = info.CouponAmount;
            PaymentAmount = info.PaymentAmount;
            TipAmount = info.TipAmount;
            TotalAmount = info.TotalAmount;
            TotalDebitAmount = info.TotalDebitAmount;
            AdjustmentAmount = info.AdjustmentAmount;
            ToalDebitAmountMany = info.ToalDebitAmountMany;
            RoundAmount = info.RoundAmount;
            RemovezeroAmount = info.RemovezeroAmount;
            ServiceChargeInfo = info.ServiceChargeInfo;

            if (info.AddedCouponInfos != null)
                info.AddedCouponInfos.ForEach(UsedCouponInfos.Add);
        }
    }
}