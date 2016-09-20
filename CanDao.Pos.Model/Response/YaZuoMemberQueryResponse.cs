namespace CanDao.Pos.Model.Response
{
    public class YaZuoMemberQueryResponse : YaZuoMemberBaseResponse
    {
        public string psCardType { get; set; }

        public string psCouponsAvail { get; set; }

        public string psCouponsOverall { get; set; }

        public decimal psIntegralAvail { get; set; }

        public decimal psIntegralOverall { get; set; }

        /// <summary>
        /// 卡余额。（除以100以后才是真实的金额）
        /// </summary>
        public decimal psStoredCardsBalance { get; set; }

        public string psTicketInfo { get; set; }

        public string pszAddress { get; set; }

        public string pszBirthday { get; set; }

        public string pszEmail { get; set; }

        public string pszGender { get; set; }

        public string pszJoindate { get; set; }

        public string pszMobile { get; set; }

        public string pszName { get; set; }

        public string pszPan { get; set; }

        public string pszRestInfo { get; set; }

        public string pszTrack2 { get; set; }

        public string pszdescription { get; set; }

    }
}