namespace CanDao.Pos.Model.Response
{
    public class GetOrderMemberInfoResponse : CanDaoMemberBaseResponse
    {
        public new bool IsSuccess
        {
            get { return Retcode == 1; }
        }

        /// <summary>
        /// 消费时交易号。
        /// </summary>
        public string serial { get; set; }

        /// <summary>
        /// 会员卡号。
        /// </summary>
        public string cardno { get; set; }
    }
}