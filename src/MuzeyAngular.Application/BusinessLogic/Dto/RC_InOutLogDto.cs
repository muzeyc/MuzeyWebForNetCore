using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class RC_InOutLogDto
    {
        public long? ID { get; set; }
        public string Area { get; set; }
        public string Road { get; set; }
        public string Place { get; set; }
        public string VIN { get; set; }
        public string State { get; set; }
        public string OpTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,Area
            ,Road
            ,Place
            ,VIN
            ,State
            ,OpTime
        }

    }
}
