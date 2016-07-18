namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取菜品分组信息返回类。
    /// </summary>
    public class GetMenuDishGroupInfoResponse : RowsResponse<MenuDishGroupInfoResponse>
    {

    }

    public class MenuDishGroupInfoResponse
    {
        /// <summary>
        /// 菜单分组名。
        /// </summary>
        public string itemdesc { get; set; }
        /// <summary>
        /// 菜单分组排序号。
        /// </summary>
        public int itemsort { get; set; }

        public string itemid { get; set; }
        public string isShow { get; set; }
    }
}