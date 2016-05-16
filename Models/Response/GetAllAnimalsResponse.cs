using System.Collections.Generic;

namespace Models.Response
{
    public class GetAllAnimalsResponse : JavaResponse
    {
        public string msg { get; set; }

        public List<AnimalDataResponse> data { get; set; }
    }

    public class AnimalDataResponse
    {
        public int id { get; set; }

        public int status { get; set; }

        public string name { get; set; }

    }
}