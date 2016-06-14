using CanDaoCD.Pos.Common.Controls.CSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CanDaoCD.Pos.Common.Operates
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
        public static void ShowPopupWindow(UserControlBase showUc)
        {
            var wPopup = new WPopup();
            wPopup.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            wPopup.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            wPopup.WindowStartupLocation= WindowStartupLocation.CenterScreen;
            wPopup.SetShowUc(showUc);
            wPopup.IsDialog = true;//设置模态方式打开
            wPopup.ShowDialog();
        }

        /// <summary>
        /// 模态方式打开消息窗口
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isShowClose"></param>
        public static bool ShowMessageWindow(string message, bool isShowClose)
        {
            var wMessage=new WMessage();
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
