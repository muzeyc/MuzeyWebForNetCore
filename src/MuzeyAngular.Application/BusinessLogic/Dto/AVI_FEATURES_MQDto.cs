using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_FEATURES_MQDto
    {
        public long? ID { get; set; }
        public string FamilyCode { get; set; }
        public string FamilyName { get; set; }
        public string FeatureCode { get; set; }
        public string FeatureName { get; set; }

        public enum DtoEnum
        {
            ID
            ,FamilyCode
            ,FamilyName
            ,FeatureCode
            ,FeatureName
        }

    }
}
