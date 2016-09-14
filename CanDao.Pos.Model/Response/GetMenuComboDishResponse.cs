using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取套餐包含菜品的返回类。
    /// </summary>
    public class GetMenuComboDishResponse : NewHttpBaseRowsResponse<MenuComboDishMainResponse>
    {
    }

    /// <summary>
    /// 获取淘宝包含菜品的主体类。
    /// </summary>
    public class MenuComboDishMainResponse
    {
        public string dishid { get; set; }

        public List<MenuSingleDishDataResponse> only { get; set; }

        public List<MenuComboDishDataResponse> combo { get; set; }
    }

    /// <summary>
    /// 单个菜品的数据类。
    /// </summary>
    public class MenuSingleDishDataResponse
    {
        public decimal? vipprice { get; set; }

        public int status { get; set; }

        public string ordernum { get; set; }

        /// <summary>
        /// 菜品名称。
        /// </summary>
        public string contactdishname { get; set; }

        public string id { get; set; }

        public decimal? weigh { get; set; }

        public decimal price { get; set; }

        public string contactdishid { get; set; }

        public string dishid { get; set; }

        public string dishunitid { get; set; }

        public string groupid { get; set; }

        public int ispot { get; set; }

        public int dishtype { get; set; }

        public string insertuserid { get; set; }

        public int dishnum { get; set; }
    }

    public class MenuComboDishDataResponse
    {
        /// <summary>
        /// 备选个数。
        /// </summary>
        public int startnum { get; set; }

        /// <summary>
        /// 选择个数。
        /// </summary>
        public int endnum { get; set; }

        /// <summary>
        /// 状态。（没用）
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 分类名称。
        /// </summary>
        public string columnname { get; set; }

        /// <summary>
        /// 排序顺序。
        /// </summary>
        public int ordernum { get; set; }

        /// <summary>
        /// 分类id。
        /// </summary>
        public string columnid { get; set; }

        public string id { get; set; }

        public string dishid { get; set; }

        public string inserttime { get; set; }

        public string itemDesc { get; set; }

        public List<MenuSingleAllDishDataResponse> alldishes { get; set; }

        public string insertuserid { get; set; }

        public string selecttype { get; set; }
    }
}