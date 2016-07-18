using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    public class GetFishPotDishResponse : RestOrderResponse<FishPotDishResponse>
    {

    }

    public class FishPotDishResponse
    {
        /// <summary>
        /// 口味。
        /// </summary>
        public string imagetitle { get; set; }

        public decimal? vipprice { get; set; }

        public string columnid { get; set; }

        public string introduction { get; set; }

        public int dishtype { get; set; }

        public int ispot { get; set; }

        public string image { get; set; }

        public decimal? ordernum { get; set; }

        public int weigh { get; set; }

        public string source { get; set; }

        public string py { get; set; }

        public string content { get; set; }

        public string userid { get; set; }

        public string dishno { get; set; }
        public string dishid { get; set; }

        public string title { get; set; }

        public string unit { get; set; }

        public decimal price { get; set; }
    }
}