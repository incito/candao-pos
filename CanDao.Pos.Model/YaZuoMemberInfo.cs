using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 雅座会员信息。
    /// </summary>
    public class YaZuoMemberInfo
    {
        /// <summary>
        /// 手机号。
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 储值余额。
        /// </summary>
        public decimal StoredBalance { get; set; }

        /// <summary>
        /// 积分余额。
        /// </summary>
        public decimal IntegralBalance { get; set; }

        /// <summary>
        /// 卡号。
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 多卡号集合。如果只有一个卡号，该字段为null。
        /// </summary>
        public List<string> CardNoList { get; set; }

        /// <summary>
        /// 雅座会员优惠券集合。
        /// </summary>
        public List<YaZuoCouponInfo> CouponList { get; set; }
    }
}