using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    public class GetAllTableInfoesResponse : NewHttpBaseResponse
    {
        public List<AreaInfoResponse> data { set; get; }
    }
}
