using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class ADC_SINGLE_OUTPUTDto
    {
        public long? ID { get; set; }
        public string LineCode { get; set; }
        public int? MouldNumber { get; set; }
        public int? NowProductNumber { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long? ProductionNumber { get; set; }
        public int? AdcState { get; set; }

        public enum DtoEnum
        {
            ID
            ,LineCode
            ,MouldNumber
            ,NowProductNumber
            ,StartTime
            ,EndTime
            ,ProductionNumber
            ,AdcState
        }

    }
}
