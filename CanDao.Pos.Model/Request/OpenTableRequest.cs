namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 开台请求类。
    /// </summary> 
    public class OpenTableRequest
    {
        /// <summary>
        /// 餐桌名。
        /// </summary>
        public string tableNo { get; set; }

        /// <summary>
        /// 服务员编号。
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 男性就餐人数。
        /// </summary>
        public int manNum { get; set; }

        /// <summary>
        /// 女性就餐人数。
        /// </summary>
        public int womanNum { get; set; }

        /// <summary>
        /// 年龄段描述串。
        /// </summary>
        public string ageperiod { get; set; }
    }
}