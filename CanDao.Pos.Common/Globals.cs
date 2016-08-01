using System.Collections.Generic;
using System.Linq;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Common
{
    /// <summary>
    /// 全局变量缓存。
    /// </summary>
    public static class Globals
    {
        static Globals()
        {
            UserRight = new UserRight();
            UserInfo = new UserInfo();
            Authorizer = new UserInfo();
            SystemSetDatas = new List<SystemSetData>();
        }

        /// <summary>
        /// 用户权限。
        /// </summary>
        public static UserRight UserRight { get; private set; }

        /// <summary>
        /// 用户信息。
        /// </summary>
        public static UserInfo UserInfo { get; set; }

        /// <summary>
        /// 授权人。
        /// </summary>
        public static UserInfo Authorizer { get; set; }

        /// <summary>
        /// 系统设置集合。
        /// </summary>
        public static List<SystemSetData> SystemSetDatas { get; private set; }

        /// <summary>
        /// 菜谱菜品集合。
        /// </summary>
        public static List<MenuDishGroupInfo> DishGroupInfos { get; set; }

        /// <summary>
        /// 所有银行集合。
        /// </summary>
        public static List<BankInfo> BankInfos { get; set; }

        /// <summary>
        /// 挂账单位集合。
        /// </summary>
        public static List<OnCompanyAccountInfo> OnCompanyInfos { get; set; }

        /// <summary>
        /// 分店信息。
        /// </summary>
        public static BranchInfo BranchInfo { get; set; }

        /// <summary>
        /// 电话。
        /// </summary>
        public static string BranchTelephone { get; set; }

        /// <summary>
        /// 餐具信息。
        /// </summary>
        public static OrderDishInfo DinnerWareInfo { get; set; }

        /// <summary>
        /// 是否启用餐具。
        /// </summary>
        public static bool IsDinnerWareEnable
        {
            get
            {
                if (!SystemSetDatas.Any())
                    return false;

                var dinnerWareSetting = SystemSetDatas.FirstOrDefault(t => t.Type == EnumSystemDataType.DISHES);
                if (dinnerWareSetting == null)
                    return false;

                return dinnerWareSetting.IsEnable;
            }
        }

        /// <summary>
        /// 是否是雅座会员。
        /// </summary>
        public static bool IsYazuoMember
        {
            get { return MemberSystem == EnumMemberSystem.Yazuo; }
        }

        /// <summary>
        /// 是否是餐道会员。
        /// </summary>
        public static bool IsCanDaoMember
        {
            get { return MemberSystem == EnumMemberSystem.Candao; }
        }

        /// <summary>
        /// 会员系统类型。
        /// </summary>
        public static EnumMemberSystem MemberSystem { get; set; }

        /// <summary>
        /// 零头处理方式。
        /// </summary>
        private static EnumOddModel? _oddModel;
        /// <summary>
        /// 获取零头处理方式。
        /// </summary>
        public static EnumOddModel OddModel
        {
            get
            {
                if (_oddModel.HasValue)
                    return _oddModel.Value;

                if (!SystemSetDatas.Any())
                    return EnumOddModel.None;

                var oddSetting = SystemSetDatas.FirstOrDefault(t => t.Type == EnumSystemDataType.ROUNDING);
                _oddModel = oddSetting == null ? EnumOddModel.None : (EnumOddModel)oddSetting.ItemId;
                return _oddModel.Value;
            }
            set { _oddModel = value; }
        }

        /// <summary>
        /// 零头处理精度。
        /// </summary>
        private static EnumOddAccuracy? _oddAccuracy;

        public static EnumOddAccuracy OddAccuracy
        {
            get
            {
                if (_oddAccuracy.HasValue)
                    return _oddAccuracy.Value;

                if (!SystemSetDatas.Any())
                    return EnumOddAccuracy.Jiao;

                var oddAccSetting = SystemSetDatas.FirstOrDefault(t => t.Type == EnumSystemDataType.ACCURACY);
                _oddAccuracy = oddAccSetting == null ? EnumOddAccuracy.Fen : (EnumOddAccuracy)oddAccSetting.ItemId;
                return _oddAccuracy.Value;
            }
        }

        /// <summary>
        /// 忌口设置集合。
        /// </summary>
        private static List<string> _dietSetting;
        /// <summary>
        /// 忌口设置集合。
        /// </summary>
        public static List<string> DietSetting
        {
            get
            {
                if (_dietSetting != null && _dietSetting.Any())
                    return _dietSetting;

                if (!SystemSetDatas.Any())
                    return new List<string>();

                _dietSetting = SystemSetDatas.Where(t => t.Type == EnumSystemDataType.JI_KOU_SPECIAL).Select(t => t.ItemDesc).ToList();
                return _dietSetting;
            }
        }

        /// <summary>
        /// 营业时间。
        /// </summary>
        public static TradeTime TradeTime { get; set; }
    }
}