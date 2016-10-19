using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Application = System.Windows.Application;

namespace CanDao.Pos.Common
{
    public class WindowHelper
    {
        /// <summary>
        /// 窗口的顺序集合信息。
        /// </summary>
        private static readonly List<Window> WindowStack;

        private static object _syncObj = new object();

        /// <summary>
        /// 最后一个当前显示的窗口。
        /// </summary>
        public static Window LastShowWindow
        {
            get { return WindowStack.Last(); }
        }

        static WindowHelper()
        {
            WindowStack = new List<Window>();
        }

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
            return ShowDialog(wnd, null);
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
            return wnd.ShowDialog() == true;
        }

        /// <summary>
        /// 显示模态窗口。
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static bool ShowDialog(NormalWindowViewModel vm)
        {
            return ShowDialog(vm, null);
        }

        /// <summary>
        /// 显示模态窗口。
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="ownerWnd"></param>
        /// <returns></returns>
        public static bool ShowDialog(NormalWindowViewModel vm, Window ownerWnd)
        {
            return ShowDialog(vm.OwnerWindow, ownerWnd);
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
            {
                if (WindowStack.Any())
                    ownerWnd = WindowStack.Last();
            }

            if (ownerWnd == null || Equals(wnd, ownerWnd))
            {
                wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                wnd.Owner = ownerWnd;
                wnd.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }

            wnd.Closed += delegate
            {
                lock (_syncObj)
                {
                    WindowStack.Remove(wnd);
                }
            };

            lock (_syncObj)
            {
                WindowStack.Add(wnd);
            }
        }
    }
}