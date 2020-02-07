using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_REPORT_MQDto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public string Type { get; set; }
        public string LineType { get; set; }
        public string OrderNum { get; set; }
        public string OrderType { get; set; }
        public string VIN { get; set; }
        public string CarType { get; set; }
        public string AECarType { get; set; }
        public string BECarType { get; set; }
        public string BodySelCode { get; set; }
        public string SerialNumber { get; set; }
        public string Line { get; set; }
        public string ReportType { get; set; }
        public string ReportID { get; set; }
        public string QcosIp { get; set; }
        public string QcosJobs { get; set; }
        public DateTime? ReportTime { get; set; }
        public string ReportState { get; set; }
        public DateTime? SendTime { get; set; }
        public DateTime? ConfirmTime { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string SKID { get; set; }
        public string PANum { get; set; }
        public string MCNum { get; set; }
        public string FFNum { get; set; }
        public string RCNum { get; set; }
        public string SILNum { get; set; }
        public string SIRNum { get; set; }
        public string SOLNum { get; set; }
        public string SORNum { get; set; }
        public string FTLNum { get; set; }
        public string FTRNum { get; set; }
        public string SALNum { get; set; }
        public string SARNum { get; set; }
        public string RTLNum { get; set; }
        public string RTRNum { get; set; }
        public int? ReportSystem { get; set; }
        public string ReportSystemTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,Type
            ,LineType
            ,OrderNum
            ,OrderType
            ,VIN
            ,CarType
            ,AECarType
            ,SerialNumber
            ,Line
            ,ReportType
            ,ReportID
            ,QcosIp
            ,QcosJobs
            ,ReportTime
            ,ReportState
            ,SendTime
            ,ConfirmTime
            ,ErrorCode
            ,ErrorMessage
            , ReportSystem
            , ReportSystemTime
        }

    }
}
