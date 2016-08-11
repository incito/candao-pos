using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    public class ZeroToNullStringConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (DataHelper.Parse2Decimal(value) == 0) ? "" : value.ToString();
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value as string;
            return !string.IsNullOrWhiteSpace(str) ? DataHelper.Parse2Decimal(str) : 0m;
        }
    }
}