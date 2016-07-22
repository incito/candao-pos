using System;
using CanDao.Pos.Common;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    /// <summary>
    /// 菜品称重窗口VM。
    /// </summary>
    public class DishWeightWndVm : BaseViewModel
    {
        public DishWeightWindow OwnerWindow { get; set; }

        private string _dishWeightText;

        /// <summary>
        /// 菜品重量文本。
        /// </summary>
        public string DishWeightText
        {
            get { return _dishWeightText; }
            set
            {
                _dishWeightText = value;
                if (string.IsNullOrEmpty(value))
                {
                    DishWeight = 0;
                    OwnerWindow.CloseBtnEnable = false;
                }
                else
                {
                    DishWeight = Convert.ToDecimal(value);
                    OwnerWindow.CloseBtnEnable = true;
                }
            }
        }

        public decimal DishWeight { get; set; }

    }
}