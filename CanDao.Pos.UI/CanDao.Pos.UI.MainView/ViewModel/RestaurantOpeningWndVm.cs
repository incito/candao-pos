using System;
using CanDao.Pos.Common;

namespace CanDao.Pos.UI.MainView.ViewModel
{
    public class RestaurantOpeningWndVm : NormalWindowViewModel
    {
        public RestaurantOpeningWndVm()
        {
            DateTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        public string DateTimeNow { get; set; }

    }
}