using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACLineSEReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType("vin")]
        public string vin { get; set; }
        [MuzeyReqType("sTime", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("sTime", InputType.DateTimeE)]
        public string eTime { get; set; }
        [MuzeyReqType]
        public string lineTypeName { get; set; }
        [MuzeyReqType]
        public string line { get; set; }
        [MuzeyReqType]
        public string orderNum { get; set; }
        [MuzeyReqType]
        public string wsCarType { get; set; }
        [MuzeyReqType]
        public string lineName { get; set; }
        [MuzeyReqType]
        public string serialNumber { get; set; }
    }
}
