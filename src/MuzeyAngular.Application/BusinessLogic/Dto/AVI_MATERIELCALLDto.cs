using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_MATERIELCALLDto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string TransactionID { get; set; }
        public string TagID { get; set; }
        public string CallStatus { get; set; }
        public DateTime? CallTime { get; set; }
        public DateTime? AnswerTime { get; set; }
        public DateTime? ArriveTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,Line
            ,Station
            ,TransactionID
            ,TagID
            ,CallStatus
            ,CallTime
            ,AnswerTime
            ,ArriveTime
        }

    }
}
