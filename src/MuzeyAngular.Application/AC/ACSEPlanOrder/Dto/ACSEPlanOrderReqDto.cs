using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACSEPlanOrderReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string orderNum { get; set; }
        [MuzeyReqType("PlanDate",InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("PlanDate", InputType.DateTimeE)]
        public string eTime { get; set; }
        public AVI_PLANORDERDto saveData { get; set; }
    }
}
