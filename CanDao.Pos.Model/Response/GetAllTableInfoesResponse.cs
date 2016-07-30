using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
    public class GetAllTableInfoesResponse:NewHttpBaseResponse 
    {
        public List<TableInfoResponse> data { set; get; }
    }
}
