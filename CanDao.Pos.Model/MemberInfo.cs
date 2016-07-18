using System;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 会员信息。
    /// </summary>
    public class MemberInfo
    {
        /// <summary>
        /// 卡号。
        /// </summary>
        public string CardNo { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号。
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 生日。
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别。
        /// </summary>
        public EnumGender Gender { get; set; }

        /// <summary>
        /// 储值余额。
        /// </summary>
        public decimal StoredBalance { get; set; }

        /// <summary>
        /// 积分。
        /// </summary>
        public decimal Integral { get; set; }

        /// <summary>
        /// 会员卡等级。
        /// </summary>
        public string CardLevel { get; set; }
    }
}