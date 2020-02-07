using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class ALARM_SYSTEMDto
    {
        public long? ID { get; set; }
        public int? SystemTypeCode { get; set; }
        public string SystemTypeName { get; set; }
        public string SystemTypeDesc { get; set; }
        public string Remarks { get; set; }

        public enum DtoEnum
        {
            ID
            , SystemTypeCode
            , SystemTypeName
            , SystemTypeDesc
            , Remarks
        }

    }
}
