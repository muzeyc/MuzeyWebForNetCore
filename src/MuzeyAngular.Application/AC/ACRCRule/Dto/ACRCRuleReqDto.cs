using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACRCRuleReqDto
    {
        public string id { get; set; }
        [MuzeyReqType(DbName ="Area")]
        public string workShop { get; set; }
        [MuzeyReqType]
        public string ruleDesign { get; set; }
        public RC_RuleDto saveData { get; set; }
    }
}
