using System;
using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// 品项统计信息。
    /// </summary>
    public class DishSaleFullInfo
    {
        /// <summary>
        /// 品项统计信息集合。
        /// </summary>
        public List<DishSaleInfo> DishSaleInfos { get; set; }

        /// <summary>
        /// 统计起始时间。
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 统计结束时间。
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 分店ID。
        /// </summary>
        public string BranchId { get; set; }

        /// <summary>
        /// 总金额。
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 当前时间，打印时间。
        /// </summary>
        public DateTime CurrentTime { get; set; }

    }
}