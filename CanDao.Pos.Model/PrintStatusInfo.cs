using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    /// <summary>
    /// 打印机状态信息。
    /// </summary>
    public class PrintStatusInfo
    {
        /// <summary>
        /// 打印机IP。
        /// </summary>
        public string PrintIp { get; set; }

        /// <summary>
        /// 打印机名称。
        /// </summary>
        public string PrintName { get; set; }

        /// <summary>
        /// 打印机状态。
        /// </summary>
        public EnumPrintStatus PrintStatus { get; set; }

        /// <summary>
        /// 打印机状态描述。
        /// </summary>
        public string PrintStatusDes { get; set; }
    }
}