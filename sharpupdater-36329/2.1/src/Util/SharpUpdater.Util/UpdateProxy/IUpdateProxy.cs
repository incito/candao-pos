﻿namespace CnSharp.Windows.Updater.Util.UpdateProxy
{
    public interface IUpdateProxy
    {
        //string GetLatestVersion();
        string GetUpdateLogBetweenVersion(string baseVersion, string topVersion);
        //Updater GetUpdaterVersion();
    }
}