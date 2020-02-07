using Castle.Core.Logging;
using CommonUtils;
using DBUtility;
using MuzeyServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BusinessLogic
{
    public class MuzeyBusinessLogic<T> where T : class, new()
    {
        public ILogger Logger { get; set; }

        private Type clazz;
        private PropertyInfo[] fs;
        private string tableName;
        private List<string> pks;
        private string selectStr;
        private string insertStr;
        private string updateStr;
        private bool withNoLock = false;
        private string DbName = "";

        private string CreateSelect(bool withNoLock = false)
        {
            StringBuilder StringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(DbName))
            {
                StringBuilder.Append(string.Format("◎{0}◎", DbName));
            }

            StringBuilder.Append("SELECT ");
            for (int i = 0; i < fs.Length; i++)
            {

                StringBuilder.Append(fs[i].Name);
                if (i != fs.Length - 1)
                    StringBuilder.Append(", ");
            }
            StringBuilder.Append(" FROM ");
            StringBuilder.Append(tableName + (withNoLock ? " WITH(NOLOCK)" : ""));
            StringBuilder.Append(" WHERE 1=1 ");

            return StringBuilder.ToString();
        }

        private string CreateInsert()
        {
            StringBuilder StringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(DbName))
            {
                StringBuilder.Append(string.Format("◎{0}◎", DbName));
            }
            StringBuilder.Append("INSERT INTO ");
            StringBuilder.Append(tableName + "(");
            for (int i = 0; i < fs.Length; i++)
            {

                StringBuilder.Append("⊙" + fs[i].Name + "⊙");
                if (i != fs.Length - 1)
                    StringBuilder.Append(",");
            }
            StringBuilder.Append(") values ");

            return StringBuilder.ToString();
        }

        private string CreateUpdate()
        {
            StringBuilder StringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(DbName))
            {
                StringBuilder.Append(string.Format("◎{0}◎", DbName));
            }
            StringBuilder.Append("UPDATE ");
            StringBuilder.Append(tableName + " SET 〓〓 WHERE ");
            bool first = true;
            foreach (string pk in pks)
            {
                if (first)
                {
                    first = !first;
                }
                else
                {
                    StringBuilder.Append(" AND ");
                }

                StringBuilder.Append(pk + " = '@" + pk + "@'");
            }

            return StringBuilder.ToString();
        }

        private string GetInsertStr(T dto)
        {
            string insertStr = this.insertStr;
            insertStr += "(";
            try
            {
                for (int i = 0; i < fs.Length; i++)
                {
                    string fName = fs[i].Name;
                    object obj = fs[i].GetValue(dto, null);
                    if (obj == null)
                    {
                        insertStr = insertStr.Replace("⊙" + fName + "⊙", "").Replace(",,", ",");
                    }
                    else
                    {
                        insertStr = insertStr.Replace("⊙" + fName + "⊙", fName);
                        if (i != 0)
                            insertStr += ",";

                        insertStr += "'" + obj.ToString() + "'";
                    }
                }
                insertStr += ")";
                insertStr = insertStr.Replace("(,", "(");
                insertStr = insertStr.Replace(",)", ")");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
            }

            return insertStr;
        }

        private string GetUpdateStr(T dto, bool allFlag, string strWhere = null)
        {
            string updateStr = this.updateStr;
            string setValStr = "";
            bool first = true;
            try
            {
                for (int i = 0; i < fs.Length; i++)
                {
                    string fName = fs[i].Name;
                    object obj = fs[i].GetValue(dto, null);
                    if (obj == null)
                    {
                        if (pks.Contains(fName) && strWhere == null)
                        {

                            throw new Exception("PK:" + fName + "value is null!");
                        }

                        if (allFlag)
                        {

                            if (first)
                            {

                                first = !first;
                            }
                            else
                            {

                                setValStr += ", ";
                            }

                            setValStr += fName + "=" + "null";
                        }
                    }
                    else
                    {
                        if (pks.Contains(fName))
                        {
                            updateStr = updateStr.Replace("@" + fName + "@", obj.ToString());
                            continue;
                        }

                        if (first)
                        {
                            first = !first;
                        }
                        else
                        {

                            setValStr += ",";
                        }

                        setValStr += fName + "= '" + obj.ToString() + "'";
                    }
                }

                updateStr = updateStr.Replace("〓〓", setValStr);
                if (strWhere != null)
                {
                    updateStr = updateStr.Split(new string[] { "WHERE " }, StringSplitOptions.None)[0] + "WHERE 1=1 " + strWhere;
                }

            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
            }
            return updateStr;
        }

        public MuzeyBusinessLogic(string tableName, string[] pks)
        {
            this.clazz = typeof(T);
            this.fs = clazz.GetProperties();
            this.tableName = tableName;
            this.pks = pks.ToList();
            this.selectStr = CreateSelect();
            this.insertStr = CreateInsert();
            this.updateStr = CreateUpdate();
        }

        public MuzeyBusinessLogic()
        {
            this.clazz = typeof(T);
            this.fs = clazz.GetProperties();
            this.tableName = clazz.Name.Replace("Dto", "");
            pks = PKInfo.GetPK(tableName).ToList();
            this.selectStr = CreateSelect();
            this.insertStr = CreateInsert();
            this.updateStr = CreateUpdate();
        }

        public MuzeyBusinessLogic(string DbName)
        {
            this.DbName = DbName;
            this.clazz = typeof(T);
            this.fs = clazz.GetProperties();
            this.tableName = clazz.Name.Replace("Dto", "");
            pks = PKInfo.GetPK(tableName).ToList();
            this.selectStr = CreateSelect();
            this.insertStr = CreateInsert();
            this.updateStr = CreateUpdate();
        }

        public MuzeyBusinessLogic(bool withNoLock)
        {
            this.clazz = typeof(T);
            this.fs = clazz.GetProperties();
            this.tableName = clazz.Name.Replace("Dto", "");
            pks = PKInfo.GetPK(tableName).ToList();
            this.selectStr = CreateSelect(withNoLock);
            this.insertStr = CreateInsert();
            this.updateStr = CreateUpdate();
            this.withNoLock = withNoLock;
        }

        public List<T> GetDtoList(string strWhere, string otherName = null)
        {
            StringBuilder StringBuilder = new StringBuilder(selectStr);
            if (strWhere.Trim() != "")
            {
                StringBuilder.Append(strWhere);
            }

            string sql = StringBuilder.ToString();
            if (otherName != null)
            {
                sql = sql.Replace(tableName, tableName + " " + otherName);
            }

            return SqlHelp.Query(sql).Tables[0].DataTableToList<T>();
        }

        public List<MuzeySelectModel> GetSelectList(string strWhere, string text, string val, string otherName = null)
        {
            StringBuilder StringBuilder = new StringBuilder(selectStr);
            if (strWhere.Trim() != "")
            {
                StringBuilder.Append(strWhere);
            }

            string sql = StringBuilder.ToString();
            if (otherName != null)
            {
                sql = sql.Replace(tableName, tableName + " " + otherName);
            }

            var res = new List<MuzeySelectModel>();
            foreach(DataRow dr in SqlHelp.Query(sql).Tables[0].Rows)
            {
                res.Add(new MuzeySelectModel() { text=dr[text].ToStr() , val= dr[val].ToStr() });
            }

            return res;
        }

        public Dictionary<string, T> GetDtoDic(string strWhere, string key, string otherName = null)
        {
            StringBuilder StringBuilder = new StringBuilder(selectStr);
            if (strWhere.Trim() != "")
            {
                StringBuilder.Append(strWhere);
            }

            string sql = StringBuilder.ToString();
            if (otherName != null)
            {
                sql = sql.Replace(tableName, tableName + " " + otherName);
            }

            return SqlHelp.Query(sql).Tables[0].DataTableToDic<T>(key);
        }

        public void InsertDto(T dto)
        {
            SqlHelp.ExecuteSql(GetInsertStr(dto));
        }

        /**
         * 只更新有值的字段
         * 
         * @param dto
         */
        public void UpdateDtoToPart(T dto, string strWhere=null)
        {
            SqlHelp.ExecuteSql(GetUpdateStr(dto, false, strWhere));
        }

        /**
         * 更新全部字段
         * 
         * @param dto
         */
        public void UpdateDtoToAll(T dto, string strWhere=null)
        {
            SqlHelp.ExecuteSql(GetUpdateStr(dto, true, strWhere));
        }

        public void UpdateDtoListToPart(List<T> dtoList)
        {
            string sqlStr = "";
            bool repFlag = false;
            string dbStr = string.Format("◎{0}◎", DbName);
            if (!string.IsNullOrEmpty(DbName))
            {
                repFlag = true;
            }
            foreach (T t in dtoList)
            {
                sqlStr += (repFlag ? GetUpdateStr(t,false).Replace(dbStr, "") : GetUpdateStr(t,false)) + ";";
            }
            SqlHelp.ExecuteSql((repFlag ? dbStr : "") + sqlStr);
        }

        public void UpdateDtoListToAll(List<T> dtoList)
        {
            string sqlStr = "";
            bool repFlag = false;
            string dbStr = string.Format("◎{0}◎", DbName);
            if (!string.IsNullOrEmpty(DbName))
            {
                repFlag = true;
            }
            foreach (T t in dtoList)
            {
                sqlStr += (repFlag ? GetUpdateStr(t, true).Replace(dbStr, "") : GetUpdateStr(t, true)) + ";";
            }
            SqlHelp.ExecuteSql((repFlag ? dbStr : "") + sqlStr);
        }

        public void InsertDtoList(List<T> dtoList)
        {

            string sqlStr = "";
            bool repFlag = false;
            string dbStr = string.Format("◎{0}◎", DbName);
            if (!string.IsNullOrEmpty(DbName))
            {
                repFlag = true;
            }
            foreach (T t in dtoList)
            {
                sqlStr += (repFlag ? GetInsertStr(t).Replace(dbStr,"") : GetInsertStr(t)) + ";";
            }

            SqlHelp.ExecuteSql((repFlag ? dbStr : "" ) + sqlStr);
        }

        public T GetDtoByPK(T dto)
        {

            string strWhere = "";
            T resDto = new T();
            try
            {

                foreach (string pk in pks)
                {

                    object obj = clazz.GetProperty(pk).GetValue(dto, null);
                    if (obj == null)
                    {

                        throw new Exception("主键值不能为空");
                    }
                    else
                    {

                        strWhere += "AND " + pk + "='" + obj.ToString() + "' ";
                    }
                }

                List<T> list = SqlHelp.Query(selectStr + strWhere).Tables[0].DataTableToList<T>();
                if (list != null)
                {
                    if (list.Count != 0)
                    {

                        resDto = list[0];
                        return resDto;
                    }
                }
            }
            catch (Exception e)
            {

                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
            }

            return null;
        }

        private string GetDeleteStr(T dto)
        {
            var StringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(DbName))
            {
                StringBuilder.Append(string.Format("◎{0}◎", DbName));
            }
            StringBuilder.Append("DELETE FROM " + tableName + " WHERE 1=1 ");
            try
            {
                if (dto != null)
                {
                    foreach (PropertyInfo f in fs)
                    {
                        object obj = f.GetValue(dto, null);
                        if (obj != null && !string.IsNullOrEmpty(obj.ToString()))
                        {
                            StringBuilder.Append("AND " + f.Name + "='" + obj.ToString() + "' ");
                        }
                    }
                }
            }
            catch (Exception e)
            {

                
            }

            return StringBuilder.ToString();
        }

        /// <summary>
        /// 按照给定的DTO里的值过滤删除
        /// </summary>
        /// <param name="dto"></param>
        public void DeleteDto(T dto)
        {
            bool repFlag = false;
            string dbStr = string.Format("◎{0}◎", DbName);
            if (!string.IsNullOrEmpty(DbName))
            {
                repFlag = true;
            }
            SqlHelp.ExecuteSql(GetDeleteStr(dto));
        }

        public void DeleteDtoList(List<T> dtoList)
        {

            string sqlStr = "";
            bool repFlag = false;
            string dbStr = string.Format("◎{0}◎", DbName);
            if (!string.IsNullOrEmpty(DbName))
            {
                repFlag = true;
            }
            foreach (T t in dtoList)
            {
                sqlStr += (repFlag ? GetDeleteStr(t).Replace(dbStr, "") : GetDeleteStr(t)) + ";";
            }

            SqlHelp.ExecuteSql((repFlag ? dbStr : "") + sqlStr);
        }

        /// <summary>
        /// 获取分页List
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderBy">当前行号从0开始</param>
        /// <param name="offset">页行数</param>
        /// <param name="size">页大小</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>使用后通过该对象的pageCount属性获取总行数</returns>
        public List<T> GetPageList(string strWhere, string strOrderBy, int offset, int size, out int totalCount)
        {
            StringBuilder filedSB = new StringBuilder();
            for (int i = 0; i < fs.Length; i++)
            {

                filedSB.Append(fs[i].Name);
                if (i != fs.Length - 1)
                    filedSB.Append(", ");
            }

            string tableName = clazz.Name;
            tableName = tableName.Replace("com.muzey.dto.", "");
            tableName = tableName.Replace("Dto", "");

            StringBuilder StringBuilder = new StringBuilder();
            StringBuilder.AppendLine("SELECT");
            StringBuilder.AppendLine(filedSB.ToString());
            StringBuilder.AppendLine("FROM (");
            StringBuilder.AppendLine("  SELECT ");
            if (string.IsNullOrEmpty(strOrderBy))
            {
                StringBuilder.AppendLine("  ROW_NUMBER() OVER(ORDER BY id DESC) AS RowNum,  ");
            }
            else
            {
                StringBuilder.AppendLine("  ROW_NUMBER() OVER(ORDER BY " + strOrderBy + ") AS RowNum,  ");
            }
            StringBuilder.AppendLine("  " + filedSB.ToString());
            StringBuilder.AppendLine("  FROM " + tableName);
            StringBuilder.AppendLine("  WHERE 1=1 ");
            StringBuilder.AppendLine("  " + strWhere);
            StringBuilder.AppendLine(") AS data ");
            StringBuilder.AppendLine(" WHERE 1=1");

            var count_sql = "";
            if (!string.IsNullOrEmpty(DbName))
            {
                count_sql = string.Format("◎{0}◎", DbName);
            }
            count_sql += "SELECT count(*) as totalCount  FROM (" + StringBuilder.ToString() + ") AS data"; //计算条数的 sql

            totalCount = int.Parse(SqlHelp.Query(count_sql).Tables[0].Rows[0][0].ToString());

            StringBuilder.AppendLine(" AND data.RowNum > " + offset);
            StringBuilder.AppendLine(" AND data.RowNum <= " + (offset + size));
            var dataSql = StringBuilder.ToString();
            if (!string.IsNullOrEmpty(DbName))
            {
                dataSql = string.Format("◎{0}◎", DbName) + dataSql;
            }
            return SqlHelp.Query(dataSql).Tables[0].DataTableToList<T>();
        }

        /// <summary>
        /// 获取分页List(同类表)
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrderBy">当前行号从0开始</param>
        /// <param name="offset">页行数</param>
        /// <param name="size">页大小</param>
        /// <param name="totalCount">总行数</param>
        /// <returns>使用后通过该对象的pageCount属性获取总行数</returns>
        public List<T> GetSamePageList(string strWhere, string strOrderBy, int offset, int size, out int totalCount, List<string> tableNames)
        {
            var dataSql = SameUnion(tableNames, strWhere);
            return SqlHelp.QueryPageList(dataSql, strOrderBy, offset, size, out totalCount).Tables[0].DataTableToList<T>();
        }

        /// <summary>
        /// 增加一条数据--并返回当前种子
        /// </summary>
        /// <returns></returns>
        public int InsertEx(T dto)
        {
            return SqlHelp.ExecuteScalar(GetInsertStr(dto) + ";SELECT @@IDENTITY", null).ToInt();
        }

        public void ReplaceCol(Dictionary<string,string> replaceDic)
        {
            foreach(var kv in replaceDic)
            {
                this.selectStr = this.selectStr.Replace((string.IsNullOrEmpty(kv.Value)? ", " : "") + kv.Key, kv.Value);
                var insertReplaceStr = 
                this.insertStr = this.insertStr.Replace((string.IsNullOrEmpty(kv.Value) ? "," : "") + "⊙" + kv.Key + "⊙",
                (string.IsNullOrEmpty(kv.Value)) ? "" : "⊙" + kv.Value + "⊙");
            }

            var fsList = new List<PropertyInfo>();
            for(int i=0;i< fs.Length;i++)
            {
                if (!replaceDic.ContainsKey(fs[i].Name))
                {
                    fsList.Add(fs[i]);
                }
            }

            fs = fsList.ToArray();
        }

        public string SameUnion(List<string> tableNames, string strWhere)
        {
            var resStr = "";
            var first = true;
            if(selectStr.Contains(string.Format("◎{0}◎", DbName)))
            {
                resStr += string.Format("◎{0}◎", DbName);
            }
            var baseSelectStr = selectStr.Replace(string.Format("◎{0}◎", DbName),"");
            foreach (var tn in tableNames)
            {
                if (!first)
                {
                    resStr += " UNION ";
                }
                else
                {
                    first = false;
                }

                resStr += baseSelectStr.Replace(tableName, tn) + strWhere;
            }

            return resStr;
        }

        public void ChangeTableName(string tableName)
        {
           this.selectStr = this.selectStr.Replace(this.tableName,tableName);
           this.insertStr = this.insertStr.Replace(this.tableName, tableName);
           this.updateStr = this.updateStr.Replace(this.tableName, tableName);
        }
    }
}
