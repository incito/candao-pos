using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace CanDao.Pos.Common.Controls.CSystem
{
    /// <summary>
    /// 用户控件基础类
    /// </summary>
    public class UserControlBase :UserControl
    {
        #region 构造函数

        public UserControlBase()
        {

        }

        #endregion

        #region 事件
        /// <summary>
        /// 关闭事件
        /// </summary>
        public Action UcClose { set; get; }

        /// <summary>
        /// 关闭事件加状态
        /// </summary>
        public Action<bool> UcCloseaAction { set; get; }
        #endregion
    }
}
