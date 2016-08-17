using System;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model
{
    public class TableInfo : BaseNotifyObject
    {
        /// <summary>
        /// 餐台编号。
        /// </summary>
        public string TableId { get; set; }

        /// <summary>
        /// 餐桌状态。
        /// </summary>
        private EnumTableStatus _tableStatus;
        /// <summary>
        /// 餐桌状态。
        /// </summary>
        public EnumTableStatus TableStatus
        {
            get { return _tableStatus; }
            set
            {
                _tableStatus = value;
                RaisePropertyChanged("TableStatus");
                RaisePropertyChanged("IsInDinner");
            }
        }

        /// <summary>
        /// 是否是就餐状态。
        /// </summary>
        public bool IsInDinner
        {
            get { return TableStatus == EnumTableStatus.Dinner; }
        }

        /// <summary>
        /// 餐台是否可用。
        /// </summary>
        private bool _tableEnable;
        /// <summary>
        /// 餐台是否可用。
        /// </summary>
        public bool TableEnable
        {
            get { return _tableEnable; }
            set
            {
                _tableEnable = value;
                RaisePropertyChanged("TableEnable");
            }
        }

        /// <summary>
        /// 餐台类型。
        /// </summary>
        public EnumTableType TableType { get; set; }

        /// <summary>
        /// 餐桌可以就坐人数。
        /// </summary>
        public int PeopleNumber { get; set; }

        /// <summary>
        /// 餐桌名称。
        /// </summary>
        private string _tableName;
        /// <summary>
        /// 餐桌名称。
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                RaisePropertyChanged("TableName");
            }
        }

        /// <summary>
        /// 餐桌编号。（现餐桌名称与编号同）
        /// </summary>
        public string TableNo { get; set; }

        /// <summary>
        /// 餐桌所属区域名。
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 餐桌所属区域编号。
        /// </summary>
        public string AreaNo { get; set; }

        /// <summary>
        /// 该餐桌订单号。
        /// </summary>
        private string _orderId;
        /// <summary>
        /// 该餐桌订单号。
        /// </summary>
        public string OrderId
        {
            get { return _orderId; }
            set
            {
                _orderId = value;
                RaisePropertyChanged("OrderId");
            }
        }

        public decimal MinPrice { get; set; }

        public decimal FixPrice { get; set; }

        /// <summary>
        /// 开台时间。
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 开台持续时间。
        /// </summary>
        private string _dinnerDuration;
        /// <summary>
        /// 开台持续时间。
        /// </summary>
        public string DinnerDuration
        {
            get { return _dinnerDuration; }
            set
            {
                if (_dinnerDuration == value)
                    return;

                _dinnerDuration = value;
                RaisePropertyChanged("DinnerDuration");
            }
        }

        /// <summary>
        /// 应收金额。
        /// </summary>
        private decimal? _amount;

        /// <summary>
        /// 应收金额。
        /// </summary>
        public decimal? Amount
        {
            get { return _amount; }
            set
            {
                _amount = value;
                RaisePropertyChanged("Amount");
            }
        }

        /// <summary>
        /// 这个餐桌的服务员id。
        /// </summary>
        public string WaiterId { get; set; }

        /// <summary>
        /// 是否是挂单的订单。
        /// </summary>
        public bool IsHangOrder { get; set; }

        /// <summary>
        /// 获取是否是咖啡台。
        /// </summary>
        public bool IsCoffeeTable
        {
            get { return TableType == EnumTableType.CFTable; }
        }

        /// <summary>
        /// 是否是外卖台。
        /// </summary>
        public bool IsTakeoutTable
        {
            get { return TableType == EnumTableType.CFTakeout || TableType == EnumTableType.Takeout; }
        }

        public void CloneData(TableInfo srcInfo)
        {
            if (TableId != srcInfo.TableId)
                return;

            TableStatus = srcInfo.TableStatus;
            BeginTime = srcInfo.BeginTime;
            Amount = srcInfo.Amount;
            TableEnable = srcInfo.TableEnable;
            TableName = srcInfo.TableName;
            OrderId = srcInfo.OrderId;
            IsHangOrder = srcInfo.IsHangOrder;
        }

        /// <summary>
        /// 更新就餐时长时间。
        /// </summary>
        public void UpdateDinnerDuration()
        {
            if (!IsInDinner)
                return;

            try
            {
                var duration = DateTime.Now - BeginTime;
                DinnerDuration = string.Format("{0:00}:{1:00}", duration.Value.Hours, duration.Value.Minutes);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }
    }
}