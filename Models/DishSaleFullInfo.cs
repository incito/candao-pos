using System;
using System.Collections.Generic;

namespace Models
{
    public class DishSaleFullInfo
    {
        public List<DishSaleInfo> DishSaleInfos { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string BranchId { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime CurrentTime { get; set; }

    }
}