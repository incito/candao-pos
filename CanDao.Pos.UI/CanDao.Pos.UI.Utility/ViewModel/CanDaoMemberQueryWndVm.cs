using System;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility.ViewModel
{
    public class CanDaoMemberQueryWndVm : NormalWindowViewModel
    {
        #region Properties
        /// <summary>
        /// 会员信息。
        /// </summary>
        private MemberInfo _data;
        /// <summary>
        /// 会员信息。
        /// </summary>
        public MemberInfo Data
        {
            get { return _data; }
            set
            {
                _data = value;
                RaisePropertiesChanged("Data");
            }
        }

        /// <summary>
        /// 手机号。
        /// </summary>
        private string _mobile;
        /// <summary>
        /// 手机号。
        /// </summary>
        public string Mobile
        {
            get { return _mobile; }
            set
            {
                _mobile = value;
                RaisePropertiesChanged("Mobile");
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// 操作命令。
        /// </summary>
        public ICommand OperCmd { get; private set; }

        #endregion

        #region Command Method

        /// <summary>
        /// 操作命令的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        private void OperMethod(object arg)
        {
            switch (arg as string)
            {
                case "ModifyPwd":
                    WindowHelper.ShowDialog(new CanDaoMemberModifyPwdWindow(Data), OwnerWindow);
                    break;
                case "Storage":
                    if (WindowHelper.ShowDialog(new CanDaoMemberStorageWindow(Mobile, Data.CardNo), OwnerWindow))
                        QueryMemberAsync();
                    break;
                case "ReportLoss":
                    if (!MessageDialog.Quest(string.Format("确定挂失会员号：{0}", Mobile)))
                        return;

                    var request = new CanDaoMemberReportLossRequest(Globals.BranchInfo.BranchId, Data.CardNo);
                    TaskService.Start(request, ReportLossProcess, ReportLossComplete, "会员挂失中...");
                    break;
                case "Cancel":
                    var request1 = new CanDaoMemberReportLossRequest(Globals.BranchInfo.BranchId, Data.CardNo);
                    TaskService.Start(request1, CancelProcess, CancelComplete, "会员注销中...");
                    break;
            }
        }

        /// <summary>
        /// 操作命令是否可用的判断方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool CanOperMethod(object arg)
        {
            return Data != null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 异步查询会员信息。
        /// </summary>
        public void QueryMemberAsync()
        {
            if (string.IsNullOrEmpty(Mobile))
            {
                MessageDialog.Warning("请输入会员号查询。", OwnerWindow);
                return;
            }

            var request = new CanDaoMemberQueryRequest { branch_id = Globals.BranchInfo.BranchId, cardno = Mobile };
            TaskService.Start(request, QueryMemberProcess, QueryMemberComplete, "会员信息查询中...");
        }

        protected override void InitCommand()
        {
            base.InitCommand();
            OperCmd = CreateDelegateCommand(OperMethod, CanOperMethod);
        }

        /// <summary>
        /// 查询会员的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object QueryMemberProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, MemberInfo>("创建IMemberService服务失败。", null);

            return service.QueryCanndao((CanDaoMemberQueryRequest)arg);
        }

        /// <summary>
        /// 查询会员执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void QueryMemberComplete(object arg)
        {
            var result = (Tuple<string, MemberInfo>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                MessageDialog.Warning(result.Item1, OwnerWindow);
                return;
            }

            Data = result.Item2;

        }

        /// <summary>
        /// 挂失的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object ReportLossProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, MemberInfo>("创建IMemberService服务失败。", null);

            return service.ReportLoss((CanDaoMemberReportLossRequest)arg);
        }

        /// <summary>
        /// 挂失执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void ReportLossComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                return;
            }

            MessageDialog.Warning("挂失成功。", OwnerWindow);
            ResetAllData();
        }

        /// <summary>
        /// 注销执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CancelProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return new Tuple<string, MemberInfo>("创建IMemberService服务失败。", null);

            return service.Cancel((CanDaoMemberReportLossRequest)arg);
        }

        /// <summary>
        /// 注销执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void CancelComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                MessageDialog.Warning(result, OwnerWindow);
                return;
            }

            MessageDialog.Warning("注销成功。", OwnerWindow);
            ResetAllData();
        }

        /// <summary>
        /// 重置数据。
        /// </summary>
        private void ResetAllData()
        {
            Mobile = "";
            Data = null;
        }

        #endregion
    }
}