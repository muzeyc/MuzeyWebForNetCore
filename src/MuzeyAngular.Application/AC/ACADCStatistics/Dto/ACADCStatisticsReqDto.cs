using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACADCStatisticsReqDto
    {
        public string id { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType("ta.MouldNumber")]
        public string mouldNumber { get; set; }
    }
}
