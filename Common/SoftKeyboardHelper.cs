using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Common
{
    public class SoftKeyboardHelper
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "MoveWindow")]
        private static extern bool MoveWindow(System.IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static Process _oskProcess;

        /// <summary>
        /// 显示软键盘。
        /// </summary>
        /// <returns></returns>
        public static string ShowSoftKeyboard()
        {
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Lib\osk.exe");
                if (!File.Exists(filePath))
                    return "软键盘可执行文件不存在。";
            
                _oskProcess = Process.Start(filePath);
                IntPtr intptr = IntPtr.Zero;
                while (IntPtr.Zero == intptr)
                {
                    System.Threading.Thread.Sleep(100);
                    intptr = FindWindow(null, "屏幕键盘");
                }
                // 获取屏幕尺寸
                int iActulaWidth = Screen.PrimaryScreen.Bounds.Width;
                int iActulaHeight = Screen.PrimaryScreen.Bounds.Height;

                // 设置软键盘的显示位置，底部居中
                int posX = (iActulaWidth - 1000) / 2;
                int posY = (iActulaHeight - 300);

                //设定键盘显示位置
                MoveWindow(intptr, posX, posY, 1000, 300, true);
                return null;
            }
            catch (Exception ex)
            {
                AllLog.Instance.E("打开软键盘时异常：", ex);
                return string.Format("打开软键盘时异常：{0}", ex.Message);
            }
        }

        /// <summary>
        /// 关闭软键盘。
        /// </summary>
        /// <returns></returns>
        public static string CloseSoftKeyboard()
        {
            try
            {
                if (_oskProcess != null)
                    _oskProcess.Kill();
                return null;
            }
            catch (Exception ex)
            {
                AllLog.Instance.E("关闭软键盘失败。",ex);
                return "关闭软键盘失败。";
            }
        }
    }
}