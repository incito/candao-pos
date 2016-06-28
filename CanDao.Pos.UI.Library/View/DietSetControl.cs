using System.Linq;
using System.Windows.Controls;
using CanDao.Pos.UI.Library.Model;
using Common;

namespace CanDao.Pos.UI.Library.View
{
    public class DietSetControl : UserControl
    {
        private AllowMultSelectorControl _allowMultSelectorCtrl;

        public DietSetControl()
        {
            _allowMultSelectorCtrl = new AllowMultSelectorControl();
            _allowMultSelectorCtrl.Title = "忌口";
            if (Globals.DietSetting != null)
                _allowMultSelectorCtrl.Source = Globals.DietSetting.Select(t => new AllowSelectInfo { Name = t }).ToList();
            Content = _allowMultSelectorCtrl;
        }

        /// <summary>
        /// 选择的忌口。
        /// </summary>
        public string Diet
        {
            get { return _allowMultSelectorCtrl.SelectInfo; }
        }
    }
}