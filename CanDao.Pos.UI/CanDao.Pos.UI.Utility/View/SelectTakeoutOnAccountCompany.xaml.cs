using CanDao.Pos.Model;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 选择外卖挂单窗口。
    /// </summary>
    public partial class SelectTakeoutOnAccountCompany
    {
        public SelectTakeoutOnAccountCompany()
        {
            InitializeComponent();
            DataContext = new SelectTakeoutOnAccountCompanyVm { OwnerWindow = this };
        }

        /// <summary>
        /// 获取挂账单位信息。
        /// </summary>
        public OnCompanyAccountInfo OnAccountInfo
        {
            get { return ((SelectTakeoutOnAccountCompanyVm)DataContext).OnAccountInfo; }
        }

        /// <summary>
        /// 联系电话。
        /// </summary>
        public string ContactMobile
        {
            get
            {
                var mobile = ((SelectTakeoutOnAccountCompanyVm) DataContext).ContactMobile;
                return string.IsNullOrWhiteSpace(mobile) ? " " : mobile;//没有输入的时候要求一个空格占位符，在拼URL的时候用。
            }
        }

        /// <summary>
        /// 联系人。
        /// </summary>
        public string ContactName
        {
            get
            {
                var name = ((SelectTakeoutOnAccountCompanyVm)DataContext).ContactName;
                return string.IsNullOrWhiteSpace(name) ? " " : name;//没有输入的时候要求一个空格占位符，在拼URL的时候用。
            }
        }
    }
}
