using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 账单查询状态跟是否选中的转换类。
    /// </summary>
    public class QueryOrderStatusToCheckedConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var curStatus = ((EnumQueryOrderStatus)value).ToString("G");
            return curStatus.Equals(parameter as string);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}