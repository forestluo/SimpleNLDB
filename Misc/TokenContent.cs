namespace Misc
{
    public class TokenContent
    {
        public static void CreateTable()
        {
            // 记录日志
            Log.LogMessage("TokenContent", "CreateTable", "创建数据表！");

            // 指令字符串
            string cmdString =
                // 删除之前的索引
                "IF OBJECT_ID('TokenContentContentIndex') IS NOT NULL " +
                "DROP INDEX dbo.TokenContentContentIndex; " +
                // 删除之前的表
                "IF OBJECT_ID('TokenContent') IS NOT NULL " +
                "DROP TABLE dbo.TokenContent; " +
                // 创建数据表
                "CREATE TABLE dbo.TokenContent " +
                "( " +
                // 编号
                "[tid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
                // 计数器
                "[count]                INT                     NOT NULL                    DEFAULT 1, " +
                // 内容
                "[content]              NVARCHAR(1)             NOT NULL, " +
                // Unicode编码值
                "[unicode]              INT                     NOT NULL                    DEFAULT 0, " +
                // 备注
                "[remark]               NVARCHAR(32)            NULL, " +
                // 操作标志
                "[operation]            INT                     NOT NULL                    DEFAULT 0, " +
                // 结果状态
                "[consequence]          INT                     NOT NULL                    DEFAULT 0 " +
                "); " +
                // 创建简单索引
                "CREATE INDEX TokenContentContentIndex ON dbo.TokenContent([content]); ";

            // 执行指令
            Common.ExecuteNonQuery(cmdString);

            // 记录日志
            Log.LogMessage("TokenContent", "CreateTable", "数据表已创建！");
        }
    }
}
