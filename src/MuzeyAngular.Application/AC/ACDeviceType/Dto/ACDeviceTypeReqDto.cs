using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACDeviceTypeReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string deviceTypeName { get; set; }
        public ALARM_DEVICETYPEDto saveData { get; set; }
    }
}
