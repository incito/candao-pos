using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Library.Model;
using WebServiceReference;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// 赠菜原因选择窗口。
    /// </summary>
    public partial class DishGiftReasonSelectWindow
    {
        public DishGiftReasonSelectWindow()
        {
            InitializeComponent();
            DataContext = new NormalWindowViewModel { OwnerWindow = this };
            if (RestClient.DishGiftReasonList != null)
                SelectorCtrl.ItemsSource = RestClient.DishGiftReasonList.Select(t => new AllowSelectInfo { Name = t }).ToList();
        }

        /// <summary>
        /// 选择的原因。
        /// </summary>
        public string SelectedReason
        {
            get { return SelectorCtrl.SelectedInfo; }
        }
    }
}
