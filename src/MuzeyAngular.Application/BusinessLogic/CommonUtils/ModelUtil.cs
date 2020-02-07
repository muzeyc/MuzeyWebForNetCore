using CommonUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CommonUtils
{
    public static class ModelUtil
    {
        /// <summary>
        /// 给JoinDto 某个字段赋值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="enumField"></param>
        /// <param name="value"></param>
        public static void SetDtoValue(this object obj, object enumField, object value)
        {
            var t = obj.GetType();
            var typeName = t.GetProperty(enumField.ToString()).PropertyType.Name;
            bool isNullable = "Nullable`1".Equals(typeName);
            if (isNullable)
            {
                typeName = t.GetProperty(enumField.ToString()).PropertyType.GenericTypeArguments[0].Name;
            }
            switch (typeName)
            {
                case "String":
                    t.GetProperty(enumField.ToString()).SetValue(obj, value.ToStr(), null);
                    break;
                case "Int32":
                    t.GetProperty(enumField.ToString()).SetValue(obj, value.ToInt(), null);
                    break;
                case "Double":
                    t.GetProperty(enumField.ToString()).SetValue(obj, value.ToDouble(), null);
                    break;
                case "Decimal":
                    t.GetProperty(enumField.ToString()).SetValue(obj, value.ToDec(), null);
                    break;
                case "Int64":
                    t.GetProperty(enumField.ToString()).SetValue(obj, value.ToLong(), null);
                    break;
                case "DateTime":
                    t.GetProperty(enumField.ToString()).SetValue(obj, value.ToDateTime(), null);
                    break;
                default:
                    t.GetProperty(enumField.ToString()).SetValue(obj, value, null);
                    break;
            }
        }

        public static void SetDtoValue(this object obj, object dataDto)
        {
            //获得该类的Type
            Type t = obj.GetType();
            Type rt = dataDto.GetType();
            string tableName = rt.Name.Replace("Dto", "");
            foreach (PropertyInfo pi in rt.GetProperties())
            {
                var pn = tableName + "__" + pi.Name;
                var tp = t.GetProperty(pn);
                if (tp != null)
                {
                    t.GetProperty(pn).SetValue(obj, rt.GetProperty(pi.Name).GetValue(dataDto, null), null);
                }
            }
        }

        /// <summary>
        /// ListModel转DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataList"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static DataTable ListModelToDataTable<T>(List<T> dataList, string tableName, List<string> columns = null)
        {
            DataTable dt = new DataTable(tableName);
            List<string> colNameList = new List<string>();
            if (dataList.Count > 0)
            {
                //追加列
                Type t = dataList[0].GetType();//获得该类的Type
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    if (columns != null)
                    {
                        if (columns.Contains(pi.Name))
                        {
                            dt.Columns.Add(pi.Name);
                            colNameList.Add(pi.Name);
                        }
                    }
                    else
                    {
                        dt.Columns.Add(pi.Name);
                        colNameList.Add(pi.Name);
                    }
                }

                //追加行
                foreach (T dataT in dataList)
                {
                    DataRow dr = dt.NewRow();
                    foreach (string colName in colNameList)
                    {
                        dr[colName] = t.GetProperty(colName).GetValue(dataT, null);
                    }

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> DataTableToList<T>(this DataTable dt)
        {
            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T obj = System.Activator.CreateInstance<T>();
                Type t = obj.GetType();

                foreach (var pi in t.GetProperties())
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        var typeName = t.GetProperty(pi.Name).PropertyType.Name;
                        bool isNullable = "Nullable`1".Equals(typeName);
                        if (isNullable)
                        {
                            typeName = t.GetProperty(pi.Name).PropertyType.GenericTypeArguments[0].Name;
                            if (DBNull.Value.Equals(row[pi.Name]))
                            {
                                t.GetProperty(pi.Name).SetValue(obj, null, null);
                                continue;
                            }
                        }
                        switch (typeName)
                        {
                            case "String":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToStr(), null);
                                break;
                            case "Int32":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToInt(), null);
                                break;
                            case "Double":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDouble(), null);
                                break;
                            case "Decimal":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDec(), null);
                                break;
                            case "Int64":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToLong(), null);
                                break;
                            case "DateTime":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDateTime(), null);
                                break;
                            default:
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name], null);
                                break;
                        }
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static Dictionary<string, T> DataTableToDic<T>(this DataTable dt, string key)
        {
            var dic = new Dictionary<string, T>();
            foreach (DataRow row in dt.Rows)
            {
                T obj = System.Activator.CreateInstance<T>();
                Type t = obj.GetType();

                foreach (var pi in t.GetProperties())
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        var typeName = t.GetProperty(pi.Name).PropertyType.Name;
                        bool isNullable = "Nullable`1".Equals(typeName);
                        if (isNullable)
                        {
                            typeName = t.GetProperty(pi.Name).PropertyType.GenericTypeArguments[0].Name;
                            if (DBNull.Value.Equals(row[pi.Name]))
                            {
                                t.GetProperty(pi.Name).SetValue(obj, null, null);
                                continue;
                            }
                        }
                        switch (typeName)
                        {
                            case "String":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToStr(), null);
                                break;
                            case "Int32":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToInt(), null);
                                break;
                            case "Double":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDouble(), null);
                                break;
                            case "Decimal":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDec(), null);
                                break;
                            case "Int64":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToLong(), null);
                                break;
                            case "DateTime":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDateTime(), null);
                                break;
                            default:
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name], null);
                                break;
                        }
                    }
                }

                string keyVal = t.GetProperty(key).GetValue(obj, null).ToStr();
                if (dic.ContainsKey(keyVal))
                {
                    dic[keyVal] = obj;
                }
                else
                {
                    dic.Add(keyVal, obj);
                }
                
            }

            return dic;
        }

        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<object> DataTableToList(this DataTable dt, Type t)
        {
            var list = new List<object>();
            foreach (DataRow row in dt.Rows)
            {
                var obj = System.Activator.CreateInstance(t);
                foreach (var pi in t.GetProperties())
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        var typeName = t.GetProperty(pi.Name).PropertyType.Name;
                        bool isNullable = "Nullable`1".Equals(typeName);
                        if (isNullable)
                        {
                            typeName = t.GetProperty(pi.Name).PropertyType.GenericTypeArguments[0].Name;
                            if (DBNull.Value.Equals(row[pi.Name]))
                            {
                                t.GetProperty(pi.Name).SetValue(obj, null, null);
                                continue;
                            }
                        }
                        switch (typeName)
                        {
                            case "String":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToStr(), null);
                                break;
                            case "Int32":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToInt(), null);
                                break;
                            case "Double":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDouble(), null);
                                break;
                            case "Decimal":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDec(), null);
                                break;
                            case "Int64":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToLong(), null);
                                break;
                            case "DateTime":
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name].ToDateTime(), null);
                                break;
                            default:
                                t.GetProperty(pi.Name).SetValue(obj, row[pi.Name], null);
                                break;
                        }
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// Dto复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromObj"></param>
        /// <returns></returns>
        public static T JoinDtoCopy<T>(object obj)
        {
            //获得该类的Type
            Type t = obj.GetType();
            T newObj = System.Activator.CreateInstance<T>();
            Type rt = newObj.GetType();
            string tableName = rt.Name.Replace("Dto","");

            var dic = new Dictionary<string, object>();
            foreach (PropertyInfo pi in t.GetProperties())
            {
                dic[pi.Name] = t.GetProperty(pi.Name).GetValue(obj, null);
            }

            
            foreach (PropertyInfo pi in rt.GetProperties())
            {
                var pn = tableName + "__" + pi.Name;
                if (dic.ContainsKey(pn))
                {
                    rt.GetProperty(pi.Name).SetValue(newObj, dic[pn], null);
                }
            }

            return newObj;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fromObj"></param>
        /// <returns></returns>
        public static T Copy<T>(object obj)
        {
            //获得该类的Type
            Type t = obj.GetType();

            var dic = new Dictionary<string, object>();

            foreach (PropertyInfo pi in t.GetProperties())
            {
                dic[pi.Name] = t.GetProperty(pi.Name).GetValue(obj, null);
            }

            T newObj = System.Activator.CreateInstance<T>();
            Type rt = newObj.GetType();
            foreach (PropertyInfo pi in rt.GetProperties())
            {
                if (dic.ContainsKey(pi.Name))
                {
                    rt.GetProperty(pi.Name).SetValue(newObj, dic[pi.Name], null);
                }
            }

            return newObj;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="fromObj"></typeparam>
        /// <param name="toObj"></param>
        /// <returns></returns>
        public static void Copy(object fromObj, object toObj)
        {
            //获得该类的Type
            Type t = fromObj.GetType();

            var dic = new Dictionary<string, object>();

            foreach (PropertyInfo pi in t.GetProperties())
            {
                dic[pi.Name] = t.GetProperty(pi.Name).GetValue(fromObj, null);
            }

            Type rt = toObj.GetType();
            foreach (PropertyInfo pi in rt.GetProperties())
            {
                if (dic.ContainsKey(pi.Name))
                {
                    rt.GetProperty(pi.Name).SetValue(toObj, dic[pi.Name], null);
                }
            }
        }
    }
}
