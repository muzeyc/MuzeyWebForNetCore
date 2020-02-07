using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACShiftrestReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string shiftName { get; set; }
        [MuzeyReqType("WorkDay", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("WorkDay", InputType.DateTimeE)]
        public string eTime { get; set; }
        public AVI_SHIFTRESTDto saveData { get; set; }
    }
}
