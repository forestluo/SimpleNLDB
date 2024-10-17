using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace Misc
{
    public class SentenceStatistic
    {
        public static void MakeStatistic()
        {
            // 记录日志
            Log.LogMessage("SentenceStatistic", "MakeStatistic", "开始统计！");

            // 短语词典
            Dictionary<string, int> phrases = new Dictionary<string, int>();
            // 加载数据记录
            {
                // 记录日志
                Log.LogMessage("SentenceStatistic", "MakeStatistic", "加载数据记录！");

                // 指令字符串
                string cmdString =
                    "SELECT [content] FROM [dbo].[SentenceContent] WHERE rid = 0;";

                // 创建数据库连接
                SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

                try
                {
                    // 开启数据库连接
                    sqlConnection.Open();
                    // 创建指令
                    SqlCommand sqlCommand =
                        new SqlCommand(cmdString, sqlConnection);
                    // 创建数据阅读器
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    // 计数器
                    int total = 0;
                    // 循环处理
                    while (reader.Read())
                    {
                        // 增加计数
                        total++;
                        // 检查计数
                        if (total % 10000 == 0)
                        {
                            // 打印记录
                            Log.LogMessage(string.Format("{0} phrases loaded !", total));
                        }

                        // 获得内容
                        string value = reader.IsDBNull(0) ? null : reader.GetString(0);
                        // 检查结果
                        if (value == null || value.Length <= 0) continue;
                        // 检查数据
                        if (!phrases.ContainsKey(value)) phrases.Add(value, 0);
                    }
                    // 关闭数据阅读器
                    reader.Close();
                }
                catch (System.Exception ex)
                {
                    // 记录日志
                    Log.LogMessage("SentenceStatistic", "MakeStatistic", "unexpected exit ！");
                    Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
                }
                finally
                {
                    // 检查状态并关闭连接
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }

                // 记录日志
                Log.LogMessage("\tphrases.count = " + phrases.Count);
                // 记录日志
                Log.LogMessage("SentenceStatistic", "MakeStatistic", "数据记录已加载！");
            }
            // 开始统计
            {
                // 计数器
                int taskCount = 0;
                // 同步锁对象
                object lockObject = new object();
                // 任务数组
                List<Task> tasks = new List<Task>();
                // 生成任务控制器
                TaskFactory factory = new TaskFactory(
                    new LimitedConcurrencyLevelTaskScheduler(Common.MAX_THREADS));

                // 指令字符串
                string cmdString =
                    "SELECT [content] FROM [dbo].[RawContent];";

                // 创建数据库连接
                SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

                try
                {
                    // 获得最大长度
                    int length = DictionaryContent.GetMaxLength();
                    // 检查结果
                    if (length <= 0) length = 50;
                    // 记录日志
                    Log.LogMessage(string.Format("\tmax length = {0}", length));

                    // 开启数据库连接
                    sqlConnection.Open();
                    // 创建指令
                    SqlCommand sqlCommand =
                        new SqlCommand(cmdString, sqlConnection);
                    // 创建数据阅读器
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    // 计数器
                    int total = 0;
                    // 循环处理
                    while (reader.Read())
                    {
                        // 增加计数
                        total++;
                        // 检查结果
                        if (total % 10000 == 0)
                        {
                            // 记录日志
                            Log.LogMessage("SentenceStatistic",
                                "MakeStatistic",
                                string.Format("{0} items processed !", total));
                        }

                        // 获得内容
                        string content = reader.IsDBNull(0) ? null : reader.GetString(0);
                        // 检查结果
                        if (content == null || content.Length <= 0) continue;

                        // 清理内容
                        content = MiscTool.ClearContent(content);
                        // 检查结果
                        if (content == null || content.Length <= 0) continue;

                        // 清理线程
                        tasks.RemoveAll(task => { return task.IsCompleted; });

                        // 启动进程
                        tasks.Add(factory.StartNew
                        (() =>
                        {
                            // 执行循环
                            for (int j = 1; j <= length; j++)
                            {
                                // 检查长度
                                for (int i = 0; i <= content.Length - j; i++)
                                {
                                    // 获得子字符串
                                    string value =
                                        content.Substring(i, j);
                                    // 检查结果
                                    if (value == null ||
                                        value.Length != j) continue;
                                    // 保证线程同步
                                    lock(phrases)
                                    {
                                        // 增加记录
                                        if (phrases.ContainsKey(value)) phrases[value]++;
                                    }
                                }
                            }
                            lock (lockObject)
                            {
                                // 增加计数
                                taskCount++;
                                // 检查计数
                                if (taskCount % 10000 == 0)
                                {
                                    // 打印记录
                                    Log.LogMessage(string.Format("{0} tasks finished !", taskCount));
                                }
                            }
                        }));
                    }
                    // 关闭数据阅读器
                    reader.Close();

                    // 记录日志
                    Log.LogMessage("SentenceStatistic", "MakeStatistic", "任务已创建！");
                    // 等待全部任务结束
                    Task.WaitAll(tasks.ToArray());
                    // 记录日志
                    Log.LogMessage("SentenceStatistic", "MakeStatistic", "任务全部结束！");
                }
                catch (System.Exception ex)
                {
                    // 记录日志
                    Log.LogMessage("SentenceStatistic", "MakeStatistic", "unexpected exit !");
                    Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
                }
                finally
                {
                    // 检查状态并关闭连接
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }

                // 记录日志
                Log.LogMessage("SentenceStatistic", "MakeStatistic", "统计已完成！");
            }
            // 更新数据
            {
                // 记录日志
                Log.LogMessage("SentenceStatistic", "MakeStatistic", "更新数据！");
                Log.LogMessage(string.Format("\tphrases.count = {0}", phrases.Count));

                // 生成批量处理语句
                string cmdString =
                    "DECLARE @SqlHash BINARY(64); " +
                    "SELECT @SqlHash = HASHBYTES('SHA2_512', @SqlContent); " +
                    "UPDATE [dbo].[SentenceContent] SET [count] = @SqlCount WHERE [hash] = @SqlHash; ";

                // 创建数据库连接
                SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

                try
                {
                    // 开启数据库连接
                    sqlConnection.Open();
                    // 记录日志
                    Log.LogMessage("SentenceStatistic", "MakeStatistic", "数据连接已开启！");

                    // 创建指令
                    SqlCommand sqlCommand =
                        new SqlCommand(cmdString, sqlConnection);
                    // 记录日志
                    Log.LogMessage("SentenceStatistic", "MakeStatistic", "T-SQL指令已创建！");

                    // 计数器
                    int total = 0;
                    // 遍历参数
                    foreach (KeyValuePair<string, int> kvp in phrases)
                    {
                        // 获得描述
                        int count = kvp.Value;
                        // 记录日志
                        //Log.LogMessage(string.Format("content = {0}", kvp.Key));
                        //Log.LogMessage(string.Format("\tcount = {0}", count));
                        // 清理参数
                        sqlCommand.Parameters.Clear();
                        // 设置参数
                        sqlCommand.Parameters.AddWithValue("SqlCount", count);
                        sqlCommand.Parameters.AddWithValue("SqlContent", kvp.Key);
                        // 执行指令
                        sqlCommand.ExecuteNonQuery();

                        // 增加计数
                        total++;
                        // 检查计数
                        if (total % 10000 == 0)
                        {
                            // 打印记录
                            Log.LogMessage(string.Format("{0} phrases updated !", total));
                        }
                    }
                    // 记录日志
                    Log.LogMessage("SentenceStatistic", "MakeStatistic", "更新指令已提交！");
                }
                catch (System.Exception ex)
                {
                    // 记录日志
                    Log.LogMessage("SentenceStatistic", "MakeStatistic", "unexpected exit ！");
                    Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
                }
                finally
                {
                    // 检查状态并关闭连接
                    if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
                }

                // 记录日志
                Log.LogMessage("SentenceStatistic", "MakeStatistic", "数据记录已更新！");
            }
        }
    }
}
