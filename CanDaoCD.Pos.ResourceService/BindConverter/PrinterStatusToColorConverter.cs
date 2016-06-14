using System.Windows.Data;
using System.Windows.Media;
using Models.Enum;

namespace CanDaoCD.Pos.ResourceService.BindConverter
{
    public class PrinterStatusToColorConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var status = (EnumPrintStatus)value;
            var color = Brushes.DarkOrange;
            switch (status)
            {
                case EnumPrintStatus.None:
                    break;
                case EnumPrintStatus.Good:
                    color = Brushes.Green;
                    break;
                case EnumPrintStatus.NotReachable:
                    color = Brushes.Red;
                    break;
            }
            return color;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}