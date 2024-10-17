using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using System.Runtime.CompilerServices;

namespace Misc
{
    public class GammaStatistic
    {
        // 创建数据字典
        private static Dictionary<string, Segmentation> items = new Dictionary<string, Segmentation>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int GetItemCount(string entry)
        {
            // 对象
            Segmentation segmentation = null;
            // 返回结果
            if (!items.ContainsKey(entry))
            {
                // 尝试加载
                segmentation = LoadItem(entry);
            }
            else segmentation = items[entry];
            // 返回结果
            return segmentation != null ? segmentation.Count : 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static double GetItemGamma(string entry)
        {
            // 对象
            Segmentation segmentation = null;
            // 返回结果
            if (!items.ContainsKey(entry))
            {
                // 尝试加载
                segmentation = LoadItem(entry);
            }
            else segmentation = items[entry];
            // 返回结果
            return segmentation != null ? segmentation.Gamma : 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ClearItems()
        {
            // 记录日志
            Log.LogMessage("GammaStatistic", "ClearItems", "清理数据记录！");
            // 清理数据
            items.Clear();
            // 记录日志
            Log.LogMessage("GammaStatistic", "ClearItems", "数据清理完毕！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void MakeStatistic()
        {
            // 记录日志
            Log.LogMessage("GammaStatistic", "MakeStatistic", "开始统计！");

            // 记录日志
            Log.LogMessage("GammaStatistic", "MakeStatistic", "重新加载数据！");
            // 重新加载数据
            ReloadItems();

            // 记录日志
            Log.LogMessage("GammaStatistic", "MakeStatistic", "抹除原来的数据！");
            // 抹除之前的计算数值
            Common.ExecuteNonQuery("UPDATE [dbo].[GammaContent] SET [gamma] = -1.0, [segmentation] = NULL;");

            // 记录日志
            Log.LogMessage("GammaStatistic", "MakeStatistic", "重新计算数据！");

            // 对象
            List<Segmentation> segmentations = new List<Segmentation>();
            // 生成链表
            foreach (KeyValuePair<string, Segmentation> kvp in items) segmentations.Add(kvp.Value);
            // 按照长度排序
            segmentations = segmentations.OrderBy(segment => segment.Length).ToList();

            // 计数器
            int total = 0;
            // 循环处理
            foreach (Segmentation segmentation in segmentations)
            {
                // 增加计数
                total++;
                // 检查结果
                if (total % 10000 == 0)
                {
                    // 记录日志
                    Log.LogMessage("GammaStatistic",
                        "MakeStatistic",
                        string.Format("{0} items processed !", total));
                }

                // 检查结果
                if (segmentation.Value.Length == 1)
                {
                    // 设置初始值
                    segmentation.Gamma = 1.0f; segmentation.Paths = null;
                }
                else
                {
                    // 获得所有路径
                    Segmentation[] paths =
                        Segmentation.GetPaths(segmentation.Value);
                    // 检查结果
                    if (paths != null && paths.Length >= 1)
                    {
                        // 获得Gamma数值和子路径
                        segmentation.Gamma = paths[0].Gamma; segmentation.Paths = paths[0].Paths;
                    }
                }
            }
            // 清理数据
            segmentations.Clear();

            // 记录日志
            Log.LogMessage("GammaStatistic", "MakeStatistic", "保存所有数据！");
            // 加载所有数据
            UpdateItems();

            // 记录日志
            Log.LogMessage("GammaStatistic", "MakeStatistic", "统计执行完毕！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void ReloadItems()
        {
            // 记录日志
            Log.LogMessage("GammaStatistic", "ReloadItems", "清理数据记录！");
            // 清理数据
            items.Clear();

            // 记录日志
            Log.LogMessage("GammaStatistic", "ReloadItems", "加载数据记录！");

            // 指令字符串
            string cmdString =
                "SELECT [gid], [count], [content], [gamma], [segmentation] FROM [dbo].[GammaContent];";

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
                        // 记录日志
                        Log.LogMessage("GammaStatistic", "ReloadItems",
                            string.Format("{0} items loaded !", total));
                    }

                    // 获得内容
                    string value = reader.IsDBNull(2) ? null : reader.GetString(2);
                    // 检查结果
                    if (value == null || value.Length <= 0) continue;

                    // 生成对象
                    Segmentation segmentation = new Segmentation();
                    // 设置参数值
                    segmentation.Index = reader.GetInt32(0);
                    segmentation.Length = value.Length;
                    segmentation.Value = value;
                    segmentation.Count = reader.GetInt32(1);
                    segmentation.Gamma = reader.GetDouble(3);
                    // 获得路径描述
                    string description =
                        reader.IsDBNull(4) ? null : reader.GetString(4);
                    // 检查路径描述
                    if(description != null && description.Length > 0)
                    {
                        // 分割路径成分
                        string[] descriptions = description.Split('|');

                        // 索引值
                        int i = 0;
                        // 链表
                        List<Segmentation> path = new List<Segmentation>();
                        // 生成子路径
                        foreach(string desc in descriptions)
                        {
                            // 生成对象
                            Segmentation subSegmenation = new Segmentation();
                            // 设置数值
                            subSegmenation.Index = i;
                            subSegmenation.Value = desc;
                            subSegmenation.Length = desc.Length;
                            // 增加索引
                            i += desc.Length;
                            // 增加对象
                            path.Add(subSegmenation);
                        }
                        // 设置子路径
                        segmentation.Paths = path.ToArray();
                    }

                    // 检查数据
                    if (!items.ContainsKey(value)) items.Add(value, segmentation);
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("GammaStatistic", "ReloadItems", "unexpected exit ！");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("\tentries.count = " + items.Count);
            // 记录日志
            Log.LogMessage("GammaStatistic", "ReloadItems", "数据记录已加载！");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static Segmentation LoadItem(string item)
        {
            // 记录日志
            //Log.LogMessage("GammaStatistic", "LoadItem", "加载数据记录！");

            // 计数器
            int count = -1;
            // 指令字符串
            string cmdString =
                "SELECT TOP 1 " + 
                "[gid], [count], [content], [gamma], [segmentation] " +
                "FROM [dbo].[GammaContent] WHERE [content] = @SqlItem;";

            // 对象
            Segmentation segmentation = null;
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
                sqlCommand.Parameters.AddWithValue("SqlItem", item);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();
                // 循环处理
                while (reader.Read())
                {
                    // 获得内容
                    string value = reader.IsDBNull(2) ? null : reader.GetString(2);
                    // 检查结果
                    if (value == null || value.Length <= 0) continue;

                    // 生成对象
                    segmentation = new Segmentation();
                    // 设置参数值
                    segmentation.Index = reader.GetInt32(0);
                    segmentation.Length = value.Length;
                    segmentation.Value = value;
                    segmentation.Count = reader.GetInt32(1);
                    segmentation.Gamma = reader.GetDouble(3);
                    // 获得路径描述
                    string description =
                        reader.IsDBNull(4) ? null : reader.GetString(4);
                    // 检查路径描述
                    if (description != null && description.Length > 0)
                    {
                        // 分割路径成分
                        string[] descriptions = description.Split('|');

                        // 索引值
                        int i = 0;
                        // 链表
                        List<Segmentation> path = new List<Segmentation>();
                        // 生成子路径
                        foreach (string desc in descriptions)
                        {
                            // 生成对象
                            Segmentation subSegmenation = new Segmentation();
                            // 设置数值
                            subSegmenation.Index = i;
                            subSegmenation.Value = desc;
                            subSegmenation.Length = desc.Length;
                            // 增加索引
                            i += desc.Length;
                            // 增加对象
                            path.Add(subSegmenation);
                        }
                        // 设置子路径
                        segmentation.Paths = path.ToArray();
                    }

                    // 检查数据
                    if (!items.ContainsKey(value)) items.Add(value, segmentation);
                }
                // 关闭数据阅读器
                reader.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("GammaStatistic", "LoadItem", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            //Log.LogMessage("GammaStatistic", "LoadItem", "数据记录已加载！");
            // 返回结果
            return segmentation;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void UpdateItems()
        {
            // 记录日志
            Log.LogMessage("GammaStatistic", "UpdateItems", "更新数据！");
            Log.LogMessage(string.Format("\titems.count = {0}", items.Count));

            // 生成批量处理语句
            string cmdString =
                "UPDATE [dbo].[GammaContent] " +
                    "SET [count] = @SqlCount, [gamma] = @SqlGamma, " +
                    "[segmentation] = @SqlSegmentation WHERE [content] = @SqlEntry; " +
                "IF @@ROWCOUNT <= 0 " +
                    "INSERT INTO [dbo].[GammaContent] " +
                    "([content], [count], [length], [gamma], [segmentation]) " +
                    "VALUES (@SqlEntry, @SqlCount, LEN(@SqlEntry), @SqlGamma, @SqlSegmentation); ";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 开启数据库连接
                sqlConnection.Open();
                // 记录日志
                Log.LogMessage("GammaStatistic", "UpdateItems", "数据连接已开启！");

                // 开启事务处理模式
                SqlTransaction sqlTransaction =
                    sqlConnection.BeginTransaction();
                // 记录日志
                Log.LogMessage("GammaStatistic", "UpdateItems", "事务处理已开启！");

                // 创建指令
                SqlCommand sqlCommand =
                    new SqlCommand(cmdString, sqlConnection);
                // 设置事物处理模式
                sqlCommand.Transaction = sqlTransaction;
                // 记录日志
                Log.LogMessage("GammaStatistic", "UpdateItems", "T-SQL指令已创建！");

                // 计数器
                int total = 0;
                // 遍历参数
                foreach (KeyValuePair<string, Segmentation> kvp in items)
                {
                    // 获得对象
                    Segmentation segmentation = kvp.Value;
                    // 清理参数
                    sqlCommand.Parameters.Clear();
                    // 设置参数
                    sqlCommand.Parameters.AddWithValue("SqlEntry", kvp.Key);
                    sqlCommand.Parameters.AddWithValue("SqlCount", segmentation.Count);
                    sqlCommand.Parameters.AddWithValue("SqlGamma", segmentation.Gamma);
                    // 检查参数
                    if(segmentation.Paths == null || segmentation.Paths.Length <= 0)
                    {
                        // 设置参数
                        sqlCommand.Parameters.AddWithValue("SqlSegmentation", DBNull.Value);
                    }
                    else
                    {
                        // 创建对象
                        StringBuilder sb = new StringBuilder();
                        // 循环处理
                        foreach(Segmentation seg in segmentation.Paths)
                        {
                            if (seg.Value != null && seg.Value.Length > 0)
                            {
                                sb.Append(String.Format("|{0}",seg.Value));
                            }
                        }
                        // 设置参数
                        sqlCommand.Parameters.AddWithValue("SqlSegmentation", sb.ToString().Substring(1));
                    }

                    // 执行指令（尚未执行）
                    sqlCommand.ExecuteNonQuery();

                    // 增加计数
                    total++;
                    // 检查计数
                    if (total % 10000 == 0)
                    {
                        // 打印记录
                        Log.LogMessage("GammaStatistic", "UpdateItems",
                                string.Format("{0} items updated !", total));
                    }
                }
                // 记录日志
                Log.LogMessage("GammaStatistic", "UpdateItems", "批量指令已添加！");

                // 提交事务处理
                sqlTransaction.Commit();
                // 记录日志
                Log.LogMessage("GammaStatistic", "UpdateItems", "批量指令已提交！");
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("GammaStatistic", "UpdateItems", "unexpected exit ！");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("GammaStatistic", "UpdateItems", "数据记录已更新！");
        }

        public static void Merge()
        {
            // 记录日志
            Log.LogMessage("GammaStatistic", "Merge", "开始合并！");

            // 记录日志
            Log.LogMessage("GammaStatistic", "Merge", "装载Token！");
            // 装载Tokens
            TokenStatistic.ReloadTokens();
            // 记录日志
            Log.LogMessage("GammaStatistic", "Merge", "装载Dictionary！");
            // 装栽Dictionay
            DictionaryStatistic.ReloadEntries();

            // 获得最大长度
            int length = DictionaryContent.GetMaxLength();
            // 检查结果
            if (length <= 0) length = 64;
            // 记录日志
            Log.LogMessage(string.Format("\tmax length = {0}", length));

            // 循环处理
            for (int i = 1; i <= length; i++)
            {
                // 记录日志
                Log.LogMessage("GammaStatistic", "Merge", "开始合并！");
                // 合并
                Merge(i);
                // 记录日志
                Log.LogMessage("GammaStatistic", "Merge", "合并结束！");
            }

            // 记录日志
            Log.LogMessage("GammaStatistic", "Merge", "合并已完成！");
        }

        private static void Merge(int length)
        {
            // 记录日志
            Log.LogMessage("GammaStatistic", "Merge", "合并数据记录！");
            // 记录日志
            Log.LogMessage("\tlength = " + length);

            // 指令字符串
            string cmdString =
                "SELECT [content], [count] " +
                "FROM [dbo].[SentenceContent] " +
                "WHERE [rid] = 0 AND [length] = @SqlLength;";

            // 计数器
            int taskCount = 0;
            // 同步锁对象
            object lockObject = new object();
            // 任务数组
            List<Task> tasks = new List<Task>();
            // 生成任务控制器
            TaskFactory factory = new TaskFactory(
                new LimitedConcurrencyLevelTaskScheduler(Common.MAX_THREADS));

            // 计数器
            int total = 0;
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
                sqlCommand.Parameters.AddWithValue("SqlLength", length);
                // 创建数据阅读器
                SqlDataReader reader = sqlCommand.ExecuteReader();

                // 循环处理
                while (reader.Read())
                {
                    // 增加计数
                    total++;
                    // 检查计数
                    if (total % 10000 == 0)
                    {
                        // 打印记录
                        Log.LogMessage(string.Format("{0} items processed !", total));
                    }

                    // 获得内容
                    string value = reader.IsDBNull(0) ? null : reader.GetString(0);
                    // 检查结果
                    if (value == null || value.Length <= 0) continue;

                    // 获得计数
                    int count = reader.GetInt32(1);

                    // 清理线程
                    tasks.RemoveAll(task => { return task.IsCompleted; });

                    // 启动进程
                    tasks.Add(factory.StartNew
                    (() =>
                    {
                        // 获得计数
                        int newCount = 0;
                        // 检查长度
                        if (length <= 1)
                        {
                            // 获得计数
                            newCount = TokenStatistic.GetTokenCount(value[0]);
                        }
                        else
                        {
                            // 获得计数
                            newCount = DictionaryStatistic.GetEntryCount(value);
                        }

                        // 检查结果
                        // 结果为-1表明字典中不存在
                        if (newCount > 0)
                        {
                            // 检查计数结果
                            if (count != newCount)
                            {
                                // 打印记录
                                Log.LogMessage(
                                    string.Format("内容({0})计数({1})与字典计数({2})不相等 !", value, count, newCount));
                                // 选择较大者
                                count = count > newCount ? count : newCount;
                            }
                            // 增加记录
                            GammaContent.AddContent(value, count);
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
                Log.LogMessage("GammaStatistic", "MakeStatistic", "任务已创建！");
                // 等待全部任务结束
                Task.WaitAll(tasks.ToArray());
                // 记录日志
                Log.LogMessage("GammaStatistic", "MakeStatistic", "任务全部结束！");
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("GammaStatistic", "Merge", "unexpected exit ！");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("\titems.count = " + total);
            // 记录日志
            Log.LogMessage("GammaStatistic", "Merge", "数据记录已合并！");
        }
    }
}
