using JunLan.Common.Base;

namespace CanDao.Pos.Common
{
    public class ClientLog : BaseLog
    {
        private const string CfgFile = @"Config\log4net.config";

        protected ClientLog(string logName)
        {
            InitConfig(CfgFile, logName);
        }
    }
}