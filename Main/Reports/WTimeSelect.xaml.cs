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
using System.Windows.Shapes;

namespace KYPOS.Reports
{
    /// <summary>
    /// WTimeSelect.xaml 的交互逻辑
    /// </summary>
    public partial class WTimeSelect : Window
    {

        #region 事件

        public Action<DateTime> SelectTimeAction;

        #endregion

        #region 构造函数

        public WTimeSelect()
        {
            InitializeComponent();
            CldDate.SelectedDate = DateTime.Now;
            CldDate.DisplayDateEnd=DateTime.Now;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCancel_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSure_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectTimeAction != null)
            {
                var datetime = (DateTime) CldDate.SelectedDate;
                var time = TimeSelect.GetData();
                var selectDate = new DateTime(datetime.Year, datetime.Month, datetime.Day, time[0], time[1], time[2]);
                SelectTimeAction(selectDate);
            }
            this.DialogResult = true;
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="dateTime"></param>
        public void Init(DateTime dateTime)
        {
            CldDate.SelectedDate = dateTime;
            CldDate.DisplayDate = dateTime;
           
            TimeSelect.Init(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        #endregion
    }
}