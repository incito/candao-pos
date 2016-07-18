using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CanDao.Pos.Common
{
    public class IniFileHelper
    {
        #region Extern

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STRINGBUFFER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szText;
        }

        //读写INI文件的API函数 
        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string section, string key, string def, out STRINGBUFFER retVal, int size, string filePath);

        #endregion

        public IniFileHelper(string iniFile)
        {
            if (string.IsNullOrEmpty(iniFile))
                throw new ArgumentNullException("iniFile");

            IniPath = iniFile;
            if (File.Exists(IniPath))
            {
                try
                {
                    var sw = File.CreateText(IniPath);
                    sw.Close();
                }
                catch (Exception ex)
                {
                    ErrLog.Instance.E(ex);
                }
            }
        }

        /// <summary>
        /// Ini文件路径。
        /// </summary>
        public string IniPath { get; set; }

        /// <summary>
        /// 写入Ini数据。
        /// </summary>
        /// <param name="section">节点名。</param>
        /// <param name="key">名称。</param>
        /// <param name="value">写入的值。</param>
        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, IniPath);
        }

        /// <summary>
        /// 读取Ini数据。
        /// </summary>
        /// <param name="section">节点名。</param>
        /// <param name="key">名称。</param>
        /// <param name="defValue">默认值。</param>
        /// <returns>返回Ini里的数据。</returns>
        public string ReadValue(string section, string key, string defValue)
        {
            STRINGBUFFER retVal;
            GetPrivateProfileString(section, key, defValue, out retVal, 255, IniPath);
            return retVal.szText.Trim();
        }
    }
}