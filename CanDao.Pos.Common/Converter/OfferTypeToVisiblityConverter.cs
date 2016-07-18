using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 优惠类型与选中状态的转换类。
    /// </summary>
    public class OfferTypeToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var offerType = (EnumOfferType)value;
            var temp = ((string)parameter).Split('_');
            return temp.Contains(offerType.ToString("g")) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}