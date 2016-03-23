namespace Models.Request
{
    /// <summary>
    /// 退菜请求类。
    /// </summary>
    public class BackDishRequest
    {
        public BackDishRequest()
        {
            sequence = 999999;
            operationType = 2;
        }

        /// <summary>
        /// 操作类型，0：单个菜，1：整单。
        /// </summary>
        public string actionType { get; set; }

        public string userName { get; set; }

        public string orderNo { get; set; }

        public string dishNo { get; set; }

        public string tableNo { get; set; }

        public decimal dishNum { get; set; }

        public string dishunit { get; set; }

        public string currenttableid { get; set; }

        public string dishtype { get; set; }

        /// <summary>
        /// 火锅的id。
        /// </summary>
        public string potdishid { get; set; }

        /// <summary>
        /// 1锅底  0鱼
        /// </summary>
        public string hotflag { get; set; }

        /// <summary>
        /// 1：下单；2：退菜
        /// </summary>
        public int operationType { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int sequence { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public string primarykey { get; set; }

        /// <summary>
        /// 退菜授权人。
        /// </summary>
        public string discardUserId { get; set; }

        /// <summary>
        /// 退菜原因。
        /// </summary>
        public string discardReason { get; set; }
    }
}