using System;
using System.Collections.Generic;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;
using JunLan.Common.Base;
using HttpHelper = CanDao.Pos.Common.HttpHelper;

namespace CanDao.Pos.ServiceImpl
{
    public class MemberServiceImpl : IMemberService
    {
        public Tuple<string, MemberInfo> QueryCanndao(CanDaoMemberQueryRequest request)
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
                    return new Tuple<string, MemberInfo>(response.RetInfo ?? "会员查询失败。", null);

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
                    return new Tuple<string, CanDaoMemberStorageResponse>(response.RetInfo ?? "储值失败。", null);

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
                return !response.IsSuccess ? response.RetInfo ?? "修改餐道会员密码失败。" : null;
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
                    var msg = string.Format("发送验证码失败。{0}", response.RetInfo ?? "发送验证码失败。");
                    ErrLog.Instance.E(msg);
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
            {
                var msg = "检测会员手机号是否重复地址为空。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return response.IsSuccess ? null : (!string.IsNullOrEmpty(response.RetInfo) ? response.RetInfo : "检测手机号是否重复时错误。");
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
                return response.IsSuccess ? null : (string.IsNullOrEmpty(response.RetInfo) ? "会员注册失败。" : response.RetInfo);
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
                return response.IsSuccess ? null : (string.IsNullOrEmpty(response.RetInfo) ? "会员挂失失败。" : response.RetInfo);
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
                return response.IsSuccess ? null : (string.IsNullOrEmpty(response.RetInfo) ? "会员注销失败。" : response.RetInfo);
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
                return new Tuple<string, CanDaoMemberSaleResponse>("餐道会员消费地址为空。", null);

            try
            {
                var response = HttpHelper.HttpPost<CanDaoMemberSaleResponse>(addr, request);
                if (!response.IsSuccess)
                    return new Tuple<string, CanDaoMemberSaleResponse>(!string.IsNullOrEmpty(response.RetInfo) ? response.RetInfo : "会员消费失败。", null);

                return new Tuple<string, CanDaoMemberSaleResponse>(null, response);
            }
            catch (Exception exp)
            {
                return new Tuple<string, CanDaoMemberSaleResponse>(exp.MyMessage(), null);
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
                var result = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return result.IsSuccess ? null : (string.IsNullOrEmpty(result.RetInfo) ? "餐道会员登录失败。" : result.RetInfo);
            }
            catch (Exception ex)
            {
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
                var result = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, request);
                return result.IsSuccess ? null : (string.IsNullOrEmpty(result.RetInfo) ? "餐道会员登出失败。" : result.RetInfo);
            }
            catch (Exception ex)
            {
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
                return result.IsSuccess ? null : (string.IsNullOrEmpty(result.RetInfo) ? "添加餐道会员消费信息失败。" : result.RetInfo);
            }
            catch (Exception ex)
            {
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
                    return new Tuple<string, PrintMemberPayInfo>(string.IsNullOrEmpty(result.Item2.Info) ? "获取会员消费打印信息失败。" : result.Item2.Info, null);

                var data = DataConverter.ToPrintMemberPayInfo(result.Item2.OrderJson.First());
                return new Tuple<string, PrintMemberPayInfo>(null, data);
            }
            catch (Exception ex)
            {
                return new Tuple<string, PrintMemberPayInfo>(ex.MyMessage(), null);
            }
        }

        /// <summary>
        /// 会员查询（一户多卡）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Tuple<string, Common.Models.VipModels.MVipInfo> VipQuery(CanDaoVipQueryRequest request)
        {
            var addr = ServiceAddrCache.GetServiceAddr("VipQuery");

            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("会员查询地址为空。");
                return new Tuple<string, Common.Models.VipModels.MVipInfo>("会员查询地址为空。", null);
            }

            try
            {

                var parm = new Dictionary<string, string>();
                parm.Add("branch_id", request.branch_id);
                parm.Add("securityCode", request.securityCode);
                parm.Add("cardno", request.cardno);

                var response = HttpHelper.HttpPost<CanDaoVipQueryResponse>(addr, parm);
                if (!response.IsSuccess)
                    return new Tuple<string, Common.Models.VipModels.MVipInfo>(response.retInfo ?? "会员查询失败。", null);

                var result = DataConverter.ToVipInfo(response);
                return new Tuple<string, Common.Models.VipModels.MVipInfo>(null, result);
            }
            catch (Exception exp)
            {
                return new Tuple<string, Common.Models.VipModels.MVipInfo>(exp.MyMessage(), null);
            }
        }


       /// <summary>
       /// 修改会员卡号
       /// </summary>
       /// <param name="branch_id"></param>
       /// <param name="cardNum"></param>
       /// <param name="newCardNum"></param>
       /// <returns></returns>
        public string VipChangeCardNum(string branch_id, string cardNum, string newCardNum)
        {
           var addr = ServiceAddrCache.GetServiceAddr("VipChangeCardNum");

           var parm = new Dictionary<string, string>();
           parm.Add("branch_id", branch_id);
           parm.Add("securitycode", "");
           parm.Add("cardno", cardNum);
           parm.Add("new_cardno", newCardNum);

            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E("修改会员卡号地址为空。");
                return "修改会员卡号地址为空。";
            }

            try
            {
                var response = HttpHelper.HttpPost<CanDaoVipBaseResponse>(addr, parm);
                return !response.IsSuccess ? response.retInfo ?? "修改会员卡号失败。" : null;
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
        /// <param name="branch_id"></param>
        /// <param name="changeInfo"></param>
        /// <param name="newTelNum"></param>
        /// <returns></returns>
        public string VipChangeInfo(string branch_id, Common.Models.VipModels.MVipChangeInfo changeInfo, string newTelNum = "")
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
                parm.Add("branch_id", branch_id);
                parm.Add("securitycode", "");
                parm.Add("mobile", changeInfo.TelNum);
                parm.Add("new_mobile", newTelNum);
                parm.Add("password", changeInfo.Password);
                parm.Add("name", changeInfo.VipName);
                parm.Add("gender", changeInfo.Sex);
                parm.Add("birthday", changeInfo.Birthday);
                parm.Add("member_address", changeInfo.Address);


                var response = HttpHelper.HttpPost<CanDaoVipBaseResponse>(addr, parm);
                return !response.IsSuccess ? response.retInfo ?? msg + "失败。" : null;
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
        /// <param name="branch_id"></param>
        /// <param name="cardno"></param>
        /// <param name="insideId"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public string VipInsertCard(string branch_id, string cardno, string insideId, string level = "0")
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
                parm.Add("branch_id", branch_id);
                parm.Add("level", level);

                var response = HttpHelper.HttpPost<CanDaoVipBaseResponse>(addr, parm);
                return !response.IsSuccess ? response.retInfo ?? msg + "失败。" : null;
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
        /// <param name="branch_id"></param>
        /// <param name="cardno"></param>
        /// <returns></returns>
        public string VipCheckCard(string branch_id, string cardno)
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
                parm.Add("branch_id", branch_id);

                var response = HttpHelper.HttpPost<VipCheckCardResponse>(addr, parm);
                return !response.IsSuccess ? response.msg ?? msg + "失败。" : null;
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
        /// <param name="branch_id"></param>
        /// <param name="cardno"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string VipChangePsw(string branch_id, string cardno, string password)
        {
            string msg = "密码修改";

            var addr = ServiceAddrCache.GetServiceAddr("VipCheckCard");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return msg + "地址为空。";
            }
            try
            {

                var parm = new Dictionary<string, object>();
                parm.Add("mobile", cardno);
                parm.Add("branch_id", branch_id);
                parm.Add("securitycode", "");
                parm.Add("password", password);

                var response = HttpHelper.HttpPost<CanDaoMemberBaseResponse>(addr, parm);
                return !response.IsSuccess ? response.RetInfo ?? msg + "失败。" : null;
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
        /// <param name="branch_id"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Tuple<string, List<Common.Models.VipModels.MVipCoupon>> GetCouponList(string branch_id, string currentPage = "", string pageSize = "")
        {
            string msg = "获取优惠列表";

            var addr = ServiceAddrCache.GetServiceAddr("GetCouponList");
            if (string.IsNullOrEmpty(addr))
            {
                ErrLog.Instance.E(msg + "地址为空。");
                return new Tuple<string, List<Common.Models.VipModels.MVipCoupon>>(msg + "地址为空。", null);
            }
            try
            {

                var parm = new Dictionary<string, object>();
                parm.Add("current", currentPage);
                parm.Add("branch_id", branch_id);
                parm.Add("pageSize", pageSize);


                var response = HttpHelper.HttpPost<GetCouponListResponse>(addr, parm);
                var result = DataConverter.ToCouponList(response);
                return new Tuple<string, List<Common.Models.VipModels.MVipCoupon>>(null, result);

            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(msg + "时异常。", exp);
                return new Tuple<string, List<Common.Models.VipModels.MVipCoupon>>(exp.MyMessage(), null);
            }
        }
    }
}