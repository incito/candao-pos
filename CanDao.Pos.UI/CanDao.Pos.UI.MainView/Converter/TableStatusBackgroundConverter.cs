using System;
using System.Windows.Data;
using System.Windows.Media;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.UI.MainView.Converter
{
    /// <summary>
    /// 餐台状态与背景色的转换。
    /// </summary>
    public class TableStatusBackgroundConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            EnumTableStatus status = (EnumTableStatus)value;
            switch (status)
            {
                case EnumTableStatus.Idle:
                    return Brushes.ForestGreen;
                case EnumTableStatus.Dinner:
                    return Brushes.Coral;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}