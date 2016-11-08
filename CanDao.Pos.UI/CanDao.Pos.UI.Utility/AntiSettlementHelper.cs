using System;
using System.Windows;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.Utility.View;
using CanDao.Pos.UI.Utility.ViewModel;

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
        /// 会员卡号或手机号。
        /// </summary>
        private string _cardNo;

        /// <summary>
        /// 交易号。
        /// </summary>
        private string _traceCode;

        /// <summary>
        /// 反结算交易号。
        /// </summary>
        private string _antiSettlementTraceCode;

        #endregion

        #region Public Methods

        /// <summary>
        /// 异步执行反结算。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="memberNo">会员号。</param>
        /// <param name="afterAntiSettlementWf">反结算执行完成后执行的工作流。</param>
        public void AntiSettlementAsync(string orderId, string memberNo, WorkFlowInfo afterAntiSettlementWf)
        {
            _orderId = orderId;

            var checkWf = new WorkFlowInfo(CheckOrderCanResettlementProcess, CheckOrderCanResettlementComplete);//检测订单是否允许反结工作流。
            var currentWf = checkWf;//当前工作流。
            if (!string.IsNullOrEmpty(memberNo))
            {
                if (Globals.IsCanDaoMember)
                {
                    var canDaoMemberResettlementWf = GenerateCanDaoMemberResettlememnntWf();//餐道会员反结算工作流。
                    checkWf.NextWorkFlowInfo = canDaoMemberResettlementWf;
                    currentWf = canDaoMemberResettlementWf;
                }
                else if (Globals.IsYazuoMember)
                {
                    var yaZuoMemberResettlementWf = GenerateYaZuoResettlementWf();//雅座会员反结算工作流。
                    checkWf.NextWorkFlowInfo = yaZuoMemberResettlementWf;
                    currentWf = yaZuoMemberResettlementWf;
                }
            }

            var antiSettlementWf = new WorkFlowInfo(AntiSettlementProcess, AntiSettlementComplete, "账单反结算中...") { NextWorkFlowInfo = afterAntiSettlementWf };
            if (Globals.IsCanDaoMember)
                antiSettlementWf.ErrorWorkFlowInfo = GenerateCanDaoMemberUnResettlementWf();

            currentWf.NextWorkFlowInfo = antiSettlementWf;

            WorkFlowService.Start(null, checkWf);
        }

        /// <summary>
        /// 获取反结算工作流（自动反结算）
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="memberNo"></param>
        /// <param name="ownerWnd"></param>
        public WorkFlowInfo GetAntiSettlement(string orderId, string memberNo, Window ownerWnd)
        {
            _orderId = orderId;
            return new WorkFlowInfo(AutoAntiSettlementProcess1, AntiSettlementComplete, "账单反结算中...");
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
                MessageDialog.Warning(result);
                return new Tuple<bool, object>(false, null);
            }

            InfoLog.Instance.I("反结算账单：{0}完成。", _orderId);
            return new Tuple<bool, object>(true, null);
        }

        #endregion

        #region Private Methods

        private object AutoAntiSettlementProcess1(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return "创建IOrderService服务失败。";

            InfoLog.Instance.I("开始检测账单是否允许反结算...");
            var result = service.CheckCanAntiSettlement(_orderId, Globals.UserInfo.UserName);
            if (!string.IsNullOrEmpty(result))
                return string.Format("检测账单是否允许反结算失败：{0}", result);

            InfoLog.Instance.I("结束检测账单是否允许反结算。");
            InfoLog.Instance.I("开始自动反结算账单：{0}...", _orderId);
            return service.AntiSettlementOrder(Globals.UserInfo.UserName, _orderId, "会员结算失败，系统自动反结");
        }


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
                MessageDialog.Warning(result);
                return null;
            }

            InfoLog.Instance.I("结束检测账单是否允许反结算。");
            InfoLog.Instance.I("弹出选择反结算原因选择窗口，选择反结算原因...");
            var reasonSelectorWnd = new AntiSettlementReasonSelectorWndVm();
            if (!WindowHelper.ShowDialog(reasonSelectorWnd))
            {
                InfoLog.Instance.I("取消选择反结算原因，退出反结算流程。");
                return null;
            }

            _antiSettlementReason = reasonSelectorWnd.SelectedReason;
            InfoLog.Instance.I("结束选择反结算原因：{0}", reasonSelectorWnd.SelectedReason);
            InfoLog.Instance.I("开始反结算授权...");
            if (!WindowHelper.ShowDialog(new AuthorizationWndVm(EnumRightType.AntiSettlement)))
            {
                InfoLog.Instance.I("反结算授权失败，退出反结算流程。");
                return null;
            }

            InfoLog.Instance.I("反结算授权成功，开始会员系统反结算...");
            return new Tuple<bool, object>(true, _orderId);
        }

        /// <summary>
        /// 生成餐道会员反结算工作流。
        /// </summary>
        /// <returns></returns>
        private WorkFlowInfo GenerateCanDaoMemberResettlememnntWf()
        {
            return new WorkFlowInfo(CanDaoMemberResettlementProcess, MemberResettlementComplete);
        }

        /// <summary>
        /// 生产雅座会员反结算工作流。
        /// </summary>
        /// <returns></returns>
        private WorkFlowInfo GenerateYaZuoResettlementWf()
        {
            return new WorkFlowInfo(YaZuoMemberResettlementProcess, MemberResettlementComplete);
        }

        /// <summary>
        /// 生成会员取消反结算工作流。
        /// </summary>
        /// <returns></returns>
        private WorkFlowInfo GenerateCanDaoMemberUnResettlementWf()
        {
            return new WorkFlowInfo(CanDaoMemberUnResettlementProcess, CanDaoMemberUnResettlementComplete, "取消会员消费反结算中...");
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
            _cardNo = result.Item2.cardno;
            _traceCode = result.Item2.serial;
            var voidSaleRequest = new CanDaoMemberVoidSaleRequest(_orderId)
            {
                branch_id = Globals.BranchInfo.BranchId,
                cardno = _cardNo,
                TraceCode = _traceCode,
            };

            var voidSaleResult = memberService.VoidSale(voidSaleRequest);
            if (string.IsNullOrEmpty(voidSaleResult.Item1))
                _antiSettlementTraceCode = voidSaleResult.Item2;

            return voidSaleResult.Item1;
        }

        /// <summary>
        /// 雅座会员反结算执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object YaZuoMemberResettlementProcess(object arg)
        {
            InfoLog.Instance.I("开始会员消费反结算。");

            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
                return "创建IMemberService服务失败。";

            return service.AntiSettlementYaZuo(_orderId);
        }

        /// <summary>
        /// 会员反结算执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> MemberResettlementComplete(object arg)
        {
            var result = (string)arg;
            if (!string.IsNullOrWhiteSpace(result))
            {
                var errMsg = string.Format("会员消费反结算失败：{0}", result);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg);
                return new Tuple<bool, object>(false, null);
            }

            InfoLog.Instance.I("会员消费反结算完成。");
            return new Tuple<bool, object>(true, null);
        }

        /// <summary>
        /// 取消会员消费反结算的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CanDaoMemberUnResettlementProcess(object arg)
        {
            InfoLog.Instance.I("开始取消订单：\"{0}\"的反结算信息...", _orderId);
            var service = ServiceManager.Instance.GetServiceIntance<IMemberService>();
            if (service == null)
            {
                var msg = "创建IMemberService服务失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            var request = new CanDaoMemberUnVoidSaleRequest
            {
                cardno = _cardNo,
                deal_no = _antiSettlementTraceCode,
                tracecode = _traceCode,
            };
            return service.UnVoidSale(request);
        }

        /// <summary>
        /// 会员消费取消反结算执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Tuple<bool, object> CanDaoMemberUnResettlementComplete(object arg)
        {
            var result = (string)arg;
            if (!string.IsNullOrWhiteSpace(result))
            {
                var errMsg = string.Format("会员消费取消反结算失败：{0}", result);
                ErrLog.Instance.E(errMsg);
                MessageDialog.Warning(errMsg);
                return new Tuple<bool, object>(false, null);
            }

            InfoLog.Instance.I("会员消费取消反结算完成。");
            return new Tuple<bool, object>(true, null);
        }

        #endregion
    }
}