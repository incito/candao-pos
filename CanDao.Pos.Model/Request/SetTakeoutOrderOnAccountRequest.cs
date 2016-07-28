namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 外卖挂账单位信息。
    /// </summary>
    public class SetTakeoutOrderOnAccountRequest
    {
        /// <summary>
        /// 挂账单位编号。
        /// </summary>
        public string CmpCode { get; set; }

        /// <summary>
        /// 挂账单位名称。
        /// </summary>
        public string CmpName { get; set; }

        /// <summary>
        /// 联系手机号。
        /// </summary>
        public string ContactMobile { get; set; }

        /// <summary>
        /// 联系人姓名。
        /// </summary>
        public string ContactName { get; set; }
    }
}