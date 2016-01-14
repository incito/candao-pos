using System;

namespace CnSharp.Windows.Updater.Util
{
    public struct UpdateInfo
    {
        public string Version { get; set; }
        public string UpdateLog { get; set; }
        public string PackUrl { get; set; }
        public bool NewVersionFound { get; set; }
        public bool Success { get; set; }
        public Exception Exception { get; set; }
    }
}
