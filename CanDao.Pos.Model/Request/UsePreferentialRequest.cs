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
        public string toalFreeAmount { set; get; }
        public string toalDebitAmount { set; get; }
        public string toalDebitAmountMany { set; get; }
        public string adjAmout { set; get; }
        public string preferentialNum { set; get; }
        public string preferentialAmout { set; get; }
        public string isCustom { set; get; }

        public string dishid { set; get; }
    }
}
