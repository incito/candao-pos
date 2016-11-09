namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 会员取消反结算参数。
    /// </summary>
    public class CanDaoMemberUnVoidSaleRequest
    {
        /// <summary>
        /// 手机号或卡号。
        /// </summary>
        public string cardno { get; set; }

        /// <summary>
        /// 反结算交易号。
        /// </summary>
        public string deal_no { get; set; }

        /// <summary>
        /// 会员系统交易号。
        /// </summary>
        public string tracecode { get; set; }
    }
}