using System;
using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// 品项统计信息。
    /// </summary>
    public class DishSaleFullInfo : StatisticInfoBase
    {
        /// <summary>
        /// 品项统计信息集合。
        /// </summary>
        public List<DishSaleInfo> DishSaleInfos { get; set; }

    }
}