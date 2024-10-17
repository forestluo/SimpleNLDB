using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Misc
{
    public class Common
    {
        // 最大线程数
        public static readonly int MAX_THREADS = 10;
#if _USE_CLR
        public static readonly string CONNECT_STRING = "context connection = true";
#elif _USE_CONSOLE
        public static readonly string CONNECT_STRING =
            "Data Source=MSITRIDENT3;Initial Catalog=nldb3;Integrated Security=TRUE";
#endif

        public static bool IsTooLong(string strValue)
        {
            // 返回结果
            return strValue is null ? false : strValue.Length > 500;
        }

        public static void ExecuteNonQuery(string cmdString)
        {
            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 执行指令
                sqlCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("Common", "ExecuteNonQuery", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }
        }

        public static void ExecuteNonQuery(string cmdString, Dictionary<string, string> parameters)
        {
            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 遍历参数
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    sqlCommand.Parameters.AddWithValue(kvp.Key, kvp.Value);
                }
                // 执行指令
                sqlCommand.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("Common", "ExecuteNonQuery", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }
        }
    }
}
