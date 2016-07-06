namespace Models
{
    /// <summary>
    /// 国际化辅助类。
    /// </summary>
    public class InternationaHelper
    {
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
    }
}