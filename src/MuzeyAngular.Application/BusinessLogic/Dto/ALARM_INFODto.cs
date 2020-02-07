using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class ALARM_INFODto
    {
        public long ID { get; set; }
        public int? LineCode { get; set; }
        public int? StationCode { get; set; }
        public int? AlarmTypeCode { get; set; }
        public int? DeviceTypeCode { get; set; }
        public int? SystemTypeCode { get; set; }
        public int? AlarmStatus { get; set; }
        public string AlarmDesc { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? PlayFlag { get; set; }
        public int? PlayNum { get; set; }
        public DateTime? PlayTime { get; set; }
        public int? Proprity { get; set; }
        public int? NoiseFlag { get; set; }
        public int? ToReportMESFlag { get; set; }
        public DateTime? ToReprotMESTime { get; set; }
        public int? ConfirmedFlag { get; set; }
        public int? ConfirmedUser { get; set; }
        public DateTime? ConfirmedTime { get; set; }
        public long AlarmID { get; set; }
        public string Remarks { get; set; }

        public enum DtoEnum
        {
            ID
            ,LineCode
            ,StationCode
            ,AlarmTypeCode
            ,DeviceTypeCode
            ,SystemTypeCode
            ,AlarmStatus
            ,AlarmDesc
            ,StartTime
            ,EndTime
            ,PlayFlag
            ,PlayNum
            ,PlayTime
            ,Proprity
            ,NoiseFlag
            ,ToReportMESFlag
            ,ToReprotMESTime
            ,ConfirmedFlag
            ,ConfirmedUser
            ,ConfirmedTime
            ,AlarmID
            ,Remarks
        }

    }
}
