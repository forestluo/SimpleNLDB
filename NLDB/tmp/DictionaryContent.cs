using Misc;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;

public partial class DictionaryContent
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateDictionaryTable()
    {
        // ��¼��־
        Log.LogMessage("DictionaryContent", "CreateDictionaryTable", "�������ݱ�");

        // ָ���ַ���
        string cmdString =
            // ɾ������
            "IF OBJECT_ID('DictionaryContentDIDIndex') IS NOT NULL " +
            "DROP INDEX dbo.DictionaryContentDIDIndex; " +
            // ɾ������
            "IF OBJECT_ID('DictionaryContentContentIndex') IS NOT NULL " +
            "DROP INDEX dbo.DictionaryContentContentIndex; " +
            // ɾ������
            "IF OBJECT_ID('DictionaryContent') IS NOT NULL " +
            "DROP TABLE dbo.DictionaryContent; " +
            // �����ֵ��
            "CREATE TABLE dbo.DictionaryContent " +
            "( " +
            // ���
            "[did]                  INT                     IDENTITY(1, 1)               NOT NULL, " +
            // ��������
            "[source]               NVARCHAR(64)            NULL, " +
            // ������
            "[count]                INT                     NOT NULL                     DEFAULT 1, " +
            // ���ݳ���
            "[length]               INT                     NOT NULL                     DEFAULT 0, " +
            // ��������
            "[content]              NVARCHAR(450)           NOT NULL, " +
            // ʹ��
            "[enable]               INT                     NOT NULL                     DEFAULT 0, " +
            // ��ע
            "[remark]               NVARCHAR(MAX)           NULL, " +
            // ������־
            "[operation]            INT                     NOT NULL                     DEFAULT 0, " +
            // ���״̬
            "[consequence]          INT                     NOT NULL                     DEFAULT 0 " +
            "); " +
            // ����������
            "CREATE INDEX DictionaryContentDIDIndex ON dbo.DictionaryContent(did); " +
            "CREATE INDEX DictionaryContentContentIndex ON dbo.DictionaryContent(content); ";

        // ִ��ָ��
        NLDB.ExecuteNonQuery(cmdString);

        // ��¼��־
        Log.LogMessage("DictionaryContent", "CreateDictionaryTable", "���ݱ��Ѵ�����");
    }

    public static int GetMaxLength()
    {
        // ָ���ַ���
        string cmdString =
            "SELECT MAX([length]) FROM [dbo].[DictionaryContent];";

        // ���ò���
        int nLength = -1;
        // �������ݿ�����
        SqlConnection sqlConnection = new SqlConnection("context connection = true");

        try
        {
            // �������ݿ�����
            sqlConnection.Open();
            // ����ָ��
            SqlCommand sqlCommand =
                new SqlCommand(cmdString, sqlConnection);
            // ���������Ķ���
            SqlDataReader reader = sqlCommand.ExecuteReader();
            // ѭ������
            while (reader.Read())
            {
                // ��ó���
                nLength = reader.GetInt32(0);
            }
            // �ر������Ķ���
            reader.Close();
        }
        catch (System.Exception ex) { throw ex; }
        finally
        {
            // ���״̬���ر�����
            if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
        }

        // ���ؽ��
        return nLength;
    }
}
