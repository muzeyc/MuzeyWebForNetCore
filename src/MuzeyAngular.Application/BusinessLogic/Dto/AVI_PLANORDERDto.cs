using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_PLANORDERDto
    {
        public long? ID { get; set; }
        public string Plant { get; set; }
        public string Line { get; set; }
        public string OrderNum { get; set; }
        public string OrderType { get; set; }
        public string ProNum { get; set; }
        public string ProName { get; set; }
        public string PlanCount { get; set; }
        public string MouldCode { get; set; }
        public string Formula { get; set; }
        public string SPM { get; set; }
        public string SEOnSeq { get; set; }
        public string PlanDate { get; set; }
        public DateTime? PlanStartTime { get; set; }
        public DateTime? PlanStopTime { get; set; }
        public string Type { get; set; }
        public string CarType { get; set; }
        public string Remarks { get; set; }
        public string FreezeState { get; set; }

        public enum DtoEnum
        {
            ID
            ,Plant
            ,Line
            ,OrderNum
            ,OrderType
            ,ProNum
            ,ProName
            ,PlanCount
            ,MouldCode
            ,Formula
            ,SPM
            ,SEOnSeq
            ,PlanDate
            ,PlanStartTime
            ,PlanStopTime
            ,Type
            ,CarType
            ,Remarks
            ,FreezeState
        }

    }
}
