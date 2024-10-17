using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace Misc
{
    public class SentenceContent
    {
        public static void CreateTable()
        {
            // 记录日志
            Log.LogMessage("SentenceContent", "CreateTable", "创建数据表！");

            // 指令字符串
            string cmdString =
                // 删除索引
                "IF OBJECT_ID('SentenceContentSIDIndex') IS NOT NULL " +
                "DROP INDEX dbo.SentenceContentSIDIndex; " +
                // 删除索引
                //"IF OBJECT_ID('SentenceContentRIDIndex') IS NOT NULL " +
                //"DROP INDEX dbo.SentenceContentRIDIndex; " +
                // 删除索引
                "IF OBJECT_ID('SentenceContentHashIndex') IS NOT NULL " +
                "DROP INDEX dbo.SentenceContentHashIndex; " +
                // 删除之前的表
                "IF OBJECT_ID('SentenceContent') IS NOT NULL " +
                "DROP TABLE dbo.SentenceContent; " +
                // 创建数据表
                "CREATE TABLE dbo.SentenceContent " +
                "( " +
                // 编号
                "[sid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
                // 编号
                "[rid]                  INT                     NOT NULL                    DEFAULT 0, " +
                // 计数器
                "[count]                INT                     NOT NULL                    DEFAULT 1, " +
                // 内容长度
                "[length]               INT                     NOT NULL                    DEFAULT 0, " +
                // 内容
                "[content]              NVARCHAR(MAX)           NOT NULL, " +
                // 哈希数值
                "[hash]                 BINARY(64)              NOT NULL, " +
                // 操作标志
                "[operation]            INT                     NOT NULL                    DEFAULT 0, " +
                // 结果状态
                "[consequence]          INT                     NOT NULL                    DEFAULT 0 " +
                "); " +
                // 创建简单索引
                "CREATE INDEX SentenceContentSIDIndex ON dbo.SentenceContent (sid); " +
                //"CREATE INDEX SentenceContentRIDIndex ON dbo.SentenceContent (rid); " +
                "CREATE INDEX SentenceContentHashIndex ON dbo.SentenceContent ([hash]); ";

            // 执行指令
            Common.ExecuteNonQuery(cmdString);

            // 记录日志
            Log.LogMessage("SentenceContent", "CreateTable", "数据表已创建！");
        }

        public static void AddPhrase(string phrase, int count)
        {
            // 指令字符串
            string cmdString =
                "DECLARE @SqlHash BINARY(64); " +
                "SELECT @SqlHash = HASHBYTES('SHA2_512', @SqlContent); " +
                "UPDATE [dbo].[SentenceContent] " +
                    "SET [count] = [count] + @SqlCount WHERE [hash] = @SqlHash; " +
                "IF @@ROWCOUNT <= 0 " +
                "   INSERT INTO [dbo].[SentenceContent] ([count], [content], [hash], [length]) " +
                "   VALUES(@SqlCount, @SqlContent, @SqlHash, LEN(@SqlContent)); ";

            // 参数字典
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            // 加入参数
            parameters.Add("SqlContent", phrase);
            parameters.Add("SqlCount", count.ToString());
            // 执行指令
            Common.ExecuteNonQuery(cmdString, parameters);
        }

        public static void AddSentence(string sentence, int rid)
        {
            // 指令字符串
            string cmdString =
                "DECLARE @SqlHash BINARY(64); " +
                "SELECT @SqlHash = HASHBYTES('SHA2_512', @SqlContent); " +
                "UPDATE [dbo].[SentenceContent] " +
                    "SET [count] = [count] + 1 WHERE [hash] = @SqlHash; " +
                "IF @@ROWCOUNT <= 0 " +
                "   INSERT INTO [dbo].[SentenceContent] ([rid], [content], [hash], [length]) " +
                "   VALUES(@SqlRid, @SqlContent, @SqlHash, LEN(@SqlContent)); ";

            // 参数字典
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            // 加入参数
            parameters.Add("SqlContent", sentence);
            parameters.Add("SqlRid", rid.ToString());
            // 执行指令
            Common.ExecuteNonQuery(cmdString, parameters);
        }

        public static int GetCount(string item)
        {
            // 记录日志
            //Log.LogMessage("SentenceContent", "GetCount", "加载数据记录！");

            // 计数器
            int count = 0;
            // 指令字符串
            string cmdString =
                "DECLARE @SqlHash BINARY(64); " +
                "SELECT @SqlHash = HASHBYTES('SHA2_512', @SqlItem); " +
                "SELECT TOP 1 [count] " +
                "FROM [dbo].[SentenceContent] WHERE [rid] = 0 AND [hash] = @SqlHash;";

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
                sqlCommand.Parameters.AddWithValue("SqlItem", item);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();
                // 循环处理
                while (reader.Read())
                {
                    // 设置参数
                    count = reader.GetInt32(0);
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("SentenceContent", "GetCount", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            //Log.LogMessage("SentenceContent", "GetCount", "数据记录已加载！");
            // 返回结果
            return count;
        }

        public static string GetContent(int sid)
        {
            // 记录日志
            //Log.LogMessage("SentenceContent", "GetCount", "加载数据记录！");

            // 计数器
            string content = null;
            // 指令字符串
            string cmdString =
                "SELECT TOP 1 [content] " +
                "FROM [dbo].[SentenceContent] WHERE [rid] = 0 AND [sid] = @SqlSid;";

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
                sqlCommand.Parameters.AddWithValue("SqlSid", sid);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();
                // 循环处理
                while (reader.Read())
                {
                    // 设置参数
                    content = reader.IsDBNull(0) ? null : reader.GetString(0);
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("SentenceContent", "GetCount", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            //Log.LogMessage("SentenceContent", "GetCount", "数据记录已加载！");
            // 返回结果
            return content;
        }
    }
}
