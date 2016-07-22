using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CanDao.Pos.Common.Controls
{
    /// <summary>
    /// 分组选择控件。
    /// </summary>
    [TemplatePart(Name = PART_Selector, Type = typeof(Selector))]
    public class GroupSelector : Selector
    {
        #region Fields

        /// <summary>
        /// ListBox模板的名称。
        /// </summary>
        internal const string PART_Selector = "PART_Selector";

        /// <summary>
        /// 选择控件。
        /// </summary>
        internal Selector _selectorControl;

        #endregion

        #region Constructors

        static GroupSelector()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupSelector), new FrameworkPropertyMetadata(typeof(GroupSelector)));
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取是否可以跳转到第一组。
        /// </summary>
        public Boolean CanFirstGruop
        {
            get { return HasItems && GroupCount > 1 && SelectedGroupIndex != 0; }
        }

        /// <summary>
        /// 获取是否可以跳转到最后一组。
        /// </summary>
        public Boolean CanLastGroup
        {
            get { return HasItems && GroupCount > 1 && (SelectedGroupIndex != (GroupCount - 1)); }
        }

        /// <summary>
        /// 获取是否可以跳转到下一组。
        /// </summary>
        public Boolean CanNextGruop
        {
            get { return HasItems && GroupCount > 1 && (AllowGroupSwitch || SelectedGroupIndex < GroupCount - 1); }
        }

        /// <summary>
        /// 获取是否可以跳转到上一组。
        /// </summary>
        public Boolean CanPreviousGroup
        {
            get { return HasItems && GroupCount > 1 && (AllowGroupSwitch || SelectedGroupIndex > 0); }
        }

        #endregion

        #region Dependency Properties

        #region AllowGroupSwitch

        /// <summary>
        /// 获取或设置允许在分组之间进行切换。
        /// 即当在这一组最后一个元素时选择下一个，如果值为true，则跳转到下一组的第一个元素，否则在本组内切换。
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Boolean AllowGroupSwitch
        {
            get { return (Boolean)GetValue(AllowGroupSwitchProperty); }
            set { SetValue(AllowGroupSwitchProperty, value); }
        }

        public static readonly DependencyProperty AllowGroupSwitchProperty = DependencyProperty.Register(
            "AllowGroupSwitch",
            typeof(Boolean),
            typeof(GroupSelector),
            new PropertyMetadata(false, AllowGroupSwitch_PropertyChanged));

        private static void AllowGroupSwitch_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GroupSelector)d).OnAllowGroupSwitchPropertyChanged((Boolean)e.OldValue, (Boolean)e.NewValue);
        }

        protected virtual void OnAllowGroupSwitchPropertyChanged(Boolean oldValue, Boolean newValue)
        {

        }

        #endregion

        #region ItemCountEachGroup

        /// <summary>
        /// 获取或设置每组元素个数，该值用来确定分组数据源的元素个数。
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 ItemCountEachGroup
        {
            get { return (Int32)GetValue(ItemCountEachGroupProperty); }
            set { SetValue(ItemCountEachGroupProperty, value); }
        }

        public static readonly DependencyProperty ItemCountEachGroupProperty = DependencyProperty.Register(
            "ItemCountEachGroup",
            typeof(Int32),
            typeof(GroupSelector),
            new PropertyMetadata(4, ItemCountEachGroup_PropertyChanged));

        private static void ItemCountEachGroup_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GroupSelector)d).OnItemCountEachGroupPropertyChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual void OnItemCountEachGroupPropertyChanged(Int32 oldValue, Int32 newValue)
        {
            if (ItemsSource != null)
            {
                GroupCount = GetGruopCount();
                GroupedItemsSource = GetGroupItemsSource(0);
            }
        }

        #endregion

        #region GroupCount

        /// <summary>
        /// 获取分组个数。
        /// </summary>
        public int GroupCount
        {
            get { return (int)GetValue(GroupCountProperty); }
            set { SetValue(GroupCountProperty, value); }
        }

        public static readonly DependencyProperty GroupCountProperty =
            DependencyProperty.Register("GroupCount", typeof(int), typeof(GroupSelector), new PropertyMetadata(0));

        #endregion

        #region GroupedItemsSource

        /// <summary>
        /// 获取或设置用于生成 Engine.Wpf.Toolkit.GroupSelector 的分组内容的集合。
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IEnumerable GroupedItemsSource
        {
            get { return (IEnumerable)GetValue(GroupedItemsSourceProperty); }
            set { SetValue(GroupedItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty GroupedItemsSourceProperty = DependencyProperty.Register(
            "GroupedItemsSource",
            typeof(IEnumerable),
            typeof(GroupSelector),
            new PropertyMetadata(null, GroupedItemsSource_PropertyChanged));

        private static void GroupedItemsSource_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GroupSelector)d).OnGroupedItemsSourcePropertyChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
        }

        protected virtual void OnGroupedItemsSourcePropertyChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            if (_selectorControl != null)
            {
                if (newValue != null)
                    SelectedIndex = GetCurrentGroupMinIndex();
            }
        }

        #endregion

        #region SelectedGroupIndex

        /// <summary>
        /// 获取或设置当前选择的分组索引。
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Int32 SelectedGroupIndex
        {
            get { return (Int32)GetValue(SelectedGroupIndexProperty); }
            set { SetValue(SelectedGroupIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedGroupIndexProperty = DependencyProperty.Register(
            "SelectedGroupIndex",
            typeof(Int32),
            typeof(GroupSelector),
            new PropertyMetadata(-1, SelectedGroupIndex_PropertyChanged, Coerce_SelectedGroupIndex));

        private static object Coerce_SelectedGroupIndex(DependencyObject d, object baseValue)
        {
            Int32 newValue = Convert.ToInt32(baseValue);
            return Math.Min(((GroupSelector)d).GroupCount - 1, Math.Max(0, newValue));//最小值为0,最大值为分组最大数。
        }

        private static void SelectedGroupIndex_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GroupSelector)d).OnSelectedGroupIndexPropertyChanged((Int32)e.OldValue, (Int32)e.NewValue);
        }

        protected virtual void OnSelectedGroupIndexPropertyChanged(Int32 oldValue, Int32 newValue)
        {
            if (ItemsSource == null)
                return;

            GroupedItemsSource = GetGroupItemsSource(newValue);
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// 跳转到第一组。
        /// </summary>
        /// <returns></returns>
        public virtual Boolean FirstGroup()
        {
            if (!CanFirstGruop)
                return false;

            SelectedGroupIndex = 0;
            return true;
        }

        /// <summary>
        /// 跳转到最后一个分组。
        /// </summary>
        /// <returns></returns>
        public virtual Boolean LastGroup()
        {
            if (!CanLastGroup)
                return false;

            SelectedGroupIndex = GroupCount - 1;
            return true;
        }

        /// <summary>
        /// 跳转到下一个分组。
        /// </summary>
        /// <returns></returns>
        public virtual Boolean NextGroup()
        {
            if (!CanNextGruop)
                return false;

            SelectedGroupIndex = (SelectedGroupIndex + 1) % GroupCount;
            return true;
        }

        /// <summary>
        /// 跳转到前一个分组。
        /// </summary>
        /// <returns></returns>
        public virtual Boolean PreviousGroup()
        {
            if (!CanPreviousGroup)
                return false;

            if (AllowGroupSwitch && SelectedGroupIndex == 0)
                SelectedGroupIndex = GroupCount - 1;
            else
                SelectedGroupIndex--;

            return true;
        }

        /// <summary>
        /// 跳转到第一项。
        /// 如果允许切换分组，则最后一项是整个集合的第一项，如果不允许切换分组，则最后一项是当前分组的第一项。
        /// </summary>
        /// <returns></returns>
        public Boolean FirstItem()
        {
            if (!CanFirstItem)
                return false;

            if (AllowGroupSwitch)
            {
                SelectedIndex = 0;
                SelectedGroupIndex = 0;
            }
            else
            {
                SelectedIndex = GetCurrentGroupMinIndex();
            }

            return true;
        }

        /// <summary>
        /// 跳转到最后一项。
        /// 如果允许切换分组，则最后一项是整个集合的最后一项，如果不允许切换分组，则最后一项是当前分组的最后一项。
        /// </summary>
        /// <returns></returns>
        public Boolean LastItem()
        {
            if (!CanLastItem)
                return false;

            if (AllowGroupSwitch)
            {
                SelectedIndex = Items.Count - 1;
                SelectedGroupIndex = GroupCount - 1;
            }
            else
            {
                SelectedIndex = GetCurrentGroupMaxIndex();
            }

            return true;
        }

        /// <summary>
        /// 跳转到下一项。
        /// 如果允许切换分组且当前选中项是本组的最后一项，则跳转到下一个分组的第一项。
        /// 如果不允许切换分组但是允许循环切换，则如果当前项是本组的最后一项，则跳转到本组的第一项。
        /// </summary>
        /// <returns></returns>
        public Boolean NextItem()
        {
            if (!CanNextItem)
                return false;

            Int32 maxIndex = GetCurrentGroupMaxIndex();
            if (AllowGroupSwitch)
            {
                if (SelectedIndex == maxIndex)//达到了当前分组的最后一个，允许切换分组则切换到下一分组。
                {
                    if (GroupCount > 1)
                    {
                        SelectedGroupIndex = (SelectedGroupIndex + 1) % GroupCount;
                        return true;
                    }
                }
            }

            SelectedIndex = (SelectedIndex + 1) % Items.Count;

            return true;
        }

        /// <summary>
        /// 跳转到前一项。
        /// 如果允许切换分组且当前选中项是本组的第一项，则跳转到上一个分组的最后一项。
        /// 如果不允许切换分组但是允许循环切换，则如果当前想是本组的第一项，则跳转到本组的最后一项。
        /// </summary>
        /// <returns></returns>
        public Boolean PreviousItem()
        {
            if (!CanPreviousItem)
                return false;

            Int32 minIndex = GetCurrentGroupMinIndex();
            if (AllowGroupSwitch)
            {
                if (SelectedIndex == minIndex)//达到了当前分组的第一个，允许切换分组则切换到前一个分组。
                {
                    SelectedGroupIndex = SelectedGroupIndex == 0 ? GroupCount - 1 : SelectedGroupIndex--;
                    SelectedIndex = Items.Count - 1;
                    return true;
                }
            }

            SelectedIndex--;

            return true;
        }

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
            get { return HasItems && SelectedIndex < Items.Count - 1; }
        }

        /// <summary>
        /// 获取是否可以跳转到前一项。
        /// </summary>
        public virtual Boolean CanPreviousItem
        {
            get { return HasItems && SelectedIndex > 0; }
        }

        #endregion

        #region Base Class Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _selectorControl = GetTemplateChild(PART_Selector) as Selector;
            if (_selectorControl != null)
            {
                if (_selectorControl.HasItems)
                    _selectorControl.SelectedIndex = 0;

                _selectorControl.SelectionChanged += Selector_SelectionChanged;
            }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (newValue != null)
            {
                var temp = newValue as INotifyCollectionChanged;
                if (temp != null)
                    temp.CollectionChanged += TempOnCollectionChanged;
                GroupCount = GetGruopCount();
                SelectedGroupIndex = 0;
            }
        }

        private void TempOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            GroupCount = GetGruopCount();
            SelectedGroupIndex = 0;
            GroupedItemsSource = GetGroupItemsSource(SelectedGroupIndex);
        }

        /// <summary>
        /// Selector选择项改变时执行。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((Selector)sender).SelectedItem != null)
                SelectedItem = ((Selector)sender).SelectedItem;
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Private Methods

        /// <summary>
        /// 获取分组数目。
        /// </summary>
        /// <returns></returns>
        private Int32 GetGruopCount()
        {
            return (Items.Count + ItemCountEachGroup - 1) / ItemCountEachGroup;
        }

        /// <summary>
        /// 获取当前分组的最大序号。
        /// </summary>
        /// <returns></returns>
        private Int32 GetCurrentGroupMaxIndex()
        {
            if (_selectorControl == null || !_selectorControl.HasItems)
                return 0;

            return Math.Max(-1, SelectedGroupIndex * ItemCountEachGroup + _selectorControl.Items.Count - 1);
        }

        /// <summary>
        /// 获取当前分组的最小序号。
        /// </summary>
        /// <returns></returns>
        private Int32 GetCurrentGroupMinIndex()
        {
            if (_selectorControl == null || !_selectorControl.HasItems)
                return 0;

            return Math.Max(-1, SelectedGroupIndex * ItemCountEachGroup);
        }

        /// <summary>
        /// 获取分组数据源。
        /// </summary>
        /// <param name="selectedGroupIndex"></param>
        /// <returns></returns>
        private IEnumerable GetGroupItemsSource(Int32 selectedGroupIndex)
        {
            List<Object> groupItemsSource = new List<object>();

            Int32 startIndex = selectedGroupIndex * ItemCountEachGroup;
            if (startIndex < 0)
                return groupItemsSource;

            Int32 index = 0;
            foreach (var item in ItemsSource)
            {
                if (index++ < startIndex)
                    continue;

                if (groupItemsSource.Count >= ItemCountEachGroup)
                    break;

                groupItemsSource.Add(item);
            }

            return groupItemsSource;
        }
        #endregion
    }
}
