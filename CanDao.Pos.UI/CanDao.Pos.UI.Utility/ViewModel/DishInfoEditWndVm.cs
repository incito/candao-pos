using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 菜品信息编辑窗口VM。
    /// </summary>
    public class DishInfoEditWndVm : NormalWindowViewModel
    {
        public DishInfoEditWndVm()
        {
            DishNum = 1;
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
    }
}