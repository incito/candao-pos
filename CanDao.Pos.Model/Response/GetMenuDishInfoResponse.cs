namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取所有菜品信息的返回类。
    /// </summary>
    public class GetMenuDishInfoResponse : RestOrderResponse<MenuDishInfoResponse>
    {

    }

    /// <summary>
    /// 单个菜品信息的返回类。
    /// </summary>
    public class MenuDishInfoResponse
    {
        public string imagetitle { get; set; }

        /// <summary>
        /// 会员价。
        /// </summary>
        public decimal? vipprice { get; set; }

        /// <summary>
        /// 菜谱编号。
        /// </summary>
        public string menuid { get; set; }

        public string columnid { get; set; }

        public string introduction { get; set; }

        public int? level { get; set; }

        /// <summary>
        /// 菜品类型。0：普通；1、鱼锅；2、套餐。
        /// </summary>
        public int dishtype { get; set; }

        public string image { get; set; }

        public int? ordernum { get; set; }

        public int weigh { get; set; }

        /// <summary>
        /// 分类编号。
        /// </summary>
        public string source { get; set; }

        public string py { get; set; }

        public string content { get; set; }

        public string userid { get; set; }

        public string dishno { get; set; }

        public string dishid { get; set; }

        public string title { get; set; }
        public string unit { get; set; }
        public decimal? price { get; set; }
    }
}