namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取会员消费信息返回类。
    /// </summary>
    public class GetMemberPayInfoResponse : RestOrderResponse<MemberPayInfoDataResponse>
    {

    }

    public class MemberPayInfoDataResponse
    {
        public string orderid { get; set; }
        public string userid { get; set; }
        public string ordertime { get; set; }
        public string business { get; set; }
        public string terminal { get; set; }
        public string serial { get; set; }
        public string batchno { get; set; }
        public string businessname { get; set; }
        public int operatetype { get; set; }
        public decimal score { get; set; }
        public decimal coupons { get; set; }
        public decimal stored { get; set; }
        public decimal scorebalance { get; set; }
        public decimal couponsbalance { get; set; }
        public decimal storedbalance { get; set; }
        public string cardno { get; set; }
    }
}