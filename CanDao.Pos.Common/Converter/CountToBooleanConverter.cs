using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace CanDao.Pos.Common.Converter
{
    public class CountToBooleanConverter : IValueConverter
    {
        public bool IsInvert { get; set; }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return true;

            var temp = ((string)parameter).Split('_');
            var valueList = temp.Select(System.Convert.ToDecimal).ToList();
            var result = valueList.Contains(System.Convert.ToDecimal(value));
            return IsInvert ? !result : result;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}