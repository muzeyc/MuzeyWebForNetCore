using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class STOPTIME_INFODto
    {
        public long ID { get; set; }
        public string LineCode { get; set; }
        public string StationCode { get; set; }
        public string StopDesc { get; set; }
        public int? StopStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? ToReportMESFlag { get; set; }
        public DateTime? ToReprotMESTime { get; set; }
        public int? ConfirmedFlag { get; set; }
        public int? ConfirmedUser { get; set; }
        public DateTime? ConfirmedTime { get; set; }
        public string Remarks { get; set; }

        public enum DtoEnum
        {
            ID
            ,LineCode
            ,StationCode
            ,StopDesc
            ,StopStatus
            ,StartTime
            ,EndTime
            ,ToReportMESFlag
            ,ToReprotMESTime
            ,ConfirmedFlag
            ,ConfirmedUser
            ,ConfirmedTime
            ,Remarks
        }

    }
}
