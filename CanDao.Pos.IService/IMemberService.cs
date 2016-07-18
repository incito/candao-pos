using System;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.IService
{
    public interface IMemberService
    {
        /// <summary>
        /// 餐道会员查询。
        /// </summary>
        /// <param name="request">查询请求类。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为会员信息。</returns>
        Tuple<string, MemberInfo> QueryCanndao(CanDaoMemberQueryRequest request);

        /// <summary>
        /// 餐道会员储值。
        /// </summary>
        /// <param name="request">储值请求类。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为会员储值返回类。</returns>
        Tuple<string, CanDaoMemberStorageResponse> StorageCanDao(CanDaoMemberStorageRequest request);

        /// <summary>
        /// 修改密码。
        /// </summary>
        /// <param name="request"></param>
        /// <returns>修改成功返回null，否则返回错误信息。</returns>
        string ModifyPassword(CanDaoMemberModifyPasswordRequest request);

        /// <summary>
        /// 发送验证码。
        /// </summary>
        /// <param name="request">发送验证码请求类。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为发送验证码返回类。</returns>
        Tuple<string, SendVerifyCodeResponse> SendVerifyCode(SendVerifyCodeRequest request);

        /// <summary>
        /// 检测手机号是否重复。
        /// </summary>
        /// <param name="request"></param>
        /// <returns>没有重复则返回null，否则返回错误信息。</returns>
        string CheckMobileRepeat(CanDaoMemberCheckMobileRepeatRequest request);

        /// <summary>
        /// 餐道会员注册。
        /// </summary>
        /// <param name="request">注册请求类。</param>
        /// <returns>注册成功返回null，否则返回错误信息。</returns>
        string Regist(CanDaoMemberRegistRequest request);

        /// <summary>
        /// 挂失。
        /// </summary>
        /// <param name="request">挂失请求类。</param>
        /// <returns></returns>
        string ReportLoss(CanDaoMemberReportLossRequest request);

        /// <summary>
        /// 注销。
        /// </summary>
        /// <param name="request">同挂失请求类。</param>
        /// <returns></returns>
        string Cancel(CanDaoMemberReportLossRequest request);

        /// <summary>
        /// 会员消费。
        /// </summary>
        /// <param name="request">会员消费请求类。</param>
        /// <returns>Item1当遇到错误时为错误信息，否则为null。Item2为会员消费返回类。</returns>
        Tuple<string, CanDaoMemberSaleResponse> Sale(CanDaoMemberSaleRequest request);

        /// <summary>
        /// 会员登录。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="memberCardNo">会员号。</param>
        /// <returns></returns>
        string MemberLogin(string orderId, string memberCardNo);

        /// <summary>
        /// 会员登出。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="memberCardNo">会员号。</param>
        /// <returns></returns>
        string MemberLogout(string orderId, string memberCardNo);

        /// <summary>
        /// 添加会员消费信息。
        /// </summary>
        /// <param name="request">添加会员消费信息请求类。</param>
        /// <returns>添加成功返回null，否则返回错误信息。</returns>
        string AddMemberSaleInfo(AddOrderMemberSaleInfoRequest request);

        /// <summary>
        /// 获取会员消费的打印信息。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="userId">打印的用户id。</param>
        /// <returns></returns>
        Tuple<string, PrintMemberPayInfo> GetMemberPrintPayInfo(string orderId, string userId);
    }
}