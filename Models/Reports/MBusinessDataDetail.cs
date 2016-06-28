using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Reports
{
    /// <summary>
    /// 营业数据报表
    /// </summary>
    public class MBusinessDataDetail
    {
        /// <summary>
        /// 品类列表
        /// </summary>
        public List<MCategory> Categories { set; get; }

        /// <summary>
        /// 挂账列表
        /// </summary>
        public List<MHangingMoney> HangingMonies { set; get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { set; get; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { set; get; }
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime CurrentTime { set; get; }
        /// <summary>
        /// 操作员
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 开台数
        /// </summary>
        public string kaitaishu { set; get; }
        /// <summary>
        /// 优免
        /// </summary>
        public string bastfree { set; get; }
        /// <summary>
        /// 会员积分消费
        /// </summary>
        public string integralconsum { set; get; }
        /// <summary>
        /// 会员券消费
        /// </summary>
        public string meberTicket { set; get; }
        /// <summary>
        /// 折扣优惠
        /// </summary>
        public string discountmoney { set; get; }
        /// <summary>
        /// 抹零
        /// </summary>
        public string malingincom { set; get; }
        /// <summary>
        /// 赠送金额
        /// </summary>
        public string give { set; get; }
        /// <summary>
        /// 四舍五入
        /// </summary>
        public string handervalue { set; get; }
        /// <summary>
        /// 会员储值消费虚增
        /// </summary>
        public string mebervalueadd { set; get; }
        /// <summary>
        /// 现金
        /// </summary>
        public string money { set; get; }
        /// <summary>
        /// 挂账
        /// </summary>
        public string card { set; get; }
        /// <summary>
        /// 微信
        /// </summary>
        public string weixin { set; get; }
        /// <summary>
        /// 支付宝
        /// </summary>
        public string zhifubao { set; get; }
        /// <summary>
        /// 刷卡-工行
        /// </summary>
        public string icbc { set; get; }
        /// <summary>
        /// 刷卡-他行
        /// </summary>
        public string otherbank { set; get; }
        /// <summary>
        /// 会员储值消费净值
        /// </summary>
        public string merbervaluenet { set; get; }
        /// <summary>
        /// 应收合计
        /// </summary>
        public string shouldamount { set; get; }
        /// <summary>
        /// 优惠合计
        /// </summary>
        public string discountamount { set; get; }
        /// <summary>
        /// 实收合计
        /// </summary>
        public string paidinamount { set; get; }

        /// <summary>
        /// 实收合计
        /// </summary>
        public string xiaofei { set; get; }
    }
}
