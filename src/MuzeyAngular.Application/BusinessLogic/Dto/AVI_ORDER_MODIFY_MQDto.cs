using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_ORDER_MODIFY_MQDto
    {
        public long? ID { get; set; }
        public string Plant { get; set; }
        public string Line { get; set; }
        public string Type { get; set; }
        public string PreOrderNum { get; set; }
        public string PreVIN { get; set; }
        public string OrderNum { get; set; }
        public string VIN { get; set; }
        public string Station { get; set; }
        public DateTime? DateTime { get; set; }
        public string ModifyState { get; set; }
        public string Remarks { get; set; }
        public int? DownloadPlc { get; set; }
        public DateTime? DownloadPlcTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,Plant
            ,Line
            ,Type
            ,PreOrderNum
            ,PreVIN
            ,OrderNum
            ,VIN
            ,Station
            ,DateTime
            ,ModifyState
            ,Remarks
            ,DownloadPlc
            ,DownloadPlcTime
        }

    }
}
