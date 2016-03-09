using System;
using System.IO;
using log4net;
using log4net.Config;

namespace Common
{
    [Serializable]
    public class BaseLog
    {
        #region Fields

        private ILog _logger;

        #endregion

        #region Public Method

        /// <summary>
        /// 初始化配置文件信息。
        /// </summary>
        /// <param name="cfgFile"></param>
        /// <param name="logName"></param>
        public void InitConfig(string cfgFile, string logName)
        {
            XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, cfgFile)));
            _logger = LogManager.GetLogger(logName);
        }

        /// <summary>
        /// Error方式输出
        /// </summary>
        /// <param name="message"></param>
        public void E(string message)
        {
            _logger.Error(message);
        }

        public void E(Exception exception)
        {
            _logger.Error("", exception);
        }

        /// <summary>Error</summary>
        public void E(string formatStr, params object[] paramArray)
        {
            _logger.ErrorFormat(formatStr, paramArray);
        }

        /// <summary>Error</summary>
        public void E(string message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void I(string message)
        {
            _logger.Info(message);
        }

        /// <summary>Info</summary>
        public void I(string formatStr, params object[] paramArray)
        {
            _logger.InfoFormat(formatStr, paramArray);
        }

        /// <summary>Info</summary>
        public void I(Exception exception, string message)
        {
            _logger.Info(message, exception);
        }

        /// <summary>Info</summary>
        public void I(Exception exception, string formatStr, params object[] paramArray)
        {
            _logger.InfoFormat(formatStr, paramArray);
            _logger.Info(exception);
        }

        /// <summary>Debug</summary>
        public void D(string message)
        {
            _logger.Debug(message);
        }

        /// <summary>Debug</summary>
        public void D(string formatStr, params object[] paramArray)
        {
            _logger.DebugFormat(formatStr, paramArray);
        }

        /// <summary>Debug</summary>
        public void D(string message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        /// <summary>Debug</summary>
        public void D(Exception exception, string formatStr, params object[] paramArray)
        {
            _logger.DebugFormat(formatStr, paramArray);
            _logger.Debug(exception);
        }

        /// <summary>Fatal</summary>
        public void F(string message)
        {
            _logger.Fatal(message);
        }

        /// <summary>Fatal</summary>
        public void F(string formatStr, params object[] paramArray)
        {
            _logger.FatalFormat(formatStr, paramArray);
        }

        /// <summary>Fatal</summary>
        public void F(string message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        /// <summary>Fatal</summary>
        public void F(Exception exception, string formatStr, params object[] paramArray)
        {
            _logger.FatalFormat(formatStr, paramArray);
            _logger.Fatal(exception);
        }

        /// <summary>Warn</summary>
        public void W(string message)
        {
            _logger.Warn(message);
        }

        /// <summary>Warn</summary>
        public void W(string formatStr, params object[] paramArray)
        {
            _logger.WarnFormat(formatStr, paramArray);
        }

        /// <summary>Warn</summary>
        public void W(string message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        /// <summary>Warn</summary>
        public void W(Exception exception, string formatStr, params object[] paramArray)
        {
            _logger.WarnFormat(formatStr, paramArray);
            _logger.Warn(exception);
        }
        #endregion
    }
}