using CanDao.Pos.UI.Utility.ViewModel;


namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// UcBusinessDataDetailsView.xaml 的交互逻辑
    /// </summary>
    public partial class UcBusinessDataDetailsView
    {
        public UcBusinessDataDetailsView()
        {
            InitializeComponent();
            DataContext=new UcBusinessDataDetailsViewModel();
        }
    }
}
