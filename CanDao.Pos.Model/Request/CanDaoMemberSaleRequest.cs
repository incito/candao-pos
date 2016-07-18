namespace CanDao.Pos.Model.Request
{
    public class CanDaoMemberSaleRequest : CanDaoMemberQueryRequest
    {
        public string Serial { get; set; }

        /// <summary>
        /// 现金消费金额。
        /// </summary>
        public decimal FCash { get; set; }

        /// <summary>
        /// 积分消费金额。
        /// </summary>
        public decimal FIntegral { get; set; }

        /// <summary>
        /// 储值消费金额。
        /// </summary>
        public decimal FStore { get; set; }

        public string FTicketList { get; set; }
    }
}