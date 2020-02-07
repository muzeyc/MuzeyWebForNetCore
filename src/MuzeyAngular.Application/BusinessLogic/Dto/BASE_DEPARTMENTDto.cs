using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_DEPARTMENTDto
    {
        public long? ID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }

        public enum DtoEnum
        {
            ID
            ,DepartmentCode
            ,DepartmentName
        }

    }
}
