using System.Windows.Data;
using System.Windows.Media;

namespace CanDao.Pos.UI.MainView.Converter
{
    /// <summary>
    /// 订单状态跟颜色的转换类。
    /// </summary>
    public class OrderStatusToColorConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is bool))
                return Brushes.Coral;

            return (bool) value ? Brushes.LawnGreen : Brushes.Coral;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}