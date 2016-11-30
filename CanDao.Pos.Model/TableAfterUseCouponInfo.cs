using System.Collections.Generic;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 使用优惠券以后返回的餐桌信息。
    /// </summary>
    public class TableAfterUseCouponInfo : TableServiceChargePartInfo
    {
        /// <summary>
        /// 增加的优惠券集合。
        /// </summary>
        public List<UsedCouponInfo> AddedCouponInfos { get; set; }
    }
}