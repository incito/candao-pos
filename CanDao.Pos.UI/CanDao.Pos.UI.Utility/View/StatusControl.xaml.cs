using System;
using System.Timers;
using System.Windows;

namespace CanDao.Pos.UI.Utility.View
{
    /// <summary>
    /// 状态控件。
    /// </summary>
    public partial class StatusControl
    {
        private Timer _timer;

        public StatusControl()
        {
            InitializeComponent();
            Version = Application.ResourceAssembly.GetName(false).Version.ToString();
            _timer = new Timer(1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();

            DataContext = this;
        }

        #region Properties

        /// <summary>
        /// 分店号。 
        /// </summary>
        public string BranchId
        {
            get { return (string)GetValue(BranchIdProperty); }
            set { SetValue(BranchIdProperty, value); }
        }

        public static readonly DependencyProperty BranchIdProperty =
            DependencyProperty.Register("BranchId", typeof(string), typeof(StatusControl), new PropertyMetadata(""));

        /// <summary>
        /// 登录用户权限名。 
        /// </summary>
        public string LoginUserName
        {
            get { return (string)GetValue(LoginUserNameProperty); }
            set { SetValue(LoginUserNameProperty, value); }
        }

        public static readonly DependencyProperty LoginUserNameProperty =
            DependencyProperty.Register("LoginUserName", typeof(string), typeof(StatusControl), new PropertyMetadata(""));


        /// <summary>
        /// 当前时间。
        /// </summary>
        public DateTime CurrentTime
        {
            get { return (DateTime)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register("CurrentTime", typeof(DateTime), typeof(StatusControl), new PropertyMetadata(DateTime.Now));

        /// <summary>
        /// 程序版本。
        /// </summary>
        public string Version { get; set; }

        #endregion

        /// <summary>
        /// 定时器触发时执行。 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action) delegate
            {
                CurrentTime = DateTime.Now;
            });
        }

    }
}
