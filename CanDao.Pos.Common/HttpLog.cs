namespace CanDao.Pos.Common
{
    public class HttpLog : ClientLog
    {
        private static HttpLog _instance;

        private static readonly object LockObj = new object();

        private HttpLog() : base("HttpLog") { }

        public static HttpLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new HttpLog();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}