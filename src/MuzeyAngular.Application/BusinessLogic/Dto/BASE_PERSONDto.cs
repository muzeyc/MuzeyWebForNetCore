using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_PERSONDto
    {
        public long? ID { get; set; }
        public string PersonName { get; set; }
        public string Factory { get; set; }
        public string WorkShop { get; set; }
        public string UserId { get; set; }
        public string PermissionType { get; set; }

        public enum DtoEnum
        {
            ID
            ,PersonName
            ,Factory
            ,WorkShop
            , UserId
            , PermissionType
        }

    }
}
