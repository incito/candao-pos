using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// t_table:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class t_table
    {
        public t_table()
        { }
        #region Model
        private string _tableid;
        private string _position;
        private int? _status;
        private string _restaurantid;
        private int? _isvip;
        private int? _iscompartment;
        private string _isavailable;
        private string _buildingno;
        private decimal _minprice;
        private decimal? _fixprice;
        private string _tableno;
        private int? _personnum;
        private string _tabletype;
        private string _areaid;
        private string _tablename;
        private float _amount;
                /// <summary>
        /// 
        /// </summary>
        public float amount
        {
            set { _amount = value; }
            get { return _amount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tableid
        {
            set { _tableid = value; }
            get { return _tableid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string position
        {
            set { _position = value; }
            get { return _position; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string restaurantId
        {
            set { _restaurantid = value; }
            get { return _restaurantid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isVip
        {
            set { _isvip = value; }
            get { return _isvip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? iscompartment
        {
            set { _iscompartment = value; }
            get { return _iscompartment; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string isavailable
        {
            set { _isavailable = value; }
            get { return _isavailable; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string buildingNo
        {
            set { _buildingno = value; }
            get { return _buildingno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal minprice
        {
            set { _minprice = value; }
            get { return _minprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? fixprice
        {
            set { _fixprice = value; }
            get { return _fixprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tableNo
        {
            set { _tableno = value; }
            get { return _tableno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? personNum
        {
            set { _personnum = value; }
            get { return _personnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tabletype
        {
            set { _tabletype = value; }
            get { return _tabletype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string areaid
        {
            set { _areaid = value; }
            get { return _areaid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string tableName
        {
            set { _tablename = value; }
            get { return _tablename; }
        }
        #endregion Model

    }
}

