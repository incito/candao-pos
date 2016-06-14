using CanDaoCD.Pos.Common.Classes.Mvvms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfTest
{
    public class MainModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private bool _isEn;

        public bool IsEn
        {
            get { return _isEn; }
            set
            {
                _isEn = value;
                RaisePropertyChanged(() => IsEn);
            }
        }

        private Action _sureAction;

        public Action SureAction
        {
            get { return _sureAction; }
            set
            {
                _sureAction = value;
                RaisePropertyChanged(() => SureAction);
            } 
        }
    }
}
