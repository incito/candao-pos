using Models;
using Models.Response;

namespace WebServiceReference
{
    public class DataConverter
    {
        public static BranchInfo ToBranchInfo(BranchInfoData data)
        {
            return new BranchInfo
            {
                BranchId = data.branchid,
                BranchAddress = data.branchaddress,
                BranchName = data.branchname,
            };
        } 
    }
}