using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 套餐全信息。
    /// </summary>
    public class MenuComboFullInfo
    {
        /// <summary>
        /// 套餐自身信息。
        /// </summary>
        public MenuDishInfo ComboSelfInfo { get; set; }

        /// <summary>
        /// 套餐内单品信息集合。
        /// </summary>
        public List<MenuDishInfo> SingleDishInfos { get; set; }

        /// <summary>
        /// 套餐内组合信息集合。
        /// </summary>
        public List<MenuComboDishInfo> ComboDishInfos { get; set; }
    }
}