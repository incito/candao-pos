using System;
using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// 小费统计全部信息。
    /// </summary>
    public class TipFullInfo : StatisticInfoBase
    {
        public List<TipInfo> TipInfos { get; set; }

    }
}