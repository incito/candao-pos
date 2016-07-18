namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 下单接口结果枚举。
    /// </summary>
    public enum EnumBookOrderResult
    {
        /// <summary>
        /// 下单成功。
        /// </summary>
        Success = 0,

        /// <summary>
        /// 菜谱正在更新。
        /// </summary>
        MenuUpdating = 1,

        /// <summary>
        /// 菜品正在更新。
        /// </summary>
        DishUpdating = 2,

        /// <summary>
        /// 其他错误。
        /// </summary>
        OtherError = 3,
    }
}