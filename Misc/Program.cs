// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using Misc;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;

public partial class Program
{
    static void MainCException(string[] args)
    {
        Console.WriteLine("准备创建ExceptionLog表，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("执行当前操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 创建数据表
            ExceptionLog.CreateTable();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainCRaw(string[] args)
    {
        Console.WriteLine("准备创建RawContent表，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("操作被禁止！");

                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 创建数据表
            RawContent.CreateTable();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainCToken(string[] args)
    {
        Console.WriteLine("准备创建TokenContent表，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("执行当前操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 创建数据表
            TokenContent.CreateTable();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainCDictionary(string[] args)
    {
        Console.WriteLine("准备创建DictionaryContent表，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("操作被禁止！");
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 创建数据表
            DictionaryContent.CreateTable();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainCSentence(string[] args)
    {
        Console.WriteLine("准备创建SentenceContent表，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("执行当前操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 创建数据表
            SentenceContent.CreateTable();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainCGamma(string[] args)
    {
        Console.WriteLine("准备创建GammaContent表，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("执行当前操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 创建数据表
            GammaContent.CreateTable();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainSToken(string[] args)
    {
        Console.WriteLine("准备统计Token频次，原表及其数据将被更改！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 加载Token计数
            TokenStatistic.ReloadTokens();
            // 开始统计Token
            TokenStatistic.MakeStatistic();
            // 更新Token计数
            TokenStatistic.UpdateTokens();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainSDictionary(string[] args)
    {
        Console.WriteLine("准备统计Dictionary频次，原表及其数据将被更改！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 加载Token计数
            DictionaryStatistic.ReloadEntries();
            // 开始统计Token
            DictionaryStatistic.MakeStatistic();
            // 更新Token计数
            DictionaryStatistic.UpdateEntries();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainSSentence(string[] args)
    {
        Console.WriteLine("准备统计Sentence频次，原表及其数据将被更改！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 开始统计Sentence
            SentenceStatistic.MakeStatistic();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainSGamma(string[] args)
    {
        Console.WriteLine("准备统计Gamma，原表及其数据将被更改！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 开始统计Gamma
            GammaStatistic.MakeStatistic();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainMGamma(string[] args)
    {
        Console.WriteLine("准备合并生成Gamma，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 开始合并Gamma
            GammaStatistic.Merge();
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainExPhrase(string[] args)
    {
        Console.WriteLine("准备提取Phrase数据，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 加载Token计数
            SentenceContent.CreateTable();
            // 开始统计Token
            SentenceExtractor.ExtractPhrases(true);
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainExSentence(string[] args)
    {
        Console.WriteLine("准备提取Sentence数据，原表及其数据将被删除！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 加载Token计数
            SentenceContent.CreateTable();
            // 开始统计Token
            SentenceExtractor.ExtractSentences(true);
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainRP(string[] args)
    {
        int rid = -1;
        // 尝试解析
        if (!int.TryParse(args[1], out rid))
        {
            Console.WriteLine("请输入正确的参数！"); return;
        }

        // 获得内容
        string content = RawContent.GetContent(rid);
        // 检查内容
        if (content == null || content.Length <= 0)
        {
            Console.WriteLine("RawContent.GetContent返回空字符串！"); return;
        }
        // 打印原始内容
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印原始内容！");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine(content);

        // 清理内容
        content = MiscTool.ClearContent(content);
        // 检查内容
        if (content == null || content.Length <= 0)
        {
            Console.WriteLine("MiscTool.ClearContent返回空字符串！"); return;
        }
        // 打印清洗后的内容
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印ClearContent后的内容！");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine(content);

        // 参数
        string[] output;

        // 切分字符串
        output = SentenceExtractor.SplitContent(content);
        // 打印结果
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印SplitContent后的字符串内容！");
        Console.WriteLine("----------------------------------------");
        // 打印结果
        foreach (string item in output) Console.WriteLine(item);

        // 合并字符串
        output = SentenceExtractor.MergeContent(output);
        // 打印结果
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印MergeContent后的字符串内容！");
        Console.WriteLine("----------------------------------------");
        // 打印结果
        foreach (string item in output) Console.WriteLine(item);

        // 获得句子
        string[][] sentences = SentenceExtractor.GetSentences(output);
        // 打印结果
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印GetSentences最终可以匹配的句子内容！");
        Console.WriteLine("----------------------------------------");
        // 检查结果
        if (sentences == null || sentences.Length <= 0)
        {
            Console.WriteLine("未能找到可以匹配的句式模板！！");
        }
        else
        {
            // 循环处理
            foreach (string[] sentence in sentences)
            {
                // 打印结果
                Console.WriteLine(SentenceExtractor.Concatenate(sentence));
                Console.WriteLine("----------------------------------------");
            }
        }
    }

    static void MainPH(string[] args)
    {
        int rid = -1;
        // 尝试解析
        if (!int.TryParse(args[1], out rid))
        {
            Console.WriteLine("请输入正确的参数！"); return;
        }

        // 获得内容
        string content = RawContent.GetContent(rid);
        // 检查内容
        if (content == null || content.Length <= 0)
        {
            Console.WriteLine("RawContent.GetContent返回空字符串！"); return;
        }
        // 打印原始内容
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印原始内容！");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine(content);

        // 清理内容
        content = MiscTool.ClearContent(content);
        // 检查内容
        if (content == null || content.Length <= 0)
        {
            Console.WriteLine("MiscTool.ClearContent返回空字符串！"); return;
        }
        // 打印清洗后的内容
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印ClearContent后的内容！");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine(content);

        // 参数
        string[] output;

        // 切分字符串
        output = SentenceExtractor.ExSplitContent(content);
        // 打印结果
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印ExSplitContent后的字符串内容！");
        Console.WriteLine("----------------------------------------");
        // 打印结果
        foreach (string item in output) Console.WriteLine(item);
    }

    static void MainSP(string[] args)
    {
        int sid = -1;
        // 尝试解析
        if (!int.TryParse(args[1], out sid))
        {
            Console.WriteLine("请输入正确的参数！"); return;
        }

        // 获得内容
        string content = SentenceContent.GetContent(sid);
        // 检查结果
        if(content == null || content.Length <= 0)
        {
            Console.WriteLine("无法获取内容，请输入正确的参数！"); return;
        }

        // 获得所有路径
        Segmentation[] paths = Segmentation.GetPaths(content);
        // 检查结果
        if(paths == null || paths.Length <= 0)
        {
            Console.WriteLine("无法获取路径，请输入正确的参数！"); return;
        }

        // 打印结果
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印所有路径！");
        Console.WriteLine("----------------------------------------");
        // 打印所有路径
        Segmentation.PrintPaths(paths);
    }

    static void MainGP(string[] args)
    {
        int gid = -1;
        // 尝试解析
        if (!int.TryParse(args[1], out gid))
        {
            Console.WriteLine("请输入正确的参数！"); return;
        }

        // 获得内容
        string content = GammaContent.GetContent(gid);
        // 检查结果
        if (content == null || content.Length <= 0)
        {
            Console.WriteLine("无法获取内容，请输入正确的参数！"); return;
        }

        // 获得所有路径
        Segmentation[] paths = Segmentation.GetPaths(content);
        // 检查结果
        if (paths == null || paths.Length <= 0)
        {
            Console.WriteLine("无法获取路径，请输入正确的参数！"); return;
        }

        // 打印结果
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("打印所有路径！");
        Console.WriteLine("----------------------------------------");
        // 打印所有路径
        Segmentation.PrintPaths(paths);
    }

    static void MainScanSentence(string[] args)
    {
        Console.WriteLine("准备提取Sentence数据，原表及其数据将保留！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 加载Token计数
            SentenceContent.CreateTable();
            // 开始统计Token
            SentenceExtractor.ExtractSentences(false);
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void MainScanPhrase(string[] args)
    {
        Console.WriteLine("准备提取Phrase数据，原表及其数据将保留！");
        Console.WriteLine("确认是否执行（Yes/No）？");
        // 命令行
        string line;
        // 循环处理
        do
        {
            // 读取一行
            line = Console.ReadLine();
            // 检查结果
            if (line == null || line.Length <= 0)
            {
                Console.WriteLine("请输入正确的操作符！");
                Console.WriteLine("确认是否执行（Yes/No）？");
            }
            else
            {
                if (line.Equals("Yes"))
                {
                    Console.WriteLine("开始执行操作！"); break;
                }
                else if (line.Equals("No"))
                {
                    Console.WriteLine("放弃当前操作！"); break;
                }
                else
                {
                    Console.WriteLine("无效的操作符！");
                    Console.WriteLine("请确认是否执行（Yes/No）？");
                }
            }
        } while (true);

        // 检查输入行
        if (line.Equals("Yes"))
        {
            // 开启日志
            Log.SetLog(true);
            // 创建计时器
            Stopwatch watch = new Stopwatch();
            // 开启计时器
            watch.Start();
            // 加载Token计数
            SentenceContent.CreateTable();
            // 开始统计Token
            SentenceExtractor.ExtractPhrases(false);
            // 关闭计时器
            watch.Stop();
            // 打印结果
            Console.WriteLine(string.Format("Time elapsed : {0} ms ", watch.ElapsedMilliseconds));
        }
    }

    static void Main(string[] args)
    {
        // 登记编码器
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // 检查参数
        if (args.Length == 2)
        {
            // 检查参数
            if (args[0].Equals("-p"))
            {
                // 获得正则字符串
                string rule = Quantity.GetRuleString(args[1]);
                // 检查结果
                if (rule != null && rule.Length > 0)
                {
                    Console.WriteLine(rule);
                }
                else
                {
                    Console.WriteLine("Main : invalid arguments !");
                }
            }
            else if (args[0].Equals("-c"))
            {
                if (args[1].Equals("exception")) MainCException(args);
                else if (args[1].Equals("raw")) MainCRaw(args);
                else if (args[1].Equals("token")) MainCToken(args);
                else if (args[1].Equals("dictionary")) MainCDictionary(args);
                else if (args[1].Equals("sentence")) MainCSentence(args);
                else if (args[1].Equals("gamma")) MainCGamma(args);
                else
                {
                    Console.WriteLine("Main : invalid arguments !");
                }
            }
            else if (args[0].Equals("-s"))
            {
                if (args[1].Equals("token")) MainSToken(args);
                else if (args[1].Equals("dictionary")) MainSDictionary(args);
                else if (args[1].Equals("sentence")) MainSSentence(args);
                else if (args[1].Equals("gamma")) MainSGamma(args);
                else
                {
                    Console.WriteLine("Main : invalid arguments !");
                }
            }
            else if (args[0].Equals("-m"))
            {
                if (args[1].Equals("gamma")) MainMGamma(args);
                else
                {
                    Console.WriteLine("Main : invalid arguments !");
                }
            }
            else if (args[0].Equals("-ex"))
            {
                if (args[1].Equals("phrase")) MainExPhrase(args);
                if (args[1].Equals("sentence")) MainExSentence(args);
                else
                {
                    Console.WriteLine("Main : invalid arguments !");
                }
            }
            else if (args[0].Equals("-rp")) MainRP(args);
            else if (args[0].Equals("-ph")) MainPH(args);
            else if (args[0].Equals("-sp")) MainSP(args);
            else if (args[0].Equals("-gp")) MainGP(args);
            else if (args[0].Equals("-scan"))
            {
                if (args[1].Equals("phrase")) MainScanPhrase(args);
                else if (args[1].Equals("sentence")) MainScanSentence(args);
                else
                {
                    Console.WriteLine("Main : invalid arguments !");
                }
            }
            else if (args[0].Equals("-export"))
            {
                if (args[1].Equals("raw")) FileExporter.ExportRaw();
                else if (args[1].Equals("token"))
                {
                    Console.WriteLine("Main : 暂不支持该功能！");
                }
                else if (args[1].Equals("dictionary")) FileExporter.ExportDictionary();
                else if (args[1].Equals("sentence")) FileExporter.ExportSentence();
                else
                {
                    Console.WriteLine("Main : invalid arguments !");
                }
            }
            else
            {
                Console.WriteLine("Main : invalid options !");
            }
        }
        else
        {
            Console.WriteLine("Usage : Misc [options] [arguments]");
            Console.WriteLine("\t（1）常用数量单位：");
            Console.WriteLine("\t-p [$a | $b | $c | $d | $e | $f | $n | $s | $q | $u | $v | $y]");
            Console.WriteLine("\t（2）创建数据表：");
            Console.WriteLine("\t-c [exception | raw | token | dictionary | sentence | gamma]");
            Console.WriteLine("\t（3）随机平均gamma值：");
            Console.WriteLine("\t-g [length = 2, 3, 4, 5, ......]");
            Console.WriteLine("\t（4）数据统计功能：");
            Console.WriteLine("\t-s [token | dictionary | sentence | gamma]");
            Console.WriteLine("\t（5）合并数据：");
            Console.WriteLine("\t-m [gamma]");
            Console.WriteLine("\t（6）提取数据：");
            Console.WriteLine("\t-ex [phrase | sentence]");
            Console.WriteLine("\t（7）测试提取指定内容中的句子：");
            Console.WriteLine("\t-(rp | ph | sp) [id]");
            Console.WriteLine("\t（8）测试从原始内容数据表中提取句子：");
            Console.WriteLine("\t-scan [phrase | sentence]");
            Console.WriteLine("\t（9）输出数据：");
            Console.WriteLine("\t-export [raw | token | dictionary | sentence]");
        }
    }
}

