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
        /// 检查是否开台
        /// </summary>
        /// <returns></returns>
        private bool CheckIsOpenTable()
        {

            return false;
        }

        /// <summary>
        /// 获取点菜明细
        /// </summary>
        /// <returns></returns>
        private bool GetOrderDishes()
        {
            string resOrder = RestClient.GetOrder(_tableName, Globals.UserInfo.UserID);

            if (resOrder == "0") //调用
            {
                asyncLoad.ErrorMessage = "未找到帐单,请确认是否已开台!";
                return false;
            }
            else
            {
                if (string.IsNullOrEmpty(resOrder))
                {
                    asyncLoad.ErrorMessage = "获取数据失败，请检查网络连接是否正常!";
                }
            }
            return true;
        }

        /// <summary>
        /// 获取优惠列表
        /// </summary>
        /// <returns></returns>
        private bool GetCoupons()
        {
            return false;
        }

        /// <summary>
        /// 计算账单金额
        /// </summary>
        /// <returns></returns>
        private bool SetBillInfo()
        {
            return false;
        }

       

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

        public void Set
        #endregion
    }
}
