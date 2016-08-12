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
            DishTasteInfos = new ObservableCollection<string>();
        }

        /// <summary>
        /// 菜品口味集合。
        /// </summary>
        public ObservableCollection<string> DishTasteInfos { get; private set; }

        /// <summary>
        /// 选择的口味。
        /// </summary>
        private string _selectedTaste;
        /// <summary>
        /// 选择的口味。
        /// </summary>
        public string SelectedTaste
        {
            get { return _selectedTaste; }
            set
            {
                _selectedTaste = value;
                RaisePropertyChanged("SelectedTaste");
            }
        }

        /// <summary>
        /// 初始化口味。
        /// </summary>
        /// <param name="tasteInfos"></param>
        public void InitDishTasteInfos(List<string> tasteInfos)
        {
            if (tasteInfos != null)
                tasteInfos.ForEach(DishTasteInfos.Add);
        }
    }
}