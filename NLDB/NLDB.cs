using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class NLDB
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString NLDBGetVersion()
    {
        // ���ؽ��
        return new SqlString("NLDB v3.1.0.0");
    }
}
