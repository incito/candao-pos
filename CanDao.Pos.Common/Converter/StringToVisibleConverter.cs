using System.Windows;
using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 字符串与可见性的转换类。
    /// </summary>
    public class StringToVisibleConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty((string)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}