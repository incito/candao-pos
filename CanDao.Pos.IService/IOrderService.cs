using System;
using System.Collections.Generic;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.IService
{
    /// <summary>
    /// 订单服务接口。
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// 获取餐台所有菜品信息。
        /// </summary>
        /// <param name="tableName">餐台名称。</param>
        /// <param name="userName">当前用户名。</param>
        /// <returns></returns>
        Tuple<string, TableFullInfo> GetTableDishInfoes(string tableName, string userName);

        /// <summary>
        /// 根据订单号获取餐台所有菜品信息。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="userName">当前用户名。</param>
        /// <returns></returns>
        Tuple<string, TableFullInfo> GetTableDishInfoByOrderId(string orderId, string userName);

        /// <summary>
        /// 获取订单发票抬头。
        /// </summary>
        /// <param name="orderId">订单id。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2如果不开发票，则返回null，否则返回发票抬头。</returns>
        Tuple<string, string> GetOrderInvoice(string orderId);

        /// <summary>
        /// 更新发票信息。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="invoiceAmount">发票金额。</param>
        /// <param name="cardNo">会员号。</param>
        /// <returns>更新成功返回null，否则返回错误信息。</returns>
        string UpdateOrderInvoice(string orderId, decimal invoiceAmount, string cardNo);

        /// <summary>
        /// 获取菜谱所有菜品信息。
        /// </summary>
        /// <param name="fullName">当前登录用户名称。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为菜谱中所有菜品集合。</returns>
        Tuple<string, List<MenuDishGroupInfo>> GetMenuDishInfos(string fullName);

        /// <summary>
        /// 检查菜品的状态，是否已经估清。
        /// </summary>
        /// <param name="dishId">菜品ID。</param>
        /// <param name="dishUnit">菜品单位。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为是否已经估清的标识，true为可用，false为已估清。</returns>
        Tuple<string, bool> CheckDishStatus(string dishId, string dishUnit);

        /// <summary>
        /// 根据优惠券类型获取该类型的所有优惠券。
        /// </summary>
        /// <param name="couponTypeId">优惠券类型编号。</param>
        /// <param name="userName">当前登录用户id。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为指定优惠券类型的所有优惠券集合。</returns>
        Tuple<string, List<CouponInfo>> GetCouponInfos(string couponTypeId, string userName);

        /// <summary>
        /// 计算折扣类优惠金额。
        /// </summary>
        /// <param name="request">请求参数类。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为计算后折扣优惠金额。</returns>
        Tuple<string, decimal> CalcDiscountAmount(CalcDiscountAmountRequest request);

        /// <summary>
        /// 保存优惠券使用列表。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="userName">当前登录用户Id。</param>
        /// <param name="couponInfos">优惠券信息集合。。</param>
        /// <returns>保存成功返回null，否则返回错误信息。</returns>
        string SaveUsedCoupon(string orderId, string userName, List<UsedCouponInfo> couponInfos);

        /// <summary>
        /// 获取保存的优惠券信息。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="userId">用户。</param>
        /// <returns></returns>
        Tuple<string, List<UsedCouponInfo>> GetSavedUsedCoupon(string orderId, string userId);

        /// <summary>
        /// 获取套餐信息。
        /// </summary>
        /// <param name="request">获取套餐信息的请求类。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为套餐全信息。</returns>
        Tuple<string, MenuComboFullInfo> GetMenuComboDishes(GetMenuComboDishRequest request);

        /// <summary>
        /// 获取鱼锅信息。
        /// </summary>
        /// <param name="dishId">鱼锅的菜品编号。</param>
        /// <returns></returns>
        Tuple<string, MenuFishPotFullInfo> GetFishPotDishInfo(string dishId);

        /// <summary>
        /// 菜品下单。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="tableNo">餐台号。</param>
        /// <param name="orderRemark">全单备注。</param>
        /// <param name="dishInfos">点的菜集合。</param>
        /// <returns>下单成功返回null，否则返回错误信息。</returns>
        string OrderDish(string orderId, string tableNo, string orderRemark, List<OrderDishInfo> dishInfos);

        /// <summary>
        /// 咖啡模式菜品下单。（后台处理逻辑是下单不打厨打单）
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="tableNo">餐台号。</param>
        /// <param name="orderRemark">全单备注。</param>
        /// <param name="dishInfos">点的菜集合。</param>
        /// <returns>下单成功返回null，否则返回错误信息。</returns>
        string OrderDishCf(string orderId, string tableNo, string orderRemark, List<OrderDishInfo> dishInfos);

        /// <summary>
        /// 获取退菜的菜品信息。
        /// </summary>
        /// <param name="orderid">订单号。</param>
        /// <param name="tableNo">餐台号。</param>
        /// <param name="dishId">菜品编号。</param>
        /// <param name="dishUnit">菜品单位。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为退菜的菜品信息。</returns>
        Tuple<string, List<BackDishInfo>> GetBackDishInfo(string orderid, string tableNo, string dishId, string dishUnit);

        /// <summary>
        /// 退菜。
        /// </summary>
        /// <param name="backDishComInfo">退菜组合信息。</param>
        /// <returns>退菜成功返回null，否则返回错误信息。</returns>
        string BackDish(BackDishComboInfo backDishComInfo);

        /// <summary>
        /// 退整桌菜。
        /// </summary>
        /// <param name="orderId">订单Id。</param>
        /// <param name="tableNo">餐桌名。</param>
        /// <param name="userId">授权用户Id。</param>
        /// <param name="reason">退菜原因。</param>
        /// <returns></returns>
        string BackAllDish(string orderId, string tableNo, string userId, string reason);

        /// <summary>
        /// 结账。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="userId">收银员编号。</param>
        /// <param name="payInfos">付款方式集合。</param>
        /// <returns></returns>
        string PayTheBill(string orderId, string userId, List<BillPayInfo> payInfos);

        /// <summary>
        /// 咖啡结账。（即结账后打印厨打单）
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="tableNo">餐桌号。</param>
        /// <param name="payInfos">付款方式集合。</param>
        /// <returns></returns>
        string PayTheBillCf(string orderId, string tableNo, List<BillPayInfo> payInfos);

        /// <summary>
        /// 获取打印用的订单全信息。
        /// </summary>
        /// <param name="orderId">订单Id。</param>
        /// <param name="userName">打印用户。</param>
        /// <param name="printType">打印类型。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为打印用的订单全信息。</returns>
        Tuple<string, PrintOrderFullInfo> GetPrintOrderInfo(string orderId, string userName, EnumPrintOrderType printType);

        /// <summary>
        /// 检测账单是否允许反结算。
        /// </summary>
        /// <param name="orderId">账单ID。</param>
        /// <param name="userId">用户ID。</param>
        /// <returns>允许反结算则返回null，否则返回错误原因。</returns>
        string CheckCanAntiSettlement(string orderId, string userId);

        /// <summary>
        /// 获取订单的会员信息。
        /// </summary>
        /// <param name="orderId">订单ID。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为订单的会员信息。</returns>
        Tuple<string, GetOrderMemberInfoResponse> GetOrderMemberInfo(string orderId);

        /// <summary>
        /// 设置会员价。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="memberCardNo">会员号。</param>
        /// <returns>设置成功返回null，失败返回错误信息。</returns>
        string SetMemberPrice(string orderId, string memberCardNo);

        /// <summary>
        /// 设置成普通价。（还原会员价）
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <returns>设置成功返回null，失败返回错误信息。</returns>
        string SetNormalPrice(string orderId);

        /// <summary>
        /// 清台。
        /// </summary>
        /// <param name="tableNo">餐台名。</param>
        /// <returns>清台成功返回null，否则返回错误信息。</returns>
        string ClearTable(string tableNo);

        /// <summary>
        /// 取消账单。
        /// </summary>
        /// <param name="userId">用户ID。</param>
        /// <param name="orderId">订单号。</param>
        /// <param name="tableNo">餐台名。</param>
        /// <returns>清台成功返回null，否则返回错误信息。</returns>
        string CancelOrder(string userId, string orderId, string tableNo);

        /// <summary>
        /// 咖啡模式的清台。
        /// </summary>
        /// <param name="tableNo">餐台名。</param>
        /// <returns>清台成功返回null，否则返回错误信息。</returns>
        string ClearTableCf(string tableNo);

        /// <summary>
        /// 反结算账单。
        /// </summary>
        /// <param name="userName">反结算用户。</param>
        /// <param name="orderId">订单Id。</param>
        /// <param name="reason">反结原因。</param>
        /// <returns>反结算成功返回null，否则返回错误信息。</returns>
        string AntiSettlementOrder(string userName, string orderId, string reason);

        /// <summary>
        /// 小费结算。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="tipAmount">实际小费金额。</param>
        /// <returns></returns>
        string TipSettlement(string orderId, decimal tipAmount);

        /// <summary>
        /// 菜品称重。
        /// </summary>
        /// <param name="tableNo">餐台名称。</param>
        /// <param name="dishId">称重菜品Id。</param>
        /// <param name="primaryKey">称重菜品Key。</param>
        /// <param name="dishNum">称重数量。</param>
        /// <returns></returns>
        string UpdateDishWeight(string tableNo, string dishId, string primaryKey, decimal dishNum);

        /// <summary>
        /// 设置订单为外卖单。
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        string SetOrderTakeoutOrder(string orderId);

        /// <summary>
        /// 获取餐台账单明细
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        Tuple<string, TableFullInfo> GetOrderInfo(string orderId, string tableNo, string itemid);

        /// <summary>
        /// 使用优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Tuple<string, preferentialInfoResponse> UsePreferential(UsePreferentialRequest request);

        /// <summary>
        /// 删除优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Tuple<string, preferentialInfoResponse> DelPreferential(DelPreferentialRequest request);
    }
}