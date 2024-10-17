using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class FilterRule
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CreateFilterRuleTable()
    {
        // ��¼��־
        LogTool.LogMessage("FilterRule", "CreateFilterRuleTable", "�������ݱ�");

        // ָ���ַ���
        string cmdString =
            // ɾ��֮ǰ�ı�
            "IF OBJECT_ID('FilterRule') IS NOT NULL " +
            "DROP TABLE dbo.FilterRule; " +
            // �������ݱ�
            "CREATE TABLE dbo.FilterRule " +
            "( " +
            // ���
            "[fid]                  INT                     IDENTITY(1, 1)              NOT NULL, " +
            // ����
            "[rule]                 NVARCHAR(256)           PRIMARY KEY                 NOT NULL, " +
            // ���
            "[replace]              NVARCHAR(256), " +
            // ����Ҫ��
            "[requirements]         NVARCHAR(MAX)           NULL " +
            "); ";

        // ִ��ָ��
        NLDB.ExecuteNonQuery(cmdString);

        // ��¼��־
        LogTool.LogMessage("FilterRule", "CreateFilterRuleTable", "���ݱ��Ѵ�����");

        // ��ʼ�����˹���
        InitializeFilterRuleTable();
    }

    public static void AddFilterRule(string strRule, string strReplace)
    {
        // �����ݿ���Ҳ����һ������
        string cmdString =
            "INSERT INTO [dbo].[FilterRule] " +
            "([rule], [replace]) VALUES (@SqlRule, @SqlReplace);";
        // ���ò���
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        // �������
        parameters.Add("SqlRule", strRule);
        parameters.Add("SqlReplace", strReplace);
        // ִ��ָ��
        NLDB.ExecuteNonQuery(cmdString, parameters);
    }

    public static void InitializeFilterRuleTable()
    {
        // ��¼��־
        LogTool.LogMessage("FilterRule", "InitializeFilterRuleTable", "��ʼ�����ݱ�");

        // ��ʼ�����˹���
        AddFilterRule("[\\u0020]\\s", " ");

        AddFilterRule("<(br|hr|input)((\\s|.)*)/>", " ");
        AddFilterRule("<(img|doc|url|input)((\\s|.)*)>", " ");
        AddFilterRule("<[a-zA-Z]+\\s*[^>]*>(.*?)</[a-zA-Z]+>", "$1");

        AddFilterRule("[']{2}", "'");
        AddFilterRule("[<]{2}", "<");
        AddFilterRule("[>]{2}", ">");
        AddFilterRule("[��]{2}", "��");
        AddFilterRule("[��]{2}", "��");

        AddFilterRule("---|--|��-([^0-9]|\\s)|-��", "��");

        // �ǽ�����
        AddFilterRule("(��|\\s)+��", "��");

        AddFilterRule("(��|��|\\s)+��", "��");
        AddFilterRule("��(��|��|��|\\s)+", "��");

        AddFilterRule("(��|��|\\s)+��", "��");
        AddFilterRule("��(��|��|��|\\s)+", "��");

        // ������
        AddFilterRule("(��|��|��|\\s)+��", "��");
        AddFilterRule("��(��|��|��|��|��|��|\\s)+", "��");

        AddFilterRule("(��|��|��|��|\\s)+��", "��");
        AddFilterRule("��(��|��|��|��|��|��|\\s)+", "��");

        AddFilterRule("(��|��|��|��|\\s)+��", "��");
        AddFilterRule("��(��|��|��|��|��|��|��|\\s)+", "��");

        AddFilterRule("(��|��|��|��|\\s)+��", "��");
        AddFilterRule("��(��|��|��|��|��|��|��|\\s)+", "��");

        AddFilterRule("(\\.\\.\\.|����|������|������|������|������|������)", "��");

        AddFilterRule("\\s(\\<|\\>|��|��|��|��|��|��|��|��|��|��|\\(|\\)|��|��|��|��|��|��|��|��|��|��|��|��)", "$1");
        AddFilterRule("(\\<|\\>|��|��|��|��|��|��|��|��|��|��|\\(|\\)|��|��|��|��|��|��|��|��|��|��|��|��)\\s", "$1");

        // ��¼��־
        LogTool.LogMessage("FilterRule", "InitializeFilterRuleTable", "���ݱ��Ѿ���ʼ����");
    }
}