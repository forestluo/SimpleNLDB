using Misc;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class TokenTool
{
    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read)]
    public static SqlInt32 SqlGetTokenCount(SqlString strToken)
    {
        // ������
        if (strToken.IsNull) return -1;
        // ����ַ���
        string strValue = strToken.Value;
        // ������
        if (strValue.Length <= 0) return -1;
        // ���ؽ��
        return TokenStatistic.GetTokenCount(strValue[0]);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean SqlClearTokens()
    {
        TokenStatistic.ClearTokens();
        return SqlBoolean.True;
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlReloadTokens()
    {
        TokenStatistic.ReloadTokens();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlUpdateTokens()
    {
        TokenStatistic.UpdateTokens();
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void SqlMakeTokenStatistic()
    {
        TokenStatistic.MakeStatistic();
    }
}