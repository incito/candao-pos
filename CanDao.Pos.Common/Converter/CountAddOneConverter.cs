using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 数字加一转换类。
    /// </summary>
    public class CountAddOneConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToInt32(value) + 1;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}