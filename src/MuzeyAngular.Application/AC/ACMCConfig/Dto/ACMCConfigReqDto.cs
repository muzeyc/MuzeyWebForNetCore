using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACMCConfigReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string tagID { get; set; }
        public AVI_MATERIELCALL_CONFIG_MQDto saveData { get; set; }
    }
}
