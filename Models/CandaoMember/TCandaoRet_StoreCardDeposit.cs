using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.CandaoMember
{
    public class TCandaoRet_StoreCardDeposit:TCandaoRetBase
    {
        string _tracecode;
        decimal _storeCardbalance;
        decimal _giftamount;
        decimal _integral;

        public decimal Integral
        {
            get { return _integral; }
            set { _integral = value; }
        }
        public decimal Giftamount
        {
            get { return _giftamount; }
            set { _giftamount = value; }
        }
        public decimal StoreCardbalance
        {
            get { return _storeCardbalance; }
            set { _storeCardbalance = value; }
        }
        public string Tracecode
        {
            get { return _tracecode; }
            set { _tracecode = value; }
        }
    }
}
