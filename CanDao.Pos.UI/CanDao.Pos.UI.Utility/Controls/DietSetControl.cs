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

        /// <summary>
        /// 初始化已经设置的忌口信息。
        /// </summary>
        /// <param name="diet"></param>
        public void SetInitValue(string diet)
        {
            if (string.IsNullOrWhiteSpace(diet))
                return;

            var values = diet.Split(Separator.ToCharArray()).ToList();
            foreach (AllowSelectInfo item in ItemsSource)
            {
                if (values.Contains(item.Name))
                {
                    values.Remove(item.Name);
                    item.IsSelected = true;
                }
            }

            if (values.Any())
                OtherInfo = string.Join(Separator, values);
        }
    }
}