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
using CanDaoCD.Pos.Common.Models;

namespace CanDaoCD.Pos.Common.Controls
{
    /// <summary>
    /// UcPageList.xaml 的交互逻辑
    /// </summary>
    public partial class UcPageList : UserControl
    {

        #region 字段

        private int _page = 1;
        private int Total;
        private int _pageSize = 1;
        private bool _isLoad=false;
        #endregion

        #region 构造函数
        public UcPageList()
        {
            InitializeComponent();
            ListShow = new ObservableCollection<MListBoxInfo>();
            this.Loaded+=UcPageList_Loaded;
            this.DataContext = this;
           
        }

        #endregion

        #region 属性

        /// <summary>
        /// 显示项
        /// </summary>
        public ObservableCollection<MListBoxInfo> ListShow
        {
            get { return (ObservableCollection<MListBoxInfo>)GetValue(ListShowProperty); }
            set
            {
                SetValue(ListShowProperty, value);
            }
        }
        public static readonly DependencyProperty ListShowProperty =
          DependencyProperty.Register("ListShow", typeof(ObservableCollection<MListBoxInfo>), typeof(UcPageList),
              new PropertyMetadata(new ObservableCollection<MListBoxInfo>()));
        /// <summary>
        /// 数据集
        /// </summary>

        public List<MListBoxInfo> ListData
        {
            get { return (List<MListBoxInfo>)GetValue(ListDataProperty); }
            set
            {
                SetValue(ListDataProperty, value);
            }
        }

        public static readonly DependencyProperty ListDataProperty =
            DependencyProperty.Register("ListData", typeof(List<MListBoxInfo>), typeof(UcPageList), new PropertyMetadata(ListValueChanged));

        public static void ListValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageList = d as UcPageList;
            if (pageList == null)
            {
                return;
            }
            if (pageList.ListData != null)
            {
                if (pageList.ListData.Count > 0)
                {
                    pageList.SetPageInfo();
                }

            }
        }
        /// <summary>
        /// 当前选择优惠变化事件
        /// </summary>
        public Action<MListBoxInfo> SelectChangeAction
        {
            get { return (Action<MListBoxInfo>)GetValue(SelectChangeActionProperty); }
            set
            {
                SetValue(SelectChangeActionProperty, value);
            }
        }
        public static readonly DependencyProperty SelectChangeActionProperty =
          DependencyProperty.Register("SelectChangeAction", typeof(Action<MListBoxInfo>), typeof(UcPageList));
        #endregion

        #region 事件

        #endregion

        #region 私有方法
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UcPageList_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoad = true;
            SetPageInfo();
            LbData.SelectionChanged += LbData_SelectionChanged;
        }

        void LbData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MListBoxInfo item;
            if (LbData.SelectedItem == null)
            {
                TexSelect.Text = string.Empty;
                item = null;
            }
            else
            {
                item = LbData.SelectedItem as MListBoxInfo;
                TexSelect.Text = item.Title;
            }
            if (SelectChangeAction != null)
            {
                SelectChangeAction(item);
            }
        }
        /// <summary>
        /// 设置翻页数据
        /// </summary>
        private void SetPageInfo()
        {
            if (_isLoad && ListData!=null)
            {
                int rowNum = (int)LbData.ActualWidth / 70;
                rowNum = rowNum > 0 ? rowNum : 1;
                int colNum = (int)LbData.ActualHeight / 70;
                colNum = colNum > 0 ? colNum : 1;
                _page = 1;
                _pageSize = ListData.Count / (rowNum * colNum);
                _pageSize = _pageSize > 0 ? _pageSize : 1;
                ContentShow();
            }
          
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        private void ContentShow()
        {
            try
            {
                ListShow = new ObservableCollection<MListBoxInfo>(ListData.Take(_pageSize * _page).Skip(_pageSize * (_page - 1)));
                if (ListData.Count % _pageSize == 0)
                {
                    Total = ListData.Count / _pageSize;
                }
                else
                {
                    Total = ListData.Count / _pageSize + 1;
                }
                BtnTrunUp.Visibility = _page > 1 ? Visibility.Visible : Visibility.Hidden;
                BtnTrunDown.Visibility = _page < Total ? Visibility.Visible : Visibility.Hidden;
            }
            catch
            {
               
            }
            
        }

        private void BtnTrunUp_OnClick(object sender, RoutedEventArgs e)
        {
            _page--;
            ContentShow();
        }

        private void BtnTrunDown_OnClick(object sender, RoutedEventArgs e)
        {
            _page++;
            ContentShow();
        }

        #endregion

    }
}
