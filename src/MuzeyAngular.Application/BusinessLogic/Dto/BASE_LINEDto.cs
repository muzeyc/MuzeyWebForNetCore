using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_LINEDto
    {
        public long? ID { get; set; }
        public int? LineGroup { get; set; }
        public int? LineCode { get; set; }
        public string LineName { get; set; }
        public string LineDesc { get; set; }
        public string LineFullName { get; set; }
        public string LineMESName { get; set; }

        public enum DtoEnum
        {
            ID
            ,LineGroup
            ,LineCode
            ,LineName
            ,LineDesc
            ,LineFullName
            ,LineMESName
        }

    }
}
