using Misc;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class SentenceTool
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlExtractPhrases()
    {
        // 调用函数
        SentenceExtractor.ExtractPhrases(true);
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlExtractSentences()
    {
        // 调用函数
        SentenceExtractor.ExtractSentences(true);
    }

    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read,
            FillRowMethodName = "SqlGetQuantities_FillRow",
            TableDefinition = "QIndex int, QLength int, QValue nvarchar(4000)")]
    public static IEnumerable SqlGetQuantities(SqlString sqlContent)
    {
        // 检查参数
        if (sqlContent == null ||
            sqlContent.IsNull) return null;
        // 获得参数
        string value = sqlContent.Value;
        // 检查参数
        if (value == null || value.Length <= 0) return null;
        // 返回结果
        return QuantityExtractor.GetQuantities(value);
    }

    public static void SqlGetQuantities_FillRow(object logResultObj, out SqlInt32 Index, out SqlInt32 Length, out SqlString Value)
    {
        Quantity q = (Quantity)logResultObj;
        Index = q.Index;
        Length = q.Length;
        Value = q.Value;
    }

    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read,
            FillRowMethodName = "SqlGetSentences_FillRow",
            TableDefinition = "SentenceValue nvarchar(4000)")]
    public static IEnumerable SqlGetSentences(SqlString sqlContent)
    {
        // 检查参数
        if (sqlContent == null ||
            sqlContent.IsNull) return null;
        // 获得参数
        string value = sqlContent.Value;
        // 检查参数
        if (value == null || value.Length <= 0) return null;
        // 返回结果
        return SentenceExtractor.GetSentences(value);
    }

    public static void SqlGetSentences_FillRow(object logResultObj, out SqlString Value)
    {
        Value = (string)logResultObj;
    }

    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read,
            FillRowMethodName = "SqlSplitContent_FillRow",
            TableDefinition = "ContentValue nvarchar(4000)")]
    public static IEnumerable SqlSplitContent(SqlString sqlContent)
    {
        // 检查参数
        if (sqlContent == null ||
            sqlContent.IsNull) return null;
        // 获得参数
        string value = sqlContent.Value;
        // 检查参数
        if (value == null || value.Length <= 0) return null;
        // 返回结果
        return SentenceExtractor.SplitContent(value);
    }

    public static void SqlSplitContent_FillRow(object logResultObj, out SqlString Value)
    {
        Value = (string)logResultObj;
    }

    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read,
            FillRowMethodName = "ExSqlSplitContent_FillRow",
            TableDefinition = "ExContentValue nvarchar(4000)")]
    public static IEnumerable SqlExSplitContent(SqlString sqlContent)
    {
        // 检查参数
        if (sqlContent == null ||
            sqlContent.IsNull) return null;
        // 获得参数
        string value = sqlContent.Value;
        // 检查参数
        if (value == null || value.Length <= 0) return null;
        // 返回结果
        return SentenceExtractor.ExSplitContent(value);
    }

    public static void ExSqlSplitContent_FillRow(object logResultObj, out SqlString Value)
    {
        Value = (string)logResultObj;
    }
}
