using System;
using System.Windows;
using System.Windows.Controls;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using DevExpress.Xpf.Grid;

namespace CanDao.Pos.Common.TemplateSelector
{
    /// <summary>
    /// 菜品小计模板选择类。
    /// </summary>
    public class DishPayamountTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// 正常模式。
        /// </summary>
        public DataTemplate NormalTemplate { get; set; }

        /// <summary>
        /// 待称重模式。
        /// </summary>
        public DataTemplate ToBeWeighTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var parent = ((ContentPresenter) container).Parent;
            var element = parent as FrameworkElement;
            if(element == null)
                return NormalTemplate;

            var data = element.DataContext as OrderDishInfo;
            if (data == null)
                return NormalTemplate;

            var dishStatus = data.DishStatus;
            switch (dishStatus)
            {
                case EnumDishStatus.Normal:
                    return NormalTemplate;
                case EnumDishStatus.ToBeWeighed:
                    return ToBeWeighTemplate;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}