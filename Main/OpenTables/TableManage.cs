using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanDaoCD.Pos.Common.Operates;
using Common;
using WebServiceReference;

namespace KYPOS.OpenTables
{
    /// <summary>
    /// 桌台管理
    /// </summary>
    public class TableManage
    {
        #region 字段

        /// <summary>
        /// 餐台名称
        /// </summary>
        private string _tableName;

        /// <summary>
        /// 异步加载
        /// </summary>
        private AsyncLoadServer asyncLoad;

        #endregion

        #region 属性

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorInfo { set; get; }

        #endregion

        #region 构造函数

        public TableManage()
        {
            asyncLoad = new AsyncLoadServer();
            asyncLoad.Init();
            asyncLoad.ActionWorkerState = new Action<int>(WorkerStateHandel);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 工作完成结果
        /// </summary>
        /// <param name="state"></param>
        private void WorkerStateHandel(int state)
        {
            switch (state)
            {
                case 2:
                {
                    OWindowManage.ShowMessageWindow(string.Format("打开餐台失败：{0}",asyncLoad.ErrorMessage), false);
                    break;
                }
            }
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 选择餐单
        /// </summary>
        /// <param name="tableName"></param>
        public void SelectTable(string tableName)
        {

        }

       
        #endregion
    }
}
