namespace Models.Response
{
    public class JavaResponse1
    {
        public string code { get; set; }

        public string msg { get; set; }

        public bool IsSuccess
        {
            get { return code.Equals("1"); }
        }
    }
}