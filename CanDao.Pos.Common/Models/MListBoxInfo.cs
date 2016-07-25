using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDao.Pos.Common.Classes.Mvvms;

namespace CanDao.Pos.Common.Models
{
    /// <summary>
    /// 列表框实体
    /// </summary>
    public class MListBoxInfo : ViewModelBase
    {
        #region 字段

        private string _id;
        private string _title;
        private object _listData;

        #endregion

        #region 属性

        /// <summary>
        /// ID
        /// </summary>
        public string Id
        {
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
            get { return _id; }
        }

        /// <summary>
        /// 显示标题
        /// </summary>
        public string Title
        {
            set
            {
                _title = value;
                RaisePropertyChanged(() => Title);
            }
            get { return _title; }
        }

        /// <summary>
        /// 数据体
        /// </summary>
        public object ListData
        {
            set
            {
                _listData = value;
                RaisePropertyChanged(() => ListData);
            }
            get { return _listData; }
        }

        #endregion
    }
}
