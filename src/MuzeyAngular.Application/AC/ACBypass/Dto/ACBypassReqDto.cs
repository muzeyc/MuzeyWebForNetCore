using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACBypassReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string vin { get; set; }
        [MuzeyReqType("BypassStartTime", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("BypassStartTime", InputType.DateTimeE)]
        public string eTime { get; set; }
        public ANDON_QCOS_BYPASS_INFODto saveData { get; set; }
    }
}
