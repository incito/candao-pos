using CanDao.Pos.Common;

namespace CanDao.Pos.UI.Library.Model
{
    /// <summary>
    /// 允许被选择的信息基类。
    /// </summary>
    public class AllowSelectInfo : BaseNotifyObject
    {
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