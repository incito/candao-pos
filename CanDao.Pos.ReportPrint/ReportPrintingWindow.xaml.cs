namespace CanDao.Pos.ReportPrint
{
    /// <summary>
    /// 正在打印的提示窗口。
    /// </summary>
    public partial class ReportPrintingWindow
    {
        private static ReportPrintingWindow _instance;

        private static readonly object LockObj = new object();

        public static ReportPrintingWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ReportPrintingWindow();
                        }
                    }
                }
                return _instance;
            }
        }

        private ReportPrintingWindow()
        {
            InitializeComponent();
        }
    }
}
