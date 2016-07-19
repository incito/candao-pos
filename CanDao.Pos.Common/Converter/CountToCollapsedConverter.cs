using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 指定数字隐藏的转换类。
    /// </summary>
    public class CountToCollapsedConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Visible;

            var temp = ((string)parameter).Split('_');
            var valueList = temp.Select(System.Convert.ToDecimal).ToList();
            return valueList.Contains(System.Convert.ToDecimal(value)) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}