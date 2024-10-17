using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                "CREATE INDEX GammaContentOIDIndex ON dbo.GammaContent (oid); ";

            // 执行指令
            Common.ExecuteNonQuery(cmdString);

            // 记录日志
            Log.LogMessage("GammaContent", "CreateTable", "数据表已创建！");
        }
    }
}
