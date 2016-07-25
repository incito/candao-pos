using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.ReportPrint.Operates
{
    public class OInfoManage
    {
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static string GetErrorString(int errorCode)
        {
            switch (errorCode)
            {
                case 0x01:
                {
                    return "串口句柄为空";
                }
                case 0x02:
                {
                    return "等待命令回复超时";
                }
                case 0x03:
                {
                    return "等待操作结果超时";
                }
                case 0x04:
                {
                    return "操作结果格式错误";
                }
                case 0x20:
                {
                    return "Normal";
                }
                case 0x22:
                {
                    return "No target card (only when using a no card status command) C";
                }
                case 0x23:
                {
                    return "No magnetic stripe (when inserted backward) or other error C";
                }
                case 0x31:
                {
                    return "Parity Error B";
                }
                case 0x32:
                {
                    return "No start code/end code B";
                }
                case 0x33:
                {
                    return "LRC Error B";
                }
                case 0x34:
                {
                    return "Erroneous character B";
                }
                case 0x37:
                {
                    return "Magnetic stripe writing error B";
                }
                case 0x38:
                {
                    return "Card jam B";
                }
                case 0x40:
                {
                    return "Cover open B";
                }
                case 0x41:
                {
                    return "Invalid command C";

                }
                case 0x42:
                {
                    return "Cam motor error A";
                }
                case 0x43:
                {
                    return "Erase head temperature error A";
                }
                case 0x45:
                {
                    return "EEPROM error A";
                }
                case 0x4C:
                {
                    return "Non-compatible BMP file data C";
                }
                case 0x51:
                {
                    return "Expand buffer overflow";
                }
            }

            return "Null";
        }
        /// <summary>
        /// 获取错误信息-Star(TCP300-2)
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public static string GetErrorInfo(int errorCode)
        {
            switch (errorCode)
            {
                case 0:
                {
                    return "正确返回";
                }
                case 0X9080:
                {
                    return "串口错误";
                }
                case 0X9081:
                {
                    return "超时";
                }
                case 0X9082:
                {
                    return "校验错误";
                }
                case 0x9083:
                {
                    return "无效指令";
                }
                case 0x9084:
                {
                    return "无效的返回";
                }
            }
            return "Null";
        }

    }
}
