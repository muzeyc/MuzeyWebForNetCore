using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACFillDataReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string vin { get; set; }
        [MuzeyReqType("InsertTime", InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("InsertTime", InputType.DateTimeE)]
        public string eTime { get; set; }
        public FILLDATA_INFODto saveData { get; set; }
    }
}
