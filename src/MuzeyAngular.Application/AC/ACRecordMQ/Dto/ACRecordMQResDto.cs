using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Text;

namespace MuzeyServer
{
    public class ACRecordMQResDto : AVI_REPORT_MQDto
    {

        public string vin { get; set; }
        public string orderNum { get; set; }
        public string planTypeName { get; set; }
        public string model { get; set; }
        public string reportTypeCode { get; set; }
        public string reportTypeName { get; set; }
        public string reportID { get; set; }
        public string reportName { get; set; }
        public string reportStatus { get; set; }

        public string reportTime { get; set; }
        public string type { get; set; }
        public string typeName { get; set; }
        public string wsCarType { get; set; }
        public List<MuzeySelectModel> rIdOps { get; set; }
    }
}
