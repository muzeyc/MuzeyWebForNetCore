using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_WORKSHOPDto
    {
        public long? ID { get; set; }
        public string WorkShopCode { get; set; }
        public string WorkShopName { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShopCode
            ,WorkShopName
        }

    }
}
