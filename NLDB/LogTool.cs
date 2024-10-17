using Misc;
using System.Collections;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Runtime.CompilerServices;

public partial class LogTool
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean SqlSetLog(SqlBoolean sqlLog)
    {
        Log.
        SetLog(sqlLog.Value);
        return SqlBoolean.True;
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean SqlClearLogs()
    {
        Log.ClearLogs();
        return SqlBoolean.True;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    [Microsoft.SqlServer.Server.SqlFunction
        (DataAccess = DataAccessKind.Read,
            FillRowMethodName = "SqlGetLogs_FillRow",
            TableDefinition = "LogValue nvarchar(4000)")]
    public static IEnumerable SqlGetLogs()
    {
        // ·µ»Ø½á¹û
        return Log.GetLogs();
    }

    public static void SqlGetLogs_FillRow(object logResultObj, out SqlString Value)
    {
        Value = (string)logResultObj;
    }
}