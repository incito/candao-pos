using System.Collections.Generic;

namespace Models.Response
{
    public class GetUnclearnPosInfoResponse
    {
        public string result { get; set; }

        public List<UnclearPosResponse> detail { get; set; }
    }
}