using System;
using System.IO;
using Common;
using Models;
using Models.CandaoMember;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebServiceReference
{
    public class CanDaoMemberClient
    {
        /*
         * 1、 操作员登录
           2、 注册 
           3、 消费
           4、 取消交易
           5、 查询
           6、 储值
           7、 挂失
           8、 解除挂失
           9、 修改密码
           10、 操作员退出登录
           11、 会员资料修改
           12、 会员注销
           13、 发送短信验证码
           14、获取分店ID号 memberManager/findBranchid
           15、手机号重复验证 memberManager/validateTbMemberManager
            
           //////////分店部份接口
             
           16、MemberLogin.json
           17、MemberLogout.json
           18、AddOrderMember.json
           19、DeleteOrderMember.json
           20、GetOrderMember.json
         
           */
        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="loginType"></param>
        /// <returns></returns>
        public static TCandaoRetBase OpLogin(string branchId, string userId, string password, int loginType)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            AllLog.Instance.I(string.Format("【OpLogin】 branchId：{0}，userId：{1}，loginType：{2}。", branchId, userId, loginType));
            string address = String.Format("http://{0}/padinterface/getMenuCombodish.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branchId);
            writer.WritePropertyName("userid");
            writer.WriteValue(userId);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WritePropertyName("LoginType");
            writer.WriteValue(loginType);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【OpLogin】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            ret.Securitycode = ja["SecurityCode"].ToString();
            return ret;
        }
        /// <summary>
        /// 餐道会员注册
        /// </summary>
        /// <param name="memberinfo"></param>
        /// <returns></returns>
        public static TCandaoRetBase MemberReg(TCandaoRegMemberInfo memberinfo)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            AllLog.Instance.I(string.Format("【MemberReg】 branchId：{0}，mobile：{1}，cardno：{2}。", memberinfo.Branch_id, memberinfo.Mobile, memberinfo.Cardno));
            string address = String.Format("http://{0}/member/memberManager/save.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(memberinfo.Branch_id.ToString());
            writer.WritePropertyName("securityCode");
            writer.WriteValue(memberinfo.Securitycode);
            writer.WritePropertyName("mobile");
            writer.WriteValue(memberinfo.Mobile);
            writer.WritePropertyName("cardno");
            writer.WriteValue(memberinfo.Cardno);
            writer.WritePropertyName("password");
            writer.WriteValue(memberinfo.Password);
            writer.WritePropertyName("name");
            writer.WriteValue(memberinfo.Name);
            writer.WritePropertyName("gender");
            writer.WriteValue(memberinfo.Gender);
            writer.WritePropertyName("birthday");
            writer.WriteValue(memberinfo.Birthday);
            //writer.WritePropertyName("regtype");
            //writer.WriteValue(memberinfo.Regtype);
            writer.WritePropertyName("member_avatar");
            writer.WriteValue(memberinfo.Member_avatar);
            WriteObject(ref writer, "channel", "0");
            WriteObject(ref writer, "tenant_id", "");
            WriteObject(ref writer, "createuser", Globals.UserInfo.UserName);
            WriteObject(ref writer, "updateuser", "");
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【MemberReg】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            //if (ret.Retcode.Equals("0"))
            //  ret.Cardno = ja["cardno"].ToString();
            return ret;
        }
        /// <summary>
        /// 会员消费
        /// </summary>
        /// <param name="membersale"></param>
        /// <returns></returns>
        public static TCandaoRet_Sale MemberSale(TCandaoMemberSale membersale)
        {
            TCandaoRet_Sale ret = new TCandaoRet_Sale();
            string address = String.Format("http://{0}/member/deal/MemberSale.json", WebServiceReference.Candaomemberserver);//http://10.10.2.200:8080/member/deal/MemberSale.json
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(membersale.Branch_id);
            writer.WritePropertyName("securityCode");
            writer.WriteValue(membersale.Securitycode);
            writer.WritePropertyName("cardno");
            writer.WriteValue(membersale.Cardno);
            writer.WritePropertyName("password");
            writer.WriteValue(membersale.Password);
            writer.WritePropertyName("Serial");
            writer.WriteValue(membersale.Serial);
            writer.WritePropertyName("FCash");
            writer.WriteValue(membersale.Fcash);
            writer.WritePropertyName("FIntegral");
            writer.WriteValue(membersale.Fintegral);
            writer.WritePropertyName("FStore");
            writer.WriteValue(membersale.Fstore);
            writer.WritePropertyName("FTicketList");
            writer.WriteValue(membersale.Fticketlist);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【MemberSale】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【MemberSale】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            try
            {
                if (ret.Retcode.Equals("1"))
                {
                    ret.Ret = false; return ret;
                }
            }
            catch { }

            ret.Tracecode = ja["TraceCode"].ToString();
            ret.Storecardbalance = decimal.Parse(ja["StoreCardBalance"].ToString());
            ret.Integraloverall = decimal.Parse(ja["IntegralOverall"].ToString());
            ret.Addintegral = decimal.Parse(ja["AddIntegral"].ToString());
            ret.Decintegral = decimal.Parse(ja["DecIntegral"].ToString());
            ret.Inflatedrate = decimal.Parse(ja["InflatedRate"].ToString());
            ret.Netamount = decimal.Parse(ja["NetAmount"].ToString());
            return ret;
        }

        /// <summary>
        /// 取消交易
        /// </summary>
        /// <param name="voidsale"></param>
        /// <returns></returns>
        public static TCandaoRet_VoidSale VoidSale(TCandaoMemberVoidSale voidsale)
        {
            TCandaoRet_VoidSale ret = new TCandaoRet_VoidSale();
            string address = String.Format("http://{0}/member/deal/VoidSale.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(voidsale.Branch_id);
            writer.WritePropertyName("securityCode");
            writer.WriteValue(voidsale.Securitycode);
            writer.WritePropertyName("cardno");
            writer.WriteValue(voidsale.Cardno);
            writer.WritePropertyName("password");
            writer.WriteValue(voidsale.Password);
            writer.WritePropertyName("Serial");
            writer.WriteValue(voidsale.Serial);
            writer.WritePropertyName("TraceCode");
            writer.WriteValue(voidsale.Tracecode);
            writer.WritePropertyName("SUPERPWD");
            writer.WriteValue(voidsale.Superpwd);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【VoidSale】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【VoidSale】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            ret.Tracecode = ja["TraceCode"].ToString();
            ret.StoreCardbalance = decimal.Parse(ja["StoreCardBalance"].ToString());
            ret.Integraloverall = decimal.Parse(ja["IntegralOverall"].ToString());
            ret.Integral = decimal.Parse(ja["Integral"].ToString());
            ret.Store = decimal.Parse(ja["Store"].ToString());
            ret.Useintegral = decimal.Parse(ja["UseIntegral"].ToString());
            return ret;
        }

        /// <summary>
        /// 会员查询
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <param name="cardno"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static TCandaoMemberInfo QueryBalance(string branch_id, string securitycode, string cardno, string password)
        {
            TCandaoMemberInfo ret = new TCandaoMemberInfo();
            string address = String.Format("http://{0}/member/memberManager/findByParams", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branch_id);
            writer.WritePropertyName("securityCode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("cardno");
            writer.WriteValue(cardno);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【QueryBalance】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【QueryBalance】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            try
            {
                String Retcode = ja["Retcode"].ToString();
                String RetInfo = ja["RetInfo"].ToString();
                if (Retcode.Equals("1"))
                {
                    ret.Ret = false; ret.Retcode = "1"; ret.Retinfo = RetInfo; return ret;
                }
            }
            catch { }

            try
            {
                ret.Mcard = ja["CardList"].ToString();//mobile MCard
            }
            catch { ret.Ret = false; ret.Retcode = "1"; ret.Retinfo = "查询失败，服务器未返回数据！"; return ret; }
            if (ret.Mcard.Equals(""))
            {
                ret.Ret = false; ret.Retcode = "1"; ret.Retinfo = "查询失败，服务器未返回会员卡号！"; return ret;
            }
            ret.Retcode = "0";
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = "";
            ret.Storecardbalance = decimal.Parse(ja["StoreCardBalance"].ToString());
            ret.Integraloverall = decimal.Parse(ja["IntegralOverall"].ToString());
            try
            {
                ret.Couponsoverall = decimal.Parse(ja["CouponsOverall"].ToString());
            }
            catch { ret.Couponsoverall = 0; }
            ret.Ticketinfo = ja["TicketInfo"].ToString();
            ret.Ticketinfo = ja["TraceCode"].ToString();
            try
            {
                ret.Cardtype = int.Parse(ja["CardType"].ToString());
            }
            catch { ret.Cardtype = 0; }
            ret.Regdate = ja["RegDate"].ToString();
            ret.Cardlist = ja["CardList"].ToString();
            ret.Member_avatar = ja["member_avatar"].ToString();
            ret.Cardlevel = "0";// ja["CardLevel"].ToString();
            try
            {
                ret.Cardlevel = ja["CardLevel"].ToString();
            }
            catch { }
            JArray jr = (JArray)JsonConvert.DeserializeObject(ret.Cardlist);
            JObject jaCard = (JObject)jr[0];
            ret.Mcard = jaCard["cardno"].ToString();
            ret.Mobile = ja["mobile"].ToString();
            ret.Name = ja["name"].ToString();
            try
            {
                ret.Gender = int.Parse(ja["gender"].ToString());
            }
            catch { ret.Gender = 0; }
            ret.Birthday = ja["birthday"].ToString();
            ret.Memberaddress = ja["MemberAddress"].ToString();
            ret.Cardno = jaCard["cardno"].ToString();
            return ret;
        }

        /// <summary>
        /// 会员储值
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <param name="cardno"></param>
        /// <param name="serial"></param>
        /// <param name="Amount"></param>
        /// <param name="transtype"></param>
        /// <param name="chargetype"></param>
        /// <returns></returns>
        public static TCandaoRet_StoreCardDeposit StoreCardDeposit(string branch_id, string securitycode, string cardno, string serial, decimal Amount, int transtype, int chargetype)
        {
            TCandaoRet_StoreCardDeposit ret = new TCandaoRet_StoreCardDeposit();
            string address = String.Format("http://{0}/member/deal/StoreCardDeposit.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("SecurityCode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("Serial");
            writer.WriteValue(serial);
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branch_id);
            writer.WritePropertyName("cardno");
            writer.WriteValue(cardno);
            writer.WritePropertyName("Amount");
            writer.WriteValue(Amount);
            writer.WritePropertyName("TransType");
            writer.WriteValue(transtype.ToString());
            writer.WritePropertyName("ChargeType");
            writer.WriteValue(chargetype.ToString());
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【StoreCardDeposit】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【StoreCardDeposit】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            if (!ret.Retcode.Equals("0"))
            {
                return ret;
            }
            ret.Tracecode = ja["TraceCode"].ToString();
            ret.StoreCardbalance = decimal.Parse(ja["StoreCardBalance"].ToString());
            ret.Giftamount = decimal.Parse(ja["GiftAmount"].ToString());
            ret.Integral = 0;// decimal.Parse();//ja["Integral"].ToString()
            return ret;
        }
        /// <summary>
        /// 挂失卡
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <param name="cardno"></param>
        /// <param name="fmemo"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static TCandaoRet_CardLose CardLose(string branch_id, string securitycode, string cardno, string fmemo, string password)
        {
            TCandaoRet_CardLose ret = new TCandaoRet_CardLose();
            string address = String.Format("http://{0}/member/deal/CardLose.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branch_id);
            writer.WritePropertyName("securityCode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("cardno");
            writer.WriteValue(cardno);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WritePropertyName("FMemo");
            writer.WriteValue(fmemo);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【CardLose】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【CardLose】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            ret.StoreCardbalance = decimal.Parse(ja["value"].ToString());
            ret.Integraloverall = decimal.Parse(ja["point"].ToString());
            ret.Couponsoverall = 0;// decimal.Parse(ja["CouponsOverall"].ToString());
            return ret;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <param name="cardno"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static TCandaoRetBase ChangePwd(string branch_id, string securitycode, string cardno, string password)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/" + RestClient.ApiPath + "/padinterface/ChangePwd.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branch_id);
            writer.WritePropertyName("securitycode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WritePropertyName("cardno");
            writer.WriteValue(cardno);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【ChangePwd】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【ChangePwd】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            return ret;
        }

        /// <summary>
        /// 解除挂失
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <param name="cardno"></param>
        /// <param name="password"></param>
        /// <param name="fmemo"></param>
        /// <returns></returns>
        public static TCandaoRetBase UnCardLose(string branch_id, string securitycode, string cardno, string password, string fmemo)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/" + RestClient.ApiPath + "/padinterface/UnCardLose.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branch_id);
            writer.WritePropertyName("securitycode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WritePropertyName("cardno");
            writer.WriteValue(cardno);
            writer.WritePropertyName("fmemo");
            writer.WriteValue(fmemo);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【UnCardLose】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【UnCardLose】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            return ret;
        }

        /// <summary>
        /// 10、 操作员退出登录
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static TCandaoRetBase Logout(string branch_id, string securitycode, string userid)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/" + RestClient.ApiPath + "/padinterface/Logout.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branch_id);
            writer.WritePropertyName("securitycode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("userid");
            writer.WriteValue(userid);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【Logout】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【Logout】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            return ret;
        }

        /// <summary>
        /// 会员资料编辑
        /// </summary>
        /// <param name="memberinfo"></param>
        /// <returns></returns>
        public static TCandaoRetBase MemberEdit(TCandaoRegMemberInfo memberinfo)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/member/memberManager/MemberEdit.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(memberinfo.Branch_id.ToString());
            writer.WritePropertyName("securitycode");
            writer.WriteValue(memberinfo.Securitycode);
            writer.WritePropertyName("mobile");
            writer.WriteValue(memberinfo.Mobile);
            writer.WritePropertyName("cardno");
            writer.WriteValue(memberinfo.Cardno);
            writer.WritePropertyName("password");
            writer.WriteValue(memberinfo.Password);
            writer.WritePropertyName("name");
            writer.WriteValue(memberinfo.Name);
            writer.WritePropertyName("gender");
            writer.WriteValue(memberinfo.Gender);
            writer.WritePropertyName("birthday");
            writer.WriteValue(memberinfo.Birthday);
            writer.WritePropertyName("member_avatar");
            writer.WriteValue(memberinfo.Member_avatar);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【MemberEdit】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【MemberEdit】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            try
            {
                ret.Cardno = ja["cardno"].ToString();
            }
            catch { }
            return ret;
        }

        /// <summary>
        /// 会员注销
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <param name="cardno"></param>
        /// <param name="fmemo"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static TCandaoRet_CardLose CardCancellation(string branch_id, string securitycode, string cardno, string fmemo, string password)
        {
            TCandaoRet_CardLose ret = new TCandaoRet_CardLose();
            string address = String.Format("http://{0}/member/memberManager/delete.json", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branch_id);
            writer.WritePropertyName("securityCode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("cardno");
            writer.WriteValue(cardno);
            writer.WritePropertyName("password");
            writer.WriteValue(password);
            writer.WritePropertyName("FMemo");
            writer.WriteValue(fmemo);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【CardCancellation】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【CardCancellation】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            if (!ret.Retcode.Equals("0")) { return ret; }
            try
            {
                ret.StoreCardbalance = decimal.Parse(ja["StoreCardBalance"].ToString());
                ret.Integraloverall = decimal.Parse(ja["IntegralOverall"].ToString());
                ret.Couponsoverall = 0;// decimal.Parse(ja["CouponsOverall"].ToString());
            }
            catch { }
            return ret;
        }

        /// <summary>
        /// 获取短信验证码
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static TCandaoRetBase SendAccountByMobile(string branch_id, string securitycode, string mobile, out string valicode)
        {
            valicode = "";
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/member/memberManager/sendAccountByMobile", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branch_id);
            writer.WritePropertyName("securityCode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("mobile");
            writer.WriteValue(mobile);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【SendAccountByMobile】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【SendAccountByMobile】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            valicode = ja["valicode"].ToString();
            return ret;
        }
        /// <summary>
        /// 获取分店号
        /// </summary>
        /// <param name="branch_id"></param>
        /// <param name="securitycode"></param>
        /// <returns></returns>
        public static TCandaoRetBase findBranchid(out string branch_id, string securitycode)
        {
            branch_id = "";
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/member/memberManager/findBranchid", RestClient.JavaServer);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("securityCode");
            writer.WriteValue(securitycode);
            writer.WriteEndObject();
            writer.Flush();
            String jsonResult = RestClient.Post_Rest(address, sw);
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            branch_id = ja["branch_id"].ToString();
            return ret;
        }

        public static TCandaoRetBase MemberLogin(string orderid, string mobile)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/newspicyway/member/MemberLogin.json", RestClient.JavaServer);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderid");
            writer.WriteValue(orderid);
            writer.WritePropertyName("mobile");
            writer.WriteValue(mobile);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【MemberLogin】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【MemberLogin】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            return ret;
        }

        public static TCandaoRetBase MemberLogout(string orderid, string mobile)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/newspicyway/member/MemberLogout.json", RestClient.JavaServer);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderid");
            writer.WriteValue(orderid);
            writer.WritePropertyName("mobile");
            writer.WriteValue(mobile);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【MemberLogout】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【MemberLogout】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            return ret;
        }
        public static TCandaoRetBase DeleteOrderMember(string orderid, string mobile)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/newspicyway/member/DeleteOrderMember.json", RestClient.JavaServer);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderid");
            writer.WriteValue(orderid);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【DeleteOrderMember】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【DeleteOrderMember】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            return ret;
        }

        public static TCandaoRetBase AddOrderMember(TCandaoOrderMemberInfo ordermemberinfo)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/newspicyway/member/AddOrderMember.json", RestClient.JavaServer);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderid");
            writer.WriteValue(ordermemberinfo.Orderid);
            writer.WritePropertyName("cardno");
            writer.WriteValue(ordermemberinfo.Cardno);
            writer.WritePropertyName("userid");
            writer.WriteValue(ordermemberinfo.Userid);
            writer.WritePropertyName("business");
            writer.WriteValue(ordermemberinfo.Business);
            writer.WritePropertyName("terminal");
            writer.WriteValue(ordermemberinfo.Terminal);
            writer.WritePropertyName("serial");
            writer.WriteValue(ordermemberinfo.Serial);
            writer.WritePropertyName("businessname");
            writer.WriteValue(ordermemberinfo.Businessname);
            writer.WritePropertyName("score");
            writer.WriteValue(ordermemberinfo.Score);
            writer.WritePropertyName("coupons");
            writer.WriteValue(ordermemberinfo.Coupons);
            writer.WritePropertyName("stored");
            writer.WriteValue(ordermemberinfo.Stored);
            writer.WritePropertyName("scorebalance");
            writer.WriteValue(ordermemberinfo.Scorebalance);
            writer.WritePropertyName("couponsbalance");
            writer.WriteValue(ordermemberinfo.Couponsbalance);
            writer.WritePropertyName("storedbalance");
            writer.WriteValue(ordermemberinfo.Storedbalance);
            writer.WritePropertyName("psexpansivity");
            writer.WriteValue(ordermemberinfo.Psexpansivity);
            writer.WritePropertyName("netvalue");
            writer.WriteValue(ordermemberinfo.Netvalue);
            writer.WritePropertyName("inflated");
            writer.WriteValue(ordermemberinfo.Inflated1);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【AddOrderMember】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【AddOrderMember】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            return ret;
        }

        public static TCandaoRetBase GetOrderMember(string orderid)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/newspicyway/member/GetOrderMember.json", RestClient.JavaServer);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("orderid");
            writer.WriteValue(orderid);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【getOrderMember】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【getOrderMember】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("1");
            ret.Retinfo = ja["RetInfo"].ToString();
            try
            {
                ret.Tracecode2 = ja["serial"].ToString();//消费时的交易号
                ret.Cardno = ja["cardno"].ToString();
            }
            catch { }
            return ret;
        }

        /// <summary>
        /// 手机号重复校验
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="securitycode"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static TCandaoRetBase ValidateTbMemberManager(string branchId, string securitycode, string mobile)
        {
            TCandaoRetBase ret = new TCandaoRetBase();
            string address = String.Format("http://{0}/member/memberManager/validateTbMemberManager", WebServiceReference.Candaomemberserver);
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("branch_id");
            writer.WriteValue(branchId);
            writer.WritePropertyName("securityCode");
            writer.WriteValue(securitycode);
            writer.WritePropertyName("mobile");
            writer.WriteValue(mobile);
            writer.WriteEndObject();
            writer.Flush();
            AllLog.Instance.I(string.Format("【validateTbMemberManager】 reqeust：{0}。", sw));
            String jsonResult = RestClient.Post_Rest(address, sw);
            AllLog.Instance.I(string.Format("【validateTbMemberManager】 result：{0}。", jsonResult));
            JObject ja = null;
            ret.Ret = true;
            try
            {
                ja = (JObject)JsonConvert.DeserializeObject(jsonResult);

            }
            catch { ret.Ret = false; return ret; }
            ret.Retcode = ja["Retcode"].ToString();
            ret.Ret = ret.Retcode.Equals("0");
            ret.Retinfo = ja["RetInfo"].ToString();
            return ret;
        }

        public static void WriteObject(ref JsonWriter writer, string propertyName, string value)
        {
            if (value == null)
                value = "";
            writer.WritePropertyName(propertyName);
            writer.WriteValue(value);
        }
    }
}
