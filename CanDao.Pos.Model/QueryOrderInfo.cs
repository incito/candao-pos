using System;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 账单查询里的账单信息。
    /// </summary>
    public class QueryOrderInfo
    {
        /// <summary>
        /// 餐台编号。
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// 账单号。
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 区域。
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 餐台名。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 服务员。
        /// </summary>
        public string UserId { get; set; }
        
        /// <summary>
        /// 开台时间。
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结账时间。
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 顾客人数。
        /// </summary>
        public int? CustomerNum { get; set; }

        /// <summary>
        /// 应收金额。
        /// </summary>
        public decimal Dueamount { get; set; }

        /// <summary>
        /// 挂账单位。
        /// </summary>
        public string DebitCompany { get; set; }

        /// <summary>
        /// 挂账电话。
        /// </summary>
        public string DebitTelphone { get; set; }

        /// <summary>
        /// 挂账联系人。
        /// </summary>
        public string DebitPeople { get; set; }

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
    }
}