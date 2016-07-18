namespace CanDao.Pos.Common
{
    public class AllLog : ClientLog
    {
        private static AllLog _instance;

        private static readonly object LockObj = new object();

        private AllLog() : base("All") { }

        public static AllLog Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new AllLog();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}