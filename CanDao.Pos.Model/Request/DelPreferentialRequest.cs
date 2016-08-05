using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Request
{
   public class DelPreferentialRequest
    {
       public string orderid { set; get; }
       /// <summary>
       /// 优惠关系ID
       /// </summary>
       public string DetalPreferentiald { set; get; }
       /// <summary>
       /// 是否全部清除（0：否1：是）默认0
       /// </summary>
       public int clear { set; get; }
    }
}
