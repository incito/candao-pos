using System.Linq;
using CanDao.Pos.UI.Library.ViewModel;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// 忌口设置控件。
    /// </summary>
    public partial class DietSetControl
    {
        public DietSetControl()
        {
            InitializeComponent();
        }

        public string Diet
        {
            get
            {
                var vm = ((DietSetControlVm)DataContext);
                var selectedDiet = vm.DietInfos.Where(t => t.IsSelected).Select(y => y.DietName).ToList();
                if (!string.IsNullOrEmpty(vm.OtherDiet))
                    selectedDiet.Add(vm.OtherDiet);
                return string.Join(";", selectedDiet);
            }
        }
    }
}
