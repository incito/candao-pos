namespace CanDao.Pos.Common
{
    public class ErrLog : ClientLog
    {
        private static ErrLog _instance;

        private static readonly object LockObj = new object();

        private ErrLog() : base("ErrLog") { }

        public static ErrLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new ErrLog();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}