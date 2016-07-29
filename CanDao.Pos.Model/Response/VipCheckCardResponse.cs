using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
   /// <summary>
   /// 会员检查实体卡是否存在返回类
   /// </summary>
   public class VipCheckCardResponse
    {
        /// <summary>
        /// 返回Code
        /// </summary>
       public int code { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
       public string msg { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        public bool IsSuccess
        {
            get { return code == 0; }
        }
    }
}
