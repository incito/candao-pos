using Models.Enum;

namespace Models
{
    public class BackAllDishInfo
    {
        public EnumBackDishType BackDishType { get; set; }

        public string TableNo { get; set; }

        public string OrderId { get; set; }

        public string DiscardReason { get; set; }

        public string UserName { get; set; }

    }
}