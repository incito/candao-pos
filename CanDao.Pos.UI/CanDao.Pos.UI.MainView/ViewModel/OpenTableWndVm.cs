using System;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.Utility;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    /// <summary>
    /// 开台窗口Vm。
    /// </summary>
    public class OpenTableWndVm : NormalWindowViewModel
    {
        private readonly TableInfo _tableInfo;
        private string _maleCustomer;
        private string _femaleCustomer;

        public OpenTableWndVm(TableInfo tableInfo)
        {
            _tableInfo = tableInfo;
            TableName = _tableInfo.TableName;
            IsDinnerWareEnable = Globals.IsDinnerWareEnable;
        }

        /// <summary>
        /// 服务员编号。
        /// </summary>
        public string WaiterId { get; set; }

        /// <summary>
        /// 餐桌编号。
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 男性人数。
        /// </summary>
        public string MaleCustomer
        {
            get { return _maleCustomer; }
            set
            {
                _maleCustomer = value;
                int cus = Convert2Int(value) + Convert2Int(FemaleCustomer);
                CustomerNumber = cus > 0 ? cus.ToString() : null;
                RaisePropertiesChanged("CustomerNumber");
            }
        }

        /// <summary>
        /// 女性人数。
        /// </summary>
        public string FemaleCustomer
        {
            get { return _femaleCustomer; }
            set
            {
                _femaleCustomer = value;
                int cus = Convert2Int(value) + Convert2Int(MaleCustomer);
                CustomerNumber = cus > 0 ? cus.ToString() : null;
                RaisePropertiesChanged("CustomerNumber");
            }
        }

        /// <summary>
        /// 是否启用餐具。
        /// </summary>
        public bool IsDinnerWareEnable { get; set; }

        /// <summary>
        /// 餐具数量。
        /// </summary>
        public string CustomerNumber { get; set; }

        /// <summary>
        /// 是否选中儿童。
        /// </summary>
        public bool HasChild { get; set; }

        /// <summary>
        /// 是否选中青年。
        /// </summary>
        public bool HasYoung { get; set; }

        /// <summary>
        /// 是否选中中年。
        /// </summary>
        public bool HasMiddleAge { get; set; }

        /// <summary>
        /// 是否选中老年。
        /// </summary>
        public bool HasOld { get; set; }

        protected override void Confirm(object param)
        {
            if (string.IsNullOrEmpty(WaiterId))
            {
                MessageDialog.Warning("请输入服务员编号。", OwnerWindow);
                return;
            }

            int male = Convert2Int(MaleCustomer);
            int famale = Convert2Int(FemaleCustomer);
            if (male + famale <= 0)
            {
                MessageDialog.Warning("请输入就餐人数。", OwnerWindow);
                return;
            }
            var checkUser = CheckIsUser();
            if (!checkUser.Item2)//验证服务员输入
            {
                MessageDialog.Warning(checkUser.Item1, OwnerWindow);
                return;
            }

            if (!MessageDialog.Quest("确定开台吗？"))
                return;

            var request = new OpenTableRequest
            {
                manNum = male,
                tableNo = TableName,
                username = WaiterId,
                womanNum = famale,
                ageperiod = GetAgePeriod(),
            };
            InfoLog.Instance.I("开始开台...");
            TaskService.Start(request, OpenTableProcess, OpenTableComplete, "正在开台...");
        }

        /// <summary>
        /// 判断服务员输入
        /// </summary>
        private Tuple<string,bool> CheckIsUser()
        {
            var service = ServiceManager.Instance.GetServiceIntance<IAccountService>();
            if (service == null)
                return new Tuple<string,bool>("创建IRestaurantService服务失败。",false);

            return service.VerifyUser(WaiterId);
        }

        /// <summary>
        /// 开台执行方法。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private object OpenTableProcess(object param)
        {
            var request = (OpenTableRequest)param;
            var service = ServiceManager.Instance.GetServiceIntance<IRestaurantService>();
            if (service == null)
                return new Tuple<string, string>("创建IRestaurantService服务失败。", null);

            return service.OpenTable(request);
        }

        /// <summary>
        /// 开台执行完成。
        /// </summary>
        /// <param name="param"></param>
        private void OpenTableComplete(object param)
        {
            var result = (Tuple<string, string>)param;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("开台失败：{0}", result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            InfoLog.Instance.I("开台完成，订单号：{0}", result.Item2);
            _tableInfo.OrderId = result.Item2;
            CloseWindow(true);
        }

        /// <summary>
        /// 获取年龄段描述串。
        /// </summary>
        /// <returns></returns>
        private string GetAgePeriod()
        {
            string agePeriod = "";
            if (HasChild)
                agePeriod += "1";
            if (HasYoung)
                agePeriod += "2";
            if (HasMiddleAge)
                agePeriod += "3";
            if (HasOld)
                agePeriod += "4";
            return agePeriod;
        }

        /// <summary>
        /// 将string转换成int。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int Convert2Int(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex.Message);
                return 0;
            }
        }
    }
}