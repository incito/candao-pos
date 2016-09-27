using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CanDao.Pos.Common;
using CanDao.Pos.Common.Models.VipModels;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Reports;
using CanDao.Pos.Model.Request;
using CanDao.Pos.Model.Response;

namespace CanDao.Pos.ServiceImpl
{
    internal class DataConverter
    {
        private const string DateTimeFormat = "yyyyMMdd HH:mm:ss";

        private const string DateTimeFormat2 = "yyyy-MM-dd HH:mm:ss";

        internal static TableInfo ToTableInfo(TableInfoResponse response)
        {
            return new TableInfo
            {
                AreaName = response.areaname,
                AreaNo = response.areaid,
                OrderId = response.orderid,
                PeopleNumber = response.personNum,
                DinnerNumber = response.custnum ?? 0,
                TableStatus = (EnumTableStatus)response.status,
                TableId = response.tableid,
                TableName = response.tableName,
                TableNo = response.tableNo,
                TableType = (EnumTableType)response.tabletype,
                MinPrice = Math.Round(response.minprice, 2),
                FixPrice = Math.Round(response.fixprice, 2),
                BeginTime = Parse2DateTime(response.begintime, DateTimeFormat2),
                Amount = Math.Round(response.amount ?? 0, 2),
            };
        }

        internal static SystemSetData ToSystemSetData(SystemSetDataResponse response)
        {
            return new SystemSetData
            {
                Id = response.id,
                IsEnable = response.status.Equals("1"),
                ItemDesc = response.itemDesc,
                ItemId = Parse2Int(response.itemid),
                ItemSort = response.itemSort,
                Type = (EnumSystemDataType)Enum.Parse(typeof(EnumSystemDataType), response.type),
                TypeName = response.typename,
                Value = response.item_value,
            };
        }

        internal static TableFullInfo ToTableFullInfo(GetOrderDishListResponse response)
        {
            if (response.OrderJson == null || !response.OrderJson.Any())
                return null;

            var orderResponse = response.OrderJson.First();
            return ToTableFullInfo(orderResponse, response.JSJson);
        }

        internal static OrderDishInfo ToDishInfo(OrderDishDataResponse response)
        {
            var dishName = InternationaHelper.GetBeforeSeparatorFlagData(response.title);
            return new OrderDishInfo
            {
                DishDesc = response.itemdesc,
                DishId = response.dishid,
                DishName = dishName,
                RelateDishId = response.relatedishid,
                DishNum = response.dishnum,
                DishStatus = response.dishstatus != null ? (EnumDishStatus)response.dishstatus : EnumDishStatus.Normal,
                DishType = (EnumDishType)response.dishtype,
                DishUnit = InternationaHelper.GetBeforeSeparatorFlagData(response.dishunit),
                TempDishName = dishName.Contains("临时菜") ? response.avoid.Split('|')[2] : "",
                SrcDishUnit = response.dishunit,
                Price = response.orderprice ?? 0,
                PrimaryKey = response.primarykey,
                PayAmount = response.payamount ?? 0,
                IsPot = Convert.ToBoolean(response.ispot),
                IsMaster = response.ismaster == 1,
            };
        }

        internal static PrintOrderFullInfo ToPrintOrderFullInfo(GetPrintOrderInfoResponse response)
        {
            var item = new PrintOrderFullInfo();
            var orderResponse = response.OrderJson.First();
            item.OrderInfo = ToPrintOrderInfo(orderResponse);

            if (response.ListJson != null && response.ListJson.Any())
                item.OrderDishInfos = response.ListJson.Select(ToOrderDishInfo).ToList();

            if (response.JSJson != null && response.JSJson.Any())
                item.PayDetails = response.JSJson.Select(ToPrintPayDetail).ToList();

            return item;
        }

        internal static OrderDishInfo ToDishInfo(DinnerWareInfoResponse response)
        {
            return new OrderDishInfo
            {
                DishId = response.dishid,
                DishName = response.dishname,
                DishType = (EnumDishType)response.dishtype,
                DishUnit = InternationaHelper.GetBeforeSeparatorFlagData(response.unit),
                SrcDishUnit = response.unit,
                PriceSource = response.price ?? 0,
                Price = response.price ?? 0,
                MemberPrice = response.vipprice ?? response.price ?? 0,
            };
        }

        internal static MenuDishGroupInfo ToMenuDishGroupInfo(MenuDishGroupInfoResponse response)
        {
            return new MenuDishGroupInfo
            {
                GroupId = response.itemid,
                GroupName = InternationaHelper.GetBeforeSeparatorFlagData(response.itemdesc),
                GroupSortIndex = response.itemsort,
            };
        }

        internal static MenuDishInfo ToMenuDishInfo(MenuDishInfoResponse response)
        {
            var item = new MenuDishInfo
            {
                DishName = InternationaHelper.GetBeforeSeparatorFlagData(response.title),
                DishType = (EnumDishType)response.dishtype,
                DishId = response.dishid,
                GroupId = response.source,
                Menuid = response.menuid,
                NeedWeigh = Convert.ToBoolean(response.weigh),
                Price = response.price,
                SrcUnit = response.unit,
                Unit = InternationaHelper.GetBeforeSeparatorFlagData(response.unit),
                VipPrice = response.vipprice,
                FirstLetter = response.py,
                Tastes = !string.IsNullOrEmpty(response.imagetitle) ? response.imagetitle.Split(',').ToList() : null,
                //口味是个以逗号分隔的集合。
            };

            if (item.DishType == EnumDishType.FishPot)
                item.FishPotType = response.level == 1 ? EnumFishPotType.New : EnumFishPotType.Normal;
            else
                item.FishPotType = EnumFishPotType.None;

            return item;
        }

        internal static CouponInfo ToCouponInfo(CouponInfoResponse response)
        {
            return new CouponInfo
            {
                Amount = response.amount,
                BillAmount = response.bill_amount,
                FreeAmount =
                    (response.bill_amount != null && response.amount != null)
                        ? response.bill_amount - response.amount
                        : response.amount ?? 0, // 当账单金额和实际金额都有时，优免金额等于二者的差，否则等于实际金额。
                Name = DataHelper.GetNoneNullValueByOrder(response.name, response.company_name, response.free_reason),
                PartnerName = response.company_name ?? "",
                CouponId = response.preferential,
                RuleId = response.id,
                Color = response.color ?? "LightBlue",
                DishId = response.dish,
                CouponType = (EnumCouponType)response.type,
                Discount = response.discount ?? 0m,
                HandCouponType = Convert2HandCounCouponType(response.free_reason),
                IsUncommonlyUsed = response.status == "2",
            };
        }

        internal static SaveCouponInfoRequest ToSaveCouponInfoRequest(UsedCouponInfo data)
        {
            return new SaveCouponInfoRequest
            {
                yhname = data.Name,
                partnername = data.CouponInfo.PartnerName,
            };
        }

        /// <summary>
        /// 转成下单的请求类。
        /// </summary>
        /// <param name="orderId">订单号。</param>
        /// <param name="tableId">餐台号。</param>
        /// <param name="orderRemark">全单备注</param>
        /// <param name="dishInfos">点的菜品集合。</param>
        /// <returns></returns>
        internal static OrderDishRequest ToOrderDishRequest(string orderId, string tableId, string orderRemark, List<OrderDishInfo> dishInfos)
        {
            var list = dishInfos.Where(t => !t.IsComboDish && !t.IsFishPotDish).ToList();
            //过滤掉套餐和鱼锅内部的菜，套餐和鱼锅内部的菜在套餐里面处理。
            var request = new OrderDishRequest
            {
                currenttableid = tableId,
                orderid = orderId,
                globalsperequire = orderRemark,
                rows = ToDishDetailRequestList(list),
            };

            return request;
        }

        internal static BackDishInfo ToBackDishInfo(BackDishInfoDataResponse response)
        {
            return new BackDishInfo
            {
                IsPot = Convert.ToBoolean(response.ispot),
                DishType = (EnumDishType)response.dishtype,
                IsMaster = Convert.ToBoolean(response.ismaster),
                ChildDishType = response.childdishtype,
                PrimaryKey = response.primarykey,
                DishId = response.dishid,
                DishUnit = response.dishunit
            };
        }

        internal static BackDishRequest ToBackDishRequest(string orderId, string tableNo, string authorizerId, string userId, OrderDishInfo dishInfo, decimal backDishNum, string backReason)
        {
            if (dishInfo.DishType == EnumDishType.FishPot || dishInfo.IsFishPotDish)
            {
                return new BackFishPotDishRequest
                {
                    userName = userId,
                    orderNo = orderId,
                    dishNo = dishInfo.DishId,
                    currenttableid = tableNo,
                    discardReason = backReason,
                    dishtype = ((int)dishInfo.DishType).ToString(),
                    primarykey = dishInfo.PrimaryKey,
                    discardUserId = authorizerId,
                    dishunit = dishInfo.DishUnit,
                    dishNum = backDishNum.ToString(CultureInfo.InvariantCulture),
                    hotflag = dishInfo.IsPot ? "1" : "0",
                    potdishid = dishInfo.IsPot ? dishInfo.RelateDishId : dishInfo.DishId
                    //potdishid = dishInfo.DishId,
                };
            }
            else
            {
                return new BackDishRequest
                {
                    userName = userId,
                    orderNo = orderId,
                    dishNo = dishInfo.DishId,
                    currenttableid = tableNo,
                    discardReason = backReason,
                    dishtype = ((int)dishInfo.DishType).ToString(),
                    primarykey = dishInfo.PrimaryKey,
                    discardUserId = authorizerId,
                    dishunit = dishInfo.DishUnit,
                    dishNum = backDishNum.ToString(CultureInfo.InvariantCulture),
                };
            }
        }

        internal static PayBillRequest ToPayBillRequest(string orderId, string userId, List<BillPayInfo> payInfos)
        {
            return new PayBillRequest
            {
                orderNo = orderId,
                userName = userId,
                payDetail = payInfos.Select(ToPayBillDetailRequest).ToList(),
            };
        }

        internal static BankInfo ToBankInfo(GetBankInfoResponse response)
        {
            return new BankInfo
            {
                Id = response.itemid,
                Name = response.itemDesc,
                SortIndex = response.itemSort,
            };
        }

        internal static OnCompanyAccountInfo ToOnCpyAccInfo(GetOnCompanyAccountResponse response)
        {
            return new OnCompanyAccountInfo
            {
                Id = response.preferential,
                Name = response.name,
                NameFirstLetter = response.name_first_letter,
            };
        }

        internal static UnclearMachineInfo ToUnclearMachineInfo(UnclearPosResponse response)
        {
            return new UnclearMachineInfo
            {
                UserName = response.username,
                MachineFlag = response.ipaddress,
            };
        }

        internal static MemberInfo ToMemberInfo(CanDaoMemberQueryResponse response)
        {
            return new MemberInfo
            {
                CardNo = response.MCard,
                Name = response.name,
                Mobile = response.mobile,
                Birthday = DateTime.ParseExact(response.birthday, "yyyy-MM-dd", null),
                Gender = (EnumGender)response.gender,
                StoredBalance = response.StoreCardBalance,
                CardLevel = response.CardLevel,
                Integral = response.IntegralOverall,
            };
        }

        /// <summary>
        /// 转雅座会员信息。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static YaZuoMemberInfo ToYaZuoMemberInfo(YaZuoMemberQueryResponse response)
        {
            var item = new YaZuoMemberInfo
            {
                CardNo = response.pszPan,
                StoredBalance = response.psStoredCardsBalance / 100,
                Integral = response.psIntegralOverall / 100,
                Mobile = response.pszMobile,
            };

            if (!string.IsNullOrEmpty(response.pszTrack2))
            {
                var tempList = response.pszTrack2.Split(',');
                if (tempList.Count() > 1)
                    item.CardNoList = tempList.ToList();
            }

            if (!string.IsNullOrEmpty(response.psTicketInfo))
            {
                var ticketList = response.psTicketInfo.Split(';').ToList();
                var couponItemList = ticketList.Select(ToYaZuoCouponInfo).Where(t => t != null).ToList();
                if (couponItemList.Any())
                    item.CouponList = couponItemList;
            }

            return item;
        }

        /// <summary>
        /// 转雅座优惠券信息。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static YaZuoCouponInfo ToYaZuoCouponInfo(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var tempList = data.Split('|').ToList();
            try
            {
                var item = new YaZuoCouponInfo
                    {
                        CouponId = tempList[0],
                        CouponType = tempList[2],
                        CouponName = tempList[3],
                        CouponCount = Parse2Int(tempList[4])
                    };

                return item;
            }
            catch (Exception ex)
            {
                ErrLog.Instance.E(string.Format("雅座优惠券处理时异常：{0}", ex.MyMessage()));
                return null;
            }
        }

        /// <summary>
        /// 转换会员对象
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static MVipInfo ToVipInfo(CanDaoVipQueryResponse response)
        {
            var info = new MVipInfo();
            info.VipName = response.name;
            info.Sex = int.Parse(response.gender);
            info.Birthday = response.birthday;
            info.Address = response.member_address;
            //info.Creattime = response.createtime;
            info.TelNum = response.mobile;

            foreach (var card in response.result)
            {
                var cInfo = new MVipCardInfo();
                cInfo.CardNum = card.MCard;
                cInfo.Balance = decimal.Parse(card.StoreCardBalance);
                cInfo.Integral = decimal.Parse(card.IntegralOverall);
                cInfo.CouponBalance = float.Parse(card.CouponsOverall);
                cInfo.TraceCode = card.TraceCode;
                cInfo.CardType = int.Parse(card.card_type);
                cInfo.CardLevel = int.Parse(card.level);
                cInfo.CardLevelName = card.level_name;
                cInfo.CardState = int.Parse(card.status);
                info.CardInfos.Add(cInfo);
            }

            return info;
        }

        /// <summary>
        /// 转换雅座储值信息。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static YaZuoStorageInfo ToYaZuoStorageInfo(YaZuoMemberStorageResponse response)
        {
            return new YaZuoStorageInfo
            {
                CardNo = response.pszPan,
                StoredBalance = response.psStoreCardBalance / 100,
                TradeCode = response.pszTrace,
            };
        }

        /// <summary>
        /// 转换成会员卡激活信息。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static YaZuoCardActiveInfo ToYaZuoCardActiveInfo(YaZuoCardActiveResponse response)
        {
            return new YaZuoCardActiveInfo
            {

            };
        }

        /// <summary>
        /// 转换成优惠券列表
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static List<MVipCoupon> ToCouponList(GetCouponListResponse response)
        {
            var mVipCoupons = new List<MVipCoupon>();
            foreach (var coupon in response.datas)
            {
                var info = new MVipCoupon
                {
                    Id = coupon.id,
                    CouponType = coupon.type,
                    DealValue = coupon.dealValue,
                    PresentValu = coupon.presentValue
                };
                mVipCoupons.Add(info);
            }

            return mVipCoupons;
        }

        internal static MenuComboFullInfo ToMenuComboFullInfo(MenuComboDishMainResponse response)
        {
            var item = new MenuComboFullInfo();
            if (response.only != null)
                item.SingleDishInfos = response.only.Select(ToMenuDishInfo).ToList();

            if (response.combo != null)
                item.ComboDishInfos = response.combo.Select(ToComboDishInfo).ToList();

            return item;
        }

        internal static MenuComboDishInfo ToComboDishInfo(MenuComboDishDataResponse response)
        {
            var item = new MenuComboDishInfo
            {
                ComboId = response.columnid,
                Id = response.id,
                DishId = response.dishid,
                SourceCount = response.startnum,
                SelectCount = response.endnum,
                ComboName = InternationaHelper.GetBeforeSeparatorFlagData(response.itemDesc),
            };

            if (response.alldishes != null)
                item.SourceDishes = response.alldishes.Select(ToMenuDishInfo).ToList();

            return item;
        }

        internal static ReportStatisticInfo ToReportStatisticInfo(GetReportStatisticInfoBase<ReportTipInfoResponse> response)
        {
            var item = new ReportStatisticInfo
            {
                StartTime = Parse2DateTime(response.time.startTime, DateTimeFormat2, DateTime.MinValue),
                EndTime = Parse2DateTime(response.time.endTime, DateTimeFormat2, DateTime.MinValue),
            };
            if (response.data != null)
                item.DataSource = response.data.Select(ToReportDataBase).ToList();

            return item;
        }

        internal static ReportStatisticInfo ToReportStatisticInfo(GetReportStatisticInfoBase<ReportDishInfoResponse> response)
        {
            var item = new ReportStatisticInfo
            {
                StartTime = Parse2DateTime(response.time.startTime, DateTimeFormat2, DateTime.MinValue),
                EndTime = Parse2DateTime(response.time.endTime, DateTimeFormat2, DateTime.MinValue),
            };
            if (response.data != null)
                item.DataSource = response.data.Select(ToReportDataBase).ToList();

            return item;
        }

        internal static BranchInfo ToBranchInfo(BranchInfoData data)
        {
            return new BranchInfo
            {
                BranchId = data.branchid.ToString(),
                BranchAddress = data.branchaddress,
                BranchName = data.branchname,
            };
        }

        internal static PrintMemberPayInfo ToPrintMemberPayInfo(MemberPayInfoDataResponse response)
        {
            return new PrintMemberPayInfo
            {
                BatchNo = response.batchno,
                BranchId = response.business,
                BranchName = response.businessname,
                CardNo = response.cardno,
                Coupons = response.coupons,
                OperateType = response.operatetype,
                OrderId = response.orderid,
                Score = response.score,
                ScoreBalance = response.scorebalance,
                StoredBalance = response.storedbalance,
                StoredPay = response.stored,
                TradeTime = Parse2DateTime(response.ordertime, DateTimeFormat, DateTime.MinValue),
                Terminal = response.terminal,
                UserId = response.userid,
            };
        }

        internal static QueryOrderInfo ToQueryOrderInfo(QueryOrderInfoDataResponse response)
        {
            return new QueryOrderInfo
            {
                AreaName = response.areaname,
                BeginTime = Parse2DateTime(response.begintime, DateTimeFormat, DateTime.MinValue),
                CustomerNum = response.custnum,
                DebitCompany = response.gzname,
                DebitPeople = response.gzuser,
                DebitTelphone = response.gztele,
                Dueamount = response.dueamount,
                EndTime = Parse2DateTime(response.endtime, DateTimeFormat),
                OrderId = response.orderid,
                OrderStatus = (EnumOrderStatus)response.orderstatus,
                TableId = response.tableids,
                TableName = response.tableName,
                TableType = (EnumTableType)response.tabletype,
                UserId = response.userid,
                MemberNo = response.memberno,
            };
        }

        internal static MenuFishPotFullInfo ToMenuFishPotFullInfo(GetFishPotDishResponse response)
        {
            var item = new MenuFishPotFullInfo();
            var list = response.OrderJson.Select(ToMenuDishInfo).ToList();
            item.PotInfo = list.FirstOrDefault(t => t.FishPotType == EnumFishPotType.Normal);
            list.Remove(item.PotInfo);
            item.FishDishes = list;
            return item;
        }

        internal static PrintStatusInfo ToPrintStatusInfo(PrinterStatusInfoResponse response)
        {
            return new PrintStatusInfo
            {
                PrintIp = response.ip,
                PrintName = response.name,
                PrintStatus = (EnumPrintStatus)response.status,
                PrintStatusDes = response.statusTitle,
            };
        }

        internal static UsedCouponInfo ToUsedCouponInfo(SavedCouponInfoResponse response)
        {
            return new UsedCouponInfo
            {
                BillAmount = response.amount,
                Count = response.num,
                DebitAmount = response.debitamount,
                FreeAmount = response.freeamount,
                Name = response.yhname,
            };
        }

        internal static DishGiftCouponInfo ToDishGiftCouponInfo(DishGiftCouponInfoResponse response)
        {
            return new DishGiftCouponInfo
            {
                DishId = response.dishid,
                UsedCouponCount = response.count,
            };
        }

        internal static BusinessSimpleInfo ToBusinessoSimpleInfo(BusinessSimpleInfoResponse response)
        {
            return new BusinessSimpleInfo
            {
                PaiedAmount = response.ssamount,
                PaiedOrderCount = response.orderCount,
                TotalAmount = response.totalAmount,
                TotalDinnerCount = response.custnum,
                UnpaiedAmount = response.dueamount,
            };
        }

        #region Private Method

        private static MenuDishInfo ToMenuDishInfo(FishPotDishResponse response)
        {
            return new MenuDishInfo
            {
                DishName = InternationaHelper.GetBeforeSeparatorFlagData(response.title),
                DishType = (EnumDishType)response.dishtype,
                DishId = response.dishid,
                GroupId = response.source,
                NeedWeigh = Convert.ToBoolean(response.weigh),
                Price = response.price,
                SrcUnit = response.unit,
                Unit = InternationaHelper.GetBeforeSeparatorFlagData(response.unit),
                VipPrice = response.vipprice,
                FishPotType = response.ispot == 1 ? EnumFishPotType.Normal : EnumFishPotType.None,
                IsPot = Convert.ToBoolean(response.ispot),
            };
        }

        private static ReportDataBase ToReportDataBase(ReportTipInfoResponse response)
        {
            return new ReportDataBase
            {
                Name = response.waiterName,
                Amount = response.tipMoney,
                Count = Math.Round(response.serviceCount, 2),
            };
        }

        private static ReportDataBase ToReportDataBase(ReportDishInfoResponse response)
        {
            return new ReportDataBase
            {
                Name = response.dishName,
                Amount = response.totlePrice ?? 0,
                Count = Math.Round(response.dishCount, 2),
            };
        }

        private static MenuDishInfo ToMenuDishInfo(MenuSingleDishDataResponse response)
        {
            return new MenuDishInfo
            {
                DishId = response.contactdishid,
                DishName = InternationaHelper.GetBeforeSeparatorFlagData(response.contactdishname),
                DishType = (EnumDishType)response.dishtype,
                DishCount = response.dishnum,
                Unit = InternationaHelper.GetBeforeSeparatorFlagData(response.dishunitid),
                SrcUnit = response.dishunitid,
                FishPotType = EnumFishPotType.None,
                GroupId = response.groupid,
                Menuid = response.id,
                NeedWeigh = Convert.ToBoolean(response.weigh),
                Price = response.price,
                VipPrice = response.vipprice,
            };
        }

        private static PayBillDetailRequest ToPayBillDetailRequest(BillPayInfo payInfo)
        {
            return new PayBillDetailRequest
            {
                bankCardNo = payInfo.BankCardNo,
                coupondetailid = payInfo.CouponDetailId,
                couponid = payInfo.CouponId,
                couponnum = payInfo.CouponNum.ToString(),
                payAmount = payInfo.PayAmount,
                memerberCardNo = payInfo.MemberCardNo,
                payWay = ((int)payInfo.PayType).ToString(),
            };
        }

        /// <summary>
        /// 把字符串转换成Int。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static int Parse2Int(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            int retValue;
            int.TryParse(value, out retValue);
            return retValue;
        }

        private static DateTime? Parse2DateTime(string value, string format)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            return Parse2DateTime(value, format, DateTime.MinValue);
        }

        private static DateTime Parse2DateTime(string value, string format, DateTime defaulTime)
        {
            if (string.IsNullOrEmpty(value))
                return defaulTime;

            try
            {
                return DateTime.ParseExact(value, format, null);
            }
            catch (Exception exp)
            {
                ErrLog.Instance.E(string.Format("日期转换异常。src:{0},   fmt:{1}", value, format), exp);
                return defaulTime;
            }
        }

        /// <summary>
        /// 根据顺序取第一个不为空的值。
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private static string GetAnOrderValue(params string[] values)
        {
            return values.FirstOrDefault(value => !string.IsNullOrEmpty(value));
        }

        private static List<DishDetailRequest> ToDishDetailRequestList(List<OrderDishInfo> dishInfos)
        {
            return dishInfos.Select(ToDishDetailRequest).ToList();
        }

        private static DishDetailRequest ToDishDetailRequest(OrderDishInfo dishInfo)
        {
            DishDetailRequest request = ToDishDetailRequestNormal(dishInfo);
            if (dishInfo.DishType == EnumDishType.Packages || dishInfo.DishType == EnumDishType.FishPot)
            {
                if (dishInfo.DishInfos != null && dishInfo.DishInfos.Any())
                {
                    request.dishes = dishInfo.DishInfos.Select(ToDishDetailRequestNormal).ToList();
                }
            }

            return request;
        }

        private static DishDetailRequest ToDishDetailRequestNormal(OrderDishInfo dishInfo)
        {
            return new DishDetailRequest
            {
                userName = dishInfo.UserName,
                dishid = dishInfo.DishId,
                dishunit = dishInfo.SrcDishUnit,
                dishtype = ((int)dishInfo.DishType),
                dishnum = dishInfo.DishNum,
                pricetype = ((int)dishInfo.OrderType).ToString(),
                orderprice = dishInfo.Price,
                orignalprice = dishInfo.PriceSource,
                orderid = dishInfo.OrderId,
                primarykey = dishInfo.PrimaryKey,
                dishstatus = ((int)dishInfo.DishStatus).ToString(),
                ispot = dishInfo.IsPot ? 1 : 0,
                freeuser = dishInfo.FreeUserId,
                freeauthorize = dishInfo.FreeAuthorizeId,
                freereason = dishInfo.FreeReason,
                taste = string.IsNullOrEmpty(dishInfo.TempDishName) ? dishInfo.Taste : dishInfo.TempDishName,
                sperequire = dishInfo.Diet ?? "",//忌口设成null会导致打印的厨打单上显示null。
            };
        }

        private static TableFullInfo ToTableFullInfo(TableFullInfoResponse orderResponse, List<OrderDishDataResponse> dishResponses)
        {
            var item = new TableFullInfo
            {
                AreaName = orderResponse.areaname,
                AreaNo = orderResponse.areaid,
                OrderId = orderResponse.orderid,
                PeopleNumber = orderResponse.personNum,
                CustomerNumber = orderResponse.custnum,
                TableStatus = (EnumTableStatus)orderResponse.status,
                TableId = orderResponse.tableid,
                TableName = orderResponse.tableName,
                TableNo = orderResponse.tableNo,
                TableType = (EnumTableType)orderResponse.tabletype,
                MinPrice = orderResponse.minprice,
                FixPrice = orderResponse.fixprice,
                TotalAmount = orderResponse.dueamount,
                MemberNo = orderResponse.memberno,
                OrderStatus = (EnumOrderStatus)orderResponse.orderstatus,
                WaiterId = orderResponse.userid,
                TipAmount = orderResponse.tipAmount ?? 0,
            };

            if (dishResponses != null && dishResponses.Any())
            {
                //这里需要将套餐和鱼锅的主体菜和内部的菜关联起来
                var groupSrc = dishResponses.GroupBy(t => t.parentkey);
                foreach (var group in groupSrc)
                {
                    if (group.Count() > 1)
                    {
                        var temp = group.FirstOrDefault(t => t.ismaster == 1);
                        var masterItem = ToDishInfo(temp);
                        item.DishInfos.Add(masterItem);
                        var items = group.Where(t => t.ismaster == 0).ToList();
                        if (items.Any())
                        {
                            masterItem.DishInfos = new List<OrderDishInfo>();
                            foreach (var groupItem in items)
                            {
                                var subItem = ToDishInfo(groupItem);
                                if (masterItem.DishType == EnumDishType.FishPot)
                                    subItem.IsFishPotDish = true;
                                else if (masterItem.DishType == EnumDishType.Packages)
                                    subItem.IsComboDish = true;
                                masterItem.DishInfos.Add(subItem);
                                item.DishInfos.Add(subItem);
                            }
                        }
                    }
                    else
                    {
                        foreach (var groupItem in group)
                        {
                            item.DishInfos.Add(ToDishInfo(groupItem));
                        }
                    }
                }
            }

            int index = 1;
            foreach (var dishInfo in item.DishInfos)
            {
                dishInfo.Index = index++;
            }
            return item;
        }

        private static PrintOrderInfo ToPrintOrderInfo(PrintOrderInfoResponse response)
        {
            return new PrintOrderInfo
            {
                BranchAddress = Globals.BranchInfo.BranchAddress,
                BranchId = Globals.BranchInfo.BranchId,
                BranchName = Globals.BranchInfo.BranchName,
                //BranchTelephone = Globals.BranchTelephone,
                AreaName = response.areaname,
                TableName = response.tableName,
                BeginTime = Parse2DateTime(response.begintime, DateTimeFormat, DateTime.MinValue),
                EndTime = Parse2DateTime(response.endtime, DateTimeFormat, DateTime.Now),
                CustomerNumber = response.custnum,
                OrderId = response.orderid,
                PrintSettlementTimes = response.printcount,
                PrintPresettTimes = response.befprintcount,
                DiscountPrice = response.discountamount ?? 0,
                FreeAmount = response.zdAmount ?? 0,
                TotalAmount = response.dueamount,
                TotalDishAmount = response.dueamount + (response.zdAmount ?? 0),
                RoundingAmount = response.payamount2,
                RemoveOddAmount = response.payamount ?? 0,
                PaidAmount = response.ssamount,
                Printer = Globals.UserInfo.FullName,
                WaiterName = response.fullname,
            };
        }

        private static PrintOrderDishInfo ToOrderDishInfo(OrderDishDataResponse response)
        {
            return new PrintOrderDishInfo
            {
                DishName = response.title,
                DishAmount = response.payamount ?? 0,
                DishNumUnit = string.Format("{0}{1}", response.dishnum, response.dishunit),
                DishPrice = response.orderprice ?? 0,
            };
        }

        private static PrintPayDetail ToPrintPayDetail(PayDetailResponse response)
        {
            return new PrintPayDetail
            {
                PayWay = response.itemDesc,
                Payamount = response.payamount,
                Remark = response.bankcardno
            };
        }

        /// <summary>
        /// 优免原因转成手工类优惠类型。
        /// </summary>
        /// <param name="freeReason">优免原因。</param>
        /// <returns></returns>
        private static EnumHandCouponType? Convert2HandCounCouponType(string freeReason)
        {
            if (string.IsNullOrEmpty(freeReason))
                return null;

            try
            {
                return (EnumHandCouponType)Enum.Parse(typeof(EnumHandCouponType), freeReason);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 营业名称报表数据转换

        /// <summary>
        /// 营业明细（品类、金额）
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static List<MCategory> ToCategory(List<CategoryResponse> response)
        {
            var categorys = new List<MCategory>();
            foreach (var category in response)
            {
                var cate = new MCategory();
                cate.DishName = category.itemDesc;
                cate.money = category.orignalprice;
                categorys.Add(cate);
            }
            return categorys;
        }

        /// <summary>
        /// 营业明细(团购券)
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static List<MHangingMoney> ToGroupon(List<GrouponResponse> response)
        {
            var categorys = new List<MHangingMoney>();
            foreach (var info in response)
            {
                var cate = new MHangingMoney();
                cate.HangingName = info.pname;
                cate.HangingMoney = info.payamount;
                categorys.Add(cate);
            }
            return categorys;
        }

        /// <summary>
        /// 营业明细(挂账单位)
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal static List<MHangingMoney> ToGzdw(List<GzdwResponse> response)
        {
            var categorys = new List<MHangingMoney>();
            foreach (var info in response)
            {
                var cate = new MHangingMoney();
                cate.HangingName = info.gzdw;
                cate.HangingMoney = info.gzze;
                categorys.Add(cate);
            }
            return categorys;
        }

        #endregion

        #region 订单详细数据转换

        internal static TableFullInfo ToOrderInfo(GetOrderInfoResponse response)
        {
            var tableFullInfo = new TableFullInfo();
            AddDishInfos(response.data.rows, ref tableFullInfo);
            ToAccount(response.data.preferentialInfo, ref tableFullInfo);
            ToTableBaseInfo(response.data.userOrderInfo, ref tableFullInfo);
            return tableFullInfo;
        }


        /// <summary>
        /// 添加菜品到菜单信息里
        /// </summary>
        /// <param name="dishInfosResponse"></param>
        /// <param name="tableFullInfo"></param>
        internal static void AddDishInfos(List<DishInfosResponse> dishInfosResponse, ref TableFullInfo tableFullInfo)
        {
            if (dishInfosResponse != null)
            {
                int index = 1;
                foreach (var dish in dishInfosResponse)
                {
                    if (dish.dishes != null) //套餐和鱼锅
                    {
                        var masterItem = ToDishInfo(dish, dish.dishid, index);
                        tableFullInfo.DishInfos.Add(masterItem);
                        index++;

                        masterItem.DishInfos = new List<OrderDishInfo>();
                        foreach (var groupItem in dish.dishes)
                        {

                            var subItem = ToDishInfo(groupItem, string.Empty, index);
                            if (masterItem.DishType == EnumDishType.FishPot)
                                subItem.IsFishPotDish = true;
                            else if (masterItem.DishType == EnumDishType.Packages)
                                subItem.IsComboDish = true;
                            masterItem.DishInfos.Add(subItem);
                            tableFullInfo.DishInfos.Add(subItem);
                            index++;
                        }
                    }
                    else //单品
                    {
                        tableFullInfo.DishInfos.Add(ToDishInfo(dish, string.Empty, index));
                    }
                    index++;
                }
            }
        }

        /// <summary>
        /// 转换成菜品模型
        /// </summary>
        /// <param name="response"></param>
        /// <param name="relateDishId"></param>
        /// <returns></returns>
        internal static OrderDishInfo ToDishInfo(DishGroupInfo response, string relateDishId, int index)
        {
            var dishName = InternationaHelper.GetBeforeSeparatorFlagData(response.dishname);
            var price = string.IsNullOrEmpty(response.orderprice) ? 0 : decimal.Parse(response.orderprice);
            var dishStatus = string.IsNullOrEmpty(response.dishstatus) ? 0 : int.Parse(response.dishstatus);
            return new OrderDishInfo
            {
                Index = index,
                DishId = response.dishid,
                DishName = dishName,
                RelateDishId = relateDishId,
                DishNum = response.dishnum,
                DishStatus = (EnumDishStatus)dishStatus,
                DishType = (EnumDishType)response.dishtype,
                DishUnit = InternationaHelper.GetBeforeSeparatorFlagData(response.dishunit),
                TempDishName = dishName.Contains("临时菜") ? response.taste : "",
                SrcDishUnit = response.dishunit,
                Price = price,
                PrimaryKey = response.primarykey,
                PayAmount = response.dishnum * price,
                IsPot = response.ispot.Equals("1"),
                IsMaster = !string.IsNullOrEmpty(relateDishId),//有值表示为套餐和鱼锅
            };
        }

        /// <summary>
        /// 账单金额、优惠转换
        /// </summary>
        /// <param name="preferential"></param>
        /// <param name="tableFullInfo"></param>
        internal static void ToAccount(preferentialInfoResponse preferential, ref TableFullInfo tableFullInfo)
        {
            tableFullInfo.ClonePreferentialInfo(preferential);
        }

        /// <summary>
        /// 转换桌台基础信息
        /// </summary>
        internal static void ToTableBaseInfo(UserOrderInfo userOrderInfo, ref TableFullInfo tableFullInfo)
        {
            tableFullInfo.MemberNo = userOrderInfo.memberno;
            tableFullInfo.OrderStatus = (EnumOrderStatus)userOrderInfo.orderStatus;
            tableFullInfo.CustomerNumber = userOrderInfo.customerNumber;
            tableFullInfo.OrderInvoiceTitle = userOrderInfo.orderInvoiceTitle;
            tableFullInfo.OrderId = userOrderInfo.orderid;
            tableFullInfo.TableStatus = (EnumTableStatus)userOrderInfo.tableStatus;
        }
        #endregion
    }
}