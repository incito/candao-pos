using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDaoCD.Pos.Common.Models.VipModels
{
    /// <summary>
    /// 会员修改信息
    /// </summary>
    public class MVipChangeInfo
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string TelNum { set; get; }

        /// <summary>
        /// 会员卡号
        /// </summary>
        public string CardNum { set; get; }

        /// <summary>
        /// 会员密码
        /// </summary>
        public string Password { set; get; }

        /// <summary>
        /// 会员名称
        /// </summary>
        public string VipName { set; get; }

        /// <summary>
        /// 性别{0:男 1：女}
        /// </summary>
        public int Sex { set; get; }

        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { set; get; }

        /// <summary>
        /// 会员地址
        /// </summary>
        public string Address { set; get; }
    }
}
