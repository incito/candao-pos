using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using CanDao.Pos.UI.Library.Model;
using CanDao.Pos.UI.Library.ViewModel;

namespace CanDao.Pos.UI.Library.View
{
    /// <summary>
    /// 允许多选控件。
    /// </summary>
    public partial class AllowMultSelectorControl
    {
        #region Fields

        private AllowMultSelectorCtrlVm _vm;

        #endregion

        #region Constructor

        public AllowMultSelectorControl()
        {
            InitializeComponent();
            _vm = new AllowMultSelectorCtrlVm();
            DataContext = _vm;
        }

        #endregion

        #region Properties

        #region Source

        /// <summary>
        /// 数据源。
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable Source
        {
            get { return (IEnumerable)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IEnumerable), typeof(AllowMultSelectorControl), new PropertyMetadata(null, SourceProperty_Changed));

        private static void SourceProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (AllowMultSelectorControl)d;
            ctrl.InitSource(ctrl.Source as IEnumerable<AllowSelectInfo>);
        }

        private void InitSource(IEnumerable<AllowSelectInfo> source)
        {
            _vm.ItemSource.Clear();
            if (source != null)
            {
                foreach (var item in source)
                {
                    _vm.ItemSource.Add(item);
                }
            }
        }
        #endregion

        #region Title

        /// <summary>
        /// 标题。
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(AllowMultSelectorControl), new PropertyMetadata(TitleProperty_Changed));

        private static void TitleProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (AllowMultSelectorControl)d;
            ctrl._vm.Title = ctrl.Title;
        }

        #endregion

        #region Separator

        /// <summary>
        /// 分割符。
        /// </summary>
        public string Separator
        {
            get { return (string)GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }

        public static readonly DependencyProperty SeparatorProperty =
            DependencyProperty.Register("Separator", typeof(string), typeof(AllowMultSelectorControl), new PropertyMetadata(";"));

        #endregion

        /// <summary>
        /// 选择的信息。
        /// </summary>
        public string SelectInfo
        {
            get
            {
                var selectedInfo = _vm.ItemSource.Where(t => t.IsSelected).Select(y => y.Name).ToList();
                if (!string.IsNullOrEmpty(_vm.OtherInfo))
                    selectedInfo.Add(_vm.OtherInfo);
                return string.Join(Separator, selectedInfo);
            }
        }

        #endregion
    }
}
