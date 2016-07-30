using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
   public class LoginInfoResponse
    {
        /// <summary>
        /// 登录时间。
        /// </summary>
        public string loginTime { get; set; }


        /// <summary>
        /// 用户名称。非登录账户。
        /// </summary>
        public string fullname { get; set; }
    }
}
