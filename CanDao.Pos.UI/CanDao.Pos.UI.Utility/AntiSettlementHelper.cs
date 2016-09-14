using System;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.Utility.View;

namespace CanDao.Pos.UI.Utility
{
    /// <summary>
    /// 反结算辅助类。
    /// </summary>
    public class AntiSettlementHelper
    {
        #region Fields

        /// <summary>
        /// 反结算原因。
        /// </summary>
        private string _antiSettlementReason;

        /// <summary>
        /// 反结算订单号。
        /// </summary>
        private string _orderId;

        /// <summary>
        /// 所属窗体。
        /// </summary>
        private Window _ownerWindow;

        #endregion

        #region 属性

        public string OrderId { set; get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// 异步执行反结算。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="memberNo">会员号。</param>
        /// <param name="ownerWnd">所属窗体。显示弹出窗口时设置父窗口用。</param>
        /// <param name="afterAntiSettlementWf">反结算执行完成后执行的工作流。</param>
        public void AntiSettlementAsync(string orderId, string memberNo, Window ownerWnd, WorkFlowInfo afterAntiSettlementWf)
        {
            _orderId = orderId;
            _ownerWindow = ownerWnd;

            var checkWf = new WorkFlowInfo(CheckOrderCanResettlementProcess, CheckOrderCanResettlementComplete);//检测订单是否允许反结工作流。
            var currentWf = checkWf;//当前工作流。
            if (!string.IsNullOrEmpty(memberNo))
            {
                if (Globals.IsCanDaoMember)
                {
                    var canDaoMemberResettlementWf = new WorkFlowInfo(CanDaoMemberResettlementProcess, CanDaoMemberResettlementComplete);//餐道会员反结算工作流。
                    checkWf.NextWorkFlowInfo = canDaoMemberResettlementWf;
                    currentWf = canDaoMemberResettlementWf;
                }
                else if (Globals.IsYazuoMember)
                {
                    var yaZuoMemberResettlementWf = new WorkFlowInfo(YaZuoMemberResettlementProcess, YaZuoMemberResettlementComplete);//雅座会员反结算工作流。
                    checkWf.NextWorkFlowInfo = yaZuoMemberResettlementWf;
                    currentWf = yaZuoMemberResettlementWf;
                }
            }

            var antiSettlementWf = new WorkFlowInfo(AntiSettlementProcess, AntiSettlementComplete, "账单反结算中...") { NextWorkFlowInfo = afterAntiSettlementWf };
            currentWf.NextWorkFlowInfo = antiSettlementWf;

            WorkFlowService.Start(null, checkWf);
        }

        /// <summary>
        /// 设置获取反结算工作流（自动反结算）
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="memberNo"></param>
        /// <param name="ownerWnd"></param>
        public WorkFlowInfo GetAntiSettlement(string orderId, string memberNo, Window ownerWnd)
        {
            _orderId = orderId;
            _ownerWindow = ownerWnd;

            InfoLog.Instance.I("开始检测账单是否允许反结算...");
            var checkWf = new WorkFlowInfo(CheckOrderCanResettlementProcess, OrderCanResettlementComplete);//检测订单是否允许反结工作流。
            var currentWf = checkWf;//当前工作流。
            if (!string.IsNullOrEmpty(memberNo))
            {
                if (Globals.IsCanDaoMember)
                {
                    var canDaoMemberResettlementWf = new WorkFlowInfo(CanDaoMemberResettlementProcess, CanDaoMemberResettlementComplete);//餐道会员反结算工作流。
                    currentWf.NextWorkFlowInfo = canDaoMemberResettlementWf;
                    currentWf = canDaoMemberResettlementWf;
                }
                else if (Globals.IsYazuoMember)
                {
                    var yaZuoMemberResettlementWf = new WorkFlowInfo(YaZuoMemberResettlementProcess, YaZuoMemberResettlementComplete);//雅座会员反结算工作流。
                    currentWf.NextWorkFlowInfo = yaZuoMemberResettlementWf;
                    currentWf = yaZuoMemberResettlementWf;
                }
            }

            var antiSettlementWf = new WorkFlowInfo(AutoAntiSettlementProcess, AntiSettlementComplete, "账单反结算中...");

            currentWf.NextWorkFlowInfo = antiSettlementWf;

            return checkWf;
        }

        /// <summary>
        /// 反结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public object AntiSettlementProcess(object arg)
        {
            InfoLog.Instance.I("开始反结算账单：{0}...", _orderId);
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            return service.AntiSettlementOrder(Globals.Authorizer.UserName, _orderId, _antiSettlementReason);
        }

        /// <summary>
        /// 自动反结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public object AutoAntiSettlementProcess(object arg)
        {
            InfoLog.Instance.I("开始自动反结算账单：{0}...", _orderId);
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var reason = "会员结算失败，系统自动反结";
            return service.AntiSettlementOrder(Globals.UserInfo.UserName, _orderId, reason);
        }

        /// <summary>
        /// 反结算执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public Tuple<bool, object> AntiSettlementComplete(object arg)
        {
            _antiSettlementReason = null;
            var result = (string)arg;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, _ownerWindow);
                return null;
            }

            InfoLog.Instance.I("反结算账单：{0}完成。", _orderId);
            return new Tuple<bool, object>(true, null);
        }

        #endregion

        #region Private Methods

        private object CheckOrderCanResettlementProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            var param = new Tuple<string, string>(_orderId, Globals.UserInfo.UserName);
            InfoLog.Instance.I("开始检测账单是否允许反结算...");
            return service.CheckCanAntiSettlement(param.Item1, param.Item2);
        }

        private Tuple<bool, object> CheckOrderCanResettlementComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E("检测账单是否允许反结算失败：{0}", result);
                MessageDialog.Warning(result, _ownerWindow);
                return null;
            }

            InfoLog.Instance.I("结束检测账单是否允许反结算。");
            InfoLog.Instance.I("弹出选择反结算原因选择窗口，选择反结算原因...");
            var wnd = new AntiSettlementReasonSelectorWindow();
            if (!WindowHelper.ShowDialog(wnd, _ownerWindow))
            {
                InfoLog.Instance.I("取消选择反结算原因，退出反结算流程。");
                return null;
            }

            _antiSettlementReason = wnd.SelectedReason;
            InfoLog.Instance.I("结束选择反结算原因：{0}", wnd.SelectedReason);
            InfoLog.Instance.I("开始反结算授权...");
            if (!WindowHelper.ShowDialog(new AuthorizationWindow(EnumRightType.AntiSettlement), _ownerWindow))
            {
                InfoLog.Instance.I("反结算授权失败，退出反结算流程。");
                return null;
            }

            InfoLog.Instance.I("反结算授权成功，开始会员系统反结算...");
            return new Tuple<bool, object>(true, _orderId);
        }

        /// <summary>
        /// 不需要弹出验证授权
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> OrderCanResettlementComplete(object arg)
        {
            var result = arg as string;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E("检测账单是否允许反结算失败：{0}", result);
                MessageDialog.Warning(result, _ownerWindow);
                return null;
            }

            InfoLog.Instance.I("结束检测账单是否允许反结算。");

            return new Tuple<bool, object>(true, _orderId);
        }

        /// <summary>
        /// 餐道会员反结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CanDaoMemberResettlementProcess(object arg)
        {
            InfoLog.Instance.I("开始获取订单：\"{0}\"的会员信息...", _orderId);
            var orderService = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (orderService == null)
            {
                var msg = "创建IOrderService服务失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            var result = orderService.GetOrderMemberInfo(_orderId);
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取订单会员信息失败：{0}", result.Item1);
                return result.Item1;
            }

            if (!result.Item2.IsSuccess)//没有获取到订单的会员信息。
            {
                InfoLog.Instance.I("没有获取到订单的会员信息。");
                return null;
            }

            InfoLog.Instance.I("结束获取订单会员信息，交易号：{0}，卡号：{1}", result.Item2.serial, result.Item2.cardno);
            var memberService = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (memberService == null)
            {
                var msg = "创建IMemberService服务失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            InfoLog.Instance.I("开始会员消费反结算...");
            var voidSaleRequest = new CanDaoMemberVoidSaleRequest(_orderId);
            voidSaleRequest.branch_id = Globals.BranchInfo.BranchId;
            voidSaleRequest.cardno = result.Item2.cardno;
            voidSaleRequest.TraceCode = result.Item2.serial;

            return memberService.VoidSale(voidSaleRequest);
        }

        /// <summary>
        /// 餐道会员反结算执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> CanDaoMemberResettlementComplete(object arg)
        {
            var result = (string)arg;
            if (!string.IsNullOrWhiteSpace(result))
            {
                var errMsg = string.Format("餐道会员消费反结算失败：{0}", result);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg);
                return new Tuple<bool, object>(false, null);
            }

            InfoLog.Instance.I("餐道会员消费反结算完成。");
            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 雅座会员反结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object YaZuoMemberResettlementProcess(object arg)
        {
            return null;
        }

        /// <summary>
        /// 雅座会员反结算执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> YaZuoMemberResettlementComplete(object arg)
        {
            return null;
        }

        #endregion
    }
}