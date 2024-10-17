using Misc;
using System.Text;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class SentenceTemplate
{
    // 句式模板
	// 不要调整顺序！！！
    private readonly static
        string[][] TEMPLATES =
        {
			new string[] {"“$", "$a", "(，|：|。|；|？|！)*”$", "$b", "(，|：|。|；|？|！)+“$", "$c", "(。|；|？|！)+”$"},
			new string[] {"‘$", "$a", "(，|：|。|；|？|！)*’$", "$b", "(，|：|。|；|？|！)+‘$", "$c", "(。|；|？|！)+’$"},

			// 符号嵌套
			new string[] {"“$", "$a", "(，|：|。|；|？|！)+‘$", "$b", "(。|；|？|！)+’$", "$c", "(。|；|？|！)+”$"},
			new string[] {"“$", "$a", "(，|：|。|；|？|！)+‘$", "$b", "(。|；|？|！)+’”$"},
			new string[] {"“‘$", "$a", "(，|：|。|；|？|！)+’$", "$b", "(。|；|？|！)+”$"},

			// 常见符号
			new string[] {"$a", "(：)$", "$b", "(。|；|？|！)+$"},
			new string[] {"$a", "(：)?“$", "$b", "(。|；|？|！)+”$"},
			new string[] {"$a", "(：)?‘$", "$b", "(。|；|？|！)+’$"},
			new string[] {"$a", "(：)?（$", "$b", "(。|；|？|！)+）$"},

			new string[] {"“$", "$a", "(，|：|。|；|？|！)+”$", "$b", "(。|；|？|！)+$"},
			new string[] {"‘$", "$a", "(，|：|。|；|？|！)+’$", "$b", "(。|；|？|！)+$"},

			// 符号嵌套
			new string[] {"“‘$", "$a", "(，|：|。|；|？|！)+’”$"},
			new string[] {"“（$", "$a", "(，|：|。|；|？|！)+）”$"},

			// 常见符号
			new string[] {"“$", "$a", "(，|：|。|；|？|！)+”$"},
			new string[] {"‘$", "$a", "(，|：|。|；|？|！)+’$"},
			new string[] {"（$", "$a", "(，|：|。|；|？|！)+）$"},
			new string[] {"《$", "$a", "(，|：|。|；|？|！)+》$"},
			new string[] {"【$", "$a", "(，|：|。|；|？|！)+】$"},
			// 比较少见
			new string[] {"「$", "$a", "(，|：|。|；|？|！)+」$"},
			new string[] {"『$", "$a", "(，|：|。|；|？|！)+』$"},
			new string[] {"〖$", "$a", "(，|：|。|；|？|！)+〗$"},
			new string[] {"〝$", "$a", "(，|：|。|；|？|！)+〞$"},

			// 最简单的句子
			new string[] {"$a", "(。|；|？|！)+$"},
	};

	[Microsoft.SqlServer.Server.SqlFunction]
	public static SqlInt32 SqlGetStartsWith(SqlString sqlContent)
    {
		// 检查参数
		if (sqlContent == null || sqlContent.IsNull) return -1;
		// 返回结果
		return GetStartsWith(MergeContent(SplitContent(sqlContent.Value)));
    }

	public static string[] GetTemplate(int index)
    {
        // 返回结果
        return index >= 0 &&
            index < TEMPLATES.Length ? TEMPLATES[index] : null;
    }

	public static int GetStartsWith(string[] input)
    {
		// 记录日志
		LogTool.LogMessage("SentenceTemplate", "GetStartsWith", "开始匹配模板！");
		LogTool.LogMessage(string.Format("\ttemplate.length = {0}", TEMPLATES.Length));

		// 索引
		int nIndex = -1;
        // 循环处理
        for (int i = 0; i < TEMPLATES.Length; i ++)
        {
            // 获得模板
            string[] template = TEMPLATES[i];
            // 检查结果
            if (input.Length < template.Length) continue;
			// 记录日志
			LogTool.LogMessage(string.Format("\ttemplate.index = {0}", i));

			// 索引参数
			int index = 0;
            // 循环处理
            while (index < template.Length)
            {
                // 检查起始字符
                if (template[index][0] == '$')
                {
                    // 检查起始字符
                    if (input[index][0] != '$') break;
					// 记录日志
					LogTool.LogMessage("SentenceTemplate", "GetStartsWith", "字符串！");
				}
				else
                {
                    // 检查起始字符
                    if (input[index][0] == '$') break;
					//if (!Punctuation.IsPunctuation(input[i][0])) break;
					// 记录日志
					LogTool.LogMessage(string.Format("\tinput[{0}] = {1}",index, input[index]));
					LogTool.LogMessage(string.Format("\ttemplate[{0}] = {1}", index, template[index]));
					// 获得匹配结果
					Match match =
						Regex.Match(input[index], template[index]);
					// 检查匹配结果
					if (!match.Success || match.Index != 0) break;
					// 记录日志
					LogTool.LogMessage("SentenceTemplate", "GetStartsWith", "符号匹配！");
				}
				// 增加计数
				index ++;
            }
            // 检查结果
            if (index >= template.Length) { nIndex = i; break; }
        }
		// 记录日志
		LogTool.LogMessage("SentenceTemplate", "GetStartsWith", "模板匹配结束！");
		// 返回结果
		return nIndex;
    }

	public static string[] SplitContent(string strContent)
	{
		// 检查参数
		if (strContent == null ||
			strContent.Length <= 0) return null;

		// 链表
		List<string> segments = new List<string>();
		// 循环处理
		for (int i = 0; i < strContent.Length;)
		{
			// 获得字符
			char cValue = strContent[i];

			// 创建字符串
			StringBuilder sb = new StringBuilder();
			// 检查是否为标点符号
			if (Punctuation.IsPunctuation(cValue))
			{
				// 加入字符
				sb.Append(cValue);
				// 循环处理
				for (++i; i < strContent.Length; i++)
				{
					// 获得字符
					cValue = strContent[i];
					// 跨过空格
					if (cValue == ' ') continue;
					// 检查是否为标点符号
					if (!Punctuation.IsPunctuation(cValue)) break;
					// 加入字符
					sb.Append(cValue);
				}
			}
			else
			{
				// 加入标记位
				sb.Append('$');
				// 加入字符
				sb.Append(cValue);
				// 循环处理
				for (++i; i < strContent.Length; i++)
				{
					// 获得字符
					cValue = strContent[i];
					// 检查是否为标点符号
					if (Punctuation.IsPunctuation(cValue)) break;
					// 加入字符
					sb.Append(cValue);
				}
			}
			// 加入字符串
			segments.Add(sb.ToString());
		}
		// 返回结果
		return segments.ToArray();
	}
	
	public static string[] MergeContent(string[] input)
	{
		// 检查参数
		if (input == null ||
			input.Length <= 0) return null;
		// 记录日志
		LogTool.LogMessage("SentenceTemplate", "MergeContent", "开始合并字符串！");

		// 定义参数
		string[] output;
		// 合并字符串
		output = MergeString(input);

		// 设置参数
		//input = output;
		// 合并引用串
		output = MergeString(output);

        // 循环处理
        //do
        //{
        //    // 设置输入
        //    input = output;
        //    //合并数据
        //    output = MergeQuotation(input);

        //} while (output.Length < input.Length);

        // 循环处理
  //      do
		//{
		//	// 设置输入
		//	input = output;
		//	//合并数据
		//	output = MergeSegment(input, "，|：");

		//} while (output.Length < input.Length);

		//// 循环处理
		//do
		//{
		//	// 设置输入
		//	input = output;
		//	//合并数据
		//	output = MergeSegment(input, "；|。|？|！");

		//} while (output.Length < input.Length);
		// 返回结果
		return output;
	}

	private static string[] MergeString(string[] input)
	{
		// 检查参数
		if (input == null ||
			input.Length <= 0) return null;

		// 输出参数
		string[] output = input;

		do
        {
			// 设置输入参数
			input = output;
            // 链表
            List<string> segments = new List<string>();
            // 循环处理
            for (int index = 0; index < input.Length - 1; index++)
            {
                // 检查类型
                if (input[index][0] != '$' ||
					input[index + 1][0] != '$')
                {
                    // 增加字符串
                    segments.Add(input[index]); continue;
                }
                else
                {
                    // 创建字符串
                    StringBuilder sb = new StringBuilder();
                    // 增加字符串
                    sb.Append(input[index]);
                    sb.Append(input[index + 1].Substring(1));
                    segments.Add(sb.ToString()); index++;
                }
            }
            // 设置输出结果
            output = segments.ToArray();

        } while (output.Length < input.Length);
		// 返回结果
		return output;
	}

	private static string[] MergeQuotation(string[] input)
    {
		// 检查参数
		if (input == null ||
			input.Length <= 0) return null;

		// 输出参数
		string[] output = input;

		do
		{
			// 链表
			List<string> segments = new List<string>();
			// 循环处理
			for (int index = 0; index < input.Length - 2; index++)
			{
				// 检查类型
				if (input[index][0] == '$' ||
					input[index + 1][0] != '$' ||
						input[index + 2][0] == '$')
				{
					// 增加字符串
					segments.Add(input[index]); continue;
				}

				// 匹配参数
				Match match;

				// 匹配字符
				match = Regex.Match(input[index],
					string.Format("({0})$", Punctuation.GetPairStarts()));
				// 检查结果
				if (!match.Success || match.Index != 0)
				{
					// 增加字符串
					segments.Add(input[index]); continue;
				}

				// 获得结尾字符
				char cEnd = Punctuation.GetPairEnd(match.Value[0]);
				// 继续匹配
				match = Regex.Match(input[index + 2], string.Format("{0}$", cEnd));
				// 检查结果
				if (!match.Success || match.Index != 0)
				{
					// 增加字符串
					segments.Add(input[index]); continue;
				}

				// 创建字符串
				StringBuilder sb = new StringBuilder();
				// 增加字符串
				sb.Append('$');
				sb.Append(input[index]);
				sb.Append(input[index + 1].Substring(1));
				sb.Append(input[index + 2]);
				segments.Add(sb.ToString()); index += 2;
			}

		} while (output.Length < input.Length);
		// 返回结果
		return output;
	}

	private static string[] MergeSegment(string[] input, string strPattern)
    {
		// 检查参数
		if (input == null ||
			input.Length <= 0) return null;

        // 记录日志
        LogTool.LogMessage("SentenceTemplate", "MergeSegment", "开始合并数据！");
        LogTool.LogMessage(string.Format("\tinput.length = {0}", input.Length));
        // 记录日志
        //foreach (string strValue in input)
        //{
        //    LogTool.LogMessage(string.Format("\tinput.content = {0}", strValue));
        //}

        // 链表
        List<string> segments = new List<string>();
		// 循环处理
		for (int index = 0; index < input.Length; index++)
		{
			// 创建字符串
			StringBuilder sb = new StringBuilder();
			// 增加字符串
			sb.Append(input[index]);
			// 跳过标点符号
			if (Punctuation.IsPunctuation(input[index][0]))
			{
                // 记录日志
                //LogTool.LogMessage(string.Format("\toutput.content = {0}", input[index]));
            }
			else
            {
				// 检查索引
				if (index + 1 < input.Length && input[index + 1][0] == '$')
				{
					// 扩充内容
					sb.Append(input[index + 1].Substring(1)); index += 1;
                    // 记录日志
                    //LogTool.LogMessage(string.Format("\toutput.content = {0}", sb.ToString()));
                }
				else if (index + 2 < input.Length && input[index + 2][0] == '$')
				{
					// 尝试匹配
					Match match = Regex.Match(input[index + 1], string.Format("({0})+$", strPattern));
					// 检查结果
					if(match.Success && match.Index == 0)
                    {
						// 扩充内容
						sb.Append(input[index + 1]);
						sb.Append(input[index + 2].Substring(1)); index += 2;
                        // 记录日志
                        //LogTool.LogMessage(string.Format("\toutput.content = {0}", sb.ToString()));
                    }
				}
			}
			// 增加字符串
			segments.Add(sb.ToString());
		}
        // 记录日志
        LogTool.LogMessage(string.Format("\toutput.count = {0}", segments.Count));
		// 记录日志
		foreach (string strValue in segments.ToArray())
		{
		    LogTool.LogMessage(string.Format("\tinput.content = {0}", strValue));
		}
		LogTool.LogMessage("SentenceTemplate", "MergeSegment", "数据合并完毕！");
        // 返回结果
        return segments.ToArray();
	}
}
