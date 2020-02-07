using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACQcosInfoReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string vin { get; set; }
        [MuzeyReqType]
        public string qcosStatus { get; set; }
        [MuzeyReqType("QcosTime", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("QcosTime", InputType.DateTimeE)]
        public string eTime { get; set; }
    }
}
