using System;
using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    /// <summary>
    /// 付款类型与选中状态转换类。
    /// </summary>
    public class PayTypeToCheckedConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var payType = (EnumBillPayType)value;
            return payType.ToString("g").Equals(parameter);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value && !string.IsNullOrEmpty((parameter as string)))
                return (EnumBillPayType)Enum.Parse(typeof(EnumBillPayType), (string)parameter);

            return EnumBillPayType.Cash;
        }
    }
}