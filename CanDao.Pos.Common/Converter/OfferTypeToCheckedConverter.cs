using System;
using System.Globalization;
using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    public class OfferTypeToCheckedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var payType = (EnumOfferType)value;
            return payType.ToString("g").Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value && !string.IsNullOrEmpty((parameter as string)))
                return (EnumOfferType)Enum.Parse(typeof(EnumOfferType), (string)parameter);

            return EnumOfferType.Amount;
        }
    }
}