using System.Data;
using System.Data.SqlClient;

namespace Misc
{
    public class DictionaryContent
    {
        public static void CreateTable()
        {
            // 记录日志
            Log.LogMessage("DictionaryContent", "CreateTable", "创建数据表！");

            // 指令字符串
            string cmdString =
                // 删除索引
                "IF OBJECT_ID('DictionaryContentDIDIndex') IS NOT NULL " +
                "DROP INDEX dbo.DictionaryContentDIDIndex; " +
                // 删除索引
                "IF OBJECT_ID('DictionaryContentContentIndex') IS NOT NULL " +
                "DROP INDEX dbo.DictionaryContentContentIndex; " +
                // 删除索引
                "IF OBJECT_ID('DictionaryContent') IS NOT NULL " +
                "DROP TABLE dbo.DictionaryContent; " +
                // 创建字典表
                "CREATE TABLE dbo.DictionaryContent " +
                "( " +
                // 编号
                "[did]                  INT                     IDENTITY(1, 1)               NOT NULL, " +
                // 分类描述
                "[source]               NVARCHAR(64)            NULL, " +
                // 计数器
                "[count]                INT                     NOT NULL                     DEFAULT 1, " +
                // 内容长度
                "[length]               INT                     NOT NULL                     DEFAULT 0, " +
                // 内容描述
                "[content]              NVARCHAR(450)           NOT NULL, " +
                // 使能
                "[enable]               INT                     NOT NULL                     DEFAULT 0, " +
                // 备注
                "[remark]               NVARCHAR(MAX)           NULL, " +
                // 操作标志
                "[operation]            INT                     NOT NULL                     DEFAULT 0, " +
                // 结果状态
                "[consequence]          INT                     NOT NULL                     DEFAULT 0 " +
                "); " +
                // 创建简单索引
                "CREATE INDEX DictionaryContentDIDIndex ON dbo.DictionaryContent(did); " +
                "CREATE INDEX DictionaryContentContentIndex ON dbo.DictionaryContent(content); ";

            // 执行指令
            Common.ExecuteNonQuery(cmdString);

            // 记录日志
            Log.LogMessage("DictionaryContent", "CreateTable", "数据表已创建！");
        }

        public static int GetMaxLength()
        {
            // 指令字符串
            string cmdString =
                "SELECT MAX([length]) FROM [dbo].[DictionaryContent];";

            // 设置参数
            int nLength = -1;
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
                // 循环处理
                while (reader.Read())
                {
                    // 获得长度
                    nLength = reader.GetInt32(0);
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("DictionaryContent", "GetMaxLength", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 返回结果
            return nLength;
        }
    }
}
