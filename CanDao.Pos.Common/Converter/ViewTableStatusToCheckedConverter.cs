using System;
using System.Globalization;
using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 视图上餐桌状态跟选中状态的转换类。
    /// </summary>
    public class ViewTableStatusToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var curStatus = ((EnumViewTableStatus)value).ToString("G");
            return curStatus.Equals(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}