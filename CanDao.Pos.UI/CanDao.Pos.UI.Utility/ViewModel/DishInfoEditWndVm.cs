using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 菜品信息编辑窗口VM。
    /// </summary>
    public class DishInfoEditWndVm : NormalWindowViewModel<DishInfoEditWindow>
    {
        public DishInfoEditWndVm(string dishName, decimal dishPrice, bool allowEditDishName = false, bool allowEditDishPrice = false)
        {
            DishNum = 1;
            DishName = dishName;
            DishPrice = dishPrice;
            AllowEditDishName = allowEditDishName;
            AllowEditDishPrice = allowEditDishPrice;
            WndTitle = (!allowEditDishName && !allowEditDishPrice) ? "菜品数量设置窗口" : "菜品信息编辑窗口";
        }

        public bool AllowEditDishName { get; set; }

        public bool AllowEditDishPrice { get; set; }

        public string DishName { get; set; }

        public decimal DishPrice { get; set; }

        public decimal DishNum { get; set; }

        public string WndTitle { get; set; }

        protected override bool CanConfirm(object param)
        {
            return DishNum > 0;
        }

        protected override void OnWindowLoaded(object param)
        {
            OwnerWnd.TbDishNum.Focus();
        }
    }
}