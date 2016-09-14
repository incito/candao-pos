using System;
using System.Globalization;
using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 视图餐台类型跟选中状态的转换类。
    /// </summary>
    public class ViewTableTypeToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var curStatus = ((EnumViewTableType)value).ToString("G");
            return curStatus.Equals(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        } 
    }
}