using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.Controls;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 口味设置控件VM。
    /// </summary>
    public class TasteSetControlVm : BaseViewModel
    {
        public TasteSetControlVm()
        {
            DishTasteInfos = new ObservableCollection<AllowSelectInfo>();
        }

        /// <summary>
        /// 菜品口味集合。
        /// </summary>
        public ObservableCollection<AllowSelectInfo> DishTasteInfos { get; private set; }

        /// <summary>
        /// 选择的口味。
        /// </summary>
        public string SelectedTaste
        {
            get
            {
                var item = DishTasteInfos.FirstOrDefault(t => t.IsSelected);
                return item != null ? item.Name : null;
            }
        }

        /// <summary>
        /// 初始化口味。
        /// </summary>
        /// <param name="tasteInfos"></param>
        public void InitDishTasteInfos(List<AllowSelectInfo> tasteInfos)
        {
            if (tasteInfos != null)
                tasteInfos.ForEach(DishTasteInfos.Add);
        }
    }
}