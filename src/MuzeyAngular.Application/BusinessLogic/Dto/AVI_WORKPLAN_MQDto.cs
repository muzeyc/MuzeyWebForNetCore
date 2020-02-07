using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_WORKPLAN_MQDto
    {
        public long? ID { get; set; }
        public string Plant { get; set; }
        public string OrderNum { get; set; }
        public string OrderType { get; set; }
        public string VIN { get; set; }
        public string CarType { get; set; }
        public string AECarType { get; set; }
        public string BECarType { get; set; }
        public string BodySelCode { get; set; }
        public string BodyUse { get; set; }
        public string SetIn { get; set; }
        public string SetOut { get; set; }
        public string ReportReason { get; set; }
        public string OpPac { get; set; }
        public string InTrim { get; set; }
        public string Extrim { get; set; }
        public string BColor1 { get; set; }
        public string BColor2 { get; set; }
        public string Line { get; set; }
        public string ProFlag { get; set; }
        public string SerialNumber { get; set; }
        public string AEOnSeq { get; set; }
        public string BEOnSeq { get; set; }
        public string PEOnSeq { get; set; }
        public string SAPFinSeq { get; set; }
        public DateTime? InAETime { get; set; }
        public DateTime? InBETime { get; set; }
        public DateTime? InPETime { get; set; }
        public DateTime? AEFinTime { get; set; }
        public string OperMode { get; set; }
        public string QcosIp { get; set; }
        public string QcosJobs { get; set; }
        public string FamilyCode { get; set; }
        public string FeatureCode { get; set; }
        public string Spare1 { get; set; }
        public string spare2 { get; set; }
        public string spare3 { get; set; }
        public string spare4 { get; set; }
        public string spare5 { get; set; }
        public DateTime? DateTime { get; set; }
        public int? DownloadState { get; set; }
        public DateTime? DownloadTime { get; set; }
        public int? Workdone { get; set; }
        public string WorkdoneTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,Plant
            ,OrderNum
            ,OrderType
            ,VIN
            ,AEOnSeq
            ,CarType
            ,AECarType
            ,BECarType
            ,BodyUse
            ,SetIn
            ,SetOut
            ,ReportReason
            ,OpPac
            ,InTrim
            ,Extrim
            ,BColor1
            ,BColor2
            ,Line
            ,ProFlag
            ,SerialNumber
            ,SAPFinSeq
            ,InAETime
            ,AEFinTime
            ,OperMode
            ,QcosIp
            ,QcosJobs
            ,FamilyCode
            ,FeatureCode
            ,Spare1
            ,spare2
            ,spare3
            ,spare4
            ,spare5
            ,DateTime
            ,DownloadState
            ,DownloadTime
            , Workdone
        }

    }
}
