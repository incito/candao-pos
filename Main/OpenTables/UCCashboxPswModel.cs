using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KYPOS.OpenTables
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
