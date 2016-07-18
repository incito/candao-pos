using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    public class BackDishInfo
    {
        public bool IsPot { get; set; }

        public bool IsMaster { get; set; }

        public int ChildDishType { get; set; }

        public EnumDishType DishType { get; set; }

        public string PrimaryKey { get; set; }

        public string DishId { get; set; }

        public string DishUnit { get; set; }
    }
}