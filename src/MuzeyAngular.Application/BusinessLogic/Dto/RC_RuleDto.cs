using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class RC_RuleDto
    {
        public long? ID { get; set; }
        public string Area { get; set; }
        public string Road { get; set; }
        public string InOutType { get; set; }
        public string IsDestroy { get; set; }
        public string Seq { get; set; }
        public string RuleScript { get; set; }
        public string RuleDesign { get; set; }
        public string IsEnable { get; set; }
        public string UpdatePerson { get; set; }
        public string UpdateDateTime { get; set; }

        public enum DtoEnum
        {
            ID
            ,Area
            ,Road
            ,InOutType
            ,IsDestroy
            ,Seq
            ,RuleScript
            ,RuleDesign
            , IsEnable
            , UpdatePerson
            , UpdateDateTime
        }

    }
}
