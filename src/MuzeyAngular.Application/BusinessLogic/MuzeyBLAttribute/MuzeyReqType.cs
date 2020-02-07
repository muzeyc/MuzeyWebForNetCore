using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class MuzeyReqType : System.Attribute
    {
        public InputType inputType { get; set; }
        public QueryType queryType { get; set; }
        public string DbName{ get; set; }

        public MuzeyReqType(string DbName="", InputType inputType= InputType.Input, QueryType queryType= QueryType.Like)
        {
            this.DbName = DbName;
            this.inputType = inputType;
            this.queryType = queryType;
        }
    }

    public enum InputType
    {
        Input,
        DateTime,
        DateTimeS,
        DateTimeE
    }

    public enum QueryType
    {
        Equal,
        Like
    }

    public static class MuzeyReqUtil
    {
        public static string GetSqlWhere(object o)
        {
            var resStr = "";
            var type = o.GetType();
            var continuePDic = new Dictionary<string,string>();
            string dtVal;
            foreach (var propInfo in type.GetProperties())
            {
                var dtValObj = propInfo.GetValue(o);
                if (dtValObj == null || dtValObj.ToString()=="")
                {
                    continue;
                }
                else
                {
                    dtVal = dtValObj.ToString();
                }

                var continueFlag = false;
                foreach (var cKey in continuePDic.Keys)
                {
                    if(cKey == propInfo.Name)
                    {
                        continuePDic.Remove(cKey);
                        continueFlag = true;
                        break;
                    }
                }

                if (continueFlag)
                {
                    continue;
                }

                object[] objAttrs = propInfo.GetCustomAttributes(typeof(MuzeyReqType), true);

                if (objAttrs.Length > 0)
                {
                    var pWhereStr = new StringBuilder();
                    pWhereStr.Append("AND ");
                    var attr = objAttrs[0] as MuzeyReqType;
                    if (attr != null)
                    {
                        switch (attr.inputType)
                        {
                            case InputType.Input:
                                pWhereStr.Append(attr.DbName=="" ? propInfo.Name : attr.DbName);
                                if(attr.queryType == QueryType.Equal)
                                {
                                    pWhereStr.Append(string.Format(" = '{0}'",dtVal));
                                }
                                else
                                {
                                    pWhereStr.Append(string.Format(" like '%{0}%'", dtVal));
                                }
                                break;
                            case InputType.DateTime:
                                var dbNameSE = attr.DbName.Split(',');
                                pWhereStr.Append(string.Format("'{0}' >= {1} AND '{0}'<={2}",dtVal,dbNameSE[0],dbNameSE[1]));
                                break;
                            case InputType.DateTimeS:
                                var dtValS = dtVal;
                                var conName = type.GetProperty("e" + propInfo.Name.Substring(1));
                                string dtValE;
                                if (conName.GetValue(o) == null || conName.GetValue(o).ToString() == "")
                                {
                                    dtValE = "9999-12-31 23:59:59";
                                }
                                else
                                {
                                    dtValE = conName.GetValue(o).ToString();
                                }
                                var dbName = attr.DbName == "" ? propInfo.Name.Substring(1) : attr.DbName;
                                pWhereStr.Append(string.Format("{0} >= '{1}' AND {0}<='{2}'", dbName, dtValS, dtValE));
                                continuePDic.Add(conName.Name,"");
                                break;
                            case InputType.DateTimeE:
                                dtValE = dtVal;
                                conName = type.GetProperty("s" + propInfo.Name.Substring(1));
                                if (conName.GetValue(o) == null || conName.GetValue(o).ToString() == "")
                                {
                                    dtValS = "1900-01-01 00:00:00";
                                }
                                else
                                {
                                    dtValS = conName.GetValue(o).ToString();
                                }
                                dbName = attr.DbName == "" ? propInfo.Name.Substring(1) : attr.DbName;
                                pWhereStr.Append(string.Format("{0} >= '{1}' AND {0}<='{2}'", dbName, dtValS, dtValE));
                                continuePDic.Add(conName.Name, "");
                                break;
                        }

                        pWhereStr.AppendLine();
                        resStr += pWhereStr.ToString();
                    }
                }
            }

            return resStr;
        }
    }
}
