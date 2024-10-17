using System.Data.SqlTypes;
using System.Collections.Generic;

public partial class RawContent
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateRawContentTable()
    {
        // 记录日志
        LogTool.LogMessage("RawContent", "CreateRawContent", "创建数据表！");

        // 指令字符串
        string cmdString =
            // 删除之前的索引
            "IF OBJECT_ID('RawContentTIDIndex') IS NOT NULL " +
            "DROP INDEX dbo.RawContentTIDIndex; " +
            "IF OBJECT_ID('RawContentHashIndex') IS NOT NULL " +
            "DROP INDEX dbo.RawContentHashIndex; " +
            // 删除之前的表
            "IF OBJECT_ID('RawContent') IS NOT NULL " +
            "DROP TABLE dbo.RawContent; " +
            // 创建数据表
            "CREATE TABLE dbo.RawContent " +
            "( " +
            // 编号
            "[tid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
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
            "CREATE INDEX RawContentTIDIndex ON dbo.RawContent ([tid]); " +
            "CREATE INDEX RawContentHashIndex ON dbo.RawContent ([hash]); ";

        // 执行指令
        NLDB.ExecuteNonQuery(cmdString);

        // 记录日志
        LogTool.LogMessage("RawContent", "CreateRawContent", "数据表已创建！");
    }
}