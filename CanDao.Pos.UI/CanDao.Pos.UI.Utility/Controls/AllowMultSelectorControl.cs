using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.Controls
{
    public class AllowMultSelectorControl : Selector
    {
        /// <summary>
        /// 内容项控件模板。
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTemplate ItemViewerTemplate
        {
            get { return (DataTemplate)GetValue(ItemViewerTemplateProperty); }
            set { SetValue(ItemViewerTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemViewerTemplateProperty =
            DependencyProperty.Register("ItemViewerTemplate", typeof(DataTemplate), typeof(AllowMultSelectorControl), new PropertyMetadata(null));

        /// <summary>
        /// 标题提示。
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(AllowMultSelectorControl), new PropertyMetadata("", OnTitleChanged));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (AllowMultSelectorControl)d;
            ctrl.NullText = string.Format("其他{0}", ctrl.Title);
        }

        /// <summary>
        /// 水印提示。
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NullText
        {
            get { return (string)GetValue(NullTextProperty); }
            set { SetValue(NullTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NullText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NullTextProperty =
            DependencyProperty.Register("NullText", typeof(string), typeof(AllowMultSelectorControl), new PropertyMetadata(""));


        /// <summary>
        /// 分隔符。
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Separator
        {
            get { return (string)GetValue(SeparatorProperty); }
            set { SetValue(SeparatorProperty, value); }
        }

        public static readonly DependencyProperty SeparatorProperty =
            DependencyProperty.Register("Separator", typeof(string), typeof(AllowMultSelectorControl), new PropertyMetadata(";"));

        /// <summary>
        /// 其他信息。
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string OtherInfo
        {
            get { return (string)GetValue(OtherInfoProperty); }
            set { SetValue(OtherInfoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OtherInfo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OtherInfoProperty =
            DependencyProperty.Register("OtherInfo", typeof(string), typeof(AllowMultSelectorControl), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, false, UpdateSourceTrigger.PropertyChanged));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var tb = GetTemplateChild("PART_TbOtherInfo") as TextBox;
            if (tb != null)
                tb.MouseDown += TbOnMouseDown;
        }

        private void TbOnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var wnd = new InputMoreInfoWindow(NullText, OtherInfo);
            SoftKeyboardHelper.ShowSoftKeyboard();//系统软键盘。
            if (wnd.ShowDialog() == true)
                OtherInfo = wnd.InputInfo;
            SoftKeyboardHelper.CloseSoftKeyboard();
        }

        /// <summary>
        /// 选择的信息。
        /// </summary>
        public string SelectedInfo
        {
            get
            {
                var selectedInfo = new List<string>();
                var source = ItemsSource as IEnumerable<AllowSelectInfo>;
                if (source != null)
                    selectedInfo = source.Where(t => t.IsSelected).Select(y => y.Name).ToList();
                if (!string.IsNullOrEmpty(OtherInfo))
                    selectedInfo.Add(OtherInfo);
                return string.Join(Separator, selectedInfo);
            }
        }

        static AllowMultSelectorControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AllowMultSelectorControl), new FrameworkPropertyMetadata(typeof(AllowMultSelectorControl)));
        }
    }

    /// <summary>
    /// 允许被选择的信息基类。
    /// </summary>
    public class AllowSelectInfo : BindableBase
    {
        public AllowSelectInfo(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        /// <summary>
        /// 是否被选中。
        /// </summary>
        private bool _isSelected;
        /// <summary>
        /// 是否被选中。
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
    }
}