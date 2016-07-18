using System.Windows;
using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 布尔值与控件隐藏关系的转换。
    /// </summary>
    public class BooleanToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
                return ((bool) value) ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}