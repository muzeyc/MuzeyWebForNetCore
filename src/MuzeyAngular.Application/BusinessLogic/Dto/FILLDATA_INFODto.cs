using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class FILLDATA_INFODto
    {
        public long? ID { get; set; }
        public string VIN { get; set; }
        public int? FillStatus { get; set; }
        public string FillMedia { get; set; }
        public DateTime? InsertTime { get; set; }
        public string Tagname { get; set; }

        public enum DtoEnum
        {
            ID
            ,VIN
            ,FillStatus
            ,FillMedia
            ,InsertTime
            ,Tagname
        }

    }
}
