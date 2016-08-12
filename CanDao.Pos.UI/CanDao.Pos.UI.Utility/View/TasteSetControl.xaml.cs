using System.Collections.Generic;
using CanDao.Pos.UI.Utility.Controls;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 口味设置控件。
    /// </summary>
    public partial class TasteSetControl
    {
        private List<string> _tasteInfos;

        public TasteSetControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取或设置口味集合。
        /// </summary>
        public List<string> TasteInfos
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
            get { return ((TasteSetControlVm)DataContext).SelectedTaste; }
            set { ((TasteSetControlVm) DataContext).SelectedTaste = value; }
        }
    }
}
