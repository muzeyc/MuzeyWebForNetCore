using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_FACTORYDto
    {
        public long? ID { get; set; }
        public string FactoryCode { get; set; }
        public string FactoryName { get; set; }

        public enum DtoEnum
        {
            ID
            ,FactoryCode
            ,FactoryName
        }

    }
}
