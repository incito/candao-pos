using System.Collections.Generic;
using System.Linq;
using CanDao.Pos.UI.Library.Model;
using CanDao.Pos.UI.Library.ViewModel;
using DevExpress.Data.Browsing;
using Models;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// 设置菜品口味和忌口窗口。
    /// </summary>
    public partial class SetDishTasteAndDietWindow
    {
        public SetDishTasteAndDietWindow(List<string> tasteList, DishSimpleInfo dishSimpleInfo)
        {
            InitializeComponent();
            var vm = new SetDishTasteAndDietWndVm(dishSimpleInfo) { OwnerWindow = this };
            if (tasteList != null)
            {
                TasteSetCtrl.TasteInfos = tasteList.Select(t => new TasteInfo { TasteTitle = t }).ToList();
                vm.HasTasteInfos = tasteList.Any();
            }
            DataContext = vm;
        }

        /// <summary>
        /// 忌口。
        /// </summary>
        public string Diet
        {
            get { return DietSetCtrl.Diet; }
        }

        /// <summary>
        /// 口味。
        /// </summary>
        public string Taste
        {
            get { return TasteSetCtrl.SelectedTaste; }
        }

        public int DishNum
        {
            get { return ((SetDishTasteAndDietWndVm)DataContext).DishNum; }
        }
    }
}
