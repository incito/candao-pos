namespace CanDao.Pos.Common
{
    public class InfoLog : ClientLog
    {
        private static InfoLog _instance;

        private static readonly object LockObj = new object();

        private InfoLog() : base("InfoLog") { }

        public static InfoLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new InfoLog();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}