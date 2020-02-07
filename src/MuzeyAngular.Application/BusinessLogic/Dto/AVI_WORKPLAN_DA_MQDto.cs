using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_WORKPLAN_DA_MQDto
    {
        public long? ID { get; set; }
        public string Plant { get; set; }
        public string Line { get; set; }
        public string OrderNum { get; set; }
        public string OrderType { get; set; }
        public string PartLineType { get; set; }
        public string PartLineCode { get; set; }
        public string CarType { get; set; }
        public string BECarType { get; set; }
        public string BodySelCode { get; set; }
        public string VIN { get; set; }
        public string PartsUse { get; set; }
        public string ProFlag { get; set; }
        public string SetIn { get; set; }
        public string SetOut { get; set; }
        public string ReportReason { get; set; }
        public string BESubOnSeq { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? DateTime { get; set; }
        public string spare1 { get; set; }
        public string spare2 { get; set; }
        public string spare3 { get; set; }
        public string spare4 { get; set; }
        public string spare5 { get; set; }
        public int? DownloadState { get; set; }
        public DateTime? DownloadTime { get; set; }
        public int? Workdone { get; set; }
        public DateTime? WorkdoneTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,Plant
            ,Line
            ,OrderNum
            ,OrderType
            ,PartLineType
            ,PartLineCode
            ,CarType
            ,BECarType
            ,BodySelCode
            ,VIN
            ,PartsUse
            ,ProFlag
            ,SetIn
            ,SetOut
            ,ReportReason
            ,BESubOnSeq
            ,SerialNumber
            ,DateTime
            ,spare1
            ,spare2
            ,spare3
            ,spare4
            ,spare5
            ,DownloadState
            ,DownloadTime
            ,Workdone
            ,WorkdoneTime
        }

    }
}
