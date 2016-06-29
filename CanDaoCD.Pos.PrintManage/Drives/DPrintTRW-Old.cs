using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CanDaoCD.Pos.PrintManage.Drives
{
    public class DPrintTRWOld
    {
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="port"></param>
        /// <param name="bandrate"></param>
        /// <returns></returns>
        [DllImport("RCPV11_VCDLL.dll")]
        public static extern IntPtr CommOpen(int port, int bandrate);
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="m_hCom"></param>
        /// <returns></returns>
        [DllImport("RCPV11_VCDLL.dll")]
        public static extern bool CommClose(IntPtr m_hCom);

        /// <summary>
        /// 擦除弹出打印
        /// </summary>
        /// <param name="m_hCom"></param>
        /// <param name="dischargeMode"></param>
        /// <param name="eraseMode"></param>
        /// <param name="printMode"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        [DllImport("RCPV11_VCDLL.dll")]
        public static extern int ErasePrintDischarge(IntPtr m_hCom, int dischargeMode, int eraseMode, int printMode, int waitTime);
        /// <summary>
        /// 打印弹出
        /// </summary>
        /// <param name="m_hCom"></param>
        /// <param name="mode"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>

        [DllImport("RCPV11_VCDLL.dll")]
        public static extern int PrintDischarge(IntPtr m_hCom, int mode, int waitTime);
        /// <summary>
        /// 擦除弹出
        /// </summary>
        /// <param name="m_hCom"></param>
        /// <param name="mode"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>

        [DllImport("RCPV11_VCDLL.dll")]
        public static extern int EraseDischarge(IntPtr m_hCom, int mode, int waitTime);
        /// <summary>
        /// 进卡
        /// </summary>
        /// <param name="m_hCom"></param>
        /// <param name="waitTime"></param>
        /// <returns></returns>

        [DllImport("RCPV11_VCDLL.dll")]
        public static extern int CardInOver(IntPtr m_hCom, int waitTime);


        [DllImport("RCPV11_VCDLL.dll")]
        public static extern int RequestStatus(IntPtr m_hCom,ref string[] res, int waitTime);
    }
}
