using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CanDao.Pos.ResourceService.BindConverter
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    internal class BoolToVisibilityConverter : IValueConverter
    {

        #region IValueConverter Members


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && targetType == typeof(Visibility))
            {
                bool val = (bool)value;
                if (val)
                    return Visibility.Visible;
                else
                    if (parameter != null && parameter is Visibility)
                        return parameter;
                    else
                        return Visibility.Collapsed;
            }
            if (value == null)
            {
                if (parameter != null && parameter is Visibility)
                    return parameter;
                else
                    return Visibility.Collapsed;
            }

            return Visibility.Visible;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility && targetType == typeof(bool))
            {
                Visibility val = (Visibility)value;
                if (val == Visibility.Visible)
                    return true;
                else
                    return false;
            }
            throw new ArgumentException("Invalid argument/return type. Expected argument: Visibility and return type: bool");
        }
        #endregion
    } 
}
