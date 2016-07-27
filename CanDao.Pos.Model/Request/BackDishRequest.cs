using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 退菜请求类。
    /// </summary>
    public class BackDishRequest : BackAllDishRequest
    {
        public BackDishRequest()
        {
            operationType = 2;
            sequence = 999999;
            actionType = ((int) EnumBackDishType.Single).ToString();
        }

        public int operationType { get; set; }
        public string primarykey { get; set; }
        public int sequence { get; set; }
        public string userName { get; set; }
        public string dishunit { get; set; }
        public string dishNum { get; set; }
        public string dishtype { get; set; }
        public string dishNo { get; set; }
    }
}