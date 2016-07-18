namespace CanDao.Pos.Model
{
    /// <summary>
    /// 用户权限。
    /// </summary>
    public class UserRight
    {
        /// <summary>
        /// 允许登录。
        /// </summary>
        public bool AllowLogin { get; set; }

        /// <summary>
        /// 允许开业。
        /// </summary>
        public bool AllowOpening { get; set; }

        /// <summary>
        /// 允许反结算。
        /// </summary>
        public bool AllowAntiSettlement { get; set; }

        /// <summary>
        /// 允许清机。
        /// </summary>
        public bool AllowClearn { get; set; }

        /// <summary>
        /// 允许结业。
        /// </summary>
        public bool AllowEndWork { get; set; }

        /// <summary>
        /// 允许收银。
        /// </summary>
        public bool AllowCash { get; set; }

        /// <summary>
        /// 从目标对象复制数据。
        /// </summary>
        /// <param name="srcObj"></param>
        public void CloneDataFrom(UserRight srcObj)
        {
            if (srcObj == null)
                return;

            AllowAntiSettlement = srcObj.AllowAntiSettlement;
            AllowCash = srcObj.AllowCash;
            AllowClearn = srcObj.AllowClearn;
            AllowLogin = srcObj.AllowLogin;
            AllowOpening = srcObj.AllowOpening;
            AllowEndWork = srcObj.AllowEndWork;
        }
    }
}