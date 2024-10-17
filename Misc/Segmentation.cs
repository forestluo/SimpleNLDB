using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misc
{
    public class Segmentation
    {
        public int Index { set; get; }
        public int Count { set; get; }
        public int Length { set; get; }
        public string Value { set; get; }
        public double Gamma { set; get; }
        public Segmentation[] Paths { set; get; }

        public static void PrintPaths(Segmentation[] paths)
        {
            // 检查参数
            if (paths == null ||
                paths.Length <= 0) return;

            // 获得内容
            string content = paths[0].Value;
            // 打印原始内容
            Log.LogMessage(string.Format("\tpath.Value = \"{0}\"", paths[0].Value));
            // 循环处理
            foreach (Segmentation path in paths)
            {
                // 检查结果
                if(path.Paths == null || path.Paths.Length <= 0) continue;

                // 打印结果
                Log.LogMessage(string.Format("\tpath.index = {0}", path.Index));
                Log.LogMessage(string.Format("\tpath.gamma = {0}", path.Gamma));
                // 打印切分结果
                for(int i = 0;i < path.Paths.Length; i ++)
                {
                    Log.LogMessage(string.Format("\tpaths.segment[{0}](\"{1}\").count = {2}", i, path.Paths[i].Value, path.Paths[i].Count));
                }
            }
        }

        public static Segmentation[] GetPaths(string content)
        {
            // 检查参数
            if (content == null ||
                content.Length == 0) return null;

            // 生成对象
            List<Segmentation> paths =
                new List<Segmentation>();

            // 获得总数
            int count= GammaStatistic.GetItemCount(content);

            // 循环处理
            for (int i = 0; i <= content.Length; i++)
            {
                // 生成对象
                Segmentation path
                    = new Segmentation();
                // 获得路径
                path.Paths = GetPath(content, i);
                // 检查结果
                if(path.Paths != null && path.Paths.Length > 1)
                {
                    // 检查重复情况
                    if(!IsDuplicated(paths.ToArray(), path))
                    {
                        // 设置索引
                        path.Index = i;
                        // 设置总数
                        path.Count = count;
                        // 设置内容
                        path.Value = content;
                        // 设置长度
                        path.Length = content.Length;
                        // 获得Gamma数值
                        path.Gamma = GetGamma(path.Paths);
                        // 检查结果
                        if (path.Gamma >= 0.0f) paths.Add(path);
                    }
                }
            }
            // 检查内容长度
            // 加入一条缺省路径
            if (content.Length >= 2)
            {
                // 生成对象
                Segmentation path
                    = new Segmentation();
                // 设置索引
                path.Index = -1;
                // 设置总数
                path.Count = count;
                // 设置内容
                path.Value = content;
                // 设置长度
                path.Length = content.Length;
                // 生成Paths
                path.Paths = new Segmentation[content.Length];
                // 循环处理
                for(int i = 0;i < content.Length;i++)
                {
                    // 创建对象
                    path.Paths[i] = new Segmentation();
                    // 设置索引值
                    path.Paths[i].Index = i;
                    // 设置长度
                    path.Paths[i].Length = 1;
                    // 设置内容
                    path.Paths[i].Value = content.Substring(i, 1);
                }
                // 检查重复情况
                if (!IsDuplicated(paths.ToArray(), path))
                {
                    // 获得Gamma数值
                    path.Gamma = GetGamma(path.Paths);
                    // 检查结果
                    if (path.Gamma >= 0.0f) paths.Add(path);
                }
            }
            // 返回结果（逆序）
            return paths.OrderByDescending(seg => seg.Gamma).ToArray();
        }

        public static bool IsDuplicated(Segmentation[] paths, Segmentation path)
        {
            // 检查参数
            if(path == null) return false;
            // 检查参数
            if (paths == null || paths.Length <= 0) return false;
            // 检查参数
            if (path.Paths == null || path.Paths.Length <= 0) return false;

            // 循环处理
            foreach (Segmentation p in paths)
            {
                // 检查参数
                if(p.Paths == null || p.Paths.Length <= 0) continue;
                // 检查参数
                if (p.Paths.Length != path.Paths.Length) continue;

                // 检查内容
                bool flag = true;
                // 循环处理
                for(int i = 0;i < p.Paths.Length && i < path.Paths.Length;i++)
                {
                    if (!string.Equals(p.Paths[i].Value, path.Paths[i].Value)) { flag = false; break; }
                }
                // 检查结果
                if(flag) return true;
            }
            // 返回结果
            return false;
        }

        public static Segmentation[] GetPath(string content, int index)
        {
            // 检查参数
            if (content == null ||
                content.Length == 0) return null;
            // 检查参数
            if (index < 0) return null;
            else if (index > content.Length) return null;

            // 链表
            List<Segmentation> segments = new List<Segmentation>();

            // 往左实施最大逆向匹配法
            for (int i = index - 1; i >= 0; i--)
            {
                for (int j = 0; j <= i; j++)
                {
                    // 长度
                    int length = i - j + 1;
                    // 截取字符串
                    string value =
                        content.Substring(j, length);
                    // 检查结果
                    if (value == null ||
                        value.Length != length) continue;
                    // 查询结果
                    // 先查询Gamma
                    int count = MiscTool.GetCount(value, false);
                    // 检查结果
                    if (count <= 0) continue;
                    // 创建对象
                    Segmentation segment = new Segmentation();
                    // 设置参数
                    segment.Index = j;
                    segment.Count = count;
                    segment.Value = value;
                    segment.Length = length;
                    // 加入链表
                    segments.Add(segment); i = j; break;
                }
            }

            // 逆序
            segments = segments.OrderBy(s => s.Index).ToList();

            // 往右实施最大逆向匹配法
            for (int i = index; i < content.Length; i++)
            {
                for (int j = content.Length - 1; j >= i; j--)
                {
                    // 长度
                    int length = j - i + 1;
                    // 截取字符串
                    string value =
                        content.Substring(i, length);
                    // 检查结果
                    if (value == null ||
                        value.Length != length) continue;
                    // 查询结果
                    int count = MiscTool.GetCount(value, false);
                    // 检查结果
                    if (count <= 0) continue;
                    // 创建对象
                    Segmentation segment = new Segmentation();
                    // 设置参数
                    segment.Index = i;
                    segment.Count = count;
                    segment.Value = value;
                    segment.Length = length;
                    // 加入链表
                    segments.Add(segment); i = j; break;
                }
            }

            // 返回结果
            return segments.ToArray();
        }

        public static double GetGamma(string content)
        {
            // 检查参数
            if (content == null ||
                content.Length <= 0) return -1.0f;

            // 检查参数
            if (content.Length == 1) return 1.0f;
            // 检查参数
            else if (content.Length == 2)
            {
                // 获得数值
                int f = GammaStatistic.GetItemCount(content);
                // 检查结果
                if (f < 0) return -1.0f;
                // 获得数值
                int f1 = GammaStatistic.GetItemCount(content.Substring(0, 1));
                // 检查结果
                if (f1 <= 0) return -1.0f;
                // 获得数值
                int f2 = GammaStatistic.GetItemCount(content.Substring(1, 1));
                // 检查结果
                if (f2 <= 0) return -1.0f;
                // 计算Gamma数值
                return 0.5f * (double)f * (1.0f / f1 + 1.0f / f2);
            }
            // 返回结果
            return -1.0f;
        }

        public static double GetGamma(string[] contents)
        {
            // 检查参数
            if (contents == null ||
                contents.Length <= 0) return -1.0f;

            // 检查长度
            if (contents.Length == 1)
            {
                // 返回结果
                return GetGamma(contents[0]);
            }

            // 对象
            StringBuilder sb = new StringBuilder();

            // 累计值
            double result = 0.0f;
            // 循环处理
            foreach (string content in contents)
            {
                // 增加内容
                sb.Append(content);

                // 获得数值
                int count =
                    GammaStatistic.
                    GetItemCount(content);
                // 检查结果
                if (count <= 0) return -1.0f;
                // 累加结果
                result += 1.0f / (double)count;
            }

            // 获得数值
            int f = GammaStatistic.GetItemCount(sb.ToString());
            // 检查结果
            if (f <= 0) return -1.0f;
            // 返回结果
            return result * (double)f / (double)contents.Length;
        }

        public static double GetGamma(Segmentation[] segmentations)
        {
            // 检查参数
            if (segmentations == null
                || segmentations.Length < 2) return -1.0f;

            // 对象
            StringBuilder sb = new StringBuilder();

            // 累计值
            double result = 0.0f;
            // 循环处理
            foreach (Segmentation segmentation in segmentations)
            {
                // 增加内容
                sb.Append(segmentation.Value);

                // 获得数值
                segmentation.Count =
                    MiscTool.GetCount(segmentation.Value, false);
                // 检查结果
                if (segmentation.Count <= 0) return -1.0f;

                // 累加结果
                result += 1.0f / (double)segmentation.Count;
            }

            // 获得内容
            string content = sb.ToString();
            // 获得数值
            int count = MiscTool.GetCount(content, true);
            // 检查结果
            if (count <= 0) count = 1;
            // 返回结果
            return result * (double)count / (double)segmentations.Length;
        }
    }
}
