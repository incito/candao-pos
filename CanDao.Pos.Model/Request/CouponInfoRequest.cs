using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model.Request
{
    public class CouponInfoRequest
    {
        /// <summary>
        /// 机器标识。IP地址。
        /// </summary>
        public string machineno { get; set; }

        /// <summary>
        /// 当前用户ID。
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 订单ID。
        /// </summary>
        public string orderid { get; set; }

        /// <summary>
        /// 类型编号。
        /// </summary>
        public string typeid { get; set; }
    }
}