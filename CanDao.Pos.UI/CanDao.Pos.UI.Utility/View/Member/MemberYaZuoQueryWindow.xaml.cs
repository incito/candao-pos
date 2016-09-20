﻿using CanDao.Pos.UI.Utility.ViewModel;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 雅座会员查询窗口。
    /// </summary>
    public partial class MemberYaZuoQueryWindow
    {
        public MemberYaZuoQueryWindow()
        {
            InitializeComponent();
            DataContext = new MemberYaZuoQueryWndVm { OwnerWindow = this };
        }
    }
}
