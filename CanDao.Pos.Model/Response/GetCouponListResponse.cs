using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 优惠券信息
    /// </summary>
    public class CouponResponse
    {
        public string id { set; get; }
        public string type { set; get; }
        public string dealValue { set; get; }
        public string presentValue { set; get; }
    }

    /// <summary>
    /// 优惠券列表返回值
    /// </summary>
    public class GetCouponListResponse
    {
        public List<CouponResponse> datas { set; get; }
    }
}
