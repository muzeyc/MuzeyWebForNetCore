using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACOfflineCarReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string orderNum { get; set; }
        [MuzeyReqType]
        public string vin { get; set; }
        [MuzeyReqType]
        public string orderType { get; set; }
        [MuzeyReqType(queryType =QueryType.Equal)]
        public string state { get; set; }
        [MuzeyReqType("SetOutTime", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("SetOutTime", InputType.DateTimeE)]
        public string eTime { get; set; }
        [MuzeyReqType]
        public string serialNumber { get; set; }
        [MuzeyReqType]
        public string aeCarType { get; set; }
        [MuzeyReqType]
        public string beCarType { get; set; }
        [MuzeyReqType]
        public string bodySelCode { get; set; }
        public AVI_SETIN_SETOUTDto saveData { get; set; }
    }
}
