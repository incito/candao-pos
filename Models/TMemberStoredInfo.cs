using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class TMemberStoredInfo
    {
        string _cardno;
        string _treport_membertitle;
        string _pzh;
        string _date;
        string _time;
        string _store;
        string _point;
        string _amount;
        public string Cardno
        {
            get { return _cardno; }
            set { _cardno = value; }
        }
        public string Treport_membertitle
        {
            get { return _treport_membertitle; }
            set { _treport_membertitle = value; }
        }
        public string Pzh
        {
            get { return _pzh; }
            set { _pzh = value; }
        }
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }
        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }
        public string Store
        {
            get { return _store; }
            set { _store = value; }
        }
        public string Point
        {
            get { return _point; }
            set { _point = value; }
        }
        public string Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
    }
}
