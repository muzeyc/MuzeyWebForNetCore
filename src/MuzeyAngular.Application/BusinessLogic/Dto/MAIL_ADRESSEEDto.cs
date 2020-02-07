using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class MAIL_ADRESSEEDto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public string Adressee { get; set; }
        public string AlarmTypeCode { get; set; }
        public string AlarmTypeDesc { get; set; }
        public int? AdresseeState { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,Adressee
            ,AlarmTypeCode
            ,AlarmTypeDesc
            ,AdresseeState
        }

    }
}
