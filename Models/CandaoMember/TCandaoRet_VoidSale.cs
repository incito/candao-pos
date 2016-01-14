using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.CandaoMember
{
    public class TCandaoRet_VoidSale : TCandaoRetBase
    {
        string tracecode;
        decimal storeCardbalance;
        decimal integraloverall;
        decimal integral;
        decimal store;
        decimal useintegral;

        public decimal Useintegral
        {
            get { return useintegral; }
            set { useintegral = value; }
        }
        public decimal Store
        {
            get { return store; }
            set { store = value; }
        }
        public decimal Integral
        {
            get { return integral; }
            set { integral = value; }
        }
        public decimal Integraloverall
        {
            get { return integraloverall; }
            set { integraloverall = value; }
        }
        public decimal StoreCardbalance
        {
            get { return storeCardbalance; }
            set { storeCardbalance = value; }
        }
        public string Tracecode
        {
            get { return tracecode; }
            set { tracecode = value; }
        }

    }
}
