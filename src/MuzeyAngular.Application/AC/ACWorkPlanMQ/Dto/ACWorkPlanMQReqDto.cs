using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACWorkPlanMQReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string orderNum { get; set; }
        public string workDone { get; set; }
        public string file { get; set; }
        [MuzeyReqType("DateTime",InputType.DateTimeS)]
        public string sTime{get;set;}
        [MuzeyReqType("DateTime", InputType.DateTimeE)]
        public string eTime { get; set; }
        [MuzeyReqType]
        public string orderType { get; set; }
        [MuzeyReqType]
        public string vin { get; set; }
        [MuzeyReqType]
        public string serialNumber { get; set; }
        [MuzeyReqType]
        public string aeCarType { get; set; }
        [MuzeyReqType]
        public string beCarType { get; set; }
        [MuzeyReqType]
        public string bodySelCode { get; set; }
        public AVI_WORKPLAN_MQDto dto { get; set; }
    }
}
