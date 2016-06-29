using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Common;
using WebServiceReference;

namespace KYPOS.Dishes
{
    /// <summary>
    /// 订单定时刷新
    /// </summary>
    public class DishesTimer
    {

        #region 字段

        private Timer _refreshTimer;

        private string _totalAmount;

        private string _orderID;


        #endregion

#region 事件

        public Action DataChangeAction;

        public string OrderID
        {
            set { _orderID = value; }
            get { return _orderID; }
        }
#endregion

        #region 构造函数

        public DishesTimer()
        {
            _refreshTimer = new Timer();

        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="totalAmount"></param>
        /// <param name="tableName"></param>
        /// <param name="userID"></param>
        public void Start()
        {
            _refreshTimer.Interval = 10*1000;
            _refreshTimer.Elapsed += _refreshTimer_Elapsed;
            _refreshTimer.Start();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void stop()
        {
            try
            {
                _orderID = string.Empty;
                _refreshTimer.Stop();
            }
            catch
            {
               
            }
        
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Excute();
        }

        /// <summary>
        /// 执行
        /// </summary>
        private void Excute()
        {
            try
            {
                if (string.IsNullOrEmpty(_orderID) || Globals.UserInfo.UserID == null)
                    return;

                var resTable = RestClient.GetOrderTable(_orderID, Globals.UserInfo.UserID);

                if (resTable.Rows.Count != Globals.OrderTable.Rows.Count)
                {
                    if (DataChangeAction != null)
                    {
                        DataChangeAction();
                    }
                }
                else
                {
                    double newAmount = 0;
                    double oldAmount = 0;
                    for (int i = 0; i < resTable.Rows.Count; i++)
                    {
                        double nTemp = 0;
                        double oTemp = 0;
                        if (double.TryParse(resTable.Rows[i]["amount"].ToString(), out nTemp))
                        {
                            newAmount += nTemp;
                        }
                        if (double.TryParse(Globals.OrderTable.Rows[i]["amount"].ToString(), out oTemp))
                        {
                            oldAmount += oTemp;
                        }

                    }
                    if (newAmount != oldAmount)
                    {
                        if (DataChangeAction != null)
                        {
                            DataChangeAction();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AllLog.Instance.E(string.Format("订单信息同步异常："), ex);
            }
        }
        #endregion


    }
}
