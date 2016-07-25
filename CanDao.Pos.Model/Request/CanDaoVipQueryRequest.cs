using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Request
{
    /// <summary>
    /// 餐道会员查询
    /// </summary>
    public class CanDaoVipQueryRequest : CanDaoMemberBaseRequest
    {
        public string cardno { set; get; }
    }
}
