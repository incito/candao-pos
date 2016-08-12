using System.Collections.Generic;
using System.Linq;
using CanDao.Pos.Model;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 设置口味和忌口的窗口。
    /// </summary>
    public partial class SetDishTasteAndDietWindow
    {
        public SetDishTasteAndDietWindow(List<string> tasteList, DishSimpleInfo dishSimpleInfo, bool allowInputDishNum)
        {
            InitializeComponent();
            var vm = new SetDishTasteAndDietWndVm(dishSimpleInfo, allowInputDishNum) { OwnerWindow = this };
            if (tasteList != null)
                TasteSetCtrl.TasteInfos = tasteList.ToList();

            if (!string.IsNullOrWhiteSpace(dishSimpleInfo.Taste))
                TasteSetCtrl.SelectedTaste = dishSimpleInfo.Taste;

            if (!string.IsNullOrWhiteSpace(dishSimpleInfo.Diet))
                DietSetCtrl.SetInitValue(dishSimpleInfo.Diet);

            DataContext = vm;
        }

        /// <summary>
        /// 获取设置的忌口。
        /// </summary>
        public string SelectedDiet
        {
            get { return DietSetCtrl.SelectedInfo; }
        }

        /// <summary>
        /// 获取设置的口味。
        /// </summary>
        public string SelectedTaste
        {
            get { return TasteSetCtrl.SelectedTaste; }
        }

        /// <summary>
        /// 获取设定的菜品数量。
        /// </summary>
        public int DishNum
        {
            get { return (int)((SetDishTasteAndDietWndVm)DataContext).DishNum; }
        }
    }
}
