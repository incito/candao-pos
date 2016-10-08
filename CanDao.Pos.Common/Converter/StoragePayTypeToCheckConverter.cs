using System;
using System.Globalization;
using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 储值支付方式类型跟选中状态的转换类。
    /// </summary>
    public class StoragePayTypeToCheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var curStatus = ((EnumStoragePayType)value).ToString("G");
            return curStatus.Equals(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        } 
    }
}