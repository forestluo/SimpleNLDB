using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

public partial class TokenContent
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateTokenContentTable()
    {
        // ��¼��־
        LogTool.LogMessage("TokenContent", "CreateTokenContent", "�������ݱ�");

        // ָ���ַ���
        string cmdString =
            // ɾ��֮ǰ������
            "IF OBJECT_ID('TokenContentContentIndex') IS NOT NULL " +
            "DROP INDEX dbo.TokenContentContentIndex; " +
            // ɾ��֮ǰ�ı�
            "IF OBJECT_ID('TokenContent') IS NOT NULL " +
            "DROP TABLE dbo.TokenContent; " +
            // �������ݱ�
            "CREATE TABLE dbo.TokenContent " +
            "( " +
            // ���
            "[tid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
            // ������
            "[count]                INT                     NOT NULL                    DEFAULT 1, " +
            // ����
            "[content]              NVARCHAR(1)             NOT NULL, " +
            // Unicode����ֵ
            "[unicode]              INT                     NOT NULL                    DEFAULT 0, " +
            // ��ע
            "[remark]               NVARCHAR(32)            NULL, " +
            // ������־
            "[operation]            INT                     NOT NULL                    DEFAULT 0, " +
            // ���״̬
            "[consequence]          INT                     NOT NULL                    DEFAULT 0 " +
            "); " +
            // ����������
            "CREATE INDEX TokenContentContentIndex ON dbo.TokenContent([content]); ";

        // ִ��ָ��
        NLDB.ExecuteNonQuery(cmdString);

        // ��¼��־
        LogTool.LogMessage("TokenContent", "CreateTokenContent", "���ݱ��Ѵ�����");
    }
}