namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 餐道会员接口基类。
    /// </summary>
    public class CanDaoMemberBaseResponse
    {
        public int Retcode { get; set; }

        public string RetInfo { get; set; }

        public bool IsSuccess
        {
            get { return Retcode == 0; }
        }
    }
}