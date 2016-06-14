using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CanDaoCD.Pos.Common.Classes.Mvvms;
using CanDaoCD.Pos.Common.Models;

namespace WpfTest
{
    public class MainViewModel : ViewModelBase
    {
        private List<MListBoxInfo> _infos;

        public List<MListBoxInfo> Infos
        {
            set
            {
                _infos = value;
                RaisePropertyChanged(() => Infos);
            }
            get { return _infos; }
        }
         private string _imageString;

        public string ImageString {
            set
            {
                _imageString = value;
                RaisePropertyChanged(() => ImageString);
            }
            get { return _imageString; }
        }
        public MainViewModel()
        {
            Model=new MainModel();
            Model.SureAction = new Action(suerHandel);
            Model.IsEn = false;
          SuerCommand=new RelayCommand(suerHandel);
          var data = new List<MListBoxInfo>();
            for (int i = 1; i < 30; i++)
            {
                data.Add(new MListBoxInfo()
                {
                    Id = i.ToString(),ListData = i,Title = "标题："+i.ToString()
                });

            }

            Infos = data;
            ImageString = "123";
        }

        public RelayCommand SuerCommand { set; get; }

        private void suerHandel()
        {
            Model.Name = "我的神！";
        }

        public MainModel Model { set; get; }
    }
}
