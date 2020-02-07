using CommonUtils;
using DBUtility;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BusinessLogic
{
    public class SqlHelp
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet Query(string sql)
        {
            if(sql[0] == '◎')
            {
                var ss = sql.Split('◎');
                var paramsArr = ss[1].Split('※');
                if(paramsArr.Length > 1)
                {
                    return DbHelperSQL.DirDbQuery(ss[2], paramsArr[1], ConnectionInfo.GetConnectionInfo(paramsArr[0]));
                }
                return DbHelperSQL.DirDbQuery(ss[2],ss[1]);
            }
            return DbHelperSQL.Query(sql);
        }

        /// <summary>
        /// 执行Sql
        /// </summary>
        /// <param name="sql"></param>
        public static int ExecuteSql(string sql)
        {
            if (sql[0] == '◎')
            {
                var ss = sql.Split('◎');
                var paramsArr = ss[1].Split('※');
                if (paramsArr.Length > 1)
                {
                    return DbHelperSQL.DirDbExecuteSql(ss[2], paramsArr[1], ConnectionInfo.GetConnectionInfo(paramsArr[0]));
                }
                return DbHelperSQL.DirDbExecuteSql(ss[2], ss[1]);
            }
            return DbHelperSQL.ExecuteSql(sql);
        }

        public static DataSet QueryPageList(string sql, string orderBy, int offset, int size, out int totalCount)
        {
            var dbStr = "";
            if (sql[0] == '◎')
            {
                var ss = sql.Split('◎');
                dbStr = "◎" + ss[1] + "◎";
                sql = ss[2];
            }
            var count_sql = "SELECT count(*) as totalCount  FROM (" + sql + ") AS data"; 

            totalCount = int.Parse(SqlHelp.Query(dbStr + count_sql).Tables[0].Rows[0][0].ToString());

            var page_sql = new StringBuilder();

            page_sql.AppendLine("SELECT * FROM (");
            page_sql.AppendLine("  SELECT ROW_NUMBER() OVER(ORDER BY " + orderBy + ") AS RowNum, * ");

            page_sql.AppendLine(" FROM(" + sql + ") AS data2 ");
            page_sql.AppendLine(") AS data");

            page_sql.AppendLine("WHERE 1=1");

            page_sql.AppendLine("AND data.RowNum>" + offset);
            page_sql.AppendLine("AND data.RowNum<=" + (offset + size));

            return SqlHelp.Query(dbStr + page_sql.ToString());
        }

        /// <summary>
        /// 增加一条数据--并返回当前种子
        /// </summary>
        /// <returns></returns>
        public static int ExecuteScalar(string sql, params SqlParameter[] cmdParms)
        {
            if (sql[0] == '◎')
            {
                var ss = sql.Split('◎');
                var paramsArr = ss[1].Split('※');
                if (paramsArr.Length > 1)
                {
                    return DbHelperSQL.DirDbExecuteScalar(ss[2], paramsArr[1], cmdParms, ConnectionInfo.GetConnectionInfo(paramsArr[0])).ToInt();
                }
                return DbHelperSQL.DirDbExecuteScalar(ss[2], ss[1], cmdParms).ToInt();
            }
            return DbHelperSQL.ExecuteScalar(sql, cmdParms).ToInt();
        }
    }
}
