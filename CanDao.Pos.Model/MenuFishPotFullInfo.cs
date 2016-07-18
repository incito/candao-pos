using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 鱼锅全信息。
    /// </summary>
    public class MenuFishPotFullInfo
    {
        /// <summary>
        /// 鱼锅自身信息。
        /// </summary>
        public MenuDishInfo FishPotSelfInfo { get; set; }

        /// <summary>
        /// 锅底信息。
        /// </summary>
        public MenuDishInfo PotInfo { get; set; }

        /// <summary>
        /// 鱼锅鱼信息。
        /// </summary>
        public List<MenuDishInfo> FishDishes { get; set; }
    }
}