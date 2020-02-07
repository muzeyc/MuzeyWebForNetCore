using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_CALENDARSDto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public DateTime? WorkDay { get; set; }
        public string ShiftName { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,WorkDay
            ,ShiftName
            ,BeginTime
            ,EndTime
        }

    }
}
