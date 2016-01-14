namespace CnSharp.Windows.Updater.SharpPack.Plugin
{
    public interface ISourceControl
    {
        /// <summary>
        /// check out file
        /// </summary>
        /// <param name="slnDir"></param>
        /// <param name="file"></param>
        /// <returns>-1 no version control; 0 check out failed ; >0 check out success</returns>
        int CheckOut(string slnDir,string file);
    }
}
