using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Common
{
    class OpenCash
    {
        public string PortType = "网口";
        /// <summary>
        /// 执行开钱箱操作
        /// </summary>
        private void ExecuteOpenCashBoxOperate()
        {

            //string PortType = POSSetting.OPOS_CFG_DS.OtherCFGSetDT.Rows[1][2].ToString();
            //DataRow dr = GetPortParaData(PortType);
            string port = "192.168.2.211:9001";

            bool IsOpen = false;
            BeiYangOPOS BYOPOS = new BeiYangOPOS();
            try
            {
                switch (PortType.Trim())
                {
                    case "并口":
                        //IsOpen = BYOPOS.OpenLPTPort(port);
                        break;
                    case "串口":
                        /*SerialPort sPort = new SerialPort();
                        sPort.PortName = dr[1].ToString();
                        sPort.BaudRate = int.Parse(dr[2].ToString());
                        sPort.DataBits = int.Parse(dr[3].ToString());
                        sPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), dr[4].ToString());
                        sPort.Parity = (Parity)Enum.Parse(typeof(Parity), dr[5].ToString());
                        sPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), dr[6].ToString());
                        IsOpen = BYOPOS.OpenComPort(ref sPort);
                        sPort.Dispose();*/
                        break;
                    case "USB":
                        //IsOpen = BYOPOS.OpenUSBPort(dr[1].ToString());
                        break;
                    case "驱动程序":
                        //IsOpen = BYOPOS.OpenPrinter(dr[1].ToString());
                        break;
                    case "网口":
                        IsOpen = BYOPOS.OpenNetPort(port);
                        break;
                }
                if (IsOpen)
                {
                    IntPtr res = BeiYangOPOS.POS_KickOutDrawer(0x00, 100, 80);
                    // if ((uint)res != BYOPOS.POS_SUCCESS)
                    //     LogManager.WriteLog("POSErr", "开钱箱失败!指令调用返回值：" + res.ToString());
                    BYOPOS.ClosePrinterPort();
                }
            }
            catch (Exception ex)
            {
                //LogManager.WriteLog("POSErr", ex.Message + Environment.NewLine + ex.StackTrace);
            }
            finally
            {
                BYOPOS.ClosePrinterPort();
            }
        }
    }
}
