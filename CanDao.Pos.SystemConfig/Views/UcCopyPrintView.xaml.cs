using CanDaoCD.Pos.Common.Controls.CSystem;
using CanDaoCD.Pos.Common.PublicValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.Common.Operates.FileOperate;

namespace CanDao.Pos.SystemConfig.Views
{
    /// <summary>
    /// UcCopyPrintView.xaml 的交互逻辑
    /// </summary>
    public partial class UcCopyPrintView : UserControlBase
    {
        public UcCopyPrintView()
        {
            InitializeComponent();
           
        }

#region 事件

        public Action EnterAction;
#endregion
  
        private void CkBullet_OnUnchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                PvSystemConfig.VSystemConfig.IsEnabledPrint = false;
                OXmlOperate.SerializerFile<MSystemConfig>(PvSystemConfig.VSystemConfig, PvSystemConfig.VSystemConfigFile);
            }
            catch
            {
               
            }
         
        }

        private void CkBullet_OnChecked(object sender, RoutedEventArgs e)
        {
            TexNum.Focus();
        }

        private void TexNum_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (EnterAction != null)
                {
                    EnterAction();
                }
            }
            else
            {
                var num = 1;
                if (int.TryParse(TexNum.Text, out num))
                {
                    if (num > 0 & num < 257)
                    {
                        
                    }
                    else
                    {
                        TexNum.Text = "1";
                    }
                }
                else
                {
                    TexNum.Text = "1";
                }
            }
        }
    }
}
