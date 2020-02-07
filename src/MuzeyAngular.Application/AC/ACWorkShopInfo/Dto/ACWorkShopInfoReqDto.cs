using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACWorkShopInfoReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string workShopName { get; set; }
        public BASE_WORKSHOPDto saveData { get; set; }
    }
}
