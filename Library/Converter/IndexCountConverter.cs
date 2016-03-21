using System;
using System.Windows.Data;

namespace Library.Converter
{/// <summary>
    /// index 转为count 
    /// </summary>
    public class IndexCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && value is int)
            {
                Int32 temp = System.Convert.ToInt32(value);
                return temp < 0 ? string.Format("[{0}]", Math.Abs(temp)) : (temp + 1).ToString();//小于0是分组的序号，进行特殊处理。
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}