using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACADCStatisticsInfoReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string mouldNumber { get; set; }
        [MuzeyReqType("StartTime", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("StartTime", InputType.DateTimeE)]
        public string eTime { get; set; }
    }
}
