using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACSEMouldInfoReqDto
    {
        public string MouldCode { get; set; }
        public string workShop { get; set; }
        [MuzeyReqType]
        public string mouldCode { get; set; }
        public BASE_MOULDINFODto saveData { get; set; }
        public string op { get; set; }
    }
}
