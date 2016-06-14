using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CanDaoCD.Pos.Common.Models;
using CanDaoCD.Pos.ResourceService.Controls;

namespace WpfTest
{
    public class ImageButton : Button
    {
        public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register(
     "DefaultImage", typeof(BitmapImage), typeof(ImageButton));
        /// <summary>
        /// 默认文本（未选中）
        /// </summary>
        public BitmapImage DefaultImage
        {
            get { return (BitmapImage)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        public static readonly DependencyProperty DefaultListProperty = DependencyProperty.Register(
     "DefaultList", typeof(List<MListBoxInfo>), typeof(ImageButton), new PropertyMetadata(StringValueChanged));
        /// <summary>
        /// 默认文本（未选中）
        /// </summary>
        public List<MListBoxInfo> DefaultList
        {
            get { return (List<MListBoxInfo>)GetValue(DefaultListProperty); }
            set { SetValue(DefaultListProperty, value); }
        }
        public static readonly DependencyProperty DefaultStringProperty = DependencyProperty.Register(
  "DefaultString", typeof(string), typeof(ImageButton));
        /// <summary>
        /// 默认文本（未选中）
        /// </summary>
        public string DefaultString
        {
            get { return (string)GetValue(DefaultStringProperty); }
            set { SetValue(DefaultStringProperty, value); }
        }

        public static void StringValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
             var ss=   (List<MListBoxInfo>) e.NewValue;
            }
        }

        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }
    }
}
