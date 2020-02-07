using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACWarnResDto : ALARM_INFODto
    {
        public string lineName { get; set; }
        public string stationName { get; set; }
        public string alarmTypeName { get; set; }
        public string deviceTypeName { get; set; }
        public string alarmSysName { get; set; }
        public string alarmStatuName { get; set; }
        public string continueT { get; set; }
        public string sTime { get; set; }
        public string eTime { get; set; }
    }
}
