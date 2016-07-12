using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDaoCD.Pos.Common.Models.VipModels
{
    /// <summary>
    /// 会员信息
    /// </summary>
    public class MVipInfo : MVipChangeInfo
    {
        /// <summary>
        /// 注册时间
        /// </summary>
        public string Creattime { set; get; }

        /// <summary>
        /// 卡集合
        /// </summary>
        public List<MVipCardInfo> CardInfos { set; get; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { set; get; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string ResultInfo { set; get; }

        public MVipInfo()
        {
            CardInfos = new List<MVipCardInfo>();
        }
    }
}
