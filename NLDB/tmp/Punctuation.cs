public partial class Punctuation
{
    public static bool IsPunctuation(char cValue)
    {
        // ���ؽ��
        // ע�⣺��������Ҫ�ָ�������
        return IsMajorSplitter(cValue) || IsPairSplitter(cValue);
    }

    public static bool HasPunctuation(string strValue)
    {
        for (int i = 0; i < strValue.Length; i++)
        {
            if (IsPunctuation(strValue[i])) return true;
        }
        // ���ؽ��
        return false;
    }

    // ��Ҫ�ָ��
    private readonly static
        char[] MINOR_SPLITTERS = { '��', '��', '��', '��' };
    // ��Ҫ�ָ��
    private readonly static
        char[] MAJOR_SPLITTERS = { '��', '��', '��', '��', '��', '��' };

    public static bool IsMinorSplitter(char cValue)
    {
        foreach (char item in MINOR_SPLITTERS)
        {
            if (cValue == item) return true;
        }
        // ���ؽ��
        return false;
    }

    public static bool IsMajorSplitter(char cValue)
    {
        foreach (char item in MAJOR_SPLITTERS)
        {
            if (cValue == item) return true;
        }
        // ���ؽ��
        return false;
    }

    public static bool HasMajorSplitter(string strValue)
    {
        foreach (char cValue in strValue)
        {
            if (IsMajorSplitter(cValue)) return true;
        }
        // ���ؽ��
        return false;
    }

    // �ɶԷָ��
    private readonly static
        char[] PAIR_SPLITTERS = { '��', '��', '��', '��', '��', '��', 
                                  '��', '��', '��', '��', '��', '��',
                                  '��', '��', '��', '��', '��', '��',
                                  '��', '��', '��', '��', '�v', '�w',
                                  '�x', '�y', '�z', '�{', '��', '��',
                                  // ����ַ�
                                  /*'(', ')', '[', ']', '{', '}', '<', '>'*/};

    public static bool IsPairSplitter(char cValue)
    {
        foreach (char item in PAIR_SPLITTERS)
        {
            // ���ؽ��
            if (cValue == item) return true;
        }
        // ���ؽ��
        return false;
    }

    public static string GetPairStarts()
    {
        return "��|��|��|��|��|��|��|��|��|��|��|�v|�x|�z|��";
    }

    public static char GetPairEnd(char cValue)
    {
        for (int i = 0; i < PAIR_SPLITTERS.Length; i += 2)
        {
            // ���ؽ��
            if (PAIR_SPLITTERS[i] == cValue)
                    return PAIR_SPLITTERS[i + 1];
        }
        // ���ؽ��
        return cValue;
    }

    public static bool HasPairSplitter(string strValue)
    {
        foreach (char cValue in strValue)
        {
            if (IsPairSplitter(cValue)) return true;
        }
        // ���ؽ��
        return false;
    }

    public static bool IsPairEnd(char cValue)
    {
        for (int i = 1;i < PAIR_SPLITTERS.Length;i += 2)
        {
            // ���ؽ��
            if (PAIR_SPLITTERS[i] == cValue) return true;
        }
        // ���ؽ��
        return false;
    }

    public static bool IsPairStart(char cValue)
    {
        for (int i = 0; i < PAIR_SPLITTERS.Length; i += 2)
        {
            // ���ؽ��
            if (PAIR_SPLITTERS[i] == cValue) return true;
        }
        // ���ؽ��
        return false;
    }

    public static bool IsPairMatched(char cStart, char cEnd)
    {
        for (int i = 0; i < PAIR_SPLITTERS.Length; i += 2)
        {
            // ���ؽ��
            if (PAIR_SPLITTERS[i] == cStart &&
                PAIR_SPLITTERS[i + 1] == cEnd) return true;
        }
        // ���ؽ��
        return false;
    }
}