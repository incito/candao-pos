using System;
using System.Net;

namespace CanDao.Pos.Common
{
    public static class ExceptionHelper
    {
        public static string MyMessage(this Exception exp)
        {
            while (true)
            {
                if (exp.InnerException != null)
                {
                    if (exp.InnerException is WebException)
                        return exp.InnerException.Message + "，请检查网络是否正常，服务器是否正常。";

                    exp = exp.InnerException;
                    continue;
                }

                return exp.Message;
            }
        }
    }
}