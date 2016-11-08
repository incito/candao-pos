using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Models.VipModels;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;
using HttpHelper = CanDao.Pos.Common.HttpHelper;

namespace CanDao.Pos.ServiceImpl
{
    public class MemberServiceImpl : IMemberService
    {
        #region 餐道会员

        public Tuple<string, MemberInfo> QueryCandao(CanDaoMemberQueryRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("QueryCanDao");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("会员查询地址为空。");
                return new Tuple<string, MemberInfo>("会员查询地址为空。", null);
            }

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberQueryResponse>(addr, request);
                if (!response.IsSuccess)
                    return new Tuple<string, MemberInfo>(DataHelper.GetNoneNullValueByOrder(response.RetInfo, "会员查询失败。"), null);

                var result = DataConverter.ToMemberInfo(response);
                return new Tuple<string, MemberInfo>(null, result);
            }
            catch (Exception exp)
            {
                return new Tuple<string, MemberInfo>(exp.MyMessage(), null);
            }
        }

        public Tuple<string, CanDaoMemberStorageResponse> StorageCanDao(CanDaoMemberStorageRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("StorageCanDao");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("会员储值地址为空。");
                return new Tuple<string, CanDaoMemberStorageResponse>("会员储值地址为空。", null);
            }

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberStorageResponse>(addr, request);
                if (!response.IsSuccess)
                    return new Tuple<string, CanDaoMemberStorageResponse>(DataHelper.GetNoneNullValueByOrder(response.RetInfo, "会员储值失败。"), null);

                return new Tuple<string, CanDaoMemberStorageResponse>(null, response);
            }
            catch (Exception exp)
            {
                return new Tuple<string, CanDaoMemberStorageResponse>(exp.MyMessage(), null);
            }
        }

        public string ModifyPassword(CanDaoMemberModifyPasswordRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("ModifyPwdCanDao");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("修改会员密码地址为空。");
                return "修改会员密码地址为空。";
            }

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return !response.IsSuccess ? DataHelper.GetNoneNullValueByOrder(response.RetInfo, "修改餐道会员密码失败。") : null;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("修改会员密码时异常。", exp);
                return exp.MyMessage();
            }
        }

        public Tuple<string, SendVerifyCodeResponse> SendVerifyCode(SendVerifyCodeRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SendVerifyCode");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, SendVerifyCodeResponse>("发送验证码地址为空。", null);

            try
            {
                var response = HttpHelper.HttpPost<SendVerifyCodeResponse>(addr, request);
                if (!response.IsSuccess)
                {
                    var msg = DataHelper.GetNoneNullValueByOrder(response.RetInfo, "发送验证码失败。");
                    return new Tuple<string, SendVerifyCodeResponse>(msg, null);
                }

                return new Tuple<string, SendVerifyCodeResponse>(null, response);
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("发送验证码异常。", exp);
                return new Tuple<string, SendVerifyCodeResponse>(exp.MyMessage(), null);
            }
        }

        public string CheckMobileRepeat(CanDaoMemberCheckMobileRepeatRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("MobileRepeatCheck");
            if (string.IsNullOrEmpty(addr))
                return "检测会员手机号是否重复地址为空。";

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return response.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(response.RetInfo, "检测手机号是否重复时错误。");
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("检测会员手机号是否重复时异常。", exp);
                return exp.MyMessage();
            }
        }

        public string Regist(CanDaoMemberRegistRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("RegistCanDao");
            if (string.IsNullOrEmpty(addr))
            {
                var msg = "会员注册地址为空。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return response.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(response.RetInfo, "会员注册失败。");
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("会员注册时异常。", exp);
                return exp.MyMessage();
            }
        }

        public string ReportLoss(CanDaoMemberReportLossRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("ReportLossCanDao");
            if (string.IsNullOrEmpty(addr))
            {
                var msg = "会员挂失地址为空。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return response.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(response.RetInfo, "会员挂失失败。");
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("会员挂失时异常。", exp);
                return exp.MyMessage();
            }
        }

        public string Cancel(CanDaoMemberReportLossRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("CancelCanDao");
            if (string.IsNullOrEmpty(addr))
                return "会员注销地址为空。";

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return response.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(response.RetInfo, "会员注销失败。");
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("会员注销时异常。", exp);
                return exp.MyMessage();
            }
        }

        public Tuple<string, CanDaoMemberSaleResponse> Sale(CanDaoMemberSaleRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SaleCanDao");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, CanDaoMemberSaleResponse>("餐道会员消费结算地址为空。", null);

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberSaleResponse>(addr, request);
                if (!response.IsSuccess)
                    return new Tuple<string, CanDaoMemberSaleResponse>(DataHelper.GetNoneNullValueByOrder(response.RetInfo, "会员消费结算失败。"), null);

                return new Tuple<string, CanDaoMemberSaleResponse>(null, response);
            }
            catch (Exception exp)
            {
                return new Tuple<string, CanDaoMemberSaleResponse>(exp.MyMessage(), null);
            }
        }

        public Tuple<string, string> VoidSale(CanDaoMemberVoidSaleRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("VoidSaleCanDao");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, string>("餐道会员消费反结算地址为空。", null);

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberSaleResponse>(addr, request);
                if (!response.IsSuccess)
                    return new Tuple<string, string>(DataHelper.GetNoneNullValueByOrder(response.RetInfo, "会员消费反结算失败。"), null);

                return new Tuple<string, string>(null, response.TraceCode);
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("会员消费反结算时异常。", exp);
                return new Tuple<string, string>(string.Format("会员消费反结算时异常：{0}", exp.MyMessage()), null);
            }
        }

        public string UnVoidSale(CanDaoMemberUnVoidSaleRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("UnVoidSaleCanDao");
            if (string.IsNullOrEmpty(addr))
                return "餐道会员消费取消反结算地址为空。";

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                if (!response.IsSuccess)
                    return DataHelper.GetNoneNullValueByOrder(response.RetInfo, "会员消费取消反结算失败。");

                return null;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("会员消费取消反结算时异常。", exp);
                return string.Format("会员消费取消反结算时异常：{0}", exp.MyMessage());
            }
        }

        public string MemberLogin(string orderId, string memberCardNo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("MemberLogin");
            if (string.IsNullOrEmpty(addr))
                return "餐道会员登入地址为空。";

            try
            {
                var request = new CanDaoMemberLoginLogoutRequest()
                {
                    orderid = orderId,
                    mobile = memberCardNo,
                };
                var result = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.msg, "餐道会员登录失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("餐道会员登入时异常。", ex);
                return "餐道会员登入失败：" + ex.MyMessage();
            }
        }

        public string MemberLogout(string orderId, string memberCardNo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("MemberLogout");
            if (string.IsNullOrEmpty(addr))
                return "餐道会员登出地址为空。";

            try
            {
                var request = new CanDaoMemberLoginLogoutRequest()
                {
                    orderid = orderId,
                    mobile = memberCardNo,
                };
                var result = HttpHelper.HttpPost<NewHttpBaseResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.msg, "餐道会员登出失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("餐道会员登出时异常。", ex);
                return "餐道会员登出失败：" + ex.MyMessage();
            }
        }

        public string AddMemberSaleInfo(AddOrderMemberSaleInfoRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("AddMemberSaleInfo");
            if (string.IsNullOrEmpty(addr))
                return "添加餐道会员消费信息的地址为空。";

            try
            {
                var result = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return result.IsSuccess ? null : DataHelper.GetNoneNullValueByOrder(result.RetInfo, "添加餐道会员消费信息失败。");
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("添加餐道会员消费信息时异常。", ex);
                return "添加餐道会员消费信息失败：" + ex.MyMessage();
            }
        }

        public Tuple<string, PrintMemberPayInfo> GetMemberPrintPayInfo(string orderId, string userId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("GetMemberPrintPayInfo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, PrintMemberPayInfo>("获取打印用会员消费信息的地址为空。", null);

            try
            {
                var param = new List<string> { userId, orderId };
                var result = RestHttpHelper.HttpGet<GetMemberPayInfoResponse>(addr, param);
                if (!string.IsNullOrEmpty(result.Item1))
                    return new Tuple<string, PrintMemberPayInfo>(result.Item1, null);

                if (!result.Item2.IsSuccess)
                    return new Tuple<string, PrintMemberPayInfo>(DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "获取会员消费打印信息失败。"), null);

                var data = DataConverter.ToPrintMemberPayInfo(result.Item2.OrderJson.First());
                return new Tuple<string, PrintMemberPayInfo>(null, data);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("获取会员消费打印信息时异常。", ex);
                return new Tuple<string, PrintMemberPayInfo>(ex.MyMessage(), null);
            }
        }

        /// <summary>
        /// 会员查询（一户多卡）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Tuple<string, MVipInfo> VipQuery(CanDaoVipQueryRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("VipQuery");

            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("会员查询地址为空。");
                return new Tuple<string, MVipInfo>("会员查询地址为空。", null);
            }

            try
            {

                var parm = new Dictionary<string, string>();
                parm.Add("branch_id", request.branch_id);
                parm.Add("securityCode", request.securityCode);
                parm.Add("cardno", request.cardno);

                var response = HttpHelper.HttpPost<CanDaoVipQueryResponse>(addr, parm);
                if (!response.IsSuccess)
                    return new Tuple<string, MVipInfo>(DataHelper.GetNoneNullValueByOrder(response.retInfo, "会员查询失败。"), null);

                var result = DataConverter.ToVipInfo(response);
                return new Tuple<string, MVipInfo>(null, result);
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E("会员查询时异常。", ex);
                return new Tuple<string, MVipInfo>(ex.MyMessage(), null);
            }
        }

        /// <summary>
        /// 修改会员卡号
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="cardNum"></param>
        /// <param name="newCardNum"></param>
        /// <returns></returns>
        public string VipChangeCardNum(string branchId, string cardNum, string newCardNum)
        {
            var addr = ServiceAddrCache.GetServiceAddr("VipChangeCardNum");

            var parm = new Dictionary<string, string>
            {
                {"branch_id", branchId},
                {"securitycode", ""},
                {"cardno", cardNum},
                {"new_cardno", newCardNum}
            };

            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("修改会员卡号地址为空。");
                return "修改会员卡号地址为空。";
            }

            try
            {
                var response = HttpHelper.HttpPost<CanDaoVipBaseResponse>(addr, parm);
                return !response.IsSuccess ? DataHelper.GetNoneNullValueByOrder(response.retInfo, "修改会员卡号失败。") : null;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E("修改会员密码时异常。", exp);
                return exp.MyMessage();
            }
        }

        /// <summary>
        /// 修改会员基本信息
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="changeInfo"></param>
        /// <param name="newTelNum"></param>
        /// <returns></returns>
        public string VipChangeInfo(string branchId, MVipChangeInfo changeInfo, string newTelNum = "")
        {
            string msg = "修改会员基本信息";

            var addr = ServiceAddrCache.GetServiceAddr("VipChangeInfo");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {

                var parm = new Dictionary<string, object>();
                parm.Add("branch_id", branchId);
                parm.Add("securitycode", "");
                parm.Add("mobile", changeInfo.TelNum);
                parm.Add("new_mobile", newTelNum);
                parm.Add("password", changeInfo.Password);
                parm.Add("name", changeInfo.VipName);
                parm.Add("gender", changeInfo.Sex);
                parm.Add("birthday", changeInfo.Birthday);
                parm.Add("member_address", changeInfo.Address);

                var response = HttpHelper.HttpPost<CanDaoVipBaseResponse>(addr, parm);
                return !response.IsSuccess ? DataHelper.GetNoneNullValueByOrder(response.retInfo, msg + "失败。") : null;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return exp.MyMessage();
            }
        }

        /// <summary>
        /// 新增会员实体卡
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="cardno"></param>
        /// <param name="insideId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public string VipInsertCard(string branchId, string cardno, string insideId, string level = "0")
        {
            string msg = "新增会员实体卡";

            var addr = ServiceAddrCache.GetServiceAddr("VipInsertCard");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {

                var parm = new Dictionary<string, object>();
                parm.Add("entity_cardNo", cardno);
                parm.Add("mobile", insideId);
                parm.Add("branch_id", branchId);
                parm.Add("level", level);

                var response = HttpHelper.HttpPost<CanDaoVipBaseResponse>(addr, parm);
                return !response.IsSuccess ? DataHelper.GetNoneNullValueByOrder(response.retInfo, msg + "失败。") : null;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return exp.MyMessage();
            }
        }

        /// <summary>
        /// 检查实体卡是否存在
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="cardno"></param>
        /// <returns></returns>
        public string VipCheckCard(string branchId, string cardno)
        {
            string msg = "检查实体卡是否存在";

            var addr = ServiceAddrCache.GetServiceAddr("VipCheckCard");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {

                var parm = new Dictionary<string, object>();
                parm.Add("cardno", cardno);
                parm.Add("branch_id", branchId);

                var response = HttpHelper.HttpPost<VipCheckCardResponse>(addr, parm);
                return !response.IsSuccess ? DataHelper.GetNoneNullValueByOrder(response.msg, msg + "失败。") : null;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return exp.MyMessage();
            }
        }

        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="cardno"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string VipChangePsw(string branchId, string cardno, string password)
        {
            string msg = "密码修改";

            var addr = ServiceAddrCache.GetServiceAddr("VipChangePsw");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {
                var parm = new Dictionary<string, object>();
                parm.Add("mobile", cardno);
                parm.Add("branch_id", branchId);
                parm.Add("securitycode", "");
                parm.Add("password", password);

                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, parm);
                return !response.IsSuccess ? DataHelper.GetNoneNullValueByOrder(response.RetInfo, msg + "失败。") : null;
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return exp.MyMessage();
            }
        }

        /// <summary>
        /// 获取优惠列表
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Tuple<string, List<MVipCoupon>> GetCouponList(string branchId, string currentPage = "", string pageSize = "")
        {
            string msg = "获取优惠列表";

            var addr = ServiceAddrCache.GetServiceAddr("GetCouponList");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return new Tuple<string, List<MVipCoupon>>(msg + "地址为空。", null);
            }
            try
            {

                var parm = new Dictionary<string, object>();
                parm.Add("current", currentPage);
                parm.Add("branch_id", branchId);
                parm.Add("pageSize", pageSize);


                var response = HttpHelper.HttpPost<GetCouponListResponse>(addr, parm);
                var result = DataConverter.ToCouponList(response);
                return new Tuple<string, List<MVipCoupon>>(null, result);

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return new Tuple<string, List<MVipCoupon>>(exp.MyMessage(), null);
            }
        }

        #endregion

        #region 雅座会员

        public Tuple<string, YaZuoMemberInfo> QueryYaZuo(string memberNo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("QueryYaZuo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, YaZuoMemberInfo>("雅座会员查询地址为空。", null);

            var param = new List<string> { memberNo };
            var result = RestHttpHelper.HttpGet<YaZuoMemberQueryResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, YaZuoMemberInfo>(result.Item1, null);

            if (!result.Item2.IsSuccess)
                return new Tuple<string, YaZuoMemberInfo>(DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "会员查询失败。"), null);

            return new Tuple<string, YaZuoMemberInfo>(null, DataConverter.ToYaZuoMemberInfo(result.Item2));
        }

        public Tuple<string, YaZuoStorageInfo> StorageYaZuo(string memberNo, decimal storageValue, EnumStoragePayType payType)
        {
            var addr = ServiceAddrCache.GetServiceAddr("StorageYaZuo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, YaZuoStorageInfo>("雅座会员储值地址为空。", null);

            var param = new List<string>
            {
                Globals.UserInfo.UserName,
                memberNo,
                storageValue.ToString(CultureInfo.InvariantCulture),
                DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                "0",//填充字段
                ((int) payType).ToString()
            };
            var result = RestHttpHelper.HttpGet<YaZuoMemberStorageResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, YaZuoStorageInfo>(result.Item1, null);

            if (!result.Item2.IsSuccess)
                return new Tuple<string, YaZuoStorageInfo>(DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "其他储值失败错误。"), null);

            return new Tuple<string, YaZuoStorageInfo>(null, DataConverter.ToYaZuoStorageInfo(result.Item2));
        }

        public Tuple<string, YaZuoCardActiveInfo> CardActiveYaZuo(string cardNo, string cardPassword, string mobile)
        {
            var addr = ServiceAddrCache.GetServiceAddr("CardActiveYaZuo");
            if (string.IsNullOrEmpty(addr))
                return new Tuple<string, YaZuoCardActiveInfo>("雅座会员卡激活地址为空。", null);

            var param = new List<string>
            {
                cardNo,
                cardPassword,
                mobile
            };
            var result = RestHttpHelper.HttpGet<YaZuoCardActiveResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return new Tuple<string, YaZuoCardActiveInfo>(result.Item1, null);

            if (!result.Item2.IsSuccess)
                return new Tuple<string, YaZuoCardActiveInfo>(DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "其他会员卡激活失败。"), null);

            return new Tuple<string, YaZuoCardActiveInfo>(null, DataConverter.ToYaZuoCardActiveInfo(result.Item2));
        }

        public string SettlementYaZuo(YaZuoSettlementInfo settlementInfo)
        {
            var addr = ServiceAddrCache.GetServiceAddr("SettlementYaZuo");
            if (string.IsNullOrEmpty(addr))
                return "雅座会员消费地址为空。";

            var msg = settlementInfo.CheckDataValid();
            if (!string.IsNullOrEmpty(msg))
                return msg;

            var param = new List<string>
            {
                settlementInfo.UserId,
                settlementInfo.OrderId,
                settlementInfo.MemberCardNo,
                settlementInfo.OrderId,//接口文档来看，这里是收银软件生成的标识号，用订单号来处理
                settlementInfo.CashAmount.ToString(CultureInfo.InvariantCulture),
                settlementInfo.IntegralValue.ToString(CultureInfo.InvariantCulture),
                settlementInfo.TransType,
                settlementInfo.StoredPayAmount.ToString(CultureInfo.InvariantCulture),
                !string.IsNullOrEmpty(settlementInfo.CouponUsedInfo) ? settlementInfo.CouponUsedInfo : " ",
                DataHelper.GetNoneNullValueByOrder(settlementInfo.Password, "0"),
                settlementInfo.CouponTotalAmount.ToString(CultureInfo.InvariantCulture),
                SystemConfigCache.JavaServer,
            };

            var result = RestHttpHelper.HttpGet<YaZuoMemberBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return result.Item1;

            return !result.Item2.IsSuccess ? DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "会员消费失败。") : null;
        }

        public string AntiSettlementYaZuo(string orderId)
        {
            var addr = ServiceAddrCache.GetServiceAddr("AntiSettlementYaZuo");
            if (string.IsNullOrEmpty(addr))
                return "雅座会员反结算地址为空。";

            var param = new List<string>
            {
                orderId,
                "0",//密码
                "11111111"//超级密码
            };
            var result = RestHttpHelper.HttpGet<YaZuoMemberBaseResponse>(addr, param);
            if (!string.IsNullOrEmpty(result.Item1))
                return result.Item1;

            return !result.Item2.IsSuccess ? DataHelper.GetNoneNullValueByOrder(result.Item2.Info, "雅座会员反结算失败。") : null;
        }

        #endregion
    }
}