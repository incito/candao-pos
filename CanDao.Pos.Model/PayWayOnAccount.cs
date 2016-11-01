namespace CanDao.Pos.Model
{
    /// <summary>
    /// 挂账结算方式。
    /// </summary>
    public class PayWayOnAccount : PayWayInfo
    {

        /// <summary>
        /// 选择的挂账单位。
        /// </summary>
        private OnCompanyAccountInfo _selectedOnCmpAccInfo;
        /// <summary>
        /// 获取或设置选择的挂账单位。
        /// </summary>
        public OnCompanyAccountInfo SelectedOnCmpAccInfo
        {
            get { return _selectedOnCmpAccInfo; }
            set
            {
                _selectedOnCmpAccInfo = value;
                RaisePropertyChanged("SelectedOnCmpAccInfo");
            }
        }

    }
}