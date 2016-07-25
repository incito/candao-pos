using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 调用会员接口返回基础类
    /// </summary>
   public class CanDaoVipBaseResponse
    {
       /// <summary>
       /// 返回Code
       /// </summary>
       public int retcode { get; set; }

       /// <summary>
       /// 返回信息
       /// </summary>
       public string retInfo { get; set; }

       /// <summary>
       /// 操作结果
       /// </summary>
        public bool IsSuccess
        {
            get { return retcode == 0; }
        }
    }
}
