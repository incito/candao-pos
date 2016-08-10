﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CanDao.Pos.Common
{
    public static class ExceptionHelper
    {
        public static string MyMessage(this Exception exp)
        {
            if (exp is AggregateException)
            {
                var innerExp = exp.InnerException;
                if (innerExp is TaskCanceledException)
                    return "接口调用超时，请检测网络或联系管理员。";
                if (innerExp is HttpRequestException)
                {
                    if (!NetwrokHelper.DetectNetworkConnection(SystemConfigCache.JavaServer))
                    {
                        return string.Format("与后台服务器：\"{0}\"连接失败。{1}请检查服务器是否开机，网络是否正常！", SystemConfigCache.JavaServer, Environment.NewLine);
                    }
                }
                if (innerExp != null)
                    return innerExp.Message;
            }

            return exp.Message;
            //while (true)
            //{
            //    if (exp.InnerException != null)
            //    {
            //        if (exp.InnerException is WebException)
            //            return exp.InnerException.Message + "，请检查网络是否正常，服务器是否正常。";

            //        exp = exp.InnerException;
            //        continue;
            //    }

            //    return exp.Message;
            //}
        }
    }
}