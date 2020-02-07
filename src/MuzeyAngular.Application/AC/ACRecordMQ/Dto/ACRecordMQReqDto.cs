using BusinessLogic;

namespace MuzeyServer
{
    public class ACRecordMQReqDto
    {
        public string workShop { get; set; }
        [MuzeyReqType]
        public string vin { get; set; }
        [MuzeyReqType("ReportTime",InputType.DateTimeS)]
        public string sTime { get; set; }
        [MuzeyReqType("ReportTime", InputType.DateTimeE)]
        public string eTime { get; set; }
        [MuzeyReqType]
        public string type { get; set; }
        public AVI_REPORT_MQDto saveData { get; set; }
        public string reportTime { get; set; }
        [MuzeyReqType]
        public string orderNum { get; set; }
        [MuzeyReqType]
        public string orderType { get; set; }
        [MuzeyReqType]
        public string serialNumber { get; set; }
        [MuzeyReqType]
        public string ReportID { get; set; }
        [MuzeyReqType]
        public string beCarType { get; set; }
        [MuzeyReqType]
        public string bodySelCode { get; set; }
    }
}
