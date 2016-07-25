namespace CanDao.Pos.Model.Request
{
    public class CanDaoMemberStorageRequest
    {
        public CanDaoMemberStorageRequest()
        {
            TransType = "0";
            SecurityCode = "";
        }

        public string SecurityCode { get; set; }

        public string Serial { get; set; }

        public string branch_id { get; set; }

        public string cardno { get; set; }

        public decimal Amount { get; set; }

        public string TransType { get; set; }

        /// <summary>
        /// 支付方式。
        /// </summary>
        public string ChargeType { get; set; }
        public string preferential_id { get; set; }
        public string giveValue { get; set; }
    }
}