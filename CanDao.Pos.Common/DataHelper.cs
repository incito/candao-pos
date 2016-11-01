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

        /// <summary>
        /// 用指定的分隔符组合字符串。
        /// </summary>
        /// <param name="sep"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string JoinString(string sep, params string[] args)
        {
            return string.Join(sep, args);
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

        public static decimal Parse2Decimal(object valueStr, decimal defaultValue = 0m)
        {
            if (valueStr == null)
                return defaultValue;

            try
            {
                return Convert.ToDecimal(valueStr);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(string.Format("将字符：\"{0}\"转换成decimal出错。", valueStr), ex);
                return defaultValue;
            }
        }
    }
}