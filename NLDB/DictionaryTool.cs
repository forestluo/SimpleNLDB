using Misc;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class DictionaryTool
{
    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read)]
    public static SqlInt32 SqlGetEntryCount(SqlString strEntry)
    {
        // ������
        if (strEntry.IsNull) return -1;
        // ����ַ���
        string strValue = strEntry.Value;
        // ������
        if (strValue.Length <= 0) return -1;
        // ���ؽ��
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