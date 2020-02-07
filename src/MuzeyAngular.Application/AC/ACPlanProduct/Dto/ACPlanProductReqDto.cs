using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACPlanProductReqDto
    {
        public string id { get; set; }
        [MuzeyReqType]
        public string workShop { get; set; }
        [MuzeyReqType]
        public string shiftrestName { get; set; }
        [MuzeyReqType("PlanDate",InputType.DateTimeS)]
        public string sDate { get; set; }
        [MuzeyReqType("PlanDate", InputType.DateTimeE)]
        public string eDate { get; set; }
        public BASE_PLAN_PRODUCTDto saveData { get; set; }
    }
}
