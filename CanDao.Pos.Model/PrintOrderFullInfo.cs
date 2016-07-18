using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 餐台打印信息全。
    /// </summary>
    public class PrintOrderFullInfo
    {
        public PrintOrderFullInfo()
        {
            SettlementDetails = new Dictionary<string, decimal>();
        }

        /// <summary>
        /// 订单信息。
        /// </summary>
        public PrintOrderInfo OrderInfo { get; set; }

        /// <summary>
        /// 订单菜品集合。
        /// </summary>
        public List<PrintOrderDishInfo> OrderDishInfos { get; set; }

        /// <summary>
        /// 订单结算明细。
        /// </summary>
        public List<PrintPayDetail> PayDetails { get; set; }

        /// <summary>
        /// 结算备注明细集合。
        /// </summary>
        public Dictionary<string, decimal> SettlementDetails { get; private set; }
    }
}