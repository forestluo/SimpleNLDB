using Misc;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class SentenceTool
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlExtractPhrases()
    {
        // ���ú���
        SentenceExtractor.ExtractPhrases(true);
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlExtractSentences()
    {
        // ���ú���
        SentenceExtractor.ExtractSentences(true);
    }

    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read,
            FillRowMethodName = "SqlGetQuantities_FillRow",
            TableDefinition = "QIndex int, QLength int, QValue nvarchar(4000)")]
    public static IEnumerable SqlGetQuantities(SqlString sqlContent)
    {
        // ������
        if (sqlContent == null ||
            sqlContent.IsNull) return null;
        // ��ò���
        string value = sqlContent.Value;
        // ������
        if (value == null || value.Length <= 0) return null;
        // ���ؽ��
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
        // ������
        if (sqlContent == null ||
            sqlContent.IsNull) return null;
        // ��ò���
        string value = sqlContent.Value;
        // ������
        if (value == null || value.Length <= 0) return null;
        // ���ؽ��
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
        // ������
        if (sqlContent == null ||
            sqlContent.IsNull) return null;
        // ��ò���
        string value = sqlContent.Value;
        // ������
        if (value == null || value.Length <= 0) return null;
        // ���ؽ��
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
        // ������
        if (sqlContent == null ||
            sqlContent.IsNull) return null;
        // ��ò���
        string value = sqlContent.Value;
        // ������
        if (value == null || value.Length <= 0) return null;
        // ���ؽ��
        return SentenceExtractor.ExSplitContent(value);
    }

    public static void ExSqlSplitContent_FillRow(object logResultObj, out SqlString Value)
    {
        Value = (string)logResultObj;
    }
}
