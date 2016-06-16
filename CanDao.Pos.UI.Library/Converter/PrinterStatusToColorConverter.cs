using System.Windows.Data;
using System.Windows.Media;
using Models.Enum;

namespace CanDao.Pos.UI.Library.Converter
{
    public class PrinterStatusToColorConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (EnumPrintStatus)value == EnumPrintStatus.Normal ? Brushes.Green : Brushes.Red;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}