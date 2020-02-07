using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_STATIONDto
    {
        public long? ID { get; set; }
        public int? StationCode { get; set; }
        public int? LineCode { get; set; }
        public string StationName { get; set; }

        public enum DtoEnum
        {
            ID
            ,StationCode
            ,LineCode
            ,StationName
        }

    }
}
