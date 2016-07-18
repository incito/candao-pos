using System.Collections.Generic;
using System.Linq;

namespace CanDao.Pos.Model
{
    public class MenuComboDishInfo
    {
        /// <summary>
        /// 源数据个数。
        /// </summary>
        public int SourceCount { get; set; }

        /// <summary>
        /// 选择个数。
        /// </summary>
        public int SelectCount { get; set; }

        /// <summary>
        /// 组合名称。
        /// </summary>
        public string ComboName { get; set; }

        /// <summary>
        /// 组合Id。
        /// </summary>
        public string ComboId { get; set; }

        public string Id { get; set; }

        public string DishId { get; set; }

        /// <summary>
        /// 可选菜品源。
        /// </summary>
        public List<MenuDishInfo> SourceDishes { get; set; }

        /// <summary>
        /// 是否选择完成。
        /// </summary>
        public bool IsSelectedDone
        {
            get { return SourceDishes.Sum(t => t.SelectedCount) >= SelectCount; }
        }
    }
}