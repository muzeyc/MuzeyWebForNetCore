using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_SETIN_SETOUTDto
    {
        public long? ID { get; set; }
        public string Plant { get; set; }
        public string OrderNum { get; set; }
        public string OrderType { get; set; }
        public string VIN { get; set; }
        public string AEOnSeq { get; set; }
        public string BEOnSeq { get; set; }
        public string PEOnSeq { get; set; }
        public string CarType { get; set; }
        public string AECarType { get; set; }
        public string BECarType { get; set; }
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
        public string SAPFinSeq { get; set; }
        public DateTime? InAETime { get; set; }
        public DateTime? AEFinTime { get; set; }
        public string OperMode { get; set; }
        public DateTime? SetInTime { get; set; }
        public DateTime? SetOutTime { get; set; }
        public string State { get; set; }
        public string BodySelCode { get; set; }

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
            ,SetInTime
            ,SetOutTime
            ,State
        }

    }
}
