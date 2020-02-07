using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public static class PKInfo
    {
        private static Dictionary<String, String[]> pkMap;

        static PKInfo()
        {
            pkMap = new Dictionary<string, string[]>();
            pkMap.Add("ALARM_SYSTEM", new String[] { "ID" });
            pkMap.Add("ALARM_ALARMTYPE", new String[] { "ID" });
            pkMap.Add("ALARM_DEVICETYPE", new String[] { "ID" });
            pkMap.Add("ALARM_INFO", new String[] { "ID" });
            pkMap.Add("STOPTIME_INFO", new String[] { "ID" });
            pkMap.Add("BASE_LINE", new String[] { "ID" });
            pkMap.Add("BASE_STATION", new String[] { "ID" });
            pkMap.Add("AVI_REPORT_MQ", new String[] { "ID" });
            pkMap.Add("AVI_WORKPLAN_MQ", new String[] { "ID" });
            pkMap.Add("AVI_WORKPLAN_DA_MQ", new String[] { "ID" });
            pkMap.Add("AVI_PLANORDER", new String[] { "ID" }); 
            pkMap.Add("ADC_SINGLE_OUTPUT", new String[] { "ID" });
            pkMap.Add("AVI_CALENDARS", new String[] { "ID" });
            pkMap.Add("AVI_SHIFTREST", new String[] { "ID" });
            pkMap.Add("AVI_MATERIELCALL_CONFIG_MQ", new String[] { "ID" });
            pkMap.Add("AVI_MATERIELCALL", new String[] { "ID" });
            pkMap.Add("ADC_TIME_STATISTICS", new String[] { "ID" });
            pkMap.Add("AVI_SETIN_SETOUT", new String[] { "ID" });
            pkMap.Add("FILLDATA_INFO", new String[] { "ID" });
            pkMap.Add("AVI_CONFIG_MQ", new String[] { "ID" });
            pkMap.Add("ANDON_QCOS_INFO", new String[] { "ID" });
            pkMap.Add("BASE_FACTORY", new String[] { "ID" });
            pkMap.Add("BASE_WORKSHOP", new String[] { "ID" });
            pkMap.Add("BASE_DEPARTMENT", new String[] { "ID" });
            pkMap.Add("BASE_PERSON", new String[] { "ID" });
            pkMap.Add("BASE_PERSON_PERMISSION", new String[] { "ID" });
            pkMap.Add("AbpUserAccounts", new String[] { "ID" }); 
            pkMap.Add("BASE_PLAN_PRODUCT", new String[] { "ID" });
            pkMap.Add("AVI_ORDER_MODIFY_MQ", new String[] { "ID" });
            pkMap.Add("BASE_MOULDINFO", new String[] { "MouldCode" });
            pkMap.Add("AVI_FEATURES_MQ", new String[] { "ID" });
            pkMap.Add("RC_Cache", new String[] { "CacheCode" });
            pkMap.Add("RC_InOutLog",new String[] { "ID"});
            pkMap.Add("RC_Rule", new String[] { "ID" });
            pkMap.Add("MAIL_BASECONFIG", new String[] { "ID" });
            pkMap.Add("MAIL_ADRESSEE", new String[] { "ID" });
            pkMap.Add("ANDON_QCOS_BYPASS_INFO", new String[] { "ID" });
            pkMap.Add("ANDON_WelcomeWords", new String[] { "ID" });
        }

        public static String[] GetPK(String tableName)
        {
            return pkMap[tableName];
        }
    }
}
