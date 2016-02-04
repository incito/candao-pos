using Models.Enum;

namespace Models
{
    /// <summary>
    /// 餐台信息。
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        /// 餐台编号。
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// 餐桌状态。
        /// </summary>
        public EnumTableStatus TableStatus { get; set; }

        /// <summary>
        /// 餐台类型。
        /// </summary>
        public EnumTableType TableType { get; set; }

        /// <summary>
        /// 餐桌可以就坐人数。
        /// </summary>
        public int PeopleNumber { get; set; }

        /// <summary>
        /// 餐桌名称。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 餐桌编号。（现餐桌名称与编号同）
        /// </summary>
        public string TableNo { get; set; }

        /// <summary>
        /// 餐桌所属区域名。
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 餐桌所属区域编号。
        /// </summary>
        public string AreaNo { get; set; }

        /// <summary>
        /// 该餐桌订单号。
        /// </summary>
        public string OrderId { get; set; }

        public decimal MinPrice { get; set; }

        public decimal FixPrice { get; set; }
    }
}