using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_MATERIELCALL_CONFIG_MQDto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string TagID { get; set; }
        public string State { get; set; }
        public string PlcToMqAddressId { get; set; }
        public string MqToPlcAnswerAddressId { get; set; }
        public string MqToPlcArriveAddressId { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,Line
            ,Station
            ,TagID
            ,State
            ,PlcToMqAddressId
            ,MqToPlcAnswerAddressId
            ,MqToPlcArriveAddressId
        }

    }
}
