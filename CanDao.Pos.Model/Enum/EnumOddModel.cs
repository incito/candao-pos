namespace CanDao.Pos.Model.Enum
{
    /// <summary>
    /// 零头处理方式枚举。
    /// </summary>
    public enum EnumOddModel
    {
        /// <summary>
        /// 不处理。
        /// </summary>
        None,

        /// <summary>
        /// 四舍五入
        /// </summary>
        Rounding,
        
        /// <summary>
        /// 抹零。
        /// </summary>
        Wipe,
    }
}