using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 区域信息返回类。
    /// </summary>
    public class AreaInfoResponse
    {
        /// <summary>
        /// 分区排序号。
        /// </summary>
        public int? areaSort { get; set; }

        /// <summary>
        /// 分区id。
        /// </summary>
        public string areaid { get; set; }

        /// <summary>
        /// 分区名称。
        /// </summary>
        public string areaname { get; set; }

        /// <summary>
        /// 分区包含的餐台集合。
        /// </summary>
        public List<TableInfoResponse> tables { get; set; }
    }
}