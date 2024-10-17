using Misc;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class DictionaryTool
{
    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read)]
    public static SqlInt32 SqlGetEntryCount(SqlString strEntry)
    {
        // 检查参数
        if (strEntry.IsNull) return -1;
        // 获得字符串
        string strValue = strEntry.Value;
        // 检查参数
        if (strValue.Length <= 0) return -1;
        // 返回结果
        return DictionaryStatistic.GetEntryCount(strValue);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean SqlClearEntries()
    {
        DictionaryStatistic.ClearEntries();
        return true;
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlReloadEntries()
    {
        DictionaryStatistic.ReloadEntries();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlUpdateEntries()
    {
        DictionaryStatistic.UpdateEntries();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlMakeEntryStatistic()
    {
        DictionaryStatistic.MakeStatistic();
    }
}