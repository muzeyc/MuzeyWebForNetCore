using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class ANDON_QCOS_BYPASS_INFODto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string VIN { get; set; }
        public string BypassStatus { get; set; }
        public DateTime? BypassStartTime { get; set; }
        public DateTime? BypassEndTime { get; set; }
        public string ReportMesStatus { get; set; }
        public DateTime? ReportMesTime { get; set; }
        public string Tagname { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,Line
            ,Station
            ,VIN
            ,BypassStatus
            ,BypassStartTime
            ,BypassEndTime
            ,ReportMesStatus
            ,ReportMesTime
            ,Tagname
        }

    }
}
