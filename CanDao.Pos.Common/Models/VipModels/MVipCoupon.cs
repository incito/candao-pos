using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Common.Models.VipModels
{
    /// <summary>
    /// 优惠券信息
    /// </summary>
    public class MVipCoupon
    {
        private string _id;
        private string _type;
        private string _dealValue;
        private string _presentValu;


        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 充值类型  1=多冲多送
        /// </summary>
        public string CouponType
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 充值
        /// </summary>
        public string DealValue
        {
            set { _dealValue = value; }
            get { return _dealValue; }
        }
        /// <summary>
        /// 赠送
        /// </summary>
        public string PresentValu
        {
            set { _presentValu = value; }
            get { return _presentValu; }
        }
    }
}
