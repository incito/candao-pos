using System;

namespace CnSharp.Windows.Updater.Service.API
{
    public class InvalidVersionException : Exception
    {
        public string LatestVersion { get; set; }
    }
}