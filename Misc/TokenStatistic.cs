using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Misc
{
    public class TokenStatistic
    {
        // 创建数据字典
        private static Dictionary<char, int> tokens = new Dictionary<char, int>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int GetTokenCount(char cToken)
        {
            // 返回结果
            return tokens.ContainsKey(cToken) ?
                tokens[cToken] : LoadToken(cToken);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ClearTokens()
        {
            // 记录日志
            Log.LogMessage("TokenStatistic", "ClearTokens", "清理数据记录！");
            // 清理数据
            tokens.Clear();
            // 记录日志
            Log.LogMessage("TokenStatistic", "ClearTokens", "数据清理完毕！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ReloadTokens()
        {
            // 记录日志
            Log.LogMessage("TokenStatistic", "ReloadTokens", "清理数据记录！");
            // 清理数据
            tokens.Clear();

            // 记录日志
            Log.LogMessage("TokenStatistic", "ReloadTokens", "加载数据记录！");

            // 指令字符串
            string cmdString =
                "SELECT [content], [count] FROM [dbo].[TokenContent];";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();

                // 计数器
                int total = 0;
                // 循环处理
                while (reader.Read())
                {
                    // 增加计数
                    total++;
                    // 检查计数
                    if (total % 10000 == 0)
                    {
                        // 打印记录
                        Log.LogMessage(string.Format("{0} tokens was loaded !", total));
                    }

                    // 获得内容
                    string value = reader.GetString(0);
                    // 检查结果
                    if (value == null || value.Length <= 0) continue;
                    // 增加记录
                    tokens.Add(value[0], reader.GetInt32(1));
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("TokenStatistic", "ReloadTokens", "unexpected exit ！");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage(string.Format("\ttokens.count = {0}", tokens.Count));
            // 记录日志
            Log.LogMessage("TokenStatistic", "ReloadTokens", "数据记录已加载！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void UpdateTokens()
        {
            // 记录日志
            Log.LogMessage("TokenStatistic", "UpdateTokens", "更新数据！");
            Log.LogMessage(string.Format("\ttokens.count = {0}", tokens.Count));

            // 生成批量处理语句
            string cmdString =
                "UPDATE [dbo].[TokenContent] " +
                    "SET [count] = @SqlCount WHERE [content] = @SqlToken; " +
                "IF @@ROWCOUNT <= 0 " +
                    "INSERT INTO [dbo].[TokenContent] " +
                    "([content], [unicode], [count], [remark]) " +
                        "VALUES (@SqlToken, UNICODE(@SqlToken), @SqlCount, @SqlRemark); ";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 记录日志
                Log.LogMessage("TokenStatistic", "UpdateTokens", "数据连接已开启！");

                // 开启事务处理模式
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();
                // 记录日志
                Log.LogMessage("TokenStatistic", "UpdateTokens", "事务处理已开启！");

                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 设置事物处理模式
                sqlCommand.Transaction = sqlTransaction;
                // 记录日志
                Log.LogMessage("TokenStatistic", "UpdateTokens", "T-SQL指令已创建！");

                // 计数器
                int total = 0;
                // 遍历参数
                foreach (KeyValuePair<char, int> kvp in tokens)
                {
                    // 增加计数
                    total++;
                    // 检查计数
                    if (total % 10000 == 0)
                    {
                        // 打印记录
                        Log.LogMessage(string.Format("{0} tokens updated !", total));
                    }

                    // 获得描述
                    int count = kvp.Value;
                    // 记录日志
                    //Log.LogMessage(string.Format("\tcount = {0}", count));
                    //Log.LogMessage(string.Format("\tcontent = {0}", kvp.Key));
                    // 清理参数
                    sqlCommand.Parameters.Clear();
                    // 设置参数
                    sqlCommand.Parameters.AddWithValue("SqlToken", kvp.Key);
                    sqlCommand.Parameters.AddWithValue("SqlCount", count);
                    sqlCommand.Parameters.AddWithValue("SqlRemark", Token.GetDescription(kvp.Key));
                    // 执行指令（尚未执行）
                    sqlCommand.ExecuteNonQuery();
                }
                // 打印记录
                Log.LogMessage(string.Format("{0} tokens updated !", total));
                // 记录日志
                Log.LogMessage("TokenStatistic", "UpdateTokens", "批量指令已添加！");

                // 提交事务处理
                sqlTransaction.Commit();
                // 记录日志
                Log.LogMessage("TokenStatistic", "UpdateTokens", "批量指令已提交！");
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("TokenStatistic", "UpdateTokens", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}",ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("TokenStatistic", "UpdateTokens", "数据记录已更新！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void MakeStatistic()
        {
            // 记录日志
            Log.LogMessage("TokenStatistic", "MakeStatistic", "开始统计数据！");

            // 指令字符串
            string cmdString =
                "SELECT [content] FROM [dbo].[RawContent];";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();

                // 计数器
                int total = 0;
                // 循环处理
                while (reader.Read())
                {
                    // 增加计数
                    total++;
                    // 检查计数
                    if (total % 10000 == 0)
                    {
                        // 打印记录
                        Log.LogMessage(string.Format("{0} items processed !", total));
                    }

                    // 获得内容
                    string content = reader.GetString(0);
                    // 检查结果
                    if (content == null || content.Length <= 0) continue;

                    // 清理内容
                    content = MiscTool.ClearContent(content);
                    // 检查结果
                    if (content == null || content.Length <= 0) continue;

                    //遍历子符串
                    foreach (char value in content)
                    {
                        // 检查关键字
                        if (!tokens.ContainsKey(value)) tokens.Add(value, 0); tokens[value]++;
                    }
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("TokenStatistic", "MakeStatistic", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("TokenStatistic", "MakeStatistic", "数据记录统计完毕！");
        }

        public static void AddToken(char token)
        {
            // 记录日志
            Log.LogMessage("TokenStatistic", "AddToken", "开始执行！");

            // 指令字符串
            string cmdString =
                "IF EXISTS (SELECT TOP 1 * FROM [dbo].[TokenContent] WHERE [content] = @SqlToken) " +
                "BEGIN " +
                "   UPDATE [dbo].[TokenContent] SET [count] = [count] + 1 WHERE [content] = @SqlToken " +
                "END " +
                "ELSE " +
                "BEGIN " +
                "   INSERT INTO [dbo].[TokenContent] " +
                "       ([content], [unicode], [remark]) VALUES (@SqlToken, UNICODE(@SqlToken), @SqlRemark) " +
                "END";

            // 获得注释
            string remark = Token.GetDescription(token);
            // 设置参数
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            // 增加参数
            parameters.Add("SqlRemark", remark);
            parameters.Add("SqlToken", token.ToString());
            // 执行指令
            Common.ExecuteNonQuery(cmdString, parameters);

            // 记录日志
            Log.LogMessage("TokenStatistic", "AddToken", "执行完毕！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int LoadToken(char token)
        {
            // 记录日志
            //Log.LogMessage("TokenStatistic", "LoadToken", "加载数据记录！");

            // 计数器
            int count = -1;
            // 指令字符串
            string cmdString =
                "SELECT TOP 1 [count] " +
                "FROM [dbo].[TokenContent] WHERE [content] = @SqlToken;";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 设置参数
                sqlCommand.Parameters.AddWithValue("SqlToken", token);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();
                // 循环处理
                while (reader.Read())
                {
                    // 设置参数
                    count = reader.GetInt32(0);
                    // 检查数据
                    if (!tokens.ContainsKey(token))
                    {
                        // 增加记录
                        tokens.Add(token, count < 0 ? 0 : count);
                    }
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("TokenStatistic", "LoadToken", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            //Log.LogMessage("TokenStatistic", "LoadToken", "数据记录已加载！");
            // 返回结果
            return count;
        }
    }
}
