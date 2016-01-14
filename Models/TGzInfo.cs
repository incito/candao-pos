using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TGzInfo
    {
        private string _gzname;
        private string _gzcode;
        private string telephone;
        private string relaperson;
        private string address;

        public string Gzname
        {
            get { return _gzname; }
            set { _gzname = value; }
        }

        public string Gzcode
        {
            get { return _gzcode; }
            set { _gzcode = value; }
        }

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        public string Relaperson
        {
            get { return relaperson; }
            set { relaperson = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

    }
}
