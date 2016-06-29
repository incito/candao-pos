using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CanDaoCD.Pos.ResourceService.Controls
{
   public class CtlImageButton:Button
    {
               public static readonly DependencyProperty DefaultImageProperty = DependencyProperty.Register(
            "DefaultImage", typeof(BitmapImage), typeof(CtlImageButton));
        /// <summary>
        /// 默认文本（未选中）
        /// </summary>
        public BitmapImage DefaultImage
        {
            get { return (BitmapImage)GetValue(DefaultImageProperty); }
            set { SetValue(DefaultImageProperty, value); }
        }

        static CtlImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CtlImageButton), new FrameworkPropertyMetadata(typeof(CtlImageButton)));
        }
    }
}
