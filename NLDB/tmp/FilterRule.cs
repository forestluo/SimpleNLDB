using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class FilterRule
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateFilterRuleTable()
    {
        // 记录日志
        LogTool.LogMessage("FilterRule", "CreateFilterRuleTable", "创建数据表！");

        // 指令字符串
        string cmdString =
            // 删除之前的表
            "IF OBJECT_ID('FilterRule') IS NOT NULL " +
            "DROP TABLE dbo.FilterRule; " +
            // 创建数据表
            "CREATE TABLE dbo.FilterRule " +
            "( " +
            // 编号
            "[fid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
            // 规则
            "[rule]                 NVARCHAR(256)           PRIMARY KEY                 NOT NULL, " +
            // 替代
            "[replace]              NVARCHAR(256), " +
            // 规则要求
            "[requirements]         NVARCHAR(MAX)           NULL " +
            "); ";

        // 执行指令
        NLDB.ExecuteNonQuery(cmdString);

        // 记录日志
        LogTool.LogMessage("FilterRule", "CreateFilterRuleTable", "数据表已创建！");

        // 初始化过滤规则
        InitializeFilterRuleTable();
    }

    public static void AddFilterRule(string strRule, string strReplace)
    {
        // 在数据库中也增加一条规则
        string cmdString =
            "INSERT INTO [dbo].[FilterRule] " +
            "([rule], [replace]) VALUES (@SqlRule, @SqlReplace);";
        // 设置参数
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        // 加入参数
        parameters.Add("SqlRule", strRule);
        parameters.Add("SqlReplace", strReplace);
        // 执行指令
        NLDB.ExecuteNonQuery(cmdString, parameters);
    }

    public static void InitializeFilterRuleTable()
    {
        // 记录日志
        LogTool.LogMessage("FilterRule", "InitializeFilterRuleTable", "初始化数据表！");

        // 初始化过滤规则
        AddFilterRule("[\\u0020]\\s", " ");

        AddFilterRule("<(br|hr|input)((\\s|.)*)/>", " ");
        AddFilterRule("<(img|doc|url|input)((\\s|.)*)>", " ");
        AddFilterRule("<[a-zA-Z]+\\s*[^>]*>(.*?)</[a-zA-Z]+>", "$1");

        AddFilterRule("[']{2}", "'");
        AddFilterRule("[<]{2}", "<");
        AddFilterRule("[>]{2}", ">");
        AddFilterRule("[～]{2}", "～");
        AddFilterRule("[—]{2}", "—");

        AddFilterRule("---|--|—-([^0-9]|\\s)|-—", "—");

        // 非结束符
        AddFilterRule("(、|\\s)+、", "、");

        AddFilterRule("(、|，|\\s)+，", "，");
        AddFilterRule("，(，|、|：|\\s)+", "，");

        AddFilterRule("(、|：|\\s)+：", "：");
        AddFilterRule("：(：|、|，|\\s)+", "：");

        // 结束符
        AddFilterRule("(，|、|；|\\s)+；", "；");
        AddFilterRule("；(；|，|：|。|？|！|\\s)+", "；");

        AddFilterRule("(，|、|：|。|\\s)+。", "。");
        AddFilterRule("。(。|，|：|；|？|！|\\s)+", "。");

        AddFilterRule("(，|、|：|？|\\s)+？", "？");
        AddFilterRule("？(？|，|、|：|！|；|。|\\s)+", "？");

        AddFilterRule("(，|、|：|！|\\s)+！", "！");
        AddFilterRule("！(！|，|、|：|？|；|。|\\s)+", "！");

        AddFilterRule("(\\.\\.\\.|……|。。。|，，，|．．．|～～～|、、、)", "…");

        AddFilterRule("\\s(\\<|\\>|【|】|〈|〉|“|”|‘|’|《|》|\\(|\\)|（|）|…|～|—|、|？|！|；|。|：|，)", "$1");
        AddFilterRule("(\\<|\\>|【|】|〈|〉|“|”|‘|’|《|》|\\(|\\)|（|）|…|～|—|、|？|！|；|。|：|，)\\s", "$1");

        // 记录日志
        LogTool.LogMessage("FilterRule", "InitializeFilterRuleTable", "数据表已经初始化！");
    }
}