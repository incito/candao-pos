using System.Collections.Generic;

namespace CanDao.Pos.Model.Request
{
    public class OrderDishRequest
    {
        public OrderDishRequest()
        {
            operationType = 1;
            globalsperequire = "";
            sequence = 1;
        }

        public string currenttableid { get; set; }

        public string globalsperequire { get; set; }

        public string orderid { get; set; }

        /// <summary>
        /// 1：下单;2 :退菜 3：并台  4换台。
        /// </summary>
        public int operationType { get; set; }

        /// <summary>
        /// 下单序号。（已停用）
        /// </summary>
        public int sequence { get; set; }

        public List<DishDetailRequest> rows { get; set; }
    }
}