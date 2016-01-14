using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.CandaoMember
{
    public class TCandaoRet_CardLose:TCandaoRetBase
    {
        decimal _storeCardbalance;
        decimal _integraloverall;
        decimal _couponsoverall;

        public decimal StoreCardbalance
        {
            get { return _storeCardbalance; }
            set { _storeCardbalance = value; }
        }

        public decimal Integraloverall
        {
            get { return _integraloverall; }
            set { _integraloverall = value; }
        }

        public decimal Couponsoverall
        {
            get { return _couponsoverall; }
            set { _couponsoverall = value; }
        }

    }
}
