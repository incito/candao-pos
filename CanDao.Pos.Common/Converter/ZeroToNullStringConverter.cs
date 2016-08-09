using System.Windows;
using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    public class ZeroToNullStringConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (DataHelper.Parse2Int(value) == 0) ? "" : value.ToString();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value as string;
            return !string.IsNullOrEmpty(str) ? System.Convert.ToInt32(str) : 0;
        }
    }
}