using System.Collections.Generic;
using CanDao.Pos.UI.Library.Model;
using CanDao.Pos.UI.Library.ViewModel;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// 口味设置控件。
    /// </summary>
    public partial class TasteSetControl
    {
        private List<TasteInfo> _tasteInfos;

        public TasteSetControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取或设置口味集合。
        /// </summary>
        public List<TasteInfo> TasteInfos
        {
            get { return _tasteInfos; }
            set
            {
                _tasteInfos = value;
                ((TasteSetControlVm)DataContext).InitDishTasteInfos(value);
            }
        }

        /// <summary>
        /// 选择的口味。
        /// </summary>
        public string SelectedTaste
        {
            get
            {
                return ((TasteSetControlVm)DataContext).SelectedTaste;
            }
        }
    }
}
