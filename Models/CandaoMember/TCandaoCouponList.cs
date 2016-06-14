using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.CandaoMember
{
    /// <summary>
    /// 优惠列表
    /// </summary>
   public class TCandaoCouponList:TCandaoRetBase
    {
       //优惠券列表
       public List<TCandaoCoupon> Coupons { set; get; }

        public TCandaoCouponList()
        {
            Coupons=new List<TCandaoCoupon>();
        }
    }
}
