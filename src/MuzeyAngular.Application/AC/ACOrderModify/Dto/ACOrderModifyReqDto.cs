using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACOrderModifyReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string vin { get; set; }
        [MuzeyReqType]
        public string OrderNum { get; set; }
        [MuzeyReqType("DownloadPlc", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("DownloadPlc", InputType.DateTimeE)]
        public string eTime { get; set; }
        [MuzeyReqType]
        public string DownloadPlc { get; set; }
    }
}
