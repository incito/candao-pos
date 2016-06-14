using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;


namespace CanDaoCD.Pos.Common.Operates
{
    /// <summary>
    /// 键盘设置
    /// </summary>
    public class KeyBoardPre
    {
        [DllImport("user32.dll")]

        public static extern UInt32 SendInput(UInt32 nInputs, ref INPUT pInputs, int cbSize);

        public static void SendKey(Key sendkeyNum)
        {
            var input=new INPUT();
            input.type = 1;
            input.ki.wVk = (short)KeyInterop.VirtualKeyFromKey(sendkeyNum);
            input.ki.dwFlags = 0x0002;
            SendInput(1, ref input, Marshal.SizeOf(input));
        }
    }

    #region 鼠标结构
    [StructLayout(LayoutKind.Explicit)]

        public struct INPUT

        {

            [FieldOffset(0)]

            public Int32 type;

            [FieldOffset(4)]

            public KEYBDINPUT ki;

            [FieldOffset(4)]

            public MOUSEINPUT mi;

            [FieldOffset(4)]

            public HARDWAREINPUT hi;

        }


        [StructLayout(LayoutKind.Sequential)]

        public struct MOUSEINPUT

        {

            public Int32 dx;

            public Int32 dy;

            public Int32 mouseData;

            public Int32 dwFlags;

            public Int32 time;

            public IntPtr dwExtraInfo;

        }

        [StructLayout(LayoutKind.Sequential)]

        public struct KEYBDINPUT

        {

            public Int16 wVk;

            public Int16 wScan;

            public Int32 dwFlags;

            public Int32 time;

            public IntPtr dwExtraInfo;

        }

        [StructLayout(LayoutKind.Sequential)]

        public struct HARDWAREINPUT

        {

            public Int32 uMsg;

            public Int16 wParamL;

            public Int16 wParamH;

        }
        #endregion
}
