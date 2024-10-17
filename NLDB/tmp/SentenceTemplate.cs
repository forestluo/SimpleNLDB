using Misc;
using System.Text;
using System.Data.SqlTypes;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class SentenceTemplate
{
    // ��ʽģ��
	// ��Ҫ����˳�򣡣���
    private readonly static
        string[][] TEMPLATES =
        {
			new string[] {"��$", "$a", "(��|��|��|��|��|��)*��$", "$b", "(��|��|��|��|��|��)+��$", "$c", "(��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)*��$", "$b", "(��|��|��|��|��|��)+��$", "$c", "(��|��|��|��)+��$"},

			// ����Ƕ��
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$", "$b", "(��|��|��|��)+��$", "$c", "(��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$", "$b", "(��|��|��|��)+����$"},
			new string[] {"����$", "$a", "(��|��|��|��|��|��)+��$", "$b", "(��|��|��|��)+��$"},

			// ��������
			new string[] {"$a", "(��)$", "$b", "(��|��|��|��)+$"},
			new string[] {"$a", "(��)?��$", "$b", "(��|��|��|��)+��$"},
			new string[] {"$a", "(��)?��$", "$b", "(��|��|��|��)+��$"},
			new string[] {"$a", "(��)?��$", "$b", "(��|��|��|��)+��$"},

			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$", "$b", "(��|��|��|��)+$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$", "$b", "(��|��|��|��)+$"},

			// ����Ƕ��
			new string[] {"����$", "$a", "(��|��|��|��|��|��)+����$"},
			new string[] {"����$", "$a", "(��|��|��|��|��|��)+����$"},

			// ��������
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},
			// �Ƚ��ټ�
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},
			new string[] {"��$", "$a", "(��|��|��|��|��|��)+��$"},

			// ��򵥵ľ���
			new string[] {"$a", "(��|��|��|��)+$"},
	};

	[Microsoft.SqlServer.Server.SqlFunction]
	public static SqlInt32 SqlGetStartsWith(SqlString sqlContent)
    {
		// ������
		if (sqlContent == null || sqlContent.IsNull) return -1;
		// ���ؽ��
		return GetStartsWith(MergeContent(SplitContent(sqlContent.Value)));
    }

	public static string[] GetTemplate(int index)
    {
        // ���ؽ��
        return index >= 0 &&
            index < TEMPLATES.Length ? TEMPLATES[index] : null;
    }

	public static int GetStartsWith(string[] input)
    {
		// ��¼��־
		LogTool.LogMessage("SentenceTemplate", "GetStartsWith", "��ʼƥ��ģ�壡");
		LogTool.LogMessage(string.Format("\ttemplate.length = {0}", TEMPLATES.Length));

		// ����
		int nIndex = -1;
        // ѭ������
        for (int i = 0; i < TEMPLATES.Length; i ++)
        {
            // ���ģ��
            string[] template = TEMPLATES[i];
            // �����
            if (input.Length < template.Length) continue;
			// ��¼��־
			LogTool.LogMessage(string.Format("\ttemplate.index = {0}", i));

			// ��������
			int index = 0;
            // ѭ������
            while (index < template.Length)
            {
                // �����ʼ�ַ�
                if (template[index][0] == '$')
                {
                    // �����ʼ�ַ�
                    if (input[index][0] != '$') break;
					// ��¼��־
					LogTool.LogMessage("SentenceTemplate", "GetStartsWith", "�ַ�����");
				}
				else
                {
                    // �����ʼ�ַ�
                    if (input[index][0] == '$') break;
					//if (!Punctuation.IsPunctuation(input[i][0])) break;
					// ��¼��־
					LogTool.LogMessage(string.Format("\tinput[{0}] = {1}",index, input[index]));
					LogTool.LogMessage(string.Format("\ttemplate[{0}] = {1}", index, template[index]));
					// ���ƥ����
					Match match =
						Regex.Match(input[index], template[index]);
					// ���ƥ����
					if (!match.Success || match.Index != 0) break;
					// ��¼��־
					LogTool.LogMessage("SentenceTemplate", "GetStartsWith", "����ƥ�䣡");
				}
				// ���Ӽ���
				index ++;
            }
            // �����
            if (index >= template.Length) { nIndex = i; break; }
        }
		// ��¼��־
		LogTool.LogMessage("SentenceTemplate", "GetStartsWith", "ģ��ƥ�������");
		// ���ؽ��
		return nIndex;
    }

	public static string[] SplitContent(string strContent)
	{
		// ������
		if (strContent == null ||
			strContent.Length <= 0) return null;

		// ����
		List<string> segments = new List<string>();
		// ѭ������
		for (int i = 0; i < strContent.Length;)
		{
			// ����ַ�
			char cValue = strContent[i];

			// �����ַ���
			StringBuilder sb = new StringBuilder();
			// ����Ƿ�Ϊ������
			if (Punctuation.IsPunctuation(cValue))
			{
				// �����ַ�
				sb.Append(cValue);
				// ѭ������
				for (++i; i < strContent.Length; i++)
				{
					// ����ַ�
					cValue = strContent[i];
					// ����ո�
					if (cValue == ' ') continue;
					// ����Ƿ�Ϊ������
					if (!Punctuation.IsPunctuation(cValue)) break;
					// �����ַ�
					sb.Append(cValue);
				}
			}
			else
			{
				// ������λ
				sb.Append('$');
				// �����ַ�
				sb.Append(cValue);
				// ѭ������
				for (++i; i < strContent.Length; i++)
				{
					// ����ַ�
					cValue = strContent[i];
					// ����Ƿ�Ϊ������
					if (Punctuation.IsPunctuation(cValue)) break;
					// �����ַ�
					sb.Append(cValue);
				}
			}
			// �����ַ���
			segments.Add(sb.ToString());
		}
		// ���ؽ��
		return segments.ToArray();
	}
	
	public static string[] MergeContent(string[] input)
	{
		// ������
		if (input == null ||
			input.Length <= 0) return null;
		// ��¼��־
		LogTool.LogMessage("SentenceTemplate", "MergeContent", "��ʼ�ϲ��ַ�����");

		// �������
		string[] output;
		// �ϲ��ַ���
		output = MergeString(input);

		// ���ò���
		//input = output;
		// �ϲ����ô�
		output = MergeString(output);

        // ѭ������
        //do
        //{
        //    // ��������
        //    input = output;
        //    //�ϲ�����
        //    output = MergeQuotation(input);

        //} while (output.Length < input.Length);

        // ѭ������
  //      do
		//{
		//	// ��������
		//	input = output;
		//	//�ϲ�����
		//	output = MergeSegment(input, "��|��");

		//} while (output.Length < input.Length);

		//// ѭ������
		//do
		//{
		//	// ��������
		//	input = output;
		//	//�ϲ�����
		//	output = MergeSegment(input, "��|��|��|��");

		//} while (output.Length < input.Length);
		// ���ؽ��
		return output;
	}

	private static string[] MergeString(string[] input)
	{
		// ������
		if (input == null ||
			input.Length <= 0) return null;

		// �������
		string[] output = input;

		do
        {
			// �����������
			input = output;
            // ����
            List<string> segments = new List<string>();
            // ѭ������
            for (int index = 0; index < input.Length - 1; index++)
            {
                // �������
                if (input[index][0] != '$' ||
					input[index + 1][0] != '$')
                {
                    // �����ַ���
                    segments.Add(input[index]); continue;
                }
                else
                {
                    // �����ַ���
                    StringBuilder sb = new StringBuilder();
                    // �����ַ���
                    sb.Append(input[index]);
                    sb.Append(input[index + 1].Substring(1));
                    segments.Add(sb.ToString()); index++;
                }
            }
            // ����������
            output = segments.ToArray();

        } while (output.Length < input.Length);
		// ���ؽ��
		return output;
	}

	private static string[] MergeQuotation(string[] input)
    {
		// ������
		if (input == null ||
			input.Length <= 0) return null;

		// �������
		string[] output = input;

		do
		{
			// ����
			List<string> segments = new List<string>();
			// ѭ������
			for (int index = 0; index < input.Length - 2; index++)
			{
				// �������
				if (input[index][0] == '$' ||
					input[index + 1][0] != '$' ||
						input[index + 2][0] == '$')
				{
					// �����ַ���
					segments.Add(input[index]); continue;
				}

				// ƥ�����
				Match match;

				// ƥ���ַ�
				match = Regex.Match(input[index],
					string.Format("({0})$", Punctuation.GetPairStarts()));
				// �����
				if (!match.Success || match.Index != 0)
				{
					// �����ַ���
					segments.Add(input[index]); continue;
				}

				// ��ý�β�ַ�
				char cEnd = Punctuation.GetPairEnd(match.Value[0]);
				// ����ƥ��
				match = Regex.Match(input[index + 2], string.Format("{0}$", cEnd));
				// �����
				if (!match.Success || match.Index != 0)
				{
					// �����ַ���
					segments.Add(input[index]); continue;
				}

				// �����ַ���
				StringBuilder sb = new StringBuilder();
				// �����ַ���
				sb.Append('$');
				sb.Append(input[index]);
				sb.Append(input[index + 1].Substring(1));
				sb.Append(input[index + 2]);
				segments.Add(sb.ToString()); index += 2;
			}

		} while (output.Length < input.Length);
		// ���ؽ��
		return output;
	}

	private static string[] MergeSegment(string[] input, string strPattern)
    {
		// ������
		if (input == null ||
			input.Length <= 0) return null;

        // ��¼��־
        LogTool.LogMessage("SentenceTemplate", "MergeSegment", "��ʼ�ϲ����ݣ�");
        LogTool.LogMessage(string.Format("\tinput.length = {0}", input.Length));
        // ��¼��־
        //foreach (string strValue in input)
        //{
        //    LogTool.LogMessage(string.Format("\tinput.content = {0}", strValue));
        //}

        // ����
        List<string> segments = new List<string>();
		// ѭ������
		for (int index = 0; index < input.Length; index++)
		{
			// �����ַ���
			StringBuilder sb = new StringBuilder();
			// �����ַ���
			sb.Append(input[index]);
			// ����������
			if (Punctuation.IsPunctuation(input[index][0]))
			{
                // ��¼��־
                //LogTool.LogMessage(string.Format("\toutput.content = {0}", input[index]));
            }
			else
            {
				// �������
				if (index + 1 < input.Length && input[index + 1][0] == '$')
				{
					// ��������
					sb.Append(input[index + 1].Substring(1)); index += 1;
                    // ��¼��־
                    //LogTool.LogMessage(string.Format("\toutput.content = {0}", sb.ToString()));
                }
				else if (index + 2 < input.Length && input[index + 2][0] == '$')
				{
					// ����ƥ��
					Match match = Regex.Match(input[index + 1], string.Format("({0})+$", strPattern));
					// �����
					if(match.Success && match.Index == 0)
                    {
						// ��������
						sb.Append(input[index + 1]);
						sb.Append(input[index + 2].Substring(1)); index += 2;
                        // ��¼��־
                        //LogTool.LogMessage(string.Format("\toutput.content = {0}", sb.ToString()));
                    }
				}
			}
			// �����ַ���
			segments.Add(sb.ToString());
		}
        // ��¼��־
        LogTool.LogMessage(string.Format("\toutput.count = {0}", segments.Count));
		// ��¼��־
		foreach (string strValue in segments.ToArray())
		{
		    LogTool.LogMessage(string.Format("\tinput.content = {0}", strValue));
		}
		LogTool.LogMessage("SentenceTemplate", "MergeSegment", "���ݺϲ���ϣ�");
        // ���ؽ��
        return segments.ToArray();
	}
}
