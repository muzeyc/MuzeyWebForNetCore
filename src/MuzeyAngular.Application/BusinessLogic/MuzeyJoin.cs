using DBUtility;
using CommonUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class MuzeyJoin
    {
        private string sqlBase;
        private Dictionary<string, List<object>> dataMap;
        private Dictionary<string, Type> classMap;
        private List<string> colList;
        public List<object> dataList { get; set; }

        public MuzeyJoin(List<OnInfo> onInfoList)
        {
            classMap = new Dictionary<string, Type>();
            colList = new List<string>();
            StringBuilder joinOnStr = new StringBuilder();
            string[] ts1 = onInfoList[0].f1.Split('∷');
            string[] ts2 = onInfoList[0].f2.Split('∷');
            string tableName1 = ts1[0];
            string tableName2 = ts2[0];
            joinOnStr.AppendLine(tableName1 + " AS " + tableName1);
            joinOnStr.AppendLine(onInfoList[0].je.ToString());
            joinOnStr.AppendLine(tableName2 + " AS " + tableName2);
            joinOnStr.AppendLine(" ON ");
            joinOnStr.AppendLine(tableName1 + "." + ts1[1]);
            joinOnStr.AppendLine(onInfoList[0].judgeStr);
            if (ts2[1] != "where")
            {
                joinOnStr.AppendLine(tableName2 + "." + ts2[1]);
            }
            joinOnStr.AppendLine(" " + "★" + tableName1 + "__" + tableName2 + "★");
            classMap.Add(tableName1, Type.GetType("BusinessLogic." + tableName1 + "Dto"));
            classMap.Add(tableName2, Type.GetType("BusinessLogic." + tableName2 + "Dto"));
            for (int i = 1; i < onInfoList.Count; i++)
            {
                StringBuilder js = new StringBuilder();
                ts1 = onInfoList[i].f1.Split('∷');
                ts2 = onInfoList[i].f2.Split('∷');
                tableName1 = ts1[0];
                tableName2 = ts2[0];
                string replaceStr = "★" + tableName1 + "__" + tableName2 + "★";
                if (joinOnStr.ToString().Contains(replaceStr) || joinOnStr.ToString().Contains("★" + tableName2 + "__" + tableName1 + "★") && ts2[1] != "where")
                {
                    if (joinOnStr.ToString().Contains("★" + tableName2 + "__" + tableName1 + "★"))
                    {
                        var lt = tableName1;
                        tableName1 = tableName2;
                        tableName2 = lt;
                        replaceStr = "★" + tableName1 + "__" + tableName2 + "★";
                        var lts = ts1;
                        ts1 = ts2;
                        ts2 = lts;
                    }
                    js.AppendLine(" AND ");
                }
                else
                {
                    if(classMap.ContainsKey(tableName1) && classMap.ContainsKey(tableName2))
                    {
                        js.AppendLine(" AND ");
                    }
                    else if (classMap.ContainsKey(tableName1))
                    {
                        joinOnStr.AppendLine(onInfoList[i].je.ToString());
                        joinOnStr.AppendLine(tableName2 + " AS " + tableName2);
                        joinOnStr.AppendLine(" ON");
                    }
                    else if (classMap.ContainsKey(tableName2))
                    {
                        joinOnStr.AppendLine(onInfoList[i].je.ToString());
                        joinOnStr.AppendLine(tableName1 + " AS " + tableName1);
                        joinOnStr.AppendLine(" ON");
                    }

                    joinOnStr.AppendLine(" " + replaceStr);
                }
                js.AppendLine(tableName1 + "." + ts1[1]);
                js.AppendLine(onInfoList[i].judgeStr);
                if (ts2[1] != "where")
                {
                    js.AppendLine(tableName2 + "." + ts2[1]);
                }

                joinOnStr = new StringBuilder(joinOnStr.ToString().Replace(replaceStr, js.ToString() + " " + replaceStr));
                if (!classMap.ContainsKey(tableName1))
                {
                    classMap.Add(tableName1, Type.GetType("BusinessLogic." + tableName1 + "Dto"));
                }
                if (!classMap.ContainsKey(tableName2))
                {
                    classMap.Add(tableName2, Type.GetType("BusinessLogic." + tableName2 + "Dto"));
                }
            }

            string joinS = "";
            bool rb = true;
            foreach (string ss in joinOnStr.ToString().Split('★'))
            {

                if (rb)
                {

                    joinS += ss;
                }
                rb = !rb;
            }

            StringBuilder s = new StringBuilder();
            s.AppendLine("SELECT ");
            bool isFirst = true;
            // 字段
            foreach (var kv in classMap)
            {

                string tableName = kv.Key;
                foreach (var fc in kv.Value.GetProperties())
                {
                    if (!isFirst)
                    {
                        s.AppendLine(", ");
                    }
                    else
                    {
                        isFirst = !isFirst;
                    }
                    string colStr = tableName + "__" + fc.Name;
                    colList.Add(colStr);
                    s.AppendLine(tableName + "." + fc.Name + " AS " + colStr);
                }
            }

            s.AppendLine(" FROM ");
            // 表名连接
            s.AppendLine(joinS);
            this.sqlBase = s.ToString();
        }

        public DataTable queryForDt(string strWhere)
        {

            StringBuilder stringBuffer = new StringBuilder();
            stringBuffer.AppendLine("SELECT * FROM (");
            stringBuffer.AppendLine(sqlBase);
            stringBuffer.AppendLine(") tQuery");
            if (strWhere.Trim() != "")
            {
                stringBuffer.AppendLine("  WHERE 1=1 ");
                stringBuffer.AppendLine(strWhere);
            }

            return DbHelperSQL.Query(stringBuffer.ToString()).Tables[0];
        }

        public Dictionary<string,List<object>> QueryToDtoMap(string strWhere)
        {
            StringBuilder stringBuffer = new StringBuilder(sqlBase);
            stringBuffer.AppendLine("SELECT * FROM (");
            stringBuffer.AppendLine(sqlBase);
            stringBuffer.AppendLine(") tQuery");
            if (strWhere.Trim() != "")
            {
                stringBuffer.AppendLine("  WHERE 1=1 ");
                stringBuffer.AppendLine(strWhere);
            }

            dataMap = DataTableToDs(DbHelperSQL.Query(stringBuffer.ToString()).Tables[0]);
            return dataMap;
        }

        public Dictionary<string, List<object>> queryPage(string strWhere, string strOrderBy, int offset, int size, out int totalCount)
        {
            dataMap = DataTableToDs(DbHelperSQL.Query(getPageStr(strWhere, strOrderBy, offset, size, out totalCount)).Tables[0]);
            return dataMap;
        }

        public DataTable queryPageForDt(string strWhere, string strOrderBy, int offset, int size, out int totalCount)
        {
            return DbHelperSQL.Query(getPageStr(strWhere, strOrderBy, offset, size, out totalCount)).Tables[0];
        }

        public string getSqlBase()
        {

            return sqlBase;
        }
        public List<T> getDtoList<T>()
        {
            string tableName = typeof(T).Name;
            tableName = tableName.Replace("Dto", "");
            return Enumerable.OfType<T>(dataMap[tableName]).ToList();
        }

        private string getPageStr(string strWhere, string strOrderBy, int offset, int size, out int totalCount)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(string.Join(",", colList));
            sb.AppendLine(" FROM (");
            sb.AppendLine("  SELECT ");
            if (string.IsNullOrEmpty(strOrderBy))
            {
                //  sb.AppendLine(" top " + (offset + size) + "  ROW_NUMBER() OVER(ORDER BY " + colList[0] + " DESC) AS RowNum,  "); //返回的totalCount有问题
                sb.AppendLine(" ROW_NUMBER() OVER(ORDER BY " + colList[0] + " DESC) AS RowNum,  ");
            }
            else
            {
                //   sb.AppendLine(" top " + (offset + size) + " ROW_NUMBER() OVER(ORDER BY " + strOrderBy + ") AS RowNum,  "); //返回的totalCount有问题
                sb.AppendLine(" ROW_NUMBER() OVER(ORDER BY " + strOrderBy + ") AS RowNum,  ");
            }
            sb.AppendLine("  " + string.Join(",", colList));
            sb.AppendLine("  FROM " + "(" + sqlBase + ") AS joinTable ");
            sb.AppendLine("  WHERE 1=1 ");
            sb.AppendLine("  " + strWhere);
            sb.AppendLine(") AS data ");
            sb.AppendLine(" WHERE 1=1");

            totalCount = DbHelperSQL.Query(sb.ToString()).Tables[0].Rows.Count;

            sb.AppendLine(" AND data.RowNum > " + offset);
            sb.AppendLine(" AND data.RowNum <= " + (offset + size));

            return sb.ToString();
        }
        private Dictionary<string, List<object>> DataTableToDs(DataTable dt)
        {
            Dictionary<string, List<object>> resMap = new Dictionary<string, List<object>>();
            List<string> names = new List<string>();
            //表名-起始位
            Dictionary<string, int> colSNumMap = new Dictionary<string, int>();
            //表名-长度
            Dictionary<string, int> colSizeMap = new Dictionary<string, int>();
            //表名-Class
            Dictionary<string, Type> colClassMap = new Dictionary<string, Type>();
            int sizeInt = 0;
            string tableN = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {

                string ns = dt.Columns[i].ToString();
                string tableO = tableN;
                string[] tableCol = ns.Split(new string[]{"__"},StringSplitOptions.None);
                tableN = tableCol[0];
                //出现新表
                if ((i != 0) && !colSNumMap.ContainsKey(tableN))
                {

                    colSNumMap.Add(tableN, i);
                    colClassMap.Add(tableN, Type.GetType("BusinessLogic." + tableN + "Dto"));
                    resMap.Add(tableN, new List<object>());
                    colSizeMap.Add(tableO, sizeInt);
                    sizeInt = 1;
                }
                else
                {

                    sizeInt++;
                }

                if (i == 0)
                {

                    colSNumMap.Add(tableN, i);
                    colClassMap.Add(tableN, Type.GetType("BusinessLogic." + tableN + "Dto"));
                    resMap.Add(tableN, new List<object>());
                }

                if (i == dt.Columns.Count - 1)
                {

                    colSizeMap.Add(tableN, sizeInt);
                }

                names.Add(tableCol[1]);
            }

            foreach (DataRow dr in dt.Rows)
            {
                foreach (var kv in colSNumMap)
                {

                    Type clazz = colClassMap[kv.Key];
                    object obj = clazz.Assembly.CreateInstance(clazz.FullName);
                    for (int i = kv.Value; i < kv.Value + colSizeMap[kv.Key]; i++)
                    {

                        if (DBNull.Value.Equals(dr[i]))
                        {

                            continue;
                        }
                        else
                        {

                            var typeName = clazz.GetProperty(names[i]).PropertyType.Name;
                            bool isNullable = "Nullable`1".Equals(typeName);
                            if (isNullable)
                            {
                                typeName = clazz.GetProperty(names[i]).PropertyType.GenericTypeArguments[0].Name;
                                if (DBNull.Value.Equals(dr[i]))
                                {
                                    clazz.GetProperty(names[i]).SetValue(obj, null, null);
                                    continue;
                                }
                            }
                            switch (typeName)
                            {
                                case "String":
                                    clazz.GetProperty(names[i]).SetValue(obj, dr[i].ToStr(), null);
                                    break;
                                case "Int32":
                                    clazz.GetProperty(names[i]).SetValue(obj, dr[i].ToInt(), null);
                                    break;
                                case "Double":
                                    clazz.GetProperty(names[i]).SetValue(obj, dr[i].ToDouble(), null);
                                    break;
                                case "Decimal":
                                    clazz.GetProperty(names[i]).SetValue(obj, dr[i].ToDec(), null);
                                    break;
                                case "Int64":
                                    clazz.GetProperty(names[i]).SetValue(obj, dr[i].ToLong(), null);
                                    break;
                                case "DateTime":
                                    clazz.GetProperty(names[i]).SetValue(obj, dr[i].ToDateTime(), null);
                                    break;
                                default:
                                    clazz.GetProperty(names[i]).SetValue(obj, dr[i], null);
                                    break;
                            }
                        }
                    }

                    resMap[kv.Key].Add(obj);
                }
            }
            return resMap;
        }

        public List<object> QueryToList(string strWhere)
        {
            var tls = new List<Type>();
            foreach (var kv in classMap)
            {
                tls.Add(kv.Value);
            }
            dataList = ModelUtil.DataTableToList(queryForDt(strWhere), TypeUtil.DtoMerge(tls));
            return dataList;
        }

        public List<object> QueryPageToList(string strWhere, string strOrderBy, int offset, int size, out int totalCount)
        {
            var tls = new List<Type>();
            foreach (var kv in classMap)
            {
                tls.Add(kv.Value);
            }
            dataList = ModelUtil.DataTableToList(queryPageForDt(strWhere, strOrderBy, offset, size, out totalCount), TypeUtil.DtoMerge(tls));
            return dataList;
        }
    }
}
