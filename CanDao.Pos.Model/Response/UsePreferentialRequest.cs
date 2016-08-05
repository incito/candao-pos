using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
    public class UsePreferentialResponse:NewHttpBaseResponse
    {
        public preferentialInfoResponse data { set; get; }
    }

    public class DelePreferentialResponse : NewHttpBaseResponse
    {
        public PreferentialInfo data { set; get; }
    }
    public class PreferentialInfo
    {
        public preferentialInfoResponse preferentialInfo { set; get; }
    }
}
