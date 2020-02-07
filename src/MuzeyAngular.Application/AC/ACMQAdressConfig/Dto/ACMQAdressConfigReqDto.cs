using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACMQAdressConfigReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string station { get; set; }
        public AVI_CONFIG_MQDto saveData { get; set; }
    }
}
