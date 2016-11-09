using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;
using JunLan.Common.Base;
using HttpHelper = CanDao.Pos.Common.HttpHelper;

namespace CanDao.Pos.ServiceImpl
{
    /// <summary>
    /// 订单服务的实现。
    /// </summary>
    public class OrderServiceImpl : IOrderService
    {
        public Tuple<string, TableFullInfo> GetTableDishInfoes(string tableName, string userName)
        {
            string addr = ServiceAddrCache.GetServiceAddr("GetTableDishInfos");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, TableFullInfo>("获取餐桌所有菜品信息接口地址为空。", null);

            List<string> param = new List<string> { tableName, userName };
            return GetTableInfoProcess(addr, param);
        }

        public Tuple<string, TableFullInfo> GetTableDishInfoByOrderId(string orderId, string userName)
        {
            string addr = ServiceAddrCache.GetServiceAddr("GetTableDishInfoByOrderId");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, TableFullInfo>("获取餐桌所有菜品信息接口地址为空。", null);

            List<string> param = new List<string> { orderId, userName };
            return GetTableInfoProcess(addr, param);
        }

        private static Tuple<string, TableFullInfo> GetTableInfoProcess(string addr, List<string> param)
        {
            var result = RestHttpHelper.HttpGet<GetOrderDishListResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, TableFullInfo>(result.Item1, null);

            if (!result.Item2.IsSuccess)
                return new Tuple<string, TableFullInfo>(DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "服务器内部错误，获取菜品信息失败。"), null);

            var data = DataConverter.ToTableFullInfo(result.Item2);
            return new Tuple<string, TableFullInfo>(null, data);
        }

        public Tuple<string, string> GetOrderInvoice(string orderId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetOrderInvoice");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, string>("获取订单发票地址为空。", null);

            try
            {
                var request = new GetOrderInvoiceRequest { orderid = orderId };
                var result = HttpHelper.HttpPost<GetOrderInvoiceResponse>(addr, request);
                string orderInvoice = null;
                if (result.IsSuccess && result.data != null && result.data.Any())
                {
                    orderInvoice = result.data.First().invoice_title;
                }
                return new Tuple<string, string>(null, orderInvoice);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(ex);
                return new Tuple<string, string>(ex.MyMessage(), null);
            }
        }

        public string UpdateOrderInvoice(string orderId, decimal invoiceAmount, string cardNo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("UpdateOrderInvoice");
            if (string.IsNullOrEmpty(addr))
                return "更新发票信息地址为空。";

            try
            {
                var request = new UpdateInvoiceRequest
                {
                    orderid = orderId,
                    invoice_amount = invoiceAmount,
                    cardno = cardNo,
                };
                var response = HttpHelper.HttpPost<JavaResponse>(addr, request);
                return response.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(response.msg, response.info, "获取发票信息失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("获取发票信息时异常。", ex);
                return string.Format("获取发票信息失败：{0}", ex);
            }
        }

        public Tuple<string, List<MenuDishGroupInfo>> GetMenuDishInfos(string fullName)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetDishGroupInfos");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<MenuDishGroupInfo>>("获取菜谱菜品分类地址为空。", null);

            try
            {
                var menuResult = HttpHelper.HttpPost<GetMenuDishGroupInfoResponse>(addr, null);
                if (menuResult == null)
                    return new Tuple<string, List<MenuDishGroupInfo>>("没有菜谱信息。", null);

                if (!menuResult.IsSuccess)
                    return new Tuple<string, List<MenuDishGroupInfo>>(null, new List<MenuDishGroupInfo>());

                var list = menuResult.data.Select(DataConverter.ToMenuDishGroupInfo).ToList();

                addr = ServiceAddrCache.GetServiceAddr("GetAllDishInfos");
                if (string.IsNullOrEmpty(addr))
                    return new Tuple<string, List<MenuDishGroupInfo>>("获取菜谱全部菜品地址为空。", null);

                var param = new List<string> { fullName };
                var result = RestHttpHelper.HttpGet<GetMenuDishInfoResponse>(addr, param);
                if (!string.IsNullOrEmpty(result.Item1))
                    return new Tuple<string, List<MenuDishGroupInfo>>("获取菜谱全部菜品失败。" + Environment.NewLine + result.Item1, null);

                if (!result.Item2.IsSuccess)
                    return new Tuple<string, List<MenuDishGroupInfo>>(DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "获取菜谱全部菜品失败。"), null);

                var dishGroup = result.Item2.OrderJson.Select(DataConverter.ToMenuDishInfo).GroupBy(t => t.GroupId);
                foreach (var group in dishGroup)
                {
                    var gpItem = list.FirstOrDefault(t => t.GroupId.Equals(group.Key));
                    if (gpItem != null)
                    {
                        gpItem.DishInfos = group.ToList();
                    }
                }

                return new Tuple<string, List<MenuDishGroupInfo>>(null, list);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("获取菜谱信息时异常。", ex);
                return new Tuple<string, List<MenuDishGroupInfo>>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, bool> CheckDishStatus(string dishId, string dishUnit)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetDishStatus");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, bool>("检测菜品状态地址为空。", false);

            addr = string.Format("{0}/{1}/", addr, dishId);
            var param = new Dictionary<string, string> { { "dishUnit", dishUnit } };
            var result = RestHttpHelper.HttpPost<RestBaseResponse>(addr, param);
            return !string.IsNullOrEmpty(result.Item1) ? new Tuple<string, bool>(result.Item1, false) : new Tuple<string, bool>(null, !result.Item2.Info.Equals("1"));
        }

        public Tuple<string, List<CouponInfo>> GetCouponInfos(string couponTypeId, string userName)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetCouponInfos");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<CouponInfo>>("获取优惠券集合地址为空。", null);

            var request = new CouponInfoRequest
            {
                machineno = MachineManage.GetMachineId(),
                typeid = couponTypeId,
                userid = userName,
                orderid = "0"
            };
            try
            {
                var result = HttpHelper.HttpPost<List<CouponInfoResponse>>(addr, request);
                var couponList = new List<CouponInfo>();
                if (result != null && result.Any())
                {
                    couponList = result.Select(DataConverter.ToCouponInfo).ToList();
                }
                return new Tuple<string, List<CouponInfo>>(null, couponList);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("根据优惠券类型获取所有优惠券时异常。", ex);
                return new Tuple<string, List<CouponInfo>>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, MenuComboFullInfo> GetMenuComboDishes(GetMenuComboDishRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetMenuComboDish");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, MenuComboFullInfo>("获取套餐内菜品地址为空。", null);

            try
            {
                var result = HttpHelper.HttpPost<GetMenuComboDishResponse>(addr, request);
                var data = result.data.rows;
                if (data == null || !data.Any())
                    return new Tuple<string, MenuComboFullInfo>("套餐内没有包含任何菜品", null);

                var item = DataConverter.ToMenuComboFullInfo(data.First());
                return new Tuple<string, MenuComboFullInfo>(null, item);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("获取套餐内菜品时异常。", ex);
                return new Tuple<string, MenuComboFullInfo>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, MenuFishPotFullInfo> GetFishPotDishInfo(string dishId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetFishPotDish");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, MenuFishPotFullInfo>("获取鱼锅菜品信息地址为空。", null);

            List<string> param = new List<string> { dishId };
            var result = RestHttpHelper.HttpGet<GetFishPotDishResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, MenuFishPotFullInfo>(result.Item1, null);

            if (!result.Item2.IsSuccess)
                return new Tuple<string, MenuFishPotFullInfo>(DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "服务器内部错误，获取鱼锅信息失败，请重试。"), null);

            if (result.Item2.OrderJson == null)
                return new Tuple<string, MenuFishPotFullInfo>("获取鱼锅信息失败，请重启POS后重试。", null);

            var data = DataConverter.ToMenuFishPotFullInfo(result.Item2);
            return new Tuple<string, MenuFishPotFullInfo>(null, data);
        }

        public string OrderDish(string orderId, string tableNo, string orderRemark, List<OrderDishInfo> dishInfos)
        {
            var addr = ServiceAddrCache.GetServiceAddr("OrderDish");
            if (string.IsNullOrEmpty(addr))
                return "菜品下单地址为空。";

            var request = DataConverter.ToOrderDishRequest(orderId, tableNo, orderRemark, dishInfos);
            return OrderDishProcess(addr, request);
        }

        public string OrderDishCf(string orderId, string tableNo, string orderRemark, List<OrderDishInfo> dishInfos)
        {
            var addr = ServiceAddrCache.GetServiceAddr("OrderDishCf");
            if (string.IsNullOrEmpty(addr))
                return "菜品下单地址为空。";

            var request = DataConverter.ToOrderDishRequest(orderId, tableNo, orderRemark, dishInfos);
            return OrderDishProcess(addr, request);
        }

        /// <summary>
        /// 下单的执行方法。
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string OrderDishProcess(string addr, OrderDishRequest request)
        {
            try
            {
                InfoLog.Instance.I("下单的请求Json：{0}", request.ToJson());
                var result = HttpHelper.HttpPost<JavaResponse>(addr, request);
                var enumResult = (EnumBookOrderResult)Convert.ToInt32(result.result);
                if (enumResult == EnumBookOrderResult.Success)
                    return null;

                if (!string.IsNullOrEmpty(result.msg))
                    return result.msg;

                switch (enumResult)
                {
                    case EnumBookOrderResult.MenuUpdating:
                        return "菜谱正在更新，请重启POS后再试。";
                    case EnumBookOrderResult.DishUpdating:
                        return "菜品正在更新，请重启POS后再试。";
                    default:
                        return string.Format("下单发生错误，{0}，请联系技术人员。", result.msg);
                }
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("下单时发生异常。", ex);
                return ex.MyMessage();
            }
        }

        public Tuple<string, List<BackDishInfo>> GetBackDishInfo(string orderid, string tableNo, string dishId,
            string dishUnit)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetBackDishInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<BackDishInfo>>("获取退菜信息地址为空。", null);

            var param = new List<string> { orderid, dishId, dishUnit, tableNo };
            var result = RestHttpHelper.HttpGet<GetBackDishInfoResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, List<BackDishInfo>>(result.Item1, null);

            if (result.Item2.Data == null || !result.Item2.Data.Any())
                return new Tuple<string, List<BackDishInfo>>("获取退菜信息失败。", null);

            var list = result.Item2.Data.Select(DataConverter.ToBackDishInfo).ToList();
            return new Tuple<string, List<BackDishInfo>>(null, list);
        }

        public string BackDish(BackDishComboInfo info)
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("BackDish");
                if (string.IsNullOrEmpty(addr))
                    return "退菜地址为空。";

                var request = DataConverter.ToBackDishRequest(info.OrderId, info.TableNo, info.AuthorizerUser, info.Waiter, info.DishInfo, info.BackDishNum, info.BackDishReason);
                var result = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.msg, "退菜失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("退菜时异常。", ex);
                return ex.MyMessage();
            }
        }

        public string BackAllDish(string orderId, string tableNo, string userId, string reason)
        {
            try
            {
                var addr = ServiceAddrCache.GetServiceAddr("BackDish");
                if (string.IsNullOrEmpty(addr))
                    return "退菜地址为空。";

                var request = new BackAllDishRequest
                {
                    orderNo = orderId,
                    currenttableid = tableNo,
                    discardUserId = userId,
                    discardReason = reason,
                    actionType = ((int)EnumBackDishType.All).ToString(),
                };
                var result = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.msg, "整桌退菜失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("整桌退菜时异常。", ex);
                return ex.MyMessage();
            }
        }

        public string PayTheBill(string orderId, string userId, List<BillPayInfo> payInfos)
        {
            var addr = ServiceAddrCache.GetServiceAddr("PayTheBill");
            if (string.IsNullOrEmpty(addr))
                return "获取结算地址为空。";

            var request = DataConverter.ToPayBillRequest(orderId, userId, payInfos);
            return PayTheBillProcess(addr, request);
        }

        public string PayTheBillCf(string orderId, string userId, List<BillPayInfo> payInfos)
        {
            var addr = ServiceAddrCache.GetServiceAddr("PayTheBillCf");
            if (string.IsNullOrEmpty(addr))
                return "获取咖啡结算地址为空。";

            var request = DataConverter.ToPayBillRequest(orderId, userId, payInfos);
            return PayTheBillProcess(addr, request);
        }

        private static string PayTheBillProcess(string addr, PayBillRequest request)
        {
            try
            {
                var result = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.msg, "结算失败，请联系管理员。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("结算时异常。", ex);
                return "结算失败。" + ex.MyMessage();
            }
        }

        public Tuple<string, PrintOrderFullInfo> GetPrintOrderInfo(string orderId, string userName,
            EnumPrintOrderType printType)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetPrintOrderInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, PrintOrderFullInfo>("获取打印用订单信息的地址为空。", null);

            List<string> param = new List<string> { userName, orderId, ((int)printType).ToString() };
            var result = RestHttpHelper.HttpGet<GetPrintOrderInfoResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, PrintOrderFullInfo>(result.Item1, null);

            if (!result.Item2.IsSuccess)
                return new Tuple<string, PrintOrderFullInfo>(DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "获取菜品信息失败。"), null);

            var data = DataConverter.ToPrintOrderFullInfo(result.Item2);
            return new Tuple<string, PrintOrderFullInfo>(null, data);
        }

        public string CheckCanAntiSettlement(string orderId, string userId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("CheckAntiSettleOrder");
            if (string.IsNullOrEmpty(addr))
                return "获取检测订单是否允许反结算的地址为空。";

            var param = new List<string> { userId, orderId };
            var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return result.Item1;

            return !result.Item2.IsSuccess ? DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "订单反结算检测失败。") : null;
        }

        public Tuple<string, GetOrderMemberInfoResponse> GetOrderMemberInfo(string orderId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetOrderMemberInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, GetOrderMemberInfoResponse>("获取订单会员信息地址为空。", null);

            try
            {
                var request = new GetOrderMemberInfoRequest { orderid = orderId };
                var response = HttpHelper.HttpPost<GetOrderMemberInfoResponse>(addr, request);
                return new Tuple<string, GetOrderMemberInfoResponse>(null, response);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("获取订单会员信息时异常。", ex);
                return new Tuple<string, GetOrderMemberInfoResponse>(ex.MyMessage(), null);
            }
        }

        public string SetMemberPrice(string orderId, string memberCardNo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SetMemberPrice");
            if (string.IsNullOrEmpty(addr))
                return "获取设置会员价的地址为空。";

            try
            {
                var param = new List<string> { Globals.UserInfo.UserName, orderId, PCInfoHelper.IPAddr, memberCardNo };
                var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
                if (!string.IsNullOrEmpty(result.Item1))
                    return result.Item1;

                return !result.Item2.IsSuccess ? DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "设置会员价失败。") : null;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("设置会员价时异常。", ex);
                return ex.MyMessage();
            }
        }

        public string SetNormalPrice(string orderId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SetNormalPrice");
            if (string.IsNullOrEmpty(addr))
                return "获取设置正常价的地址为空。";

            try
            {
                var param = new List<string> { Globals.UserInfo.UserName, orderId, PCInfoHelper.IPAddr };
                var result = RestHttpHelper.HttpGet<RestBaseResponse>(addr, param);
                if (!string.IsNullOrEmpty(result.Item1))
                    return result.Item1;

                return !result.Item2.IsSuccess ? DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "设置正常价失败。") : null;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("设置正常价时异常。", ex);
                return ex.MyMessage();
            }
        }

        public string ClearTable(string tableNo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("ClearTable");
            if (string.IsNullOrEmpty(addr))
                return "清台的地址为空。";

            try
            {
                var request = new ClearTableRequest { tableNo = tableNo };
                var response = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return !response.IsSuccess ? "清台失败。" : null;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("清台时异常。", ex);
                return string.Format("清台失败：{0}", ex.MyMessage());
            }
        }

        public string CancelOrder(string userId, string orderId, string tableNo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("CancelOrder");
            if (string.IsNullOrEmpty(addr))
                return "取消账单的地址为空。";

            try
            {
                var request = new List<string> { userId, orderId, tableNo };
                var response = RestHttpHelper.HttpGet<RestBaseResponse>(addr, request);
                if (!string.IsNullOrEmpty(response.Item1))
                    return response.Item1;

                return !response.Item2.IsSuccess ? "取消账单失败。" : null;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("取消账单时异常。", ex);
                return string.Format("取消账单失败：{0}", ex.MyMessage());
            }
        }

        public string ClearTableCf(string tableNo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("ClearTableCf");
            if (string.IsNullOrEmpty(addr))
                return "清台的地址为空。";

            try
            {
                var request = new ClearTableRequest { tableNo = tableNo };
                var response = HttpHelper.HttpPost<JavaResponse>(addr, request);
                return !response.IsSuccess ? "清台失败。" : null;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("清咖啡台失败。", ex);
                return string.Format("清台失败：{0}", ex.MyMessage());
            }
        }

        public string AntiSettlementOrder(string userName, string orderId, string reason)
        {
            var addr = ServiceAddrCache.GetServiceAddr("AntiSettlementOrder");
            if (string.IsNullOrEmpty(addr))
                return "反结算账单地址为空。";

            try
            {
                var request = new AntiSettlementOrderRequest
                {
                    userName = userName,
                    orderNo = orderId,
                    reason = reason,
                };
                var result = HttpHelper.HttpPost<JavaResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.msg, "反结算账单失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("反结算账单异常。", ex);
                return string.Format("反结算账单失败：{0}", ex.MyMessage());
            }
        }

        public string TipSettlement(string orderId, decimal tipAmount)
        {
            var addr = ServiceAddrCache.GetServiceAddr("TipBill");
            if (string.IsNullOrEmpty(addr))
                return "小费结算地址为空。";

            try
            {
                var request = new TipSettlementRequest
                {
                    orderid = orderId,
                    paid = tipAmount,
                };
                var result = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.msg, "小费结算失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("小费结算时异常。", ex);
                return string.Format("小费结算失败：{0}", ex.MyMessage());
            }
        }

        public string UpdateDishWeight(string orderId, string dishId, string primaryKey, decimal dishNum)
        {
            var addr = ServiceAddrCache.GetServiceAddr("UpdateDishWeigh");
            if (string.IsNullOrEmpty(addr))
                return "菜品称重地址为空。";

            try
            {
                var request = new UpdateDishWeightRequest
                {
                    orderId = orderId,
                    dishid = dishId,
                    dishnum = dishNum.ToString(CultureInfo.InvariantCulture),
                    primarykey = primaryKey,
                };
                var result = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.msg, "更新菜品称重数量失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("更新菜品称重数量时异常。", ex);
                return string.Format("更新菜品称重数量失败：{0}", ex.MyMessage());
            }
        }

        public string SetOrderTakeoutOrder(string orderId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SetOrderTakeout");
            if (string.IsNullOrEmpty(addr))
                return "设置订单为外卖单地址为空。";

            var request = new List<string> { orderId };
            var response = RestHttpHelper.HttpGet<RestBaseResponse>(addr, request);
            if (!string.IsNullOrEmpty(response.Item1))
                return response.Item1;

            return !response.Item2.IsSuccess ? DataHelper.GetNoneNullValueByOrder(response.Item2.Info, "设置订单为外卖单失败。") : null;
        }

        #region 优惠券处理

        /// <summary>
        /// 使用优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Tuple<string, PreferentialInfoResponse> UsePreferential(UsePreferentialRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("CalcDiscountAmount");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, PreferentialInfoResponse>("使用优惠券地址为空。", null);

            try
            {
                var result = HttpHelper.HttpPost<UsePreferentialResponse>(addr, request);
                return result.IsSuccess
                    ? new Tuple<string, PreferentialInfoResponse>(null, result.data)
                    : new Tuple<string, PreferentialInfoResponse>(DataHelper.GetNoneNullValueByOrder(result.msg, "使用优惠券失败。"), null);
                //这里判断错误取值是因为接口的问题，返回1表示成功，我表示很无语。
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("使用优惠券时异常。", ex);
                return new Tuple<string, PreferentialInfoResponse>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, PreferentialInfoResponse> DelPreferential(string orderId, string couponId = "")
        {
            var addr = ServiceAddrCache.GetServiceAddr("DelPreferential");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, PreferentialInfoResponse>("删除优惠券地址为空。", null);

            try
            {
                var request = new DelPreferentialRequest { orderid = orderId };
                if (string.IsNullOrEmpty(couponId))
                {
                    request.DetalPreferentiald = "";
                    request.clear = 1;
                }
                else
                {
                    request.DetalPreferentiald = couponId;
                    request.clear = 0;
                }

                var result = HttpHelper.HttpPost<DelePreferentialResponse>(addr, request);
                return result.IsSuccess
                   ? new Tuple<string, PreferentialInfoResponse>(null, result.data.preferentialInfo)
                    : new Tuple<string, PreferentialInfoResponse>(DataHelper.GetNoneNullValueByOrder(result.msg, "删除优惠券失败。"), null);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("删除优惠券时异常。", ex);
                return new Tuple<string, PreferentialInfoResponse>(ex.MyMessage(), null);
            }
        }

        public Tuple<string, List<DishGiftCouponInfo>> GetDishGiftCouponInfo(string orderId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetDishGiftCouponInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, List<DishGiftCouponInfo>>("获取赠菜优惠券使用信息地址为空。", null);

            try
            {
                var request = new GetDishGiftCouponInfoRequest { orderId = orderId };
                var result = HttpHelper.HttpPost<GetDishGiftCouponInfoResponse>(addr, request);
                if (result.IsSuccess)
                {
                    var list = result.data != null ? result.data.Select(DataConverter.ToDishGiftCouponInfo).ToList() : new List<DishGiftCouponInfo>();
                    return new Tuple<string, List<DishGiftCouponInfo>>(null, list);
                }
                return new Tuple<string, List<DishGiftCouponInfo>>(DataHelper.GetNoneNullValueByOrder(result.msg, "获取赠菜优惠券使用信息失败。"), null);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("获取赠菜优惠券使用信息时异常。", ex);
                return new Tuple<string, List<DishGiftCouponInfo>>(ex.MyMessage(), null);
            }
        }

        #endregion

        #region 账单处理

        /// <summary>
        /// 获取账单信息
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="tableNo">餐台号。</param>
        /// <param name="keepOdd">是否保留零头。</param>
        /// <returns></returns>
        public Tuple<string, TableFullInfo> GetOrderInfo(string orderId, string tableNo, int keepOdd)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetOrderInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, TableFullInfo>("获取餐台账单明细地址为空。", null);

            try
            {
                var param = new Dictionary<string, string> { { "orderid", orderId }, { "tableNo", tableNo }, { "itemid", keepOdd.ToString() } };
                var result = HttpHelper.HttpPost<GetOrderInfoResponse>(addr, param);
                return result.IsSuccess
                    ? new Tuple<string, TableFullInfo>(null, DataConverter.ToOrderInfo(result))
                    : new Tuple<string, TableFullInfo>(DataHelper.GetNoneNullValueByOrder(result.msg, "获取账单信息失败。"), null);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("获取账单信息时异常。", ex);
                return new Tuple<string, TableFullInfo>(ex.MyMessage(), null);
            }
        }

        #endregion

        public string SendMsgAsync(string orderId, EnumMsgType msgType)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SendMsgAsync");
            if (string.IsNullOrEmpty(addr))
                return "异步发送消息地址为空。";

            try
            {
                var request = new SendMsgAsyncRequest
                {
                    orderId = orderId,
                    type = (int)msgType,
                };
                var response = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return response.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(response.msg, "异步发送消息失败。");
            }
            catch (Exception ex)
            {
                return ex.MyMessage();
            }
        }

    }
}