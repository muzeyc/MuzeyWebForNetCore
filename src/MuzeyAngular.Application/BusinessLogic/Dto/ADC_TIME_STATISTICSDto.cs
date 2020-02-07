using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class ADC_TIME_STATISTICSDto
    {
        public long? ID { get; set; }
        public DateTime? StartTime { get; set; }
        public string LineCode { get; set; }
        public int? MouldNumber { get; set; }
        public int? NowProductNumber { get; set; }
        public int? NextProductNumber { get; set; }
        public int? AdcResult { get; set; }
        public int? AdcState { get; set; }
        public DateTime? EndTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,StartTime
            ,LineCode
            ,MouldNumber
            ,NowProductNumber
            ,NextProductNumber
            ,AdcResult
            ,AdcState
            ,EndTime
        }

    }
}
