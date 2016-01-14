using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// t_order:帐单信息类
    /// </summary>
    [Serializable]
    public partial class t_order
    {
        public t_order()
        { }
        #region Model
        private string _orderid;
        private string _userid;
        private DateTime? _begintime;
        private DateTime? _endtime;
        private int _orderstatus;
        private int? _custnum;
        private string _tableids;
        private string _specialrequied;
        private int? _womannum;
        private int? _childnum;
        private int? _mannum;
        private string _currenttableid;
        private float _fulldiscountrate;
        private string _memberno;
        private string _invoicetitle;

        public string Invoicetitle
        {
            get { return _invoicetitle; }
            set { _invoicetitle = value; }
        }
        public string memberno
        {
            set { _memberno = value; }
            get { return _memberno; }
        }
        public float fulldiscountrate
        {
            set { _fulldiscountrate = value; }
            get { return _fulldiscountrate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string orderid
        {
            set { _orderid = value; }
            get { return _orderid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string userid
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? begintime
        {
            set { _begintime = value; }
            get { return _begintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? endtime
        {
            set { _endtime = value; }
            get { return _endtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int orderstatus
        {
            set { _orderstatus = value; }
            get { return _orderstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? custnum
        {
            set { _custnum = value; }
            get { return _custnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tableIds
        {
            set { _tableids = value; }
            get { return _tableids; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string specialrequied
        {
            set { _specialrequied = value; }
            get { return _specialrequied; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? womanNum
        {
            set { _womannum = value; }
            get { return _womannum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? childNum
        {
            set { _childnum = value; }
            get { return _childnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? mannum
        {
            set { _mannum = value; }
            get { return _mannum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string currenttableid
        {
            set { _currenttableid = value; }
            get { return _currenttableid; }
        }
        #endregion Model

    }
}

