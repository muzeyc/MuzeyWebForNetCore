using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACMailBaseConfigReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string ID { get; set; }
        public MAIL_BASECONFIGDto saveData { get; set; }
    }
}
