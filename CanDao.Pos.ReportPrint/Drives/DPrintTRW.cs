using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using CanDao.Pos.ReportPrint.Structs;

namespace CanDao.Pos.ReportPrint.Drives
{
   public class DPrintTRW
    {
        [DllImport("PCBDll.dll")]
        public static extern int OpenCom(int bComId, int bByteSize, int bStopBits, int bParity, int dwBaudRate);
        [DllImport("PCBDll.dll")]
        public static extern int CloseCom(int iComId);
        [DllImport("PCBDll.dll")]
        public static extern int CardRearSide(ref int Status, int WaitTime = 20000);
        [DllImport("PCBDll.dll")]
        public static extern int DischargeCard(int DischargeMethod, ref int Status, int WaitTime = 20000);

        [DllImport("PCBDll.dll", CharSet = CharSet.Ansi)]
        public static extern int SettingTrackBuf(byte CodeNum, StringBuilder Data, int DataLen, ref int Status, int WaitTime = 20000);
        [DllImport("PCBDll.dll")]
        public static extern int WriteTrack(byte CodeNum, int TrackNum, ref int Status, int WaitTime = 20000);
        [DllImport("PCBDll.dll")]
        public static extern int SettingPrintChar(int Arrangement, int X, int Y, StringBuilder Data, ref int Status, int WaitTime = 6000);
        [DllImport("PCBDll.dll")]
        public static extern int ErasePrintDischarge300(int DischargerMethod, ref int Status, int WaitTime = 6000);
        [DllImport("PCBDll.dll")]
        public static extern int EraseDischarge(int DischargerMethod, ref int Status, int WaitTime = 6000);
         [DllImport("PCBDll.dll")]
        public static extern int RequestStatus(ref StatuRes statuRes, ref int Status, int WaitTime = 6000);
        /// <summary>
        /// 清除扩展区内存(字符)
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="WaitTime"></param>
        /// <returns></returns>
        [DllImport("PCBDll.dll")]
        public static extern int ClrPrtExpBuf(ref int Status, int WaitTime = 6000);
        /// <summary>
        /// 清除打印扩展缓冲区和图像缓冲区。
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="WaitTime"></param>
        /// <returns></returns>
        [DllImport("PCBDll.dll")]
        public static extern int ClrBuf(ref int Status, int WaitTime);
    }
}
