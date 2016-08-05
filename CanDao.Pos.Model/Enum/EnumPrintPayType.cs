using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 打印单子类型
    /// </summary>
    public enum EnumPrintPayType
    {
        /// <summary>
        /// 预结
        /// </summary>
        BeforehandPay = 1,
        /// <summary>
        /// 结算
        /// </summary>
        Pay=2,
        /// <summary>
        /// 客用
        /// </summary>
        CustomerUse=3
    }
}
