using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Request
{
    public class UsePreferentialRequest
    {
        public string orderid { set; get; }
        public string preferentialid { set; get; }
        public decimal disrate { set; get; }
        public string type { set; get; }
        public string sub_type { set; get; }
        public string preferentialAmt { set; get; }
        public string preferentialNum { set; get; }
        public string preferentialAmout { set; get; }
        public string isCustom { set; get; }

        public string dishname { set; get; }
    }
}
