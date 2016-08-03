using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
    public class UsePreferentialResponse:NewHttpBaseResponse
    {
         public string amount { set; get; }

         public string payamount { set; get; }
         public string freeamount { set; get; }

         public List<PreferentialList> ordePreferential { set; get; }
    }

    public class PreferentialList
    {
            public string id { set; get; }
            public string deAmount { set; get; }
            public string activity { set; get; }
            public string name { set; get; }
     
    }
}
