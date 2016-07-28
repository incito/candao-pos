
using CanDao.Pos.Common.Classes.Mvvms;

namespace CanDao.Pos.UI.Utility.Model
{
    public class UCCashboxPswModel : ViewModelBase
    {

        private string _password;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
            get { return _password; }
        }
    }
}
