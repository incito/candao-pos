namespace CanDao.Pos.Model.Response
{
    public class CanDaoMemberQueryResponse : CanDaoMemberBaseResponse
    {
        public string birthday { get; set; }

        public string CardLevel { get; set; }

        public string RegDate { get; set; }

        public decimal? CouponsOverall { get; set; }

        public string Member_avatar { get; set; }

        public string MemberAddress { get; set; }

        /// <summary>
        /// 储值余额。
        /// </summary>
        public decimal StoreCardBalance { get; set; }

        public string TraceCode { get; set; }

        public string MCard { get; set; }

        public string TicketInfo { get; set; }

        public string name { get; set; }

        public int gender { get; set; }

        public decimal IntegralOverall { get; set; }

        public int? CardType { get; set; }

        public string mobile { get; set; }
    }
}