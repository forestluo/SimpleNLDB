using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Misc
{
    public class RawContent
    {
        public static void CreateTable()
        {
            // 记录日志
            Log.LogMessage("RawContent", "CreateTable", "创建数据表！");

            // 指令字符串
            string cmdString =
                // 删除之前的索引
                "IF OBJECT_ID('RawContentRIDIndex') IS NOT NULL " +
                "DROP INDEX dbo.RawContentRIDIndex; " +
                "IF OBJECT_ID('RawContentHashIndex') IS NOT NULL " +
                "DROP INDEX dbo.RawContentHashIndex; " +
                // 删除之前的表
                "IF OBJECT_ID('RawContent') IS NOT NULL " +
                "DROP TABLE dbo.RawContent; " +
                // 创建数据表
                "CREATE TABLE dbo.RawContent " +
                "( " +
                // 编号
                "[rid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
                // 内容长度
                "[length]               INT                     NOT NULL                    DEFAULT 0, " +
                // 来源描述
                "[source]               NVARCHAR(64)            NULL, " +
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
                "CREATE INDEX RawContentRIDIndex ON dbo.RawContent ([rid]); " +
                "CREATE INDEX RawContentHashIndex ON dbo.RawContent ([hash]); ";

            // 执行指令
            Common.ExecuteNonQuery(cmdString);

            // 记录日志
            Log.LogMessage("RawContent", "CreateTable", "数据表已创建！");
        }

        public static void AddContent(string content, string source)
        {
            // 指令字符串
            string cmdString =
                "DECLARE @SqlHash BINARY(64); " +
                "SELECT @SqlHash = HASHBYTES('SHA2_512', @SqlContent); " +
                "IF (@SqlContent IS NOT NULL AND LEN(@SqlContent) > 0) " +
                " AND " +
                " NOT EXISTS (SELECT TOP 1 * FROM [dbo].[RawContent] WHERE @SqlHash = [hash]) " +
                "BEGIN " +
                "   INSERT INTO [dbo].[RawContent] ([source], [content], [hash], [length]) " +
                "   VALUES(@SqlSource, @SqlContent, @SqlHash, LEN(@SqlContent)) " +
                "END";

            // 参数字典
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            // 加入参数
            parameters.Add("SqlSource", source);
            parameters.Add("SqlContent", content);
            // 执行指令
            Common.ExecuteNonQuery(cmdString, parameters);
        }

        public static string GetContent(int rid)
        {
            // 内容
            string content = null;
            // 记录日志
            Log.LogMessage("RawContent", "GetContent", "查询原始数据！");

            //指令字符串
            string cmdString =
                  "SELECT TOP 1 [content] FROM [dbo].[RawContent] WHERE [rid] = @SqlRid;";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 扩展参数
                sqlCommand.Parameters.AddWithValue("SqlRid", rid);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();
                // 循环处理
                while (reader.Read())
                {
                    // 获得内容
                    content = reader.GetString(0);
                    // 检查内容
                    if (content == null || content.Length <= 0) continue;
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("RawContent", "GetContent", "unexpected exit ！");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("RawContent", "GetContent", "数据查询完毕！");
            // 返回结果
            return content;
        }
    }
}
