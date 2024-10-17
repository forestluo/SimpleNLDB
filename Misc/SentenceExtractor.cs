using System;
using System.Text;
using System.Data;
using System.Threading;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Misc
{
    public class SentenceExtractor
    {
		public static void ExtractPhrases(bool flag)
		{
			// 记录日志
			Log.LogMessage("SentenceExtractor", "ExtractPhrases", "提取短语数据！");

			//指令字符串
			string cmdString =
				  "SELECT [rid], [content] FROM [dbo].[RawContent];";

			// 创建数据库连接
			SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

			try
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

				// 计数器
				int total = 0;
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
					// 增加计数
					total++;
					// 检查计数
					if (total % 10000 == 0)
					{
						// 打印记录
						Log.LogMessage(string.Format("{0} items processed !", total));
					}

					// 获得编号
					int rid = reader.GetInt32(0);
					// 获得内容
					string content = reader.GetString(1);
					// 检查内容
					if (content == null || content.Length <= 0) continue;

					// 清理线程
					tasks.RemoveAll(task => { return task.IsCompleted; });                  
					
					// 启动进程
					tasks.Add(factory.StartNew
					(() =>
                    {
						lock(lockObject)
                        {
							// 增加计数
							taskCount++;
							// 检查计数
							if (taskCount % 10000 == 0)
							{
								// 打印记录
								Log.LogMessage(string.Format("{0} tasks finised !", taskCount));
							}
						}

						// 清理内容
						content = MiscTool.ClearContent(content);
						// 检查内容
						if (content == null || content.Length <= 0) return;

						// 切分字符串
						string[] output = ExSplitContent(content);
						// 检查结果
						if (output == null || output.Length <= 0) return;

						// 检查标记位
						if (flag)
						{
							// 循环处理
							foreach (string item in output)
							{
								// 检查参数
								if (item[0] != '$') continue;

								// 只处理中文
								Match match =
									Regex.Match(item, @"[\u4E00-\u9FA5]+");
								// 检查结果
								while (match.Success)
								{
									// 加入短语
									SentenceContent.AddPhrase(match.Value, 1);
									//下一个匹配项目
									match = match.NextMatch();
								}
							}
						}
					}));
				}
				// 关闭数据阅读器
				reader.Close();
				// 记录日志
				Log.LogMessage(string.Format("\tcontent.count = {0}", total));

				// 记录日志
				Log.LogMessage("SentenceExtractor", "ExtractPhrases", "等待所有任务完成 ！");
				// 等待所有任务完成
				Task.WaitAll(tasks.ToArray());
			}
			catch (System.Exception ex)
			{
				// 记录日志
				Log.LogMessage("SentenceExtractor", "ExtractPhrases", "unexpected exit ！");
				Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
			}
			finally
			{
				// 检查状态并关闭连接
				if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
			}

			// 记录日志
			Log.LogMessage("SentenceExtractor", "ExtractPhrases", "短语提取完毕！");
		}

		public static void ExtractSentences(bool flag)
        {
            // 记录日志
            Log.LogMessage("SentenceExtractor", "ExtractSentences", "提取句子数据！");

            //指令字符串
            string cmdString =
                  "SELECT [rid], [content] FROM [dbo].[RawContent];";

            // 创建数据库连接
            SqlConnection sqlConnection = new SqlConnection(Common.CONNECT_STRING);

            try
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
				
				// 计数器
				int total = 0;
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
                    // 增加计数
                    total++;
                    // 检查计数
                    if (total % 10000 == 0)
                    {
                        // 打印记录
                        Log.LogMessage(string.Format("{0} items processed !", total));
                    }

                    // 获得编号
                    int rid = reader.GetInt32(0);
                    // 获得内容
                    string content = reader.GetString(1);
                    // 检查内容
                    if (content == null || content.Length <= 0) continue;

					// 清理线程
					tasks.RemoveAll(task => { return task.IsCompleted; });

					// 启动进程
					tasks.Add(factory.StartNew
					(() =>
					{
						lock (lockObject)
						{
							// 增加计数
							taskCount++;
							// 检查计数
							if (taskCount % 10000 == 0)
							{
								// 打印记录
								Log.LogMessage(string.Format("{0} tasks finised !", taskCount));
							}
						}

						// 清理内容
						content = MiscTool.ClearContent(content);
						// 检查内容
						if (content == null || content.Length <= 0) return;

						// 切分字符串
						string[] output = SplitContent(content);
						// 检查结果
						if (output == null || output.Length <= 0) return;

						// 合并字符串
						output = MergeContent(output);
						// 检查结果
						if (output == null || output.Length <= 0) return;

						// 获得拆分结果
						string[][] sentences = GetSentences(output);
						// 检查结果
						if (sentences == null || sentences.Length <= 0) return;

						// 检查标记位
						if (flag)
						{
							// 循环处理
							foreach (string[] sentence in sentences)
							{
								// 加入记录
								SentenceContent.AddSentence(Concatenate(sentence), rid);
							}
						}
					}));
                }
                // 关闭数据阅读器
                reader.Close();
                // 记录日志
                Log.LogMessage(string.Format("\tcontent.count = {0}", total));

				// 记录日志
				Log.LogMessage("SentenceExtractor", "ExtractSentences", "等待所有任务完成 ！");
				// 等待所有任务完成
				Task.WaitAll(tasks.ToArray());
			}
			catch (System.Exception ex)
            {
                // 记录日志
                Log.LogMessage("SentenceExtractor", "ExtractSentences", "unexpected exit ！");
                Log.LogMessage(string.Format("\texception.message = {0}", ex.Message));
            }
            finally
            {
                // 检查状态并关闭连接
                if (sqlConnection.State == ConnectionState.Open) sqlConnection.Close();
            }

            // 记录日志
            Log.LogMessage("SentenceExtractor", "ExtractSentences", "句子提取完毕！");
        }

		// 句式模板
		// 不要调整顺序！！！
		private static readonly string[][] TEMPLATES =
			{
				new string[] {"“$", "$a", "(，|：|。|；|？|！|…)*”$", "$b", "(，|：)?“$", "$c", "(。|？|！|…)+”$"},
				new string[] {"‘$", "$a", "(，|：|。|；|？|！|…)*’$", "$b", "(，|：)?‘$", "$c", "(。|？|！|…)+’$"},
				new string[] {"「$", "$a", "(，|：|。|；|？|！|…)*」$", "$b", "(，|：)?“$", "$c", "(。|？|！|…)+”$"},
				new string[] {"『$", "$a", "(，|：|。|；|？|！|…)*』$", "$b", "(，|：)?‘$", "$c", "(。|？|！|…)+’$"},
				new string[] {"〝$", "$a", "(，|：|。|；|？|！|…)*〞$", "$b", "(，|：)?“$", "$c", "(。|？|！|…)+”$"},
				new string[] {"【$", "$a", "(，|：|。|；|？|！|…)*】$", "$b", "(，|：)?‘$", "$c", "(。|？|！|…)+’$"},

				new string[] {"$a", "(，|：)?“$", "$b", "(。|；|？|！|…)*”$", "$c", "(。|？|！|…)+’$"},
				new string[] {"$a", "(，|：)?‘$", "$b", "(。|；|？|！|…)*’$", "$c", "(。|？|！|…)+’$"},
				new string[] {"$a", "(，|：)?「$", "$b", "(。|；|？|！|…)*」$", "$c", "(。|？|！|…)+’$"},
				new string[] {"$a", "(，|：)?『$", "$b", "(。|；|？|！|…)*』$", "$c", "(。|？|！|…)+’$"},
				new string[] {"$a", "(，|：)?〝$", "$b", "(。|；|？|！|…)*〞$", "$c", "(。|？|！|…)+’$"},
				new string[] {"$a", "(，|：)?【$", "$b", "(。|；|？|！|…)*】$", "$c", "(。|？|！|…)+’$"},

				// 符号嵌套
				new string[] {"“$", "$a", "(，|：|。|；|？|！)+‘$", "$b", "(。|；|？|！)*’$", "$c", "(。|？|！|…)+”$"},

				new string[] {"“$", "$a", "(，|：|。|；|？|！)+‘$", "$b", "(。|；|？|！|…)*’”$"},
				new string[] {"“‘$", "$a", "(，|：|。|；|？|！|…)*’$", "$b", "(。|；|？|！|…)+”$"},

				// 常见符号
				new string[] {"$a", "(：)$", "$b", "(。|；|？|！|…)+$"},
				new string[] {"$a", "(，|：)?“$", "$b", "(。|；|？|！|…)*”$"},
				new string[] {"$a", "(，|：)?‘$", "$b", "(。|；|？|！|…)*’$"},
				new string[] {"$a", "(，|：)?（$", "$b", "(。|；|？|！|…)*）$"},
				new string[] {"$a", "(，|：)?「$", "$b", "(。|；|？|！|…)*」$"},
				new string[] {"$a", "(，|：)?『$", "$b", "(。|；|？|！|…)*』$"},
				new string[] {"$a", "(，|：)?〝$", "$b", "(。|；|？|！|…)*〞$"},

				new string[] {"$a", "(，|：)?《$", "$b", "》(。|？|！|…)+$"},
				new string[] {"$a", "(，|：)?【$", "$b", "】(。|？|！|…)+$"},

				new string[] {"“$", "$a", "(，|：|。|；|？|！|…)*”$", "$b", "(。|？|！|…)+$"},
				new string[] {"‘$", "$a", "(，|：|。|；|？|！|…)*’$", "$b", "(。|？|！|…)+$"},

				//// 符号嵌套
				new string[] {"“‘$", "$a", "(，|：|。|；|？|！|…)*’”$"},
				new string[] {"“（$", "$a", "(，|：|。|；|？|！|…)*）”$"},

				// 常见符号
				new string[] {"“$", "$a", "(，|：|。|；|？|！|…)*”$"},
				new string[] {"‘$", "$a", "(，|：|。|；|？|！|…)*’$"},
				new string[] {"（$", "$a", "(，|：|。|；|？|！|…)*）$"},
				new string[] {"《$", "$a", "(，|：|。|；|？|！|…)*》$"},
				new string[] {"【$", "$a", "(，|：|。|；|？|！|…)*】$"},
				// 比较少见
				new string[] {"「$", "$a", "(，|：|。|；|？|！|…)*」$"},
				new string[] {"『$", "$a", "(，|：|。|；|？|！|…)*』$"},
				new string[] {"〖$", "$a", "(，|：|。|；|？|！|…)*〗$"},
				new string[] {"〝$", "$a", "(，|：|。|；|？|！|…)*〞$"},

				// 最简单的句子
				new string[] {"$a", "(。|；|？|！|…)+$"},
		};

		public static string[] GetSentences(string content)
		{
			// 检查结果
			if (content == null || content.Length <= 0) return null;
			// 清理内容
			content = MiscTool.ClearContent(content);
			// 检查结果
			if (content == null || content.Length <= 0) return null;

			// 切分字符串
			string[] output = SplitContent(content);
			// 检查结果
			if (output == null || output.Length <= 0) return null;

			// 合并字符串
			output = MergeContent(output);
			// 检查结果
			if (output == null || output.Length <= 0) return null;

			// 获得拆分结果
			string[][] result = GetSentences(output);
			// 检查结果
			if (result == null || result.Length <= 0) return null;

			// 创建链表
			List<string> sentences = new List<string>();
			// 增加字符串
			foreach (string[] sentence in result)
			{
				// 拼接字符串
				string value = Concatenate(sentence);
				// 检查结果
				if (value != null && value.Length > 0) sentences.Add(value);
			}
			// 返回结果
			return sentences.ToArray();
		}

		public static string[] SplitContent(string content)
		{
			// 检查参数
			if (content == null ||
				content.Length <= 0) return null;

			// 链表
			List<string> segments = new List<string>();
			// 循环处理
			for (int i = 0; i < content.Length;)
			{
				// 获得字符
				char cValue = content[i];
				// 检查是否为标点符号
				if (Punctuation.IsPunctuation(cValue))
				{
					// 增加索引
					i++;
					// 加入字符
					segments.Add(cValue.ToString());
				}
				else
				{
					// 创建字符串
					StringBuilder sb = new StringBuilder();
					// 加入标记位
					sb.Append('$');
					// 加入字符
					sb.Append(cValue);
					// 循环处理
					for (++i; i < content.Length; i++)
					{
						// 获得字符
						cValue = content[i];
						// 检查是否为标点符号
						if (Punctuation.IsPunctuation(cValue)) break;
						// 加入字符
						sb.Append(cValue);
					}
					// 加入字符串
					segments.Add(sb.ToString());
				}
			}
			// 返回结果
			return AdjustSegments(segments.ToArray());
		}

		public static string[] ExSplitContent(string content)
		{
			// 检查参数
			if (content == null ||
				content.Length <= 0) return null;

			// 提取数量词
			Quantity[] quantities =
				QuantityExtractor.GetQuantities(content);
			// 检查结果
			if (quantities == null ||
				quantities.Length <= 0) return SplitContent(content);

			// 链表
			List<string> segments = new List<string>();
			// 循环处理
			for (int i = 0; i < content.Length;)
			{
				// 检查是否在数量词范围内
				Quantity q = QuantityExtractor.GetInside(quantities, i);
				// 检查结果
				if(q != null)
                {
					// 加入标记
					segments.Add(string.
						Format("#{0}",q.Value));
					// 增加索引
					i += q.Length; continue;
				}

				// 获得字符
				char cValue = content[i];
				// 检查是否为标点符号
				if (Punctuation.IsPunctuation(cValue))
				{
					// 增加索引
					i++;
					// 加入字符
					segments.Add(cValue.ToString());
				}
				else
				{
					// 创建字符串
					StringBuilder sb = new StringBuilder();
					// 加入标记位
					sb.Append('$');
					// 加入字符
					sb.Append(cValue);
					// 循环处理
					for (++i; i < content.Length; i++)
					{
						// 获得字符
						cValue = content[i];
						// 检查是否为标点符号
						if (Punctuation.IsPunctuation(cValue)) break;
						// 检查是否在数量词范围内
						if (QuantityExtractor.GetInside(quantities, i) != null) break;
						// 加入字符
						sb.Append(cValue);
					}
					// 加入字符串
					segments.Add(sb.ToString());
				}
			}
			// 返回结果
			return AdjustSegments(segments.ToArray());
		}

		public static string[] MergeContent(string[] input)
		{
			// 检查参数
			if (input == null ||
				input.Length <= 0) return null;

			// 定义参数
			bool bMerged = false;
			// 定义参数
			string[] output = input;
			do
			{
				// 设置参数
				bMerged = false;

				// 设置输入参数
				input = output;
				// 合并字符串
				output = MergeString(input);

				// 检查结果
				if (output.Length < input.Length) bMerged = true;

				// 设置参数
				input = output;
				// 合并引用串
				output = MergeQuotation(input);

				// 检查结果
				if (output.Length < input.Length) bMerged = true;

				// 设置参数
				input = output;
				// 合并引用串
				output = MergeSegment(input);

				// 检查结果
				if (output.Length < input.Length) bMerged = true;

				// 设置参数
				input = output;
				// 合并引用串
				output = MergeCompound(input);

				// 检查结果
				if (output.Length < input.Length) bMerged = true;

			} while (bMerged);
			// 返回结果
			return output;
		}

		private static string[] GetTemplate(int index)
		{
			// 返回结果
			return index >= 0 &&
				index < TEMPLATES.Length ? TEMPLATES[index] : null;
		}

		public static string Concatenate(string[] input)
		{
			// 创建对象
			StringBuilder sb = new StringBuilder();
			// 循环处理
			foreach (string s in input)
			{
				// 增加字符串
				sb.Append(s[0] == '$' ? s.Substring(1) : s);
			}
			// 返回结果
			return sb.ToString();
		}

		private static int GetMatchedTemplate(string[] input)
		{
			// 索引
			int nIndex = -1;
			// 循环处理
			for (int i = 0; i < TEMPLATES.Length; i++)
			{
				// 获得模板
				string[] template = TEMPLATES[i];
				// 检查结果
				if (input.Length < template.Length) continue;

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
					}
					else
					{
						// 检查起始字符
						if (input[index][0] == '$') break;
						//if (!Punctuation.IsPunctuation(input[i][0])) break;
						// 获得匹配结果
						Match match =
							Regex.Match(input[index], template[index]);
						// 检查匹配结果
						if (!match.Success || match.Index != 0) break;
					}
					// 增加计数
					index++;
				}
				// 检查结果
				if (index >= template.Length) { nIndex = i; break; }
			}
			// 返回结果
			return nIndex;
		}

		public static string[][] GetSentences(string[] input)
		{
			// 创建对象
			List<string[]> sentences = new List<string[]>();

			// 循环处理
			for (int i = 0; i < input.Length; i++)
			{
				// 创建新输入
				string[] newInput =
					new string[input.Length - i];
				// 拷贝
				for (int j = 0;
					j < newInput.Length; j++)
				{
					// 拷贝数组
					newInput[j] = input[i + j];
				}
				// 获得模板索引
				int index = GetMatchedTemplate(newInput);
				// 检查结果
				if (index >= 0)
				{
					// 创建对象
					List<string> items = new List<string>();

					// 获得模板
					string[] template = GetTemplate(index);
					// 将头部数据装入
					for (int j = 0;
						j < template.Length; j++) items.Add(newInput[j]);
					// 增加索引
					i += template.Length - 1;
					// 添加句子
					sentences.Add(items.ToArray());
				}
				/* 不符合句式，直接抛弃！
				else if(i == input.Length - 1)
                {
					// 剩余的句子
					if (input[i][0] == '$') sentences.Add(new string[] { input[0] });
                }
				*/
			}
			/* 不符合句式，直接抛弃！
			// 检查结果
			if (sentences == null || sentences.Count <= 0)
			{
				// 输入最后的字符串
				if (input.Length == 1)
				{
					sentences.Add(new string[] { input[0] });
				}
			}
			*/
			// 返回结果
			return sentences.Count > 0 ? sentences.ToArray() : null;
		}

		private static string[] AdjustSegments(string[] input)
		{
			// 链表
			List<string> segments = new List<string>();
			// 循环处理
			for (int i = 1; i < input.Length; i++)
			{
				if (input[i][0] == '…' &&
					input[i - 1][0] != '$') input[i] = "$…";
			}
			// 清理结果
			segments.Clear();
			// 循环处理
			for (int i = 0; i < input.Length; i++)
			{
				// 获得字符
				char cValue = input[i][0];
				// 检查类型
				if (cValue == '$' ||
					i >= input.Length - 1)
				{
					// 增加子符串
					segments.Add(input[i]); continue;
				}
				// 获得字符
				char cNext = input[i + 1][0];
				// 检查结果
				if (cNext == '$')
				{
					// 增加子符串
					segments.Add(input[i]); continue;
				}
				// 检查字符
				if (!((Punctuation.IsNEndMajorSplitter(cValue)
						&& Punctuation.IsNarStartSplitter(cNext)) ||
					(Punctuation.IsMajorSplitter(cValue)
						&& Punctuation.IsNarEndSplitter(cNext))))
				{
					// 增加子符串
					segments.Add(input[i]); continue;
				}
				// 创建子符串
				StringBuilder sb = new StringBuilder();
				// 增加字符
				sb.Append(cValue); sb.Append(cNext);
				// 增加子符串
				segments.Add(sb.ToString()); i++;
			}
			// 返回结果
			return segments.ToArray();
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
				int index = 0;
				while (index < input.Length - 1)
				{
					// 检查类型
					if (input[index][0] != '$' ||
						input[index + 1][0] != '$')
					{
						// 增加字符串
						segments.Add(input[index]); index++;
					}
					else
					{
						// 创建字符串
						StringBuilder sb = new StringBuilder();
						// 增加字符串
						sb.Append(input[index]);
						sb.Append(input[index + 1].Substring(1));
						segments.Add(sb.ToString()); index += 2;
					}
				}
				// 末尾字符
				for (; index < input.Length; index++) segments.Add(input[index]);

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
				// 设置输入参数
				input = output;
				// 链表
				List<string> segments = new List<string>();
				// 循环处理
				int index = 0;
				while (index < input.Length - 2)
				{
					// 检查参数
					if (input[index][0] == '$' ||
						input[index + 1][0] != '$' ||
							input[index + 2][0] == '$')
					{
						// 增加字符串
						segments.Add(input[index]); index++;
					}
					else
					{
						// 匹配参数
						Match match;

						// 匹配字符
						match = Regex.Match(input[index],
							string.Format("({0})$", Punctuation.GetPairStarts()));
						// 检查结果
						if (!match.Success || match.Index != 0)
						{
							// 增加字符串
							segments.Add(input[index]); index++;
						}
						else
						{
							// 获得结尾字符
							char cEnd = Punctuation.GetPairEnd(match.Value[0]);
							// 继续匹配
							match = Regex.Match(input[index + 2], string.Format("^{0}", cEnd));
							// 检查结果
							if (!match.Success || match.Index != 0)
							{
								// 增加字符串
								segments.Add(input[index]); index++;
							}
							else
							{
								// 创建字符串
								StringBuilder sb = new StringBuilder();
								// 增加字符串
								sb.Append('$');
								sb.Append(input[index]);
								sb.Append(input[index + 1].Substring(1));
								sb.Append(input[index + 2][0]);
								segments.Add(sb.ToString());

								// 检查长度
								if (input[index + 2].Length <= 1)
								{
									// 增加索引
									index += 3;
								}
								else
								{
									// 修改项目
									input[index + 2] = input[index + 2].Substring(1); index += 2;
								}
							}
						}
					}
				}
				// 末尾字符
				for (; index < input.Length; index++) segments.Add(input[index]);

				// 设置输出结果
				output = segments.ToArray();

			} while (output.Length < input.Length);
			// 返回结果
			return output;
		}

		private static string[] MergeSegment(string[] input)
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
				int index = 0;
				while (index < input.Length - 2)
				{
					// 检查参数
					if (input[index][0] != '$')
					{
						// 增加内容
						segments.Add(input[index]); index++;
					}
					else
					{
						// 尝试匹配
						Match match =
							Regex.Match(input[index + 1],
							string.Format("({0})+$", Punctuation.GetNEndMajorSplitters()));
						// 检查参数
						if (!match.Success || match.Index != 0)
						{
							// 增加内容
							segments.Add(input[index]); index++;
						}
						else
						{
							// 检查参数
							if (input[index + 2][0] != '$')
							{
								// 增加内容
								segments.Add(input[index]); index++;
							}
							else
							{
								// 创建字符串
								StringBuilder sb = new StringBuilder();
								// 增加字符串
								sb.Append(input[index]);
								sb.Append(input[index + 1]);
								sb.Append(input[index + 2].Substring(1));
								segments.Add(sb.ToString()); index += 3;
							}
						}
					}
				}
				// 末尾字符
				for (; index < input.Length; index++) segments.Add(input[index]);

				// 设置输出结果
				output = segments.ToArray();

			} while (output.Length < input.Length);

			// 返回结果
			return output;
		}

		private static string[] MergeCompound(string[] input)
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
				// 增加内容
				segments.Add(input[0]);
				// 循环处理
				int index = 1;
				while (index < input.Length - 2)
				{
					// 检查字符
					if (input[index][0] != '$')
					{
						// 增加内容
						segments.Add(input[index]); index++;
					}
					else
					{
						// 尝试匹配
						Match match =
							Regex.Match(input[index + 1],
							string.Format("({0})+$", Punctuation.GetEndMajorSplitters()));
						// 检查参数
						if (!match.Success || match.Index != 0)
						{
							// 增加内容
							segments.Add(input[index]); index++;
						}
						else
						{
							// 检查参数
							if (input[index + 2][0] != '$')
							{
								// 增加内容
								segments.Add(input[index]); index++;
							}
							else
							{
								// 获得字符
								char cLast = input[index - 1][input[index - 1].Length - 1];
								// 检查结果
								if (input[index - 1][0] == '$' ||
									!Punctuation.IsPairStart(cLast))
								{
									// 增加内容
									segments.Add(input[index]); index++;
								}
								else
								{
									// 创建字符串
									StringBuilder sb = new StringBuilder();
									// 增加字符串
									sb.Append(input[index]);
									sb.Append(input[index + 1]);
									sb.Append(input[index + 2].Substring(1));
									segments.Add(sb.ToString()); index += 3;
								}
							}
						}
					}
				}
				// 末尾字符
				for (; index < input.Length; index++) segments.Add(input[index]);

				// 设置输出结果
				output = segments.ToArray();

			} while (output.Length < input.Length);

			// 返回结果
			return output;
		}
	}
}
