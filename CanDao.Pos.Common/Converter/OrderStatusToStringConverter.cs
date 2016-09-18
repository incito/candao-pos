using System;
using System.Globalization;
using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 订单状态跟显示字符的转换类。
    /// </summary>
    public class OrderStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((EnumOrderStatus)value)
            {
                case EnumOrderStatus.Ordered:
                    return "未结";
                case EnumOrderStatus.CanceledOrder:
                    return "已取消";
                case EnumOrderStatus.InternalSettle:
                    return "已结";
                default:
                    return "其他";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}