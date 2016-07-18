namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 授权类型枚举。
    /// </summary>
    public enum EnumRightType
    {
        /// <summary>
        /// 保留字段。
        /// </summary>
        None = 0,
        /// <summary>
        /// 登陆。
        /// </summary>
        Login = 30201,
        /// <summary>
        /// 开业。
        /// </summary>
        Opening = 30202,
        /// <summary>
        /// 反结算。
        /// </summary>
        AntiSettlement = 30203,
        /// <summary>
        /// 清机。
        /// </summary>
        Clearner = 30204,
        /// <summary>
        /// 结业。
        /// </summary>
        EndWork = 30205,
        /// <summary>
        /// 收银。
        /// </summary>
        Cash = 30206,
        /// <summary>
        /// 赠菜。
        /// </summary>
        FreeDish = 30207,
        /// <summary>
        /// 退菜。
        /// </summary>
        BackDish = 30102,
    }
}