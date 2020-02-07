using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class ALARM_DEVICETYPEDto
    {
        public long? ID { get; set; }
        public int? DeviceTypeCode { get; set; }
        public string DeviceTypeName { get; set; }
        public string DeviceTypeDesc { get; set; }
        public string Remarks { get; set; }

        public enum DtoEnum
        {
            ID
            ,DeviceTypeCode
            ,DeviceTypeName
            ,DeviceTypeDesc
            ,Remarks
        }

    }
}
