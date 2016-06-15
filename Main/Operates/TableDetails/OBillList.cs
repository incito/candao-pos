using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KYPOS.Operates.TableDetails
{
    /// <summary>
    /// 菜单操作管理类
    /// </summary>
    public class OBillList
    {
        #region 字段
        //当前DataGridView
        private DataGridView _currentDataGrid = null;

        private DataSet _currentData = null;
        #endregion

        #region 属性
        /// <summary>
        /// 当前数据集
        /// </summary>
        private DataSet CurrentData
        {
            set
            {
                _currentData = value;
            }
            get
            {
                return _currentData;
            }
        }
        #endregion

        #region 构造函数
        public OBillList(DataGridView dataGrid)
        {
            _currentDataGrid = dataGrid;
            _currentDataGrid.AutoGenerateColumns = false;
            _currentDataGrid.Tag = 0;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置数据集
        /// </summary>
        /// <param name="ds"></param>
        public void SetData(DataSet ds)
        {
            _currentData = ds;
            _currentDataGrid.DataSource = _currentData;
        }

        public void ChanageRowValue()
        {

        }
        #endregion
    }
}
