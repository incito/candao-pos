using System;
using System.Windows;
using System.Windows.Controls;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common.TemplateSelector
{
    /// <summary>
    /// 付款方式模板选择类。
    /// </summary>
    public class PayWayTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// 普通模板。
        /// </summary>
        public DataTemplate NormalTemplate { get; set; }

        /// <summary>
        /// 现金模板。
        /// </summary>
        public DataTemplate CashTemplate { get; set; }

        /// <summary>
        /// 银行卡模板。
        /// </summary>
        public DataTemplate BankTemplate { get; set; }

        /// <summary>
        /// 会员卡模板。
        /// </summary>
        public DataTemplate MemberTemplate { get; set; }

        /// <summary>
        /// 挂账模板。
        /// </summary>
        public DataTemplate OnAccountTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var parent = ((ContentPresenter)container).Parent;
            var element = parent as FrameworkElement;
            if (element == null)
                return NormalTemplate;

            var data = item as PayWayInfo;
            if (data == null)
                return NormalTemplate;

            switch (data.ItemId)
            {
                case 0://现金。
                    return CashTemplate;
                case 1://银行卡。
                    return BankTemplate;
                case 8://会员卡。
                    return MemberTemplate;
                case 5://挂账。
                    return OnAccountTemplate;
                default:
                    return NormalTemplate;
            }
        }
    }
}