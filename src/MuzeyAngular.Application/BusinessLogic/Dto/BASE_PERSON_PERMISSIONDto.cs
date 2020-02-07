using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_PERSON_PERMISSIONDto
    {
        public long? ID { get; set; }
        public string PersonID { get; set; }
        public string ViewName { get; set; }

        public enum DtoEnum
        {
            ID
            ,PersonID
            ,ViewName
        }

    }
}
