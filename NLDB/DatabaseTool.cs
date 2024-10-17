using Misc;
using System.Data.SqlTypes;
using System.Collections.Generic;

public partial class TableTool
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateExceptionLogTable()
    {
        ExceptionLog.CreateTable();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateRawContentTable()
    {
        RawContent.CreateTable();        
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateTokenContentTable()
    {
        TokenContent.CreateTable();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateDictionaryContentTable()
    {
        DictionaryContent.CreateTable();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateSentenceContentTable()
    {
        SentenceContent.CreateTable();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateGammaContentTable()
    {
        GammaContent.CreateTable();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlCatchExceptionLog(SqlString sqlTips)
    {
        // 检查参数
        if (sqlTips == null || sqlTips.IsNull) return;
        // 获得内容
        string strValue = sqlTips.Value;
        // 检查参数
        if (strValue == null || strValue.Length <= 0) return;
        // 捕捉异常
        ExceptionLog.CatchExceptionLog(strValue);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlClearCorpus(SqlString sqlValue)
    {
        // 检查参数
        if (sqlValue.IsNull)
            return SqlString.Null;
        // 返回结果
        return MiscTool.ClearContent(sqlValue.Value);
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlAddRawContent(SqlString sqlContent, SqlString sqlSource)
    {
        // 检查参数
        if (sqlContent == null || sqlContent.IsNull) return;
        // 调用函数
        RawContent.AddContent(sqlContent.Value,
            (sqlSource == null || sqlSource.IsNull) ? null : sqlSource.Value);
    }
}
