using CommonUtils;
using System;
using System.Data;
namespace BusinessLogic
{
    public class AVI_CONFIG_MQDto
    {
        public long? ID { get; set; }
        public string WorkShop { get; set; }
        public string TopCategory { get; set; }
        public string Reclassify { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string LineType { get; set; }
        public int? KeyPoint { get; set; }
        public string Remarks { get; set; }
        public string PlcToMqSignal1 { get; set; }
        public string PlcToMqSignal2 { get; set; }
        public string PlcToMqSignal3 { get; set; }
        public string PlcToMqValue1 { get; set; }
        public string PlcToMqValue2 { get; set; }
        public string PlcToMqValue3 { get; set; }
        public string MqToPlcSignal1 { get; set; }
        public string MqToPlcSignal2 { get; set; }
        public string MqToPlcSignal3 { get; set; }
        public string MqToPlcValue1 { get; set; }
        public string MqToPlcValue2 { get; set; }
        public string MqToPlcValue3 { get; set; }
        public string ConfigType { get; set; }
        public string PlcToMqHeart { get; set; }
        public string PlcToMqRequest { get; set; }
        public string MqToPlcResult { get; set; }
        public string PlcToMqReceivedFinish { get; set; }
        public string PlcToMqPartNum { get; set; }
        public string PlcToMqReportType { get; set; }
        public string PlcToMqOrderNum { get; set; }
        public string PlcToMqOrderType { get; set; }
        public string PlcToMqVIN { get; set; }
        public string PlcToMqCarType { get; set; }
        public string PlcToMqCarFun { get; set; }
        public string PlcToMqConType { get; set; }
        public string PlcToMqLSH { get; set; }
        public string PlcToMqFFNum { get; set; }
        public string PlcToMqFTLNum { get; set; }
        public string PlcToMqFTRNum { get; set; }
        public string PlcToMqMCNum { get; set; }
        public string PlcToMqRCNum { get; set; }
        public string PlcToMqSkidNum { get; set; }
        public string PlcToMqPANum { get; set; }
        public string PlcToMqRTLNum { get; set; }
        public string PlcToMqRTRNum { get; set; }
        public string PlcToMqSALNum { get; set; }
        public string PlcToMqSARNum { get; set; }
        public string PlcToMqSILNum { get; set; }
        public string PlcToMqSIRNum { get; set; }
        public string PlcToMqSOLNum { get; set; }
        public string PlcToMqSORNum { get; set; }
        public string PlcToMqREV001 { get; set; }
        public string PlcToMqREV002 { get; set; }
        public string PlcToMqREV003 { get; set; }
        public string PlcToMqREV004 { get; set; }
        public string PlcToMqREV005 { get; set; }
        public string PlcToMqREV006 { get; set; }
        public string PlcToMqREV007 { get; set; }
        public string PlcToMqREV008 { get; set; }
        public string PlcToMqREV009 { get; set; }
        public string PlcToMqREV010 { get; set; }
        public string PlcToMqREV011 { get; set; }
        public string PlcToMqREV012 { get; set; }
        public string PlcToMqREV013 { get; set; }
        public string PlcToMqREV014 { get; set; }

        public enum DtoEnum
        {
            ID
            ,WorkShop
            ,TopCategory
            ,Reclassify
            ,Line
            ,Station
            ,LineType
            ,KeyPoint
            ,Remarks
            ,PlcToMqSignal1
            ,PlcToMqSignal2
            ,PlcToMqSignal3
            ,PlcToMqValue1
            ,PlcToMqValue2
            ,PlcToMqValue3
            ,MqToPlcSignal1
            ,MqToPlcSignal2
            ,MqToPlcSignal3
            ,MqToPlcValue1
            ,MqToPlcValue2
            ,MqToPlcValue3
            , ConfigType
        }

    }
}
