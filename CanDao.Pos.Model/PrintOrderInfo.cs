using System;

namespace CanDao.Pos.Model
{
    public class PrintOrderInfo
    {
        /// <summary>
        /// 分店Id。
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// 分店名称。
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// 电话。
        /// </summary>
        public string BranchTelephone { get; set; }

        /// <summary>
        /// 地址。
        /// </summary>
        public string BranchAddress { get; set; }

        /// <summary>
        /// 订单号。
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 区域名称。
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 餐台名称。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 顾客人数。
        /// </summary>
        public int CustomerNumber { get; set; }

        /// <summary>
        /// 服务员名称。
        /// </summary>
        public string WaiterName { get; set; }

        /// <summary>
        /// 开台时间。
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结账时间。
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 打印预结单次数。
        /// </summary>
        public int PrintPresettTimes { get; set; }

        /// <summary>
        /// 打印结账单次数。
        /// </summary>
        public int PrintSettlementTimes { get; set; }

        /// <summary>
        /// 优惠价格。
        /// </summary>
        public decimal DiscountPrice { get; set; }

        /// <summary>
        /// 赠送金额。
        /// </summary>
        public decimal FreeAmount { get; set; }

        /// <summary>
        /// 应收金额。
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 品项费。
        /// </summary>
        public decimal TotalDishAmount { get; set; }

        /// <summary>
        /// 抹零金额。
        /// </summary>
        public decimal RemoveOddAmount { get; set; }

        /// <summary>
        /// 四舍五入金额。
        /// </summary>
        public decimal RoundingAmount { get; set; }

        /// <summary>
        /// 实付金额。
        /// </summary>
        public decimal PaidAmount { get; set; }

        /// <summary>
        /// 打印人。
        /// </summary>
        public string Printer { get; set; }
    }
}