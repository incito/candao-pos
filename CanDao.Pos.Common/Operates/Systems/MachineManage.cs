using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace CanDao.Pos.Common.Operates.Systems
{
    /// <summary>
    /// 机器管理类
    /// </summary>
    public class MachineManage
    {
        #region 构造函数

        public MachineManage()
        {

        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 获取MD5的机器唯一识别
        /// </summary>
        /// <returns></returns>
        public static string GetMachineId()
        {
            var str = string.Format("{0}::{1}::{2}", GetCpuId(), BiosSn(), MbSn());

            return Md5Change(str);
        }

        #endregion

        #region 系统信息获取
        /// <summary>
        /// 32位Md5加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string Md5Change(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0').ToUpper();
        }

        /// <summary>
        /// 获取系统信息
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="className">进程名称</param>
        /// <returns></returns>
        private static string GetOSInfo(string propertyName, string className)
        {
            try
            {
                var query = new SelectQuery(string.Format("Select {0} From {1}", propertyName, className));
                var searcher = new ManagementObjectSearcher(query);
                var moCollection = searcher.Get();
                foreach (var mo in moCollection)
                {
                    foreach (PropertyData property in mo.Properties)
                    {
                        if (property.Name.Equals(propertyName))
                        {
                            return property.Value.ToString();
                        }
                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurs in WMI query.", ex.InnerException);
            }
        }

        /// <summary>
        /// 获取CpuId
        /// </summary>
        /// <returns></returns>
        private static string GetCpuId()
        {
            return MachineManage.GetOSInfo("ProcessorId", "Win32_Processor");
        }

        /// <summary>
        /// 获取Bios序列号
        /// </summary>
        /// <returns></returns>
        private static string BiosSn()
        {
            return MachineManage.GetOSInfo("SerialNumber", "Win32_BIOS");
        }

        /// <summary>
        /// 主板序列号
        /// </summary>
        /// <returns></returns>
        private static string MbSn()
        {
            return MachineManage.GetOSInfo("SerialNumber", "Win32_BaseBoard");
        }

        #endregion
    }
}
