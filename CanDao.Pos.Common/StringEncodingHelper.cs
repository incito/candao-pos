using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CanDao.Pos.Common
{
    public static class StringEncodingHelper
    {
        public static string ToUnicodeString(this string str)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    strResult.Append("\\u");
                    strResult.Append(((int)str[i]).ToString("x"));
                }
            }
            return strResult.ToString();
        }

        public static string FromUnicodeString(this string str)
        {
            //最直接的方法Regex.Unescape(str);
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Contains(@"\u"))
                {
                    string[] strlist = str.Replace("\\", "").Split('u');
                    try
                    {
                        for (int i = 1; i < strlist.Length; i++)
                        {
                            int charCode = Convert.ToInt32(strlist[i], 16);
                            strResult.Append((char) charCode);
                        }
                    }
                    catch (FormatException ex)
                    {
                        return Regex.Unescape(str);
                    }
                }
                else
                {
                    return Regex.Unescape(str);
                }
            }
            return strResult.ToString();
        }
    }
}