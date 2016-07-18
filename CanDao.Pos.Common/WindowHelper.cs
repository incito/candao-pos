using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CanDao.Pos.Common
{
    public class WindowHelper
    {
        public static DependencyObject FindVisualTreeRoot(DependencyObject initial)
        {
            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null)
            {
                result = current;
                if (current is Visual || current is Visual3D)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else
                {
                    current = LogicalTreeHelper.GetParent(current);
                }
            }
            return result;
        }

        /// <summary>
        /// 显示模态窗口。
        /// </summary>
        /// <param name="wnd"></param>
        /// <returns></returns>
        public static bool ShowDialog(Window wnd)
        {
            return ShowDialog(wnd, Application.Current.MainWindow);
        }

        /// <summary>
        /// 显示模态窗口。
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="ownerWnd"></param>
        /// <returns></returns>
        public static bool ShowDialog(Window wnd, Window ownerWnd)
        {
            ProcessWindow(wnd, ownerWnd);
            var result = wnd.ShowDialog() == true;
            return result;
        }

        /// <summary>
        /// 显示非模态窗口。
        /// </summary>
        /// <param name="wnd"></param>
        public static void Show(Window wnd)
        {
            Show(wnd, Application.Current.MainWindow);
        }

        /// <summary>
        /// 显示非模态窗口。
        /// </summary>
        /// <param name="wnd"></param>
        /// <param name="ownerWnd"></param>
        public static void Show(Window wnd, Window ownerWnd)
        {
            ProcessWindow(wnd, ownerWnd);
            wnd.Show();
        }

        private static void ProcessWindow(Window wnd, Window ownerWnd)
        {
            if (wnd == null)
                throw new ArgumentNullException("wnd");

            if (ownerWnd == null)
                ownerWnd = Application.Current.MainWindow;

            if (Equals(wnd, ownerWnd))
            {
                wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                wnd.Owner = ownerWnd;
                wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
        }
    }
}