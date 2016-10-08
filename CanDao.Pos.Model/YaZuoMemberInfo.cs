using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 雅座会员信息。
    /// </summary>
    public class YaZuoMemberInfo : MemberInfo
    {
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