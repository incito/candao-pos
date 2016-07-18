using System;
using System.Windows.Data;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.Converter
{
    public class EnumGenderToCheckedConverter:IValueConverter
    {
        private EnumGender curGender;

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var gender = (EnumGender) value;
            return gender.ToString("g").Equals(parameter);
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool) value)
            {
                curGender = (EnumGender) Enum.Parse(typeof (EnumGender), (string)parameter);
                return curGender;
            }
            return curGender;
        }
    }
}