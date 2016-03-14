namespace Models
{
    /// <summary>
    /// 未清机的信息。
    /// </summary>
    public class NoClearMachineInfo
    {
        /// <summary>
        /// 用户姓名。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 机器标识。（MAC）
        /// </summary>
        public string MachineFlag { get; set; }
    }
}