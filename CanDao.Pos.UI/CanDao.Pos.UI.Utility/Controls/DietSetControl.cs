using System.Linq;
using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Utility.Controls
{
    /// <summary>
    /// 忌口设置控件。
    /// </summary>
    public class DietSetControl : AllowMultSelectorControl
    {
        public DietSetControl()
        {
            Title = "忌口";
            if (Globals.DietSetting != null && Globals.DietSetting.Any())
                ItemsSource = Globals.DietSetting.Select(t => new AllowSelectInfo(t)).ToList();
        }
    }
}