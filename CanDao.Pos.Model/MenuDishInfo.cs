using System.Collections.Generic;
using System.Windows.Input;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 菜谱菜品信息。
    /// </summary>
    public class MenuDishInfo : BaseNotifyObject
    {
        /// <summary>
        /// 菜品编号。（当是套餐内的菜品时，这个代表套餐这个菜的DishId）
        /// </summary>
        public string DishId { get; set; }

        /// <summary>
        /// 菜品名称。
        /// </summary>
        public string DishName { get; set; }

        /// <summary>
        /// 菜谱编号。
        /// </summary>
        public string Menuid { get; set; }

        /// <summary>
        /// 分类编号。对应source。
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// 首字母。
        /// </summary>
        public string FirstLetter { get; set; }

        /// <summary>
        /// 菜品口味集合。
        /// </summary>
        public List<string> Tastes { get; set; }

        /// <summary>
        /// 菜品类型。
        /// </summary>
        public EnumDishType DishType { get; set; }

        /// <summary>
        /// 鱼锅类型。
        /// </summary>
        public EnumFishPotType FishPotType { get; set; }

        /// <summary>
        /// 需要称重。
        /// </summary>
        public bool NeedWeigh { get; set; }

        public decimal? VipPrice { get; set; }

        public decimal? Price { get; set; }

        /// <summary>
        /// 单位。
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 处理前原始单位。（For 国际化）
        /// </summary>
        public string SrcUnit { get; set; }

        /// <summary>
        /// 选择的个数。
        /// </summary>
        private int _selectedCount;
        /// <summary>
        /// 选择的个数。
        /// </summary>
        public int SelectedCount
        {
            get { return _selectedCount; }
            set
            {
                if (_selectedCount != value)
                {
                    _selectedCount = value;
                    RaisePropertyChanged("SelectedCount");
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }

        /// <summary>
        /// 个数（套餐时用）
        /// </summary>
        public int DishCount { get; set; }

        /// <summary>
        /// 是否是锅底。
        /// </summary>
        public bool IsPot { get; set; }
    }
}