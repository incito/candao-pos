using System;
using System.Timers;
using CanDao.Pos.Common;
using CanDao.Pos.IService;

namespace CanDao.Pos.UI.MainView.Operates
{
    /// <summary>
    /// 订单定时刷新
    /// </summary>
    public class DishesTimer
    {

        #region 字段

        private Timer _refreshTimer;

        private decimal _totalAmount;

        private string _tableName;


        #endregion

        #region 事件

        public Action DataChangeAction;

        /// <summary>
        /// 订单ID
        /// </summary>
        public string TableName
        {
            set { _tableName = value; }
            get { return _tableName; }
        }

        /// <summary>
        /// 总计金额
        /// </summary>
        public decimal TotalAmount
        {
            set { _totalAmount = value; }
            get { return _totalAmount; }
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
        public void Start(decimal totalAmount)
        {
            _totalAmount = totalAmount;
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
                _totalAmount = 0;
                _tableName = string.Empty;
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
                _refreshTimer.Stop();
                _refreshTimer.Start();
                var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
                if (string.IsNullOrEmpty(_tableName) || Globals.UserInfo.UserName == null)
                    return;

                var resTable = service.GetTableDishInfoes(_tableName, Globals.UserInfo.UserName);

                if (!string.IsNullOrEmpty(resTable.Item1) || resTable.Item2==null)//发生异常
                {
                    return;
                }

                if (resTable.Item2.TotalAmount != _totalAmount)
                {
                    if (DataChangeAction != null)
                    {
                        DataChangeAction();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(string.Format("订单信息同步异常：{0}", ex));
              
            }
        }

        #endregion

    }
}
