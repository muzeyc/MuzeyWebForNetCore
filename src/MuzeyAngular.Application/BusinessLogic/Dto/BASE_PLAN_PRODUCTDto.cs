using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class BASE_PLAN_PRODUCTDto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public string LineMain { get; set; }
        public string LineBranch { get; set; }
        public string CarType { get; set; }
        public string PlanProduct { get; set; }
        public string PlanDate { get; set; }
        public string ShiftrestName { get; set; }
        public string PlanTimeS { get; set; }
        public string PlanTimeE { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,LineMain
            ,LineBranch
            ,CarType
            ,PlanProduct
            ,PlanDate
            ,ShiftrestName
            ,PlanTimeS
            ,PlanTimeE
        }

    }
}
