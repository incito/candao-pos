using CanDao.Pos.Common.Controls.CSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CanDao.Pos.Common.Operates
{
    /// <summary>
    /// 操作操作管理类
    /// </summary>
    public class OWindowManage
    {
        /// <summary>
        /// 模态方式打开弹出窗口
        /// </summary>
        /// <param name="showUc">显示的用户控件</param>
        public static bool ShowPopupWindow(UserControlBase showUc, Brush bgColor = null)
        {
            var wPopup = new WPopup();
            wPopup.Background = bgColor;
            wPopup.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            wPopup.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            wPopup.WindowStartupLocation= WindowStartupLocation.CenterScreen;
         
            wPopup.SetShowUc(showUc);
            wPopup.IsDialog = true;//设置模态方式打开
            if (wPopup.ShowDialog() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 模态方式打开消息窗口
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isShowClose"></param>
        public static bool ShowMessageWindow(string message, bool isShowClose)
        {
            var wMessage = new WMessage();
            wMessage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            wMessage.Init(message,isShowClose);
            if (wMessage.ShowDialog() == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
