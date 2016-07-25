using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Common.Models
{
    [Serializable]
    public class MSystemConfig
    {
        private int _serialNum=1;

        private bool _isEnabledPrint;

        private bool _isEnabledCheck;
        /// <summary>
        /// 串口号
        /// </summary>
        public int SerialNum
        {
            set
            {
                _serialNum = value;
            }
            get
            {
                return _serialNum;

            }
        }
        /// <summary>
        /// 是否启用打印
        /// </summary>
        public bool IsEnabledPrint
        {
            set
            {
                _isEnabledPrint = value;
            }
            get
            {
                return _isEnabledPrint;

            }
        }
        /// <summary>
        /// 是否启用钱箱权限验证
        /// </summary>
        public bool IsEnabledCheck
        {
            set { _isEnabledCheck = value; }
            get { return _isEnabledCheck; }
        }
    }
}
