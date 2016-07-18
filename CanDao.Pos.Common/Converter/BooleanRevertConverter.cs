using System;
using System.Globalization;
using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 布尔值反转转换类。
    /// </summary>
    public class BooleanRevertConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}