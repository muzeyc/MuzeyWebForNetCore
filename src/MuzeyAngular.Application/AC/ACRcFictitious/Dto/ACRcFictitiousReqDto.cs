using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACRcFictitiousReqDto
    {
        public string id { get; set; }
        [MuzeyReqType]
        public string area { get; set; }
        [MuzeyReqType]
        public string CacheCode { get; set; }
        public string VIN { get; set; }
        public string rcType { get; set; }
        public RC_CacheDto saveData { get; set; }
        public RC_InOutLogDto savePreinstallData { get; set; }
    }
}
