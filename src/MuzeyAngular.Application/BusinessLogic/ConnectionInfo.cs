using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public static class ConnectionInfo
    {
        private static Dictionary<string, string> ConnectionInfoDic;
        private static Dictionary<string, string> RCIpInfoDic;

        static ConnectionInfo()
        {
            ConnectionInfoDic = new Dictionary<string, string>();
            RCIpInfoDic = new Dictionary<string, string>();
            ConnectionInfoDic.Add("AE", "Data Source=.;User ID=sa; Password=123456;");
            ConnectionInfoDic.Add("BE", "Data Source=.;User ID=sa; Password=123456;");
            ConnectionInfoDic.Add("SE", "Data Source=.;User ID=sa; Password=123456;");


            //RCIpInfoDic.Add(".","PBS");
            //RCIpInfoDic.Add(".", "WBS");
            //RCIpInfoDic.Add(".", "TEST");
        }

        public static string GetConnectionInfo(string key)
        {
            return ConnectionInfoDic[key];
        }

        public static string GetRCInfo(string key)
        {
            return RCIpInfoDic[key];
        }
    }
}
