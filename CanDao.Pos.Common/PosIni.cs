using System;
using System.IO;
using System.Windows;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// Pos的数据文件。
    /// </summary>
    public class PosIni
    {
        private const string PosIniFile = "PosData.ini";

        /// <summary>
        /// 餐具数量节点名。
        /// </summary>
        private const string DinnerWareSection = "DinnerWare";

        /// <summary>
        /// 获取餐具数量。
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetDinnerWareCount(string tableName)
        {
            return GetValue(DinnerWareSection, tableName, "0");
        }

        /// <summary>
        /// 设置餐具数量。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dinnerWareCount"></param>
        /// <returns></returns>
        public static void SetDinnerWareCount(string tableName, int dinnerWareCount)
        {
            SetValue(DinnerWareSection, tableName, dinnerWareCount.ToString());
        }

        /// <summary>
        /// 获取数据。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        private static string GetValue(string section, string key, string defValue)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PosIniFile);
            IniFileHelper iniHelper = new IniFileHelper(filePath);
            return iniHelper.ReadValue(section, key, defValue);
        }

        /// <summary>
        /// 写入数据。
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private static void SetValue(string section, string key, string value)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PosIniFile);
            IniFileHelper iniHelper = new IniFileHelper(filePath);
            iniHelper.WriteValue(section, key, value);
        }
    }
}