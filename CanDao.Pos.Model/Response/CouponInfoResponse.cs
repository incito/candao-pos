namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取优惠信息返回类。
    /// </summary>
    public class CouponInfoResponse
    {
        /// <summary>
        /// 优惠活动名称。
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 优惠券类型。
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 优惠活动分类名。
        /// </summary>
        public string type_name { get; set; }

        /// <summary>
        /// 子类型编号。
        /// </summary>
        public int? sub_type { get; set; }

        /// <summary>
        /// 子类型名称。
        /// </summary>
        public string sub_type_name { get; set; }

        /// <summary>
        /// 券的ID。
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 券面金额（代金券）、抵用金额（团购券）
        /// </summary>
        public decimal? amount { get; set; }

        /// <summary>
        /// 团购券销售金额。
        /// </summary>
        public decimal? bill_amount { get; set; }

        /// <summary>
        /// 折扣（折扣券和内部优免）。
        /// </summary>
        public decimal? discount { get; set; }

        /// <summary>
        /// 适用菜品id。为空则适用所有。
        /// </summary>
        public string dish { get; set; }

        /// <summary>
        /// 特价菜品名称。
        /// </summary>
        public string dish_title { get; set; }

        /// <summary>
        /// 特价菜品计量单位。
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// 特价菜品价格。
        /// </summary>
        public decimal? price { get; set; }

        /// <summary>
        /// 优惠券颜色。
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 序号。
        /// </summary>
        public int? sequence { get; set; }

        /// <summary>
        /// 内部优免单位名称。
        /// </summary>
        public string company_name { get; set; }

        /// <summary>
        /// 内部优免单位名称首字母。
        /// </summary>
        public string company_first_letter { get; set; }

        /// <summary>
        /// 优惠券主键ID。
        /// </summary>
        public string preferential { get; set; }

        /// <summary>
        /// 内部优免是否可挂账。
        /// </summary>
        public string can_credit { get; set; }

        /// <summary>
        /// 优免原因。
        /// </summary>
        public string free_reason { get; set; }
    }
}