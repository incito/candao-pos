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
        /// 剩余未付金额。
        /// </summary>
        public decimal RemainderAmount { get; set; }

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
            set
            {
                _hasServiceCharge = value;
                RaisePropertyChanged("HasServiceCharge");
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
        /// 复制优惠券信息。
        /// </summary>
        /// <param name="preferential"></param>
        public void ClonePreferentialInfo(PreferentialInfoResponse preferential)
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

            if (preferential.detailPreferentials != null)
                preferential.detailPreferentials.ForEach(t => UsedCouponInfos.Add(Convert2UsedCouponInfo(t)));
        }

        /// <summary>
        /// 将优惠券明细信息转换成优惠券使用信息。
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private UsedCouponInfo Convert2UsedCouponInfo(GetPreferentialDetails info)
        {
            return new UsedCouponInfo
            {
                CouponInfo = new CouponInfo
                {
                    RuleId = info.coupondetailid,
                    CouponId = info.preferential
                },
                Count = 1,//默认单张
                RelationId = info.id,
                UsedCouponType = (EnumUsedCouponType)info.isCustom,
                BillAmount = info.deAmount,
                DebitAmount = info.toalDebitAmount,
                FreeAmount = info.toalFreeAmount,
                Name = info.activity.name
            };
        }
    }
}