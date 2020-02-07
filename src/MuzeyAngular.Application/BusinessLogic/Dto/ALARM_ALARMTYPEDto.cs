using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class ALARM_ALARMTYPEDto
    {
        public long? ID { get; set; }
        public int? AlarmTypeCode { get; set; }
        public string AlarmTypeName { get; set; }
        public string AlarmTypeDesc { get; set; }
        public string Remarks { get; set; }

        public enum DtoEnum
        {
            ID
            ,AlarmTypeCode
            ,AlarmTypeName
            ,AlarmTypeDesc
            ,Remarks
        }

    }
}
