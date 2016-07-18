using System;
using System.IO;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 银行信息。
    /// </summary>
    public class BankInfo
    {
        private int _id;

        /// <summary>
        /// 编号ID。
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                ImageSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", value.ToString() + ".png");
            }
        }

        /// <summary>
        /// 银行名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序序号。
        /// </summary>
        public int SortIndex { get; set; }

        /// <summary>
        /// 银行图片源。
        /// </summary>
        public string ImageSource { get; set; }
    }
}