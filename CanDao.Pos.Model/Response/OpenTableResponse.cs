﻿using System;
using CanDao.Pos.Model.Enum;

namespace CanDao.Pos.Model.Response
{
    /// <summary>
    /// 开台返回类。
    /// </summary>
    public class OpenTableResponse : JavaResponse
    {
        public string orderid { get; set; }

        public string delaytime { get; set; }

        public string vipaddress { get; set; }

        public string locktime { get; set; }

        public string backpsd { get; set; }

        public EnumOpenTableResult OpenTableResult
        {
            get { return (EnumOpenTableResult) Convert.ToInt32(result); }
        }
    }
}