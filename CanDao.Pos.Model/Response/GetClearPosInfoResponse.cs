using System;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 获取清机单信息的返回类。
    /// </summary>
    public class GetClearPosInfoResponse : RestOrderAndJsResponse<ClearPosDataResponse, SettlementTotalResponse>
    {

    }

    /// <summary>
    /// 清机单信息的返回类。
    /// </summary>
    public class ClearPosDataResponse
    {
        private string _vIn;
        private string _vOut;
        private string _workdate;
        private string _priterTime;

        /// <summary>
        /// 清机单号。
        /// </summary>
        public string classNo { get; set; }

        /// <summary>
        /// Pos ID。
        /// </summary>
        public string posID { get; set; }

        /// <summary>
        /// 操作员ID。
        /// </summary>
        public string operatorID { get; set; }

        /// <summary>
        /// 操作员姓名。
        /// </summary>
        public string operatorName { get; set; }

        /// <summary>
        /// 签到时间。
        /// </summary>
        public string vIn
        {
            get { return _vIn; }
            set
            {
                _vIn = value;
                SignInTime = DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss", null);
            }
        }

        public DateTime SignInTime { get; set; }

        /// <summary>
        /// 签退时间。
        /// </summary>
        public string vOut
        {
            get { return _vOut; }
            set
            {
                _vOut = value;
                SignOutTime = DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss", null);
            }
        }

        public DateTime SignOutTime { get; set; }

        /// <summary>
        /// 备用金。
        /// </summary>
        public decimal prettyCash { get; set; }

        /// <summary>
        /// 前班未结台数。
        /// </summary>
        public int lastNonTable { get; set; }

        /// <summary>
        /// 前班未结押金。
        /// </summary>
        public decimal lastNonDeposit { get; set; }

        /// <summary>
        /// 本班开单人数。
        /// </summary>
        public int tBeginPeople { get; set; }

        /// <summary>
        /// 本班开台总数。
        /// </summary>
        public int tBeginTableTotal { get; set; }

        /// <summary>
        /// 本班未结台数。
        /// </summary>
        public int tNonClosingTable { get; set; }

        /// <summary>
        /// 本班未结金额。
        /// </summary>
        public decimal tNonClosingMoney { get; set; }

        /// <summary>
        /// 本班未退押金。
        /// </summary>
        public decimal tNonClosingDeposit { get; set; }

        /// <summary>
        /// 本班已结台数。
        /// </summary>
        public int tClosingTable { get; set; }

        /// <summary>
        /// 本班已结人数。
        /// </summary>
        public int tClosingPeople { get; set; }

        /// <summary>
        /// 本班赠单金额。
        /// </summary>
        public decimal tPresentedMoney { get; set; }

        /// <summary>
        /// 本班退菜金额。
        /// </summary>
        public decimal tRFoodMoney { get; set; }

        /// <summary>
        /// 品项消费。
        /// </summary>
        public decimal itemMoney { get; set; }

        /// <summary>
        /// 服务费。
        /// </summary>
        public decimal serviceMoney { get; set; }

        /// <summary>
        /// 包房费。
        /// </summary>
        public decimal roomMoney { get; set; }

        /// <summary>
        /// 最低消费补齐。
        /// </summary>
        public decimal lowConsComp { get; set; }

        /// <summary>
        /// 优惠金额。
        /// </summary>
        public decimal preferenceMoney { get; set; }

        /// <summary>
        /// 应收小计。
        /// </summary>
        public decimal accountsReceivableSubtotal { get; set; }

        /// <summary>
        /// 抹零金额。
        /// </summary>
        public decimal removeMoney { get; set; }

        /// <summary>
        /// 定额优惠金额。
        /// </summary>
        public decimal ratedPreferenceMoney { get; set; }

        /// <summary>
        /// 应收合计。
        /// </summary>
        public decimal accountsReceivableTotal { get; set; }

        /// <summary>
        /// 计入收入合计。
        /// </summary>
        public decimal includedMoneyTotal { get; set; }

        /// <summary>
        /// 不计入收入合计。
        /// </summary>
        public decimal noIncludedMoneyTotal { get; set; }

        /// <summary>
        /// 合计。
        /// </summary>
        public decimal TotalMoney { get; set; }

        /// <summary>
        /// 餐具。
        /// </summary>
        public decimal tableware { get; set; }

        /// <summary>
        /// 酒水。
        /// </summary>
        public decimal drinks { get; set; }

        /// <summary>
        /// 酒水烟汤面。
        /// </summary>
        public decimal drinksSmokeNoodle { get; set; }

        /// <summary>
        /// 本日营业总额。
        /// </summary>
        public decimal todayTurnover { get; set; }

        /// <summary>
        /// 打印时间。
        /// </summary>
        public string priterTime
        {
            get { return _priterTime; }
            set
            {
                _priterTime = value;
                PrintTime = DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss", null);
            }
        }

        /// <summary>
        /// 打印时间。
        /// </summary>
        public DateTime PrintTime { get; set; }

        /// <summary>
        /// IP地址。
        /// </summary>
        public string ipaddress { get; set; }

        /// <summary>
        /// 营业时间。
        /// </summary>
        public string workdate
        {
            get { return _workdate; }
            set
            {
                _workdate = value;
                WorkDateTime = DateTime.ParseExact(value, "yyyyMMdd HH:mm:ss", null);
            }
        }

        /// <summary>
        /// 工作日期。
        /// </summary>
        public DateTime WorkDateTime { get; set; }

        /// <summary>
        /// 班别。
        /// </summary>
        public int shiftid { get; set; }
    }

    /// <summary>
    /// 结算方式统计信息返回类。
    /// </summary>
    public class SettlementTotalResponse
    {
        /// <summary>
        /// 单号。
        /// </summary>
        public string orderid { get; set; }

        /// <summary>
        /// 结算类别描述。
        /// </summary>
        public string itemDesc { get; set; }

        /// <summary>
        /// 类型。
        /// </summary>
        public int incometype { get; set; }

        /// <summary>
        /// 会员号。
        /// </summary>
        public string membercardno { get; set; }

        /// <summary>
        /// 银行卡号/优惠名称。
        /// </summary>
        public string bankcardno { get; set; }

        /// <summary>
        /// 金额。
        /// </summary>
        public decimal payamount { get; set; }
    }
}