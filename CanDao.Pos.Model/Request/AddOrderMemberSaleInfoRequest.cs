namespace CanDao.Pos.Model.Request
{
    public class AddOrderMemberSaleInfoRequest
    {
        public AddOrderMemberSaleInfoRequest()
        {
            couponsbalance = "0";
            psexpansivity = 0;
            inflated = 0;
            coupons = 0;
        }

        public string orderid { get; set; }

        public string cardno { get; set; }

        public string userid { get; set; }

        public string business { get; set; }

        public string terminal { get; set; }

        public string serial { get; set; }

        public string businessname { get; set; }

        public decimal score { get; set; }

        public decimal coupons { get; set; }

        public decimal stored { get; set; }

        public decimal scorebalance { get; set; }

        public string couponsbalance { get; set; }

        public decimal storedbalance { get; set; }

        public decimal psexpansivity { get; set; }

        public decimal netvalue { get; set; }

        public decimal inflated { get; set; }
    }
}