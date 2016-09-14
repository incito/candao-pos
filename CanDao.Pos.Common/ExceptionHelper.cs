using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CanDao.Pos.Common
{
    public static class ExceptionHelper
    {
        public static string MyMessage(this Exception exp)
        {
            if (exp is TaskCanceledException)
                return "接口调用超时，请检测网络或联系管理员。";

            if (exp.InnerException != null)
                return exp.InnerException.MyMessage();

            if (exp is HttpRequestException)
            {
                if (exp.Message == "InternalServerError")
                    return "服务器内部错误，请联系管理员。";
                if (!NetwrokHelper.DetectNetworkConnection(SystemConfigCache.JavaServer))
                    return string.Format("与后台服务器：\"{0}\"连接失败。{1}请检查服务器是否开机，网络是否正常！", SystemConfigCache.JavaServer, Environment.NewLine);
            }

            return exp.Message;
        }
    }
}