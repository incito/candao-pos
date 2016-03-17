namespace Models
{
    /// <summary>
    /// 未清机的POS信息。
    /// </summary>
    public class UnclearPosInfo
    {
        /// <summary>
        /// 用户ID。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// POS标识符。
        /// </summary>
        public string PosFlag { get; set; }
    }
}