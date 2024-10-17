using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

public partial class FilterTool
{
    // ���˹���
    private static List<string[]> rules = new List<string[]>();

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlFilterContent(SqlString sqlValue)
    {
        // ������
        if (sqlValue.IsNull)
            return SqlString.Null;
        // ���ؽ��
        return FilterContent(sqlValue.Value);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt32 SqlReloadFilterRules()
    {
        // ��¼��־
        LogTool.LogMessage("FilterTool", "SqlReloadFilterRules", "������ݼ�¼��");

        // ɾ��֮ǰ���м�¼
        rules.Clear();

        // ��¼��־
        LogTool.LogMessage("FilterTool", "SqlReloadFilterRules", "�������ݼ�¼��");

        // ָ���ַ���
        string cmdString =
            "SELECT [rule], [replace] FROM [dbo].[FilterRule] ORDER BY [fid];";

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
                // ������
                if (reader.IsDBNull(0)) continue;

                // ��������
                string[] rule = new string[2];
                // ��ù���
                rule[0] = reader.GetString(0);
                // ������
                if (rule[0].Length == 0) continue;

                // ����
                rule[1] = " ";
                // ������
                if (!reader.IsDBNull(1))
                {
                    // ������
                    rule[1] = reader.GetString(1);
                }
                // ����һ������
                rules.Add(rule);
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

        // ��¼��־
        LogTool.LogMessage("rules.count = " + rules.Count);
        // ��¼��־
        LogTool.LogMessage("FilterTool", "SqlReloadFilterRules", "���ݼ�¼�Ѽ��أ�");
        // ���ؽ��
        return rules.Count;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static string FilterContent(string strValue)
    {
        // ��־λ
        bool filtered;

        // ��¼��־
        //LogTool.LogMessage("FilterTool", "FilterContent", "��ʼ�������ݣ�");

        do
        {
            // ��ʼ����־λ
            filtered = false;
            // ִ��ѭ��
            foreach (string[] rule in rules)
            {
                // ������
                if (rule[0] == null ||
                    rule[0].Length == 0) continue;
                // ������
                if (rule[1] == null) rule[1] = "";

                // ��¼��־
                //LogTool.LogMessage("rule = " + rule[0]);

                // �滻�ַ���
                string newValue =
                    Regex.Replace(strValue, rule[0], rule[1]);
                // �����
                if (!newValue.Equals(strValue))
                {
                    filtered = true;

                    // ��¼��־
                    //LogTool.LogMessage("\tstrValue = " + strValue);

                    // ������ֵ
                    strValue = newValue;

                    // ��¼��־
                    //LogTool.LogMessage("\treplaced = " + newValue);
                }
            }

        } while (filtered);

        // ��¼��־
        //LogTool.LogMessage("FilterTool", "FilterContent", "���ݹ��˽�����");

        // ���ؽ��
        return strValue;
    }
}
