using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using CanDao.Pos.Common;
using CanDao.Pos.IService;
using CanDao.Pos.Model;
using CanDao.Pos.Model.Enum;
using CanDao.Pos.Model.Request;
using CanDao.Pos.UI.MainView.View;
using CanDao.Pos.UI.Utility;
using CanDao.Pos.UI.Utility.View;
using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    public class OrderDishWndVm : NormalWindowViewModel
    {
        #region Constrator

        public OrderDishWndVm(TableFullInfo info)
        {
            Data = info;
            DishGroups = new ObservableCollection<MenuDishGroupInfo>();
            MenuDishes = new ObservableCollection<MenuDishInfo>();
            OrderDishInfos = new ObservableCollection<OrderDishInfo>();
            OrderDishInfos = new ObservableCollection<OrderDishInfo>();
        }

        #endregion

        #region Properties

        public OrderDishWindow OwnerWnd
        {
            get { return (OrderDishWindow)OwnerWindow; }
        }

        /// <summary>
        /// 餐台信息。
        /// </summary>
        public TableFullInfo Data { get; set; }

        /// <summary>
        /// 菜品分组集合。
        /// </summary>
        public ObservableCollection<MenuDishGroupInfo> DishGroups { get; private set; }

        /// <summary>
        /// 下单的菜品集合。
        /// </summary>
        public ObservableCollection<OrderDishInfo> OrderDishInfos { get; private set; }

        /// <summary>
        /// 当前分类菜谱集合。
        /// </summary>
        public ObservableCollection<MenuDishInfo> MenuDishes { get; set; }

        /// <summary>
        /// 选中的菜品分组。
        /// </summary>
        private MenuDishGroupInfo _selectedMenuDishGroup;

        /// <summary>
        /// 选中的菜品分组。
        /// </summary>
        public MenuDishGroupInfo SelectedMenuDishGroup
        {
            get { return _selectedMenuDishGroup; }
            set
            {
                _selectedMenuDishGroup = value;
                RaisePropertiesChanged("SelectedMenuDishGroup");

                FilterMenuGruopByLetter();
            }
        }

        /// <summary>
        /// 当前选择的菜品。
        /// </summary>
        public MenuDishInfo SelectedDish { get; private set; }

        /// <summary>
        /// 总金额。
        /// </summary>
        private decimal _totalAmount;
        /// <summary>
        /// 总金额。
        /// </summary>
        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                RaisePropertyChanged("TotalAmount");
            }
        }

        /// <summary>
        /// 获取下单类型。
        /// </summary>
        public EnumOrderType OrderType { get; private set; }

        /// <summary>
        /// 当前选择的订单菜品。
        /// </summary>
        private OrderDishInfo _selectedOrderDish;
        /// <summary>
        /// 当前选择的订单菜品。
        /// </summary>
        public OrderDishInfo SelectedOrderDish
        {
            get { return _selectedOrderDish; }
            set
            {
                _selectedOrderDish = value;
                RaisePropertiesChanged("SelectedOrderDish");
            }
        }

        /// <summary>
        /// 全单备注。
        /// </summary>
        private string _orderRemark;
        /// <summary>
        /// 全单备注。
        /// </summary>
        public string OrderRemark
        {
            get { return _orderRemark; }
            set
            {
                _orderRemark = value;
                RaisePropertiesChanged("OrderRemark");
            }
        }

        /// <summary>
        /// 菜单过滤字母。
        /// </summary>
        private string _filterMenuGroup;
        /// <summary>
        /// 菜单过滤字母。
        /// </summary>
        public string FilterMenuGroup
        {
            get { return _filterMenuGroup; }
            set
            {
                _filterMenuGroup = value;
                RaisePropertiesChanged("FilterMenuGroup");

                FilterMenuGruopByLetter();
            }
        }

        /// <summary>
        /// 是否订单被挂单了。
        /// </summary>
        public bool IsOrderHanged { get; set; }

        #endregion

        #region Command

        /// <summary>
        /// 选择一个菜品命令。
        /// </summary>
        public ICommand SelectDishCmd { get; private set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// 选择菜品命令执行方法。
        /// </summary>
        /// <param name="param"></param>
        private void SelectDish(object param)
        {
            var dishInfo = param as MenuDishInfo;
            if (dishInfo == null)
                return;

            SelectedDish = dishInfo;
            var arg = new Tuple<string, string>(dishInfo.DishId, dishInfo.SrcUnit);
            InfoLog.Instance.I("选择了一个菜品，开始检测菜品状态。");
            TaskService.Start(arg, CheckDishStatusProcess, CheckDishStatusComplete, "检测菜品状态...");
        }

        #endregion

        #region Protected Methods

        protected override void InitCommand()
        {
            base.InitCommand();
            SelectDishCmd = CreateDelegateCommand(SelectDish);
        }

        protected override void OnWindowLoaded(object param)
        {
            InfoLog.Instance.I("点菜窗口加载完成时...");
            if (Globals.DishGroupInfos == null)
            {
                InfoLog.Instance.I("菜谱缓存为空，开始获取菜谱信息...");
                TaskService.Start(null, GetMenuDishGroupInfoProcess, GetMenuDishGroupInfoComplete, "加载菜谱信息中...");
                return;
            }

            InfoLog.Instance.I("菜谱有缓存，加载菜谱...");

            DishGroups.Clear();
            Globals.DishGroupInfos.ForEach(t =>
            {
                t.SelectDishCount = 0m;
                DishGroups.Add(t);
            });
        }

        protected override void OperMethod(object param)
        {
            switch (param as string)
            {
                case "Free":
                    FreeDish();
                    break;
                case "Hang":
                    HangTakeoutOrder();
                    break;
                case "Order":
                    OrderDish();
                    break;
                case "Empty":
                    EmptyOrderDishes();
                    break;
                case "DishCountIncrease":
                    IncreaseDishCount();
                    break;
                case "DishCountReduce":
                    ReduceDishCount();
                    break;
                case "OrderRemark":
                    var orderRemarkWnd = new OrderRemarkWindow();
                    if (WindowHelper.ShowDialog(orderRemarkWnd, OwnerWnd))
                        OrderRemark = orderRemarkWnd.SelectedDiet;
                    break;
                case "Remark":
                    RemarkSelectedDish();
                    break;
                case "ClearFilter":
                    FilterMenuGroup = null;
                    break;
                case "InputNum":
                    InputDishCount();
                    break;
            }
        }

        protected override bool CanOperMethod(object param)
        {
            switch (param as string)
            {
                case "DishCountIncrease":
                case "DishCountReduce":
                case "InputNum":
                case "Remark":
                    return SelectedOrderDish != null;
                case "OrderRemark":
                    return OrderDishInfos != null && OrderDishInfos.Any();
                default:
                    return true;
            }
        }

        protected override void GroupMethod(object param)
        {
            switch (param as string)
            {
                case "PreGroup":
                    OwnerWnd.LbDishGroup.PreviousGroup();
                    break;
                case "NextGroup":
                    OwnerWnd.LbDishGroup.NextGroup();
                    break;
                case "OrderDishPreGroup":
                    OwnerWnd.DishGroupSelector.PreviousGroup();
                    break;
                case "OrderDishNextGroup":
                    OwnerWnd.DishGroupSelector.NextGroup();
                    break;
                case "MenuDishPreGroup":
                    OwnerWnd.MenuDishGroupSelector.PreviousGroup();
                    break;
                case "MenuDishNextGroup":
                    OwnerWnd.MenuDishGroupSelector.NextGroup();
                    break;
            }
        }

        protected override bool CanGroupMethod(object param)
        {
            switch (param as string)
            {
                case "PreGroup":
                    return OwnerWnd.LbDishGroup.CanPreviousGroup;
                case "NextGroup":
                    return OwnerWnd.LbDishGroup.CanNextGruop;
                case "OrderDishPreGroup":
                    return OwnerWnd.DishGroupSelector.CanPreviousGroup;
                case "OrderDishNextGroup":
                    return OwnerWnd.DishGroupSelector.CanNextGruop;
                case "MenuDishPreGroup":
                    return OwnerWnd.MenuDishGroupSelector.CanPreviousGroup;
                case "MenuDishNextGroup":
                    return OwnerWnd.MenuDishGroupSelector.CanNextGruop;
                default:
                    return true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 获取菜品分组集合的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object GetMenuDishGroupInfoProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, List<MenuDishGroupInfo>>("创建IOrderService服务失败。", null);

            return service.GetMenuDishInfos(Globals.UserInfo.FullName);
        }

        /// <summary>
        /// 获取菜品分组集合执行完成。
        /// </summary>
        /// <param name="obj"></param>
        private void GetMenuDishGroupInfoComplete(object obj)
        {
            var result = (Tuple<string, List<MenuDishGroupInfo>>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取所有菜品信息失败。" + result.Item1);
                MessageDialog.Warning(result.Item1, OwnerWnd);
                CloseWindow(false);
                return;
            }

            InfoLog.Instance.I("结束获取菜谱信息，菜品分类个数：{0}", result.Item2.Count);
            Globals.DishGroupInfos = result.Item2;
            Globals.DishGroupInfos.ForEach(DishGroups.Add);
        }

        /// <summary>
        /// 下单执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object OrderDishProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, List<MenuDishGroupInfo>>("创建IOrderService服务失败。", null);

            if (OrderType == EnumOrderType.Free)
            {
                //将赠菜原因设置到菜品集合里。
                var reason = arg as string;

                foreach (var orderDishInfo in OrderDishInfos)
                {
                    orderDishInfo.OrderType = OrderType;
                    orderDishInfo.Price = 0m;
                    orderDishInfo.FreeReason = reason;
                    orderDishInfo.FreeUserId = Globals.UserInfo.UserName;
                    orderDishInfo.FreeAuthorizeId = Globals.Authorizer.UserName;
                }
            }

            var dishList = OrderDishInfos.ToList();
            InfoLog.Instance.I("开始执行下单任务...");
            if (Data.TableType == EnumTableType.CFTable || Data.TableType == EnumTableType.CFTakeout || Data.TableType == EnumTableType.Takeout) //咖啡台和外卖模式都走咖啡模式的下单。
                return service.OrderDishCf(Data.OrderId, Data.TableName, OrderRemark, dishList);

            //如果餐具收费，则添加餐具到下单菜品里。
            if (Globals.IsDinnerWareEnable && !Data.DishInfos.Any())
            {
                InfoLog.Instance.I("餐具收费，添加餐具到下单菜品列表里。");
                var dinnerWare = Globals.DinnerWareInfo.CloneData();
                UpdateOrderDishNum(dinnerWare, Data.CustomerNumber);
                dishList.Add(dinnerWare);
            }

            return service.OrderDish(Data.OrderId, Data.TableName, OrderRemark, dishList);
        }

        /// <summary>
        /// 下单执行完成。
        /// </summary>
        /// <param name="obj"></param>
        private void OrderDishComplete(object obj)
        {
            var result = (string)obj;
            if (!string.IsNullOrEmpty(result))
            {
                ErrLog.Instance.E(result);
                MessageDialog.Warning(result, OwnerWnd);
                return;
            }

            var type = (OrderType == EnumOrderType.Free) ? "赠菜" : "下单";
            var msg = string.Format("桌号\"{0}\"{1}成功。", Data.TableName, type);
            InfoLog.Instance.I(msg);
            NotifyDialog.Notify(msg, OwnerWnd.Owner);
            CommonHelper.BroadcastMessageAsync(EnumBroadcastMsgType.SyncOrder, Data.OrderId);

            OwnerWnd.DialogResult = true;
        }

        /// <summary>
        /// 检测菜品状态执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object CheckDishStatusProcess(object arg)
        {
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
                return new Tuple<string, bool>("创建IOrderService服务实例失败。", false);

            var param = (Tuple<string, string>)arg;
            return service.CheckDishStatus(param.Item1, param.Item2);
        }

        /// <summary>
        /// 检测菜品状态执行完成。
        /// </summary>
        /// <param name="obj"></param>
        private void CheckDishStatusComplete(object obj)
        {
            var result = (Tuple<string, bool>)obj;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("检测菜品状态失败。" + result.Item1);
                MessageDialog.Warning(string.Format("检测菜品状态失败：{0}", result), OwnerWnd);
                return;
            }

            if (!result.Item2)
            {
                InfoLog.Instance.I("结束检测菜品状态，选择的菜品“{0}”已估清。", SelectedDish.DishName);
                MessageDialog.Warning("选择的菜品已估清。", OwnerWnd);
                return;
            }

            InfoLog.Instance.I("结束检测菜品状态，选择的菜品“{0}”状态正常。", SelectedDish.DishName);
            if (SelectedDish.DishType == EnumDishType.Packages)//套餐处理
            {
                var request = new GetMenuComboDishRequest { dishides = SelectedDish.DishId, menuid = SelectedDish.Menuid };
                TaskService.Start(request, GetComboDishInfoProcess, GetComboDishInfoComplete, "获取套餐信息中...");
            }
            else if (SelectedDish.DishType == EnumDishType.FishPot)//鱼锅处理
            {
                TaskService.Start(SelectedDish.DishId, GetFishPotDishInfoProcess, GetFishPotDishInfoComplete, "获取鱼锅信息中...");
            }
            else if (SelectedDish.DishType == EnumDishType.Normal)//普通菜
            {


                //临时菜处理
                if (SelectedDish.DishName.Contains("临时菜"))
                {
                    var customDishes = new UcCustomDishesViewModel();
                    var wind = customDishes.GetWindow();
                    if (wind.ShowDialog() == true)
                    {
                        SelectedDish.SelectedCount = decimal.Parse(customDishes.Model.DishesCount) * decimal.Parse(customDishes.Model.Price);
                        SelectedDish.TempDishName = customDishes.Model.DishesName;
                        AddTempDishInfo(SelectedDish);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    string taste = null;
                    string diet = null;
                    SelectedDish.SelectedCount = 1;//设定点菜数量为1。
                    if (SelectedDish.Tastes != null && SelectedDish.Tastes.Any())//有口味则弹出口味设置窗口
                    {
                        var dishSimpleInfo = new DishSimpleInfo
                        {
                            DishName = SelectedDish.DishName,
                            DishPrice = SelectedDish.Price ?? 0,
                            DishUnit = SelectedDish.Unit,
                            DishNum = 1,
                        };
                        var tasteDietWnd = new SetDishTasteAndDietWindow(SelectedDish.Tastes, dishSimpleInfo, true);
                        if (!WindowHelper.ShowDialog(tasteDietWnd, OwnerWnd))
                            return;

                        taste = tasteDietWnd.SelectedTaste;
                        diet = tasteDietWnd.SelectedDiet;
                        SelectedDish.SelectedCount = tasteDietWnd.DishNum;
                    }
                    AddDishInfo(SelectedDish, taste, diet);
                }

            }
        }

        /// <summary>
        /// 添加菜品信息到点菜单集合。
        /// </summary>
        /// <param name="dishInfo">添加的菜单菜品。</param>
        /// <param name="taste">菜品的口味。</param>
        /// <param name="diet">菜品的忌口。</param>
        private void AddDishInfo(MenuDishInfo dishInfo, string taste, string diet)
        {
            var item = OrderDishInfos.FirstOrDefault(t => OrderDishEqual(t, dishInfo, taste, diet));
            if (item != null)
                UpdateOrderDishNum(item, item.DishNum + dishInfo.SelectedCount);
            else
                OrderDishInfos.Add(Convert2OrderDishInfo(SelectedDish, taste, diet));

            DoWhenDishChanged();
        }
        /// <summary>
        /// 添加临时菜到菜单集合
        /// </summary>
        /// <param name="dishInfo"></param>
        private void AddTempDishInfo(MenuDishInfo dishInfo)
        {
            var dish = Convert2OrderDishInfo(dishInfo, "", "");
            dish.TempDishName = dishInfo.TempDishName;
            OrderDishInfos.Add(dish);
            DoWhenDishChanged();
        }

        /// <summary>
        /// 检测已点菜品是否跟刚点的菜谱中的菜是用一个。
        /// </summary>
        /// <param name="srcItem">已点菜品。</param>
        /// <param name="dstItem">菜谱中刚点的菜。</param>
        /// <param name="taste">选择的口味。</param>
        /// <param name="diet">选择的忌口。</param>
        /// <returns></returns>
        private bool OrderDishEqual(OrderDishInfo srcItem, MenuDishInfo dstItem, string taste, string diet)
        {
            if (srcItem == null || dstItem == null)
                return false;

            if (srcItem.IsComboDish || srcItem.IsFishPotDish)
                return false;

            if (srcItem.DishId != dstItem.DishId || !srcItem.SrcDishUnit.Equals(dstItem.SrcUnit))
                return false;

            if (!string.IsNullOrEmpty(srcItem.Taste) && !srcItem.Taste.Equals(taste))
                return false;

            if (!string.IsNullOrEmpty(srcItem.Diet) && !srcItem.Diet.Equals(diet))
                return false;
            return true;
        }



        /// <summary>
        /// 添加套餐菜。
        /// </summary>
        /// <param name="comboFullInfo">套餐全信息。</param>
        /// <param name="diet">套餐忌口信息。</param>
        private void AddComboDishInfo(MenuComboFullInfo comboFullInfo, string diet)
        {
            var comboOrderDish = Convert2OrderDishInfo(comboFullInfo, diet);
            OrderDishInfos.Add(comboOrderDish);//添加套餐主菜
            if (comboOrderDish.DishInfos != null && comboOrderDish.DishInfos.Any())//将套餐内的菜加到列表来。
                comboOrderDish.DishInfos.ForEach(OrderDishInfos.Add);
            DoWhenDishChanged();
        }

        /// <summary>
        /// 添加鱼锅菜。
        /// </summary>
        /// <param name="fishPotFullInfo">鱼锅全信息。</param>
        /// <param name="taste">套餐口味信息。</param>
        /// <param name="diet">套餐忌口信息。</param>
        private void AddFishPotDishInfo(MenuFishPotFullInfo fishPotFullInfo, string taste, string diet)
        {
            var fishPotDish = Convert2OrderDishInfo(fishPotFullInfo, taste, diet);
            OrderDishInfos.Add(fishPotDish);
            if (fishPotDish.DishInfos != null && fishPotDish.DishInfos.Any())//将鱼锅内的菜加到列表来。
                fishPotDish.DishInfos.ForEach(OrderDishInfos.Add);
            DoWhenDishChanged();
        }

        /// <summary>
        /// 获取下单服务员的编号。
        /// </summary>
        /// <returns></returns>
        private string GetOrderWaiterId()
        {
            return !string.IsNullOrEmpty(Data.WaiterId) ? Data.WaiterId : Globals.UserInfo.UserName;
        }

        /// <summary>
        /// 更新订单菜品数量，和小计价格。
        /// </summary>
        /// <param name="orderDishInfo">订单菜品。</param>
        /// <param name="dishNum">订单菜品的数量。</param>
        private void UpdateOrderDishNum(OrderDishInfo orderDishInfo, decimal dishNum)
        {
            if (orderDishInfo == null)
                return;

            orderDishInfo.DishNum = dishNum;
            orderDishInfo.PayAmount = orderDishInfo.Price * dishNum;
        }

        /// <summary>
        /// 当菜品改变时需要做的动作。
        /// </summary>
        private void DoWhenDishChanged()
        {
            TotalAmount = OrderDishInfos.Sum(t => t.Price * t.DishNum);

            //得到每个菜单分组中被选择的菜品数量。
            foreach (var dishGroup in DishGroups)
            {
                dishGroup.SelectDishCount = OrderDishInfos.Where(t => !t.IsFishPotDish && !t.IsComboDish && t.MenuGroupId.Equals(dishGroup.GroupId)).Sum(t => t.DishNum);
            }
        }

        /// <summary>
        /// 将菜单的菜品信息转换成菜单的菜品信息。
        /// </summary>
        /// <param name="dishInfo">点的菜品信息。</param>
        /// <param name="taste">菜品口味信息。</param>
        /// <param name="diet">菜品忌口信息。</param>
        /// <param name="isComboDish">是否是套餐内的菜。</param>
        /// <returns></returns>
        private OrderDishInfo Convert2OrderDishInfo(MenuDishInfo dishInfo, string taste, string diet, bool isComboDish = false)
        {
            var dishNum = dishInfo.SelectedCount;
            var price = isComboDish ? 0 : (dishInfo.Price ?? 0);//套餐内的菜价格为0。
            var item = new OrderDishInfo
            {
                DishName = dishInfo.DishName,
                DishId = dishInfo.DishId,
                DishStatus = dishInfo.NeedWeigh ? EnumDishStatus.ToBeWeighed : EnumDishStatus.Normal,
                DishType = dishInfo.DishType,
                SrcDishUnit = dishInfo.SrcUnit,
                DishUnit = InternationaHelper.GetBeforeSeparatorFlagData(dishInfo.SrcUnit),
                Price = price,
                PriceSource = dishInfo.Price ?? 0,
                MemberPrice = dishInfo.VipPrice ?? (dishInfo.Price ?? 0),
                MenuGroupId = dishInfo.GroupId,
                PrimaryKey = Guid.NewGuid().ToString(),//当添加菜品到购物车时，生成菜品唯一标识，用来避免重复下单。
                OrderId = Data.OrderId,
                UserName = GetOrderWaiterId(),
                IsPot = dishInfo.IsPot,
                Taste = taste,
                Diet = diet,
            };
            UpdateOrderDishNum(item, dishNum);
            return item;
        }

        /// <summary>
        /// 将套餐的菜品信息转换成菜单的菜品信息。
        /// </summary>
        /// <param name="comboFullInfo">套餐总信息。</param>
        /// <param name="diet">菜品忌口信息。</param>
        /// <returns></returns>
        private OrderDishInfo Convert2OrderDishInfo(MenuComboFullInfo comboFullInfo, string diet)
        {
            //套餐的忌口信息会设置到套餐内部所有菜品上，且套餐没有口味设置。
            var comboOrderDishInfo = Convert2OrderDishInfo(comboFullInfo.ComboSelfInfo, "", diet);

            comboOrderDishInfo.DishInfos = new List<OrderDishInfo>();
            if (comboFullInfo.SingleDishInfos != null)
                comboOrderDishInfo.DishInfos.AddRange(comboFullInfo.SingleDishInfos.Select(t => Convert2OrderDishInfo(t, "", diet, true)));
            if (comboFullInfo.ComboDishInfos != null)
            {
                var allComboDishInfos = new List<MenuDishInfo>();
                comboFullInfo.ComboDishInfos.ForEach(t =>
                {
                    allComboDishInfos.AddRange(t.SourceDishes.Where(y => y.SelectedCount > 0));//将选择的菜品提取出来。
                });
                comboOrderDishInfo.DishInfos.AddRange(allComboDishInfos.Select(t => Convert2OrderDishInfo(t, "", diet, true)));
            }

            if (comboOrderDishInfo.DishInfos != null)
                comboOrderDishInfo.DishInfos.ForEach(t => t.IsComboDish = true);//标记套餐内的菜。

            return comboOrderDishInfo;
        }

        /// <summary>
        /// 将鱼锅的菜品信息转换成菜单的菜品信息。
        /// </summary>
        /// <param name="fishPotFullInfo">鱼锅总信息。</param>
        /// <param name="taste">菜品口味信息。</param>
        /// <param name="diet">菜品忌口信息。</param>
        /// <returns></returns>
        private OrderDishInfo Convert2OrderDishInfo(MenuFishPotFullInfo fishPotFullInfo, string taste, string diet)
        {
            var fishPotDishInfo = Convert2OrderDishInfo(fishPotFullInfo.FishPotSelfInfo, taste, diet);//鱼锅的口味只显示在鱼锅菜主体上。

            fishPotDishInfo.DishInfos = new List<OrderDishInfo> { Convert2OrderDishInfo(fishPotFullInfo.PotInfo, "", "") };//添加锅底。
            if (fishPotFullInfo.FishDishes != null)//鱼锅里的鱼。
                fishPotDishInfo.DishInfos.AddRange(fishPotFullInfo.FishDishes.Select(t => Convert2OrderDishInfo(t, "", "")));

            if (fishPotDishInfo.DishInfos != null)//将鱼锅里的所有菜的标记为鱼锅的菜。
                fishPotDishInfo.DishInfos.ForEach(t => t.IsFishPotDish = true);

            return fishPotDishInfo;
        }

        /// <summary>
        /// 获取套餐信息的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object GetComboDishInfoProcess(object arg)
        {
            InfoLog.Instance.I("开始获取套餐信息...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
            {
                var msg = "创建IOrderService服务失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            return service.GetMenuComboDishes((GetMenuComboDishRequest)arg);
        }

        /// <summary>
        /// 获取套餐信息执行完成。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private void GetComboDishInfoComplete(object arg)
        {
            var result = (Tuple<string, MenuComboFullInfo>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取套餐信息失败。" + result);
                MessageDialog.Warning(result.Item1, OwnerWnd);
                return;
            }

            InfoLog.Instance.I("结束获取套餐信息，弹出套餐选择窗口。");
            result.Item2.ComboSelfInfo = SelectedDish;
            var wnd = new MenuComboDishSelectWindow(result.Item2);
            if (WindowHelper.ShowDialog(wnd, OwnerWnd))
            {
                AddComboDishInfo(wnd.ComboFullInfo, wnd.SelectedDiet);
            }
        }

        /// <summary>
        /// 获取鱼锅信息的执行方法。
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private object GetFishPotDishInfoProcess(object arg)
        {
            InfoLog.Instance.I("开始获取鱼锅信息...");
            var service = ServiceManager.Instance.GetServiceIntance<IOrderService>();
            if (service == null)
            {
                var msg = "创建IOrderService服务失败。";
                ErrLog.Instance.E(msg);
                return msg;
            }

            return service.GetFishPotDishInfo((string)arg);
        }

        /// <summary>
        /// 获取鱼锅信息执行完成。
        /// </summary>
        /// <param name="arg"></param>
        private void GetFishPotDishInfoComplete(object arg)
        {
            var result = (Tuple<string, MenuFishPotFullInfo>)arg;
            if (!string.IsNullOrEmpty(result.Item1))
            {
                ErrLog.Instance.E("获取鱼锅信息失败。" + result);
                MessageDialog.Warning(result.Item1, OwnerWnd);
                return;
            }

            InfoLog.Instance.I("结束获取鱼锅信息，弹出鱼锅选择窗口。");
            result.Item2.FishPotSelfInfo = SelectedDish;
            var wnd = new MenuFishPotSelectWindow(result.Item2);
            if (WindowHelper.ShowDialog(wnd, OwnerWnd))
            {
                AddFishPotDishInfo(wnd.FishPotFullInfo, wnd.SelectedTaste, wnd.SelectedDiet);
            }
        }

        /// <summary>
        /// 清空所有已点菜品。
        /// </summary>
        private void EmptyOrderDishes()
        {
            if (MessageDialog.Quest("确定要清空已选菜品吗？"))
            {
                OrderDishInfos.Clear();
                TotalAmount = 0;
                foreach (var dishGroup in DishGroups)
                {
                    dishGroup.SelectDishCount = 0m;
                }
            }
            DoWhenDishChanged();
        }

        /// <summary>
        /// 菜品下单。
        /// </summary>
        private void OrderDish()
        {
            if (!OrderDishInfos.Any())
            {
                MessageDialog.Warning("请先选择菜品。", OwnerWnd);
                return;
            }
            if (!MessageDialog.Quest(string.Format("餐台【{0}】确定下单吗？", Data.TableName)))
                return;

            OrderType = EnumOrderType.Normal;
            TaskService.Start(null, OrderDishProcess, OrderDishComplete, null);
        }

        /// <summary>
        /// 赠菜。
        /// </summary>
        private void FreeDish()
        {
            if (!OrderDishInfos.Any())
            {
                MessageDialog.Warning("请先选择菜品。", OwnerWnd);
                return;
            }

            var reasonWnd = new DishGiftReasonSelectWindow();
            if (!WindowHelper.ShowDialog(reasonWnd, OwnerWnd))
                return;

            var authorizeWnd = new AuthorizationWindow(EnumRightType.FreeDish);
            if (!WindowHelper.ShowDialog(authorizeWnd, OwnerWnd))
                return;

            OrderType = EnumOrderType.Free;
            TaskService.Start(reasonWnd.SelectedReason, OrderDishProcess, OrderDishComplete, null);
        }

        /// <summary>
        /// 外卖挂单。
        /// </summary>
        private void HangTakeoutOrder()
        {
            var wnd = new SelectTakeoutOnAccountCompany(Data);
            if (WindowHelper.ShowDialog(wnd, OwnerWnd))
            {
                CloseWindow(true);
                IsOrderHanged = true;
            }
        }

        /// <summary>
        /// 备注选中菜品。
        /// </summary>
        private void RemarkSelectedDish()
        {
            if (SelectedOrderDish.IsComboDish)
            {
                MessageDialog.Warning("请选择套餐主体设置备注。");
                return;
            }

            if (SelectedOrderDish.IsFishPotDish)
            {
                MessageDialog.Warning("请选择鱼锅主体设置备注。");
                return;
            }

            var dishSimpleInfo = new DishSimpleInfo
            {
                DishName = SelectedOrderDish.DishName,
                DishPrice = SelectedOrderDish.Price,
                DishUnit = SelectedOrderDish.DishUnit,
                DishNum = SelectedOrderDish.DishNum,
            };
            var allowInputDishNum = SelectedOrderDish.DishType == EnumDishType.Normal; //只有单品菜才允许直接修改数量。
            var wnd = new SetDishTasteAndDietWindow(null, dishSimpleInfo, allowInputDishNum);
            if (WindowHelper.ShowDialog(wnd, OwnerWnd))
            {
                UpdateOrderDishNum(SelectedOrderDish, wnd.DishNum);
                SelectedOrderDish.Diet = wnd.SelectedDiet;
                if (SelectedOrderDish.DishType == EnumDishType.Packages && SelectedOrderDish.DishInfos != null)
                //套餐要设置套餐内部所有菜品的忌口。
                {
                    SelectedOrderDish.DishInfos.ForEach(t => { t.Diet = wnd.SelectedDiet; });
                }
                DoWhenDishChanged();
            }
        }

        /// <summary>
        /// 减少菜品数量。
        /// </summary>
        private void ReduceDishCount()
        {
            if (SelectedOrderDish == null)
                return;

            if (SelectedOrderDish.IsComboDish)
            {
                MessageDialog.Warning("请选中套餐主体减菜。");
                return;
            }

            if (SelectedOrderDish.IsFishPotDish && SelectedOrderDish.IsPot)
            {
                MessageDialog.Warning("请选中鱼锅主体减菜。");
                return;
            }

            UpdateOrderDishNum(SelectedOrderDish, SelectedOrderDish.DishNum - 1);
            if (SelectedOrderDish.DishNum <= 0)
            {
                var tempOrderDish = SelectedOrderDish;
                OrderDishInfos.Remove(SelectedOrderDish);
                if (tempOrderDish.DishType == EnumDishType.Packages || tempOrderDish.DishType == EnumDishType.FishPot)
                //和鱼锅删除时还要一并删除套餐内的菜。
                {
                    var comboDishes = OrderDishInfos.Where(t => tempOrderDish.DishInfos.Any(y => y.Equals(t))).ToList();
                    if (comboDishes.Any())
                        comboDishes.ForEach(t => OrderDishInfos.Remove(t));
                }
            }

            DoWhenDishChanged();
        }

        /// <summary>
        /// 增加菜品数量。
        /// </summary>
        private void IncreaseDishCount()
        {
            if (SelectedOrderDish == null)
                return;

            if (SelectedOrderDish.IsComboDish || SelectedOrderDish.DishType == EnumDishType.Packages)
            {
                MessageDialog.Warning("套餐加菜请通过右侧点菜。");
                return;
            }

            if (SelectedOrderDish.DishType == EnumDishType.FishPot)
            {
                MessageDialog.Warning("鱼锅请通过右侧点菜。");
                return;
            }

            UpdateOrderDishNum(SelectedOrderDish, SelectedOrderDish.DishNum + 1);
            DoWhenDishChanged();
        }

        private void InputDishCount()
        {
            if (SelectedOrderDish == null)
                return;

            if (SelectedOrderDish.IsComboDish || SelectedOrderDish.DishType == EnumDishType.Packages
                || SelectedOrderDish.IsFishPotDish || SelectedOrderDish.DishType == EnumDishType.FishPot)
            {
                MessageDialog.Warning("鱼锅和套餐不允许直接输入数量。");
                return;
            }

            var tipInfo = string.Format("菜品名称：{0}", SelectedOrderDish.DishName);
            var numWnd = new NumInputWindow("菜品数量设置", tipInfo, "菜品数量：", 0);
            if (WindowHelper.ShowDialog(numWnd, OwnerWnd))
            {
                UpdateOrderDishNum(SelectedOrderDish, numWnd.InputNum);
                DoWhenDishChanged();
            }
        }

        /// <summary>
        /// 根据字母过滤菜谱菜单列表。
        /// </summary>
        private void FilterMenuGruopByLetter()
        {
            MenuDishes.Clear();

            var temp = SelectedMenuDishGroup.DishInfos;
            if (!string.IsNullOrEmpty(FilterMenuGroup))
                temp = SelectedMenuDishGroup.DishInfos.Where(t => t.FirstLetter.ToUpper().Contains(FilterMenuGroup)).ToList();
            temp.ForEach(MenuDishes.Add);
        }

        #endregion
    }
}