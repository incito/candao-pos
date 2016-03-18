using System.ComponentModel;

namespace Models
{
    /// <summary>
    /// 赠菜信息。
    /// </summary>
    public class GiftDishInfo : INotifyPropertyChanged
    {
        private bool _isSelected;
        private int _selectGiftNum;
        private bool _showSelectString;

        /// <summary>
        /// 是否选中。
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

        /// <summary>
        /// 菜品名称。
        /// </summary>
        public string DishName { get; set; }

        /// <summary>
        /// 菜品价格。
        /// </summary>
        public decimal DishPrice { get; set; }

        /// <summary>
        /// 菜品个数。
        /// </summary>
        public decimal DishNum { get; set; }

        /// <summary>
        /// 总计。
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 选中的赠送个数。
        /// </summary>
        public int SelectGiftNum
        {
            get { return _selectGiftNum; }
            set
            {
                _selectGiftNum = value;
                if (_selectGiftNum > DishNum)
                    _selectGiftNum = 0;

                IsSelected = _selectGiftNum > 0;
                ShowSelectString = _selectGiftNum > 0;
                NumString = string.Format("{0}/{1}", _selectGiftNum, DishNum);
                RaisePropertyChanged("NumString");
            }
        }

        /// <summary>
        /// 是否选择的字符串。
        /// </summary>
        public bool ShowSelectString
        {
            get { return _showSelectString; }
            set
            {
                _showSelectString = value;
                RaisePropertyChanged("ShowSelectString");
            }
        }

        public string NumString { get; set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}