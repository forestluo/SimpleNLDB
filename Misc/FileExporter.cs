using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Misc
{
    public class FileExporter
    {
        public static void ExportRaw()
        {
            // 记录日志
            Log.LogMessage("FileExporter", "ExportRaw", "开始输出数据！");

            // 指令字符串
            string cmdString =
                "SELECT [rid], [length], [source], [content] FROM [dbo].[RawContent];";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 创建输出文件
                StreamWriter sw = new StreamWriter("raw.txt", false, Encoding.GetEncoding("GB2312"));

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
                        Log.LogMessage(string.Format("{0} items processed !", total));
                    }

                    // 获得id
                    int rid = reader.GetInt32(0);
                    // 获得长度
                    int length = reader.GetInt32(1);
                    // 获得来源
                    string source = reader.IsDBNull(2) ? null : reader.GetString(2);
                    // 获得内容
                    string content = reader.IsDBNull(3) ? null : reader.GetString(3);
                    // 检查结果
                    if (content == null || content.Length <= 0) continue;

                    // 清理内容
                    content = Blankspace.ClearInvisible(content);
                    // 检查结果
                    if (content == null || content.Length <= 0) continue;

                    // 写入文件
                    sw.WriteLine(string.Format("{0},{1},{2},{3}", rid, length,
                        (source != null && source.Length > 0) ? source : "", content));
                }
                // 关闭数据阅读器
                reader.Close();

                // 关闭文件流
                sw.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("FileExporter", "ExportRaw", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("FileExporter", "ExportRawContent", "数据输出完毕！");
        }

        public static void ExportDictionary()
        {
            // 记录日志
            Log.LogMessage("FileExporter", "ExportDictionary", "开始输出数据！");

            // 指令字符串
            string cmdString =
                "SELECT [did], [length], [source], [content], [remark] FROM [dbo].[DictionaryContent];";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 创建输出文件
                StreamWriter sw = new StreamWriter("dictionary.txt", false, Encoding.GetEncoding("GB2312"));

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
                        Log.LogMessage(string.Format("{0} items processed !", total));
                    }

                    // 获得id
                    int did = reader.GetInt32(0);
                    // 获得长度
                    int length = reader.GetInt32(1);
                    // 获得来源
                    string source = reader.IsDBNull(2) ? null : reader.GetString(2);
                    // 获得内容
                    string content = reader.IsDBNull(3) ? null : reader.GetString(3);
                    // 获得注释
                    string remark = reader.IsDBNull(4) ? null : reader.GetString(4);
                    // 检查结果
                    if (content == null || content.Length <= 0) continue;

                    // 写入文件
                    sw.WriteLine(string.Format("{0},{1},{2},{3},{4}", did, length,
                        (source != null && source.Length > 0) ? source : "",
                        content, (remark != null && remark.Length > 0) ? remark : ""));
                }
                // 关闭数据阅读器
                reader.Close();

                // 关闭文件流
                sw.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("FileExporter", "ExportDictionary", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("FileExporter", "ExportDictionary", "数据输出完毕！");
        }

        public static void ExportSentence()
        {
            // 记录日志
            Log.LogMessage("FileExporter", "ExportSentence", "开始输出数据！");

            // 指令字符串
            string cmdString =
                "SELECT [sid], [rid], [length], [content] FROM [dbo].[SentenceContent];";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
            {
                // 创建输出文件
                StreamWriter sw = new StreamWriter("sentence.txt", false, Encoding.GetEncoding("GB2312"));

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
                        Log.LogMessage(string.Format("{0} items processed !", total));
                    }

                    // 获得id
                    int did = reader.GetInt32(0);
                    // 获得id
                    int rid = reader.GetInt32(1);
                    // 获得长度
                    int length = reader.GetInt32(2);
                    // 获得内容
                    string content = reader.IsDBNull(3) ? null : reader.GetString(3);
                    // 检查结果
                    if (content == null || content.Length <= 0) continue;

                    // 写入文件
                    sw.WriteLine(string.Format("{0},{1},{2},{3}", did, rid, length, content));
                }
                // 关闭数据阅读器
                reader.Close();

                // 关闭文件流
                sw.Close();
            }
            catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("FileExporter", "ExportSentence", "unexpected exit !");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("FileExporter", "ExportSentence", "数据输出完毕！");
        }
    }
}
