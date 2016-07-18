using System;
using System.Collections;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace CanDao.Pos.Common.Controls
{
    /// <summary>
    /// 可供在指定元素集合中切换显示的控件。
    /// </summary>
    public class SwitchItemViewer : Selector, IDisposable
    {
        #region Fields

        /// <summary>
        /// 自动切换定时器
        /// </summary>
        private Timer autoSwitchTimer;

        /// <summary>
        /// 是否循环切换属性状态备份。
        /// 当设置自动切换时存储当前是否循环切换状态，当取消自动切换时还原是否循环切换属性状态值。
        /// </summary>
        private Boolean allowCycleSwitchStateBackup;

        /// <summary>
        /// 改变状态是因为自动切换属性导致的标识。
        /// 如果为true，则表示是否循环切换属性的改变是因为自动切换属性状态的改变引起的。否则是外部赋值，需要引起相关变化。
        /// </summary>
        private Boolean changeStateByAutoFlag;

        #endregion

        #region Constructors

        /// <summary>
        /// 实例化一个 Engine.Wpf.Toolkit.SwitchItemViewer 可供在指定元素集合中切换显示的控件新实例。
        /// </summary>
        public SwitchItemViewer()
        {
            InitAutoSwitchTimer();
        }

        static SwitchItemViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchItemViewer), new FrameworkPropertyMetadata(typeof(SwitchItemViewer)));
        }

        #endregion

        #region Dependency Properties

        #region ItemViewerTemplate

        /// <summary>
        /// 获取或设置显示的图像模板。
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTemplate ItemViewerTemplate
        {
            get { return (DataTemplate)GetValue(ItemViewerTemplateProperty); }
            set { SetValue(ItemViewerTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemViewerTemplateProperty = DependencyProperty.Register(
            "ItemViewerTemplate",
            typeof(DataTemplate),
            typeof(SwitchItemViewer),
            new PropertyMetadata(null));

        #endregion

        #region AllowCycleSwitch

        /// <summary>
        /// 获取或设置是否允许循环切换。
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean AllowCycleSwitch
        {
            get { return (Boolean)GetValue(AllowCycleSwitchProperty); }
            set { SetValue(AllowCycleSwitchProperty, value); }
        }

        public static readonly DependencyProperty AllowCycleSwitchProperty = DependencyProperty.Register(
            "AllowCycleSwitch",
            typeof(Boolean),
            typeof(SwitchItemViewer),
            new PropertyMetadata(false, AllowCycleSwitch_PropertyChanged));

        private static void AllowCycleSwitch_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SwitchItemViewer)d).OnAllowCycleSwitchPropertyChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual void OnAllowCycleSwitchPropertyChanged(Boolean oldValue, Boolean newValue)
        {
            if (!this.changeStateByAutoFlag)
                this.allowCycleSwitchStateBackup = newValue;
        }

        #endregion

        #region AutoSwitch

        /// <summary>
        /// 获取或设置是否自动切换。
        /// 如果设定了自动切换，则默认启动循环切换方式。
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean AutoSwitch
        {
            get { return (Boolean)GetValue(AutoSwitchProperty); }
            set { SetValue(AutoSwitchProperty, value); }
        }

        public static readonly DependencyProperty AutoSwitchProperty = DependencyProperty.Register(
            "AutoSwitch",
            typeof(Boolean),
            typeof(SwitchItemViewer),
            new PropertyMetadata(false, AutoSwitch_PropertyChanged));

        private static void AutoSwitch_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SwitchItemViewer)d).OnAutoSwitchPropertyChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual void OnAutoSwitchPropertyChanged(Boolean oldValue, Boolean newValue)
        {
            this.changeStateByAutoFlag = true;
            AllowCycleSwitch = newValue || this.allowCycleSwitchStateBackup;
            if (newValue)
                autoSwitchTimer.Start();
            else
                autoSwitchTimer.Stop();
            this.changeStateByAutoFlag = false;
        }

        #endregion

        #region AutoSwitchTimeSpan

        /// <summary>
        /// 获取或设置自动切换时间间隔，以秒为单位。
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 AutoSwitchTimeSpan
        {
            get { return (Int32)GetValue(AutoSwitchTimeSpanProperty); }
            set { SetValue(AutoSwitchTimeSpanProperty, value); }
        }

        public static readonly DependencyProperty AutoSwitchTimeSpanProperty = DependencyProperty.Register(
            "AutoSwitchTimeSpan",
            typeof(Int32),
            typeof(SwitchItemViewer),
            new PropertyMetadata(3, AutoSwitchTimeSpan_PropertyChanged));

        private static void AutoSwitchTimeSpan_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SwitchItemViewer)d).OnAutoSwitchTimeSpan((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual void OnAutoSwitchTimeSpan(Int32 oldValue, Int32 newValue)
        {
            autoSwitchTimer.Interval = newValue * 1000;
        }

        #endregion

        #region StopAutoSwitchWhenMouseOver

        /// <summary>
        /// 获取或设置当鼠标位于控件上时是否停止自动切换。
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean StopAutoSwitchWhenMouseOver
        {
            get { return (Boolean)GetValue(StopAutoSwitchWhenMouseOverProperty); }
            set { SetValue(StopAutoSwitchWhenMouseOverProperty, value); }
        }

        public static readonly DependencyProperty StopAutoSwitchWhenMouseOverProperty = DependencyProperty.Register(
            "StopAutoSwitchWhenMouseOver",
            typeof(Boolean),
            typeof(SwitchItemViewer),
            new PropertyMetadata(true, StopAutoSwitchWhenMouseOver_PropertyChanged));

        private static void StopAutoSwitchWhenMouseOver_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SwitchItemViewer)d).OnStopAutoSwitchWhenMouseOver((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual void OnStopAutoSwitchWhenMouseOver(Boolean oldValue, Boolean newValue) { }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// 获取是否可以跳转到第一项。
        /// </summary>
        public virtual Boolean CanFirstItem
        {
            get { return HasItems && SelectedIndex != 0; }
        }

        /// <summary>
        /// 获取是否可以跳转到最后一项。
        /// </summary>
        public virtual Boolean CanLastItem
        {
            get { return HasItems && (SelectedIndex != (Items.Count - 1)); }
        }

        /// <summary>
        /// 获取是否可以跳转到下一项。
        /// </summary>
        public virtual Boolean CanNextItem
        {
            get { return HasItems && (AllowCycleSwitch || SelectedIndex < Items.Count - 1); }
        }

        /// <summary>
        /// 获取是否可以跳转到前一项。
        /// </summary>
        public virtual Boolean CanPreviousItem
        {
            get { return HasItems && (AllowCycleSwitch || SelectedIndex > 0); }
        }

        #endregion

        #region Event Handler

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            SelectedIndex = newValue != null ? 0 : -1;
        }

        /// <summary>
        /// 自动切换定时器间隔时间触发时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoSwitchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate
            {
                if (!HasItems) return;

                if (StopAutoSwitchWhenMouseOver && IsMouseOver) return;

                NextItem();
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 跳转到第一项。
        /// </summary>
        /// <returns></returns>
        public virtual Boolean FirstItem()
        {
            if (!CanFirstItem)
                return false;

            SelectedIndex = 0;
            return true;
        }

        /// <summary>
        /// 跳转到最后一项。
        /// </summary>
        /// <returns></returns>
        public virtual Boolean LastItem()
        {
            if (!CanLastItem)
                return false;

            SelectedIndex = Items.Count - 1;
            return true;
        }

        /// <summary>
        /// 跳转到下一项。
        /// </summary>
        public virtual Boolean NextItem()
        {
            if (!CanNextItem)
                return false;

            SelectedIndex = (SelectedIndex + 1) % Items.Count;
            return true;
        }

        /// <summary>
        /// 跳转到前一项。
        /// </summary>
        public virtual Boolean PreviousItem()
        {
            if (!CanPreviousItem)
                return false;

            if (AllowCycleSwitch && SelectedIndex == 0)
                SelectedIndex = Items.Count - 1;
            else
                SelectedIndex--;

            return true;
        }

        /// <summary>
        /// 启动定时切换。
        /// </summary>
        /// <returns></returns>
        public void StartAutoSwitch()
        {
            AutoSwitch = true;
        }

        /// <summary>
        /// 停止自动切换。
        /// </summary>
        public void StopAutoSwitch()
        {
            AutoSwitch = false;
        }

        /// <summary>
        /// 释放定时器资源。
        /// </summary>
        public void Dispose()
        {
            UninitAutoSwitchTimer();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 初始化定时切换定时器。
        /// </summary>
        private void InitAutoSwitchTimer()
        {
            autoSwitchTimer = new Timer(AutoSwitchTimeSpan * 1000);
            autoSwitchTimer.Elapsed += autoSwitchTimer_Elapsed;
            autoSwitchTimer.AutoReset = true;
        }

        /// <summary>
        /// 反初始化定时切换定时器。
        /// </summary>
        private void UninitAutoSwitchTimer()
        {
            if (autoSwitchTimer != null)
            {
                autoSwitchTimer.Elapsed -= autoSwitchTimer_Elapsed;
                autoSwitchTimer.Enabled = false;
                autoSwitchTimer.Dispose();
                autoSwitchTimer = null;
            }
        }

        #endregion
    }
}