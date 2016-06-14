using CanDaoCD.Pos.PrintManage.Operates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CanDao.Pos.SystemConfig.ViewModels;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.Common.Operates;
using CanDaoCD.Pos.Common.Operates.FileOperate;
using CanDaoCD.Pos.Common.PublicValues;
using CanDaoCD.Pos.VIPManage.ViewModels;

namespace WpfTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var view = new MainViewModel();
            this.DataContext = view;
            var data = new List<MListBoxInfo>();
            //for (int i = 1; i < 30; i++)
            //{
            //    data.Add(new MListBoxInfo()
            //    {
            //        Id = i.ToString(),
            //        Title = "test:" + i,
            //        ListData = i
            //    });

            //}
            //PageList.ListData = view.Infos;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //OPrintManage manage=new OPrintManage();
            //manage.Init(3, 9600);
            //manage.CardState();
            //Tex.Focus();
            //KeyBoardPre.SendKey(Key.D0);
            var veModel = new UcVipSelectViewModel();
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }

        private void Tex_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //DigitalKeyboard.CurrentElement = Tex;
        }

        private void BtnConfig_OnClick(object sender, RoutedEventArgs e)
        {
         
            var veModel = new UcCopyPrintViewModel();
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }

        private void BtnVipReg_OnClick(object sender, RoutedEventArgs e)
        {
            var veModel = new UcVipRegViewModel();
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }

        private void BtnSelect_OnClick(object sender, RoutedEventArgs e)
        {
            var veModel = new UcVipSelectViewModel();
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
        }

        private void BtnCz_OnClick(object sender, RoutedEventArgs e)
        {
            var veModel = new UcVipRechargeViewModel();
            veModel.Init();
            OWindowManage.ShowPopupWindow(veModel.GetUserCtl());
       
        }

        private void BtnInit_OnClick(object sender, RoutedEventArgs e)
        {
            PvSystemConfig.VSystemConfig = OXmlOperate.DeserializeFile<MSystemConfig>(PvSystemConfig.VSystemConfigFile);
          
        }
    }
}
