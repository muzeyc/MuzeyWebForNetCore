using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_MOULDINFODto
    {
        public long? MouldCode { get; set; }
        public string ProjectName { get; set; }
        public string LeftPartCode { get; set; }
        public string LeftPartName { get; set; }
        public string RightPartCode { get; set; }
        public string RightPartName { get; set; }

        public enum DtoEnum
        {
            MouldCode
            ,ProjectName
            ,LeftPartCode
            ,LeftPartName
            ,RightPartCode
            ,RightPartName
        }

    }
}
