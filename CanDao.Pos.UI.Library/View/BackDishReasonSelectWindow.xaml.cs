using System.Linq;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Library.Model;
using WebServiceReference;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// 退菜原因选择窗口。
    /// </summary>
    public partial class BackDishReasonSelectWindow
    {
        public BackDishReasonSelectWindow()
        {
            InitializeComponent();
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
            if (RestClient.DishGiftReasonList != null)
                SelectorCtrl.Source = RestClient.BackDishReasonList.Select(t => new AllowSelectInfo { Name = t }).ToList();
        }

        /// <summary>
        /// 选择的原因。
        /// </summary>
        public string SelectedReason
        {
            get { return SelectorCtrl.SelectInfo; }
        }
    }
}
