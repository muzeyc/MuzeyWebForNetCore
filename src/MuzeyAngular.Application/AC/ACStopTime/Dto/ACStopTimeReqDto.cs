using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACStopTimeReqDto
    {
        public string workShop { get; set; }
        [MuzeyReqType("StartTime", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("StartTime", InputType.DateTimeE)]
        public string eTime { get; set; }
    }
}
