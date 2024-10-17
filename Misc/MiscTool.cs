using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Misc
{
    public class MiscTool
    {
        public static int GetCount(string key, bool ex)
        {
            // 先查询Gamma
            int count = GammaStatistic.GetItemCount(key);
            // 检查结果
            if (count <= 0)
            {
                // 检查长度
                if (key.Length == 1)
                {
                    // 查询字符表
                    count = TokenStatistic.GetTokenCount(key[0]);
                }
                else if (ex)
                {
                    // 去查字典
                    count = DictionaryStatistic.GetEntryCount(key);
                    // 去查短语表
                    if (count <= 0) count = SentenceContent.GetCount(key);
                }
            }
            // 返回结果
            return count;
        }

        public static string ClearContent(string content)
        {
            // 调整字符串
            content = FilterReplacement.AdjustContent(content);

            // 设置初始值
            int length = content.Length;
            // 循环处理
            while (true)
            {
                // 全角字符转半角字符
                content = Chinese.NarrowConvert(content);
                // 清理不可见字符
                content = Blankspace.ClearInvisible(content);

                // 循环处理
                int len;
                do
                {
                    // 获得字符串长度
                    len = content.Length;
                    // 转义XML
                    content = XML.XMLUnescape(content);

                } while (content.Length < len);

                // 检查结果
                if (content.Length >= length) break; else length = content.Length;
            }
            // 过滤字符串，替代不规则内容
            content = FilterReplacement.FilterContent(content);

            // 半角转全角
            content = Chinese.WideConvert(content);
            // 过滤字符串，替代不规则内容
            content = FilterReplacement.FilterContent(content);

            // 全角转半角
            content = Chinese.NarrowConvert(content);
            // 部分半角转全角
            content = Punctuation.WideConvert(content);
            // 返回结果
            return content.Trim();
        }
    }
}
