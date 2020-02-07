using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class ANDON_QCOS_INFODto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string VIN { get; set; }
        public string JobID { get; set; }
        public string QcosID { get; set; }
        public string QcosStatus { get; set; }
        public DateTime? QcosTime { get; set; }
        public string ReportMesStatus { get; set; }
        public DateTime? ReportMesTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,Line
            ,Station
            ,VIN
            ,JobID
            ,QcosID
            ,QcosStatus
            ,QcosTime
            ,ReportMesStatus
            ,ReportMesTime
        }

    }
}
