using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACWarnReqDto
    {
        public string workShop { get; set; }
        [MuzeyReqType("StartTime", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("StartTime", InputType.DateTimeE)]
        public string eTime { get; set; }
        public string alarmTypeCode { get; set; }
        [MuzeyReqType]
        public string systemTypeCode { get; set; }
        [MuzeyReqType]
        public string deviceTypeCode { get; set; }
    }
}
