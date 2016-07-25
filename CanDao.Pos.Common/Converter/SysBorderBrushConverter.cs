using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 系统按钮边框颜色转换类。
    /// </summary>
    public class SysBorderBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Brushes.Red : Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}