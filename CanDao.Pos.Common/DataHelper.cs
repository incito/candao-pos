using System;
using System.Linq;

namespace CanDao.Pos.Common
{
    public class DataHelper
    {
        /// <summary>
        /// 按照输入顺序取第一个非空值。
        /// </summary>
        /// <param name="values">输入顺序。</param>
        /// <returns>返回第一个非空值。</returns>
        public static string GetNoneNullValueByOrder(params string[] values)
        {
            return values.FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));
        }

        public static int Parse2Int(object valueStr, int defaultValue = 0)
        {
            if(valueStr == null)
                return defaultValue;

            try
            {
                return Convert.ToInt32(valueStr);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(string.Format("将字符：\"{0}\"转换成Int出错。", valueStr), ex);
                return defaultValue;
            }
        }
    }
}