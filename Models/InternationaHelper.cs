using System.Collections.Generic;
using System.Linq;

namespace Models
{
    /// <summary>
    /// 国际化辅助类。
    /// </summary>
    public class InternationaHelper
    {
        /// <summary>
        /// 是否包含中英文国际化的标识。
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static bool HasInternationaFlag(string srcString)
        {
            return srcString.Contains("#");
        }

        /// <summary>
        /// 过滤分隔符。
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static string FilterSeparatorFlag(string srcString)
        {
            return srcString.Replace("#", "");
        }

        /// <summary>
        /// 获取分隔符之前的数据。
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static string GetBeforeSeparatorFlagData(string srcString)
        {
            var index = srcString.IndexOf('#');
            return index > 0 ? srcString.Substring(0, index) : srcString;
        }

        /// <summary>
        /// 替换分隔符。
        /// </summary>
        /// <param name="srcString"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string ReplaceSeparatorFlag(string srcString, string flag)
        {
            return srcString.Replace("#", flag);
        }

        /// <summary>
        /// 按照中英文的分隔符进行分割成集合。
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        public static List<string> SplitBySeparatorFlag(string srcString)
        {
            return srcString.Split('#').ToList();
        }
    }
}