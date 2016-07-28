using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Common.Models.VipModels
{
    /// <summary>
    /// 会员卡信息
    /// </summary>
    public class MVipCardInfo
    {
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNum { set; get; }
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { set; get; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Integral { set; get; }
        /// <summary>
        /// 优惠券余额
        /// </summary>
        public float CouponBalance { set; get; }

        /// <summary>
        /// 会员交易号
        /// </summary>
        public string TraceCode { set; get; }

        /// <summary>
        /// 卡类型{0:虚拟卡，1：实体卡，2：微会员}
        /// </summary>
        public int CardType { set; get; }

        /// <summary>
        /// 卡等级
        /// </summary>
        public int CardLevel { set; get; }

        /// <summary>
        /// 卡等级名称
        /// </summary>
        public string CardLevelName { set; get; }

        /// <summary>
        /// 卡状态{0 注销 1 正常 2 挂失}
        /// </summary>
        public int CardState { set; get; }
    }
}
