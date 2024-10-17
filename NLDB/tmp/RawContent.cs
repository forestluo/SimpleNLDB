using System.Data.SqlTypes;
using System.Collections.Generic;

public partial class RawContent
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateRawContentTable()
    {
        // ��¼��־
        LogTool.LogMessage("RawContent", "CreateRawContent", "�������ݱ�");

        // ָ���ַ���
        string cmdString =
            // ɾ��֮ǰ������
            "IF OBJECT_ID('RawContentTIDIndex') IS NOT NULL " +
            "DROP INDEX dbo.RawContentTIDIndex; " +
            "IF OBJECT_ID('RawContentHashIndex') IS NOT NULL " +
            "DROP INDEX dbo.RawContentHashIndex; " +
            // ɾ��֮ǰ�ı�
            "IF OBJECT_ID('RawContent') IS NOT NULL " +
            "DROP TABLE dbo.RawContent; " +
            // �������ݱ�
            "CREATE TABLE dbo.RawContent " +
            "( " +
            // ���
            "[tid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
            // ���ݳ���
            "[length]               INT                     NOT NULL                    DEFAULT 0, " +
            // ��Դ����
            "[source]               NVARCHAR(64)            NULL, " +
            // ����
            "[content]              NVARCHAR(MAX)           NOT NULL, " +
            // ��ϣ��ֵ
            "[hash]                 BINARY(64)              NOT NULL, " +
            // ������־
            "[operation]            INT                     NOT NULL                    DEFAULT 0, " +
            // ���״̬
            "[consequence]          INT                     NOT NULL                    DEFAULT 0 " +
            "); " +
            // ����������
            "CREATE INDEX RawContentTIDIndex ON dbo.RawContent ([tid]); " +
            "CREATE INDEX RawContentHashIndex ON dbo.RawContent ([hash]); ";

        // ִ��ָ��
        NLDB.ExecuteNonQuery(cmdString);

        // ��¼��־
        LogTool.LogMessage("RawContent", "CreateRawContent", "���ݱ��Ѵ�����");
    }
}