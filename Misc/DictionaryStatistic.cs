using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Runtime.CompilerServices;

namespace Misc
{
    public class DictionaryStatistic
    {
        // 创建数据字典
        private static Dictionary<string, int> entries = new Dictionary<string, int>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int GetEntryCount(string entry)
        {
            // 返回结果
            return entries.ContainsKey(entry) ?
                entries[entry] : LoadEntry(entry);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ClearEntries()
        {
            // 记录日志
            Log.LogMessage("DictionaryStatistic", "ClearEntries", "清理数据记录！");
            // 清理数据
            entries.Clear();
            // 记录日志
            Log.LogMessage("DictionaryStatistic", "ClearEntries", "数据清理完毕！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ReloadEntries()
        {
            // 记录日志
            Log.LogMessage("DictionaryStatistic", "ReloadEntries", "清理数据记录！");
            // 清理数据
            entries.Clear();

            // 记录日志
            Log.LogMessage("DictionaryStatistic", "ReloadEntries", "加载数据记录！");

            // 指令字符串
            string cmdString =
                "SELECT [content], [count] FROM [dbo].[DictionaryContent];";

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
                        Log.LogMessage(string.Format("{0} entries loaded !", total));
                    }

                    // 获得内容
                    string value = reader.GetString(0);
                    // 检查结果
                    if (value == null || value.Length <= 0) continue;

                    // 获得计数
                    int count = reader.GetInt32(1);
                    // 检查数据
                    if (!entries.ContainsKey(value))
                    {
                        // 增加记录
                        entries.Add(value, count < 0 ? 0 : count);
                    }
                    else
                    {
                        // 更新记录
                        if (count > entries[value]) entries[value] = count;
                    }
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "ReloadEntries", "unexpected exit ！");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("\tentries.count = " + entries.Count);
            // 记录日志
            Log.LogMessage("DictionaryStatistic", "ReloadEntries", "数据记录已加载！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void UpdateEntries()
        {
            // 记录日志
            Log.LogMessage("DictionaryStatistic", "UpdateEntries", "更新数据！");
            Log.LogMessage(string.Format("\tentries.count = {0}", entries.Count));

            // 生成批量处理语句
            string cmdString =
                "UPDATE [dbo].[DictionaryContent] " +
                    "SET [count] = @SqlCount WHERE [content] = @SqlEntry; " +
                "IF @@ROWCOUNT <= 0 " +
                    "INSERT INTO [dbo].[DictionaryContent] " +
                    "([content], [count]) VALUES (@SqlEntry, @SqlCount); ";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "UpdateEntries", "数据连接已开启！");

                // 开启事务处理模式
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "UpdateEntries", "事务处理已开启！");

                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 设置事物处理模式
                sqlCommand.Transaction = sqlTransaction;
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "UpdateEntries", "T-SQL指令已创建！");

                // 计数器
                int total = 0;
                // 遍历参数
                foreach (KeyValuePair<string, int> kvp in entries)
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
                    sqlCommand.Parameters.AddWithValue("SqlEntry", kvp.Key);
                    // 执行指令（尚未执行）
                    sqlCommand.ExecuteNonQuery();

                    // 增加计数
                    total++;
                    // 检查计数
                    if (total % 10000 == 0)
                    {
                        // 打印记录
                        Log.LogMessage(string.Format("{0} entries updated !", total));
                    }
                }
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "UpdateEntries", "批量指令已添加！");

                // 提交事务处理
                sqlTransaction.Commit();
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "UpdateEntries", "批量指令已提交！");
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "UpdateEntries", "unexpected exit ！");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("DictionaryStatistic", "UpdateEntries", "数据记录已更新！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void MakeStatistic()
        {
            // 记录日志
            Log.LogMessage("DictionaryStatistic", "MakeStatistic", "开始统计！");

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
                        Log.LogMessage("DictionaryStatistic",
                            "MakeStatistic",
                            string.Format("{0} items processed !", total));
                    }

                    // 获得内容
                    string content = reader.GetString(0);
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
                                // 增加记录
                                DictionaryStatistic.AddEntry(value);
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
                Log.LogMessage("DictionaryStatistic", "MakeStatistic", "任务已创建！");
                // 等待全部任务结束
                Task.WaitAll(tasks.ToArray());
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "MakeStatistic", "任务全部结束！");
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "MakeStatistic", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("DictionaryStatistic", "MakeStatistic", "统计已完成！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void AddEntry(string entry)
        {
            //检查参数
            if (entry == null || entry.Length <= 0) return;

            //Log.LogMessage(string.Format("\tentry = {0}", entry));

            // 记录日志
            //Log.LogMessage("DictionaryStatistic", "AddEntry", "开始执行！");

            // 记录日志
            //Log.LogMessage("DictionaryStatistic", "AddEntry", "更新数据！");
            // 检查关键字
            if (entries.ContainsKey(entry))
            {
                // 增加计数
                entries[entry]++;
                // 记录日志
                //Log.LogMessage(string.Format("\tentries(\"{0}\") = {1}", entry, entries[entry]));
            }

            // 记录日志
            //Log.LogMessage("DictionaryStatistic", "AddEntry", "执行完毕！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int LoadEntry(string entry)
        {
            // 记录日志
            //Log.LogMessage("DictionaryStatistic", "LoadEntry", "加载数据记录！");

            // 计数器
            int count = -1;
            // 指令字符串
            string cmdString =
                "SELECT TOP 1 [count] " +
                "FROM [dbo].[DictionaryContent] WHERE [content] = @SqlEntry;";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 设置参数
                sqlCommand.Parameters.AddWithValue("SqlEntry", entry);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();
                // 循环处理
                while (reader.Read())
                {
                    // 设置参数
                    count = reader.GetInt32(0);
                    // 检查数据
                    if (!entries.ContainsKey(entry))
                    {
                        // 增加记录
                        entries.Add(entry, count < 0 ? 0 : count);
                    }
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("DictionaryStatistic", "LoadEntry", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            //Log.LogMessage("DictionaryStatistic", "LoadEntry", "数据记录已加载！");
            // 返回结果
            return count;
        }
    }
}
