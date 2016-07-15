using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using CanDao.Pos.UI.Library.Model;
using Common;
using DevExpress.Xpf.Editors;

namespace CanDao.Pos.UI.Library.View
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

        public string NullText
        {
            get { return (string)GetValue(NullTextProperty); }
            set { SetValue(NullTextProperty, value); }
        }

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

        public string OtherInfo
        {
            get { return (string)GetValue(OtherInfoProperty); }
            set { SetValue(OtherInfoProperty, value); }
        }

        public static readonly DependencyProperty OtherInfoProperty =
            DependencyProperty.Register("OtherInfo", typeof(string), typeof(AllowMultSelectorControl), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnOtherInfoChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        private static void OnOtherInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (AllowMultSelectorControl) d;
            AllLog.Instance.E(ctrl.ToString());
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var tb = GetTemplateChild("PART_TbOtherInfo") as TextEdit;
            if(tb != null)
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
}
