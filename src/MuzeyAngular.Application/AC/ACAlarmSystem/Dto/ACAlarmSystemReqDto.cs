using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACAlarmSystemReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string systemTypeDesc { get; set; }
        public ALARM_SYSTEMDto saveData { get; set; }
    }
}
