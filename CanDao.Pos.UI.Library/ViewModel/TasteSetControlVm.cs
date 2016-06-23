using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Library.Model;

namespace CanDao.Pos.UI.Library.ViewModel
{
    public class TasteSetControlVm : BaseViewModel
    {
        public TasteSetControlVm()
        {
            DishTasteInfos = new ObservableCollection<TasteInfo>();
        }

        /// <summary>
        /// 菜品口味集合。
        /// </summary>
        public ObservableCollection<TasteInfo> DishTasteInfos { get; private set; }

        /// <summary>
        /// 选择的口味。
        /// </summary>
        public string SelectedTaste
        {
            get
            {
                var item = DishTasteInfos.FirstOrDefault(t => t.IsSelected);
                return item != null ? item.TasteTitle : null;
            }
        }

        /// <summary>
        /// 初始化口味。
        /// </summary>
        /// <param name="tasteInfos"></param>
        public void InitDishTasteInfos(List<TasteInfo> tasteInfos)
        {
            if (tasteInfos != null)
                tasteInfos.ForEach(DishTasteInfos.Add);
        }
    }
}