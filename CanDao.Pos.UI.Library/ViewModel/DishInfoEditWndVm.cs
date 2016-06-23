using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Library.ViewModel
{
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