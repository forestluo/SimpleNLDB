using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace Misc
{
    public class GammaContent
    {
        public static void CreateTable()
        {
            // 记录日志
            Log.LogMessage("GammaContent", "CreateTable", "创建数据表！");

            // 指令字符串
            string cmdString =
                // 删除索引
                "IF OBJECT_ID('GammaContentGIDIndex') IS NOT NULL " +
                "DROP INDEX dbo.GammaContentGIDIndex; " +
                // 删除之前的表
                "IF OBJECT_ID('GammaContent') IS NOT NULL " +
                "DROP TABLE dbo.GammaContent; " +
                // 创建数据表
                "CREATE TABLE dbo.GammaContent " +
                "( " +
                // 编号
                "[gid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
                // 计数器
                "[count]                INT                     NOT NULL                    DEFAULT 0, " +
                // 相关系数
                "[gamma]                FLOAT                   NOT NULL                    DEFAULT -1, " +
                // 内容长度
                "[length]               INT                     NOT NULL                    DEFAULT 0, " +
                // 内容
                "[content]              NVARCHAR(64)            PRIMARY KEY                 NOT NULL, " +
                // 分割描述
                "[segmentation]         NVARCHAR(256)           NULL, " +
                // 操作标志
                "[operation]            INT                     NOT NULL                    DEFAULT 0, " +
                // 结果状态
                "[operation_result]     INT                     NOT NULL                    DEFAULT 0 " +
                "); " +
                // 创建简单索引
                "CREATE INDEX GammaContentGIDIndex ON dbo.GammaContent (gid); ";

            // 执行指令
            Common.ExecuteNonQuery(cmdString);

            // 记录日志
            Log.LogMessage("GammaContent", "CreateTable", "数据表已创建！");
        }

        public static void AddContent(string content,int count)
        {
            // 指令字符串
            string cmdString =
                "UPDATE [dbo].[GammaContent] " +
                    "SET [count] = [count] + @SqlCount WHERE [content] = @SqlContent; " +
                "IF @@ROWCOUNT <= 0 " +
                "   INSERT INTO [dbo].[GammaContent] ([count], [content], [length]) " +
                "   VALUES(@SqlCount, @SqlContent, LEN(@SqlContent)); ";

            // 参数字典
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            // 加入参数
            parameters.Add("SqlContent", content);
            parameters.Add("SqlCount", count.ToString());
            // 执行指令
            Common.ExecuteNonQuery(cmdString, parameters);
        }

        public static string GetContent(int gid)
        {
            // 记录日志
            //Log.LogMessage("GammaContent", "GetContent", "加载数据记录！");

            // 计数器
            string content = null;
            // 指令字符串
            string cmdString =
                "SELECT TOP 1 [content] " +
                "FROM [dbo].[GammaContent] WHERE [gid] = @SqlGid;";

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
                sqlCommand.Parameters.AddWithValue("SqlGid", gid);
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
                Log.LogMessage("GammaContent", "GetContent", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            //Log.LogMessage("GammaContent", "GetContent", "数据记录已加载！");
            // 返回结果
            return content;
        }
    }
}
