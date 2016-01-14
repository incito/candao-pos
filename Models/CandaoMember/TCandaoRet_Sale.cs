using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TCandaoRet_Sale :TCandaoRetBase
    {
        string tracecode;
        decimal storecardbalance;
        decimal integraloverall;
        decimal addintegral;
        decimal decintegral;
        decimal inflatedrate;
        decimal netamount;

        public string Tracecode
        {
            get { return tracecode; }
            set { tracecode = value; }
        }


        public decimal Storecardbalance
        {
            get { return storecardbalance; }
            set { storecardbalance = value; }
        }


        public decimal Integraloverall
        {
            get { return integraloverall; }
            set { integraloverall = value; }
        }


        public decimal Addintegral
        {
            get { return addintegral; }
            set { addintegral = value; }
        }


        public decimal Decintegral
        {
            get { return decintegral; }
            set { decintegral = value; }
        }


        public decimal Inflatedrate
        {
            get { return inflatedrate; }
            set { inflatedrate = value; }
        }


        public decimal Netamount
        {
            get { return netamount; }
            set { netamount = value; }
        }

    }
}
