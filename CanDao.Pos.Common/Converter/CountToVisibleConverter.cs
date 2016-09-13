using System.Windows;
using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 数字跟可见性的转换类。
    /// </summary>
    public class CountToVisibleConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int minValue = 0;
            if (parameter != null)
                minValue = System.Convert.ToInt32(parameter);
            return System.Convert.ToInt32(value) > minValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}