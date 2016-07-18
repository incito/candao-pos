using System.Windows;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// 通知对话框。
    /// </summary>
    public class NotifyDialog
    {
        /// <summary>
        /// 通知消息。
        /// </summary>
        /// <param name="message">消息内容。</param>
        /// <param name="ownerWindow">所属窗口。</param>
        public static void Notify(string message, Window ownerWindow = null)
        {
            NotifyWindow wnd = new NotifyWindow(message, ownerWindow);
            wnd.Show();
        }
    }
}