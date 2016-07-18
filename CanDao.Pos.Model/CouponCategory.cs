using System.Collections.ObjectModel;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 优惠分类。
    /// </summary>
    public class CouponCategory
    {
        public CouponCategory()
        {
            CouponInfos = new ObservableCollection<CouponInfo>();
        }

        /// <summary>
        /// 优惠集合。
        /// </summary>
        public ObservableCollection<CouponInfo> CouponInfos { get; private set; }

        /// <summary>
        /// 分类名。
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 分类类型。
        /// </summary>
        public string CategoryType { get; set; }
    }
}