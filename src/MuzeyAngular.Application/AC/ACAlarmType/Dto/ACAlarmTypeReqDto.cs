using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACAlarmTypeReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string alarmTypeCode { get; set; }
        public ALARM_ALARMTYPEDto saveData { get; set; }
    }
}
