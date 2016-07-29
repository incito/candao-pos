using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 优惠类型选择窗口。
    /// </summary>
    public partial class OfferTypeSelectWindow
    {
        public OfferTypeSelectWindow(EnumOfferType allowOffType)
        {
            InitializeComponent();
            DataContext = new OfferTypeSelectWndVm(allowOffType) { OwnerWindow = this };
        }

        /// <summary>
        /// 选中的优惠类型，优免或折扣。
        /// </summary>
        public EnumOfferType SelectedOfferType
        {
            get { return ((OfferTypeSelectWndVm)DataContext).SelectedOfferType; }
        }

        /// <summary>
        /// 输入的折扣率。
        /// </summary>
        public decimal Discount
        {
            get { return ((OfferTypeSelectWndVm)DataContext).Discount / 10; }//输入的折扣数字要除以10.
        }

        /// <summary>
        /// 输入的优免金额。
        /// </summary>
        public decimal Amount
        {
            get { return ((OfferTypeSelectWndVm)DataContext).Amount; }
        }

        protected override void CloseBtnOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var msg = ((OfferTypeSelectWndVm)DataContext).CheckInputValid();
            if (!string.IsNullOrEmpty(msg))
            {
                MessageDialog.Warning(msg);
                return;
            }

            base.CloseBtnOnClick(sender, routedEventArgs);
        }
    }
}
