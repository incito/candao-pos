using System.Collections.Generic;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// DataServer接口基本返回值。
    /// </summary>
    public class RestBaseResponse
    {
        public int Data { get; set; }

        public string Info { get; set; }

        public string workdate { get; set; }

        public bool IsSuccess
        {
            get { return Data == 1; }
        }
    }

    public class DataResponse<T> where T : class
    {
        public List<T> Data { get; set; }
    }


    /// <summary>
    /// 以OrderJson作为载体的返回类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestOrderResponse<T> : RestBaseResponse where T : class
    {
        public List<T> OrderJson { get; set; }
    }

    /// <summary>
    /// 以ListJson作为数据载体的返回类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestListJsonResponse<T> : RestBaseResponse where T : class
    {
        public List<T> ListJson { get; set; }
    }

    /// <summary>
    /// 以DoubleJson作为数据载体的返回类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestDoubleJsonResponse<T> : RestBaseResponse where T : class
    {
        public List<T> DoubleJson { get; set; }
    }

    /// <summary>
    /// 以OrderJson和JSJson作为数据载体的返回类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TK"></typeparam>
    public class RestOrderAndJsResponse<T, TK> : RestOrderResponse<T>
        where T : class
        where TK : class
    {
        public List<TK> JSJson { get; set; }
    }

    /// <summary>
    /// 以ListJson和DoubleJson作为数据载体的返回类。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TK"></typeparam>
    public class RestListAndDoubleResponse<T, TK> : RestListJsonResponse<T>
        where T : class
        where TK : class
    {
        public List<TK> DoubleJson { get; set; }
    }

    /// <summary>
    /// 以OrderJson、ListJson和JSJson作为数据载体的返回类。
    /// </summary>
    /// <typeparam name="TO"></typeparam>
    /// <typeparam name="TJ"></typeparam>
    /// <typeparam name="TL"></typeparam>
    public class RestOrderListJsResponse<TO, TJ, TL> : RestOrderAndJsResponse<TO, TJ>
        where TO : class
        where TJ : class
        where TL : class
    {
        public List<TL> ListJson { get; set; }
    }
}