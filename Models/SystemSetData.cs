using Models.Enum;

namespace Models
{
    /// <summary>
    /// 系统设置数据。
    /// </summary>
    public class SystemSetData
    {
        public string Id { get; set; }

        public bool IsEnable { get; set; }

        public string Value { get; set; }

        public string ItemSort { get; set; }

        public string TypeName { get; set; }

        public string ItemDesc { get; set; }

        public int ItemId { get; set; }

        public EnumSystemDataType Type { get; set; }
    }
}