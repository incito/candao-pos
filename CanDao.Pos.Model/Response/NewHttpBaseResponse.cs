using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanDao.Pos.Model.Response
{
    public class NewHttpBaseResponse
    {
        public string code { get; set; }
        public string msg { get; set; }
        public bool IsSuccess
        {
            get { return !string.IsNullOrEmpty(code) && code.Equals("0"); }
        }
    }

    public class NewHttpBaseResponse<T> : NewHttpBaseResponse
    {
        public RowsResponse<T> data { get; set; }
    }
}
