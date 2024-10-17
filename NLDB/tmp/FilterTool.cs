using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

public partial class FilterTool
{
    // 过滤规则
    private static List<string[]> rules = new List<string[]>();

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlFilterContent(SqlString sqlValue)
    {
        // 检查参数
        if (sqlValue.IsNull)
            return SqlString.Null;
        // 返回结果
        return FilterContent(sqlValue.Value);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt32 SqlReloadFilterRules()
    {
        // 记录日志
        LogTool.LogMessage("FilterTool", "SqlReloadFilterRules", "清除数据记录！");

        // 删除之前所有记录
        rules.Clear();

        // 记录日志
        LogTool.LogMessage("FilterTool", "SqlReloadFilterRules", "加载数据记录！");

        // 指令字符串
        string cmdString =
            "SELECT [rule], [replace] FROM [dbo].[FilterRule] ORDER BY [fid];";

        // 创建数据库连接
        SqlConnection sqlConnection = new SqlConnection("context connection = true");

        try
        {
            // 开启数据库连接
            sqlConnection.Open();
            // 创建指令
            SqlCommand sqlCommand =
                new SqlCommand(cmdString, sqlConnection);
            // 创建数据阅读器
            SqlDataReader reader = sqlCommand.ExecuteReader();
            // 循环处理
            while (reader.Read())
            {
                // 检查参数
                if (reader.IsDBNull(0)) continue;

                // 生成数组
                string[] rule = new string[2];
                // 获得规则
                rule[0] = reader.GetString(0);
                // 检查参数
                if (rule[0].Length == 0) continue;

                // 参数
                rule[1] = " ";
                // 检查参数
                if (!reader.IsDBNull(1))
                {
                    // 获得替代
                    rule[1] = reader.GetString(1);
                }
                // 增加一条规则
                rules.Add(rule);
            }
            // 关闭数据阅读器
            reader.Close();
        }
        catch (System.Exception ex) { throw ex; }
        finally
        {
            // 检查状态并关闭连接
            if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
        }

        // 记录日志
        LogTool.LogMessage("rules.count = " + rules.Count);
        // 记录日志
        LogTool.LogMessage("FilterTool", "SqlReloadFilterRules", "数据记录已加载！");
        // 返回结果
        return rules.Count;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public static string FilterContent(string strValue)
    {
        // 标志位
        bool filtered;

        // 记录日志
        //LogTool.LogMessage("FilterTool", "FilterContent", "开始过滤内容！");

        do
        {
            // 初始化标志位
            filtered = false;
            // 执行循环
            foreach (string[] rule in rules)
            {
                // 检查参数
                if (rule[0] == null ||
                    rule[0].Length == 0) continue;
                // 检查参数
                if (rule[1] == null) rule[1] = "";

                // 记录日志
                //LogTool.LogMessage("rule = " + rule[0]);

                // 替换字符串
                string newValue =
                    Regex.Replace(strValue, rule[0], rule[1]);
                // 检查结果
                if (!newValue.Equals(strValue))
                {
                    filtered = true;

                    // 记录日志
                    //LogTool.LogMessage("\tstrValue = " + strValue);

                    // 设置新值
                    strValue = newValue;

                    // 记录日志
                    //LogTool.LogMessage("\treplaced = " + newValue);
                }
            }

        } while (filtered);

        // 记录日志
        //LogTool.LogMessage("FilterTool", "FilterContent", "内容过滤结束！");

        // 返回结果
        return strValue;
    }
}
