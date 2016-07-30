using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 菜单菜品分组信息。
    /// </summary>
    public class MenuDishGroupInfo : BaseNotifyObject
    {
        public MenuDishGroupInfo()
        {
            DishInfos = new List<MenuDishInfo>();
        }

        /// <summary>
        /// 分组名。
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 分组排序号。
        /// </summary>
        public int GroupSortIndex { get; set; }

        /// <summary>
        /// 分组编号。
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 该分组下选中的菜品数量。
        /// </summary>
        private decimal _selectDishCount;
        /// <summary>
        /// 该分组下选中的菜品数量。
        /// </summary>
        public decimal SelectDishCount
        {
            get { return _selectDishCount; }
            set
            {
                _selectDishCount = value;
                RaisePropertyChanged("SelectDishCount");
            }
        }

        /// <summary>
        /// 菜品集合。
        /// </summary>
        public List<MenuDishInfo> DishInfos { get; set; }
    }
}