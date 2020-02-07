using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACFactoryInfoReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string factoryName { get; set; }
        public BASE_FACTORYDto saveData { get; set; }
    }
}
