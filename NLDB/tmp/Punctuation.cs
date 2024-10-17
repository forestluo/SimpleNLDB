public partial class Punctuation
{
    public static bool IsPunctuation(char cValue)
    {
        // 返回结果
        // 注意：不包含次要分隔符！！
        return IsMajorSplitter(cValue) || IsPairSplitter(cValue);
    }

    public static bool HasPunctuation(string strValue)
    {
        for (int i = 0; i < strValue.Length; i++)
        {
            if (IsPunctuation(strValue[i])) return true;
        }
        // 返回结果
        return false;
    }

    // 次要分割符
    private readonly static
        char[] MINOR_SPLITTERS = { '、', '—', '…', '·' };
    // 主要分割符
    private readonly static
        char[] MAJOR_SPLITTERS = { '。', '，', '；', '：', '？', '！' };

    public static bool IsMinorSplitter(char cValue)
    {
        foreach (char item in MINOR_SPLITTERS)
        {
            if (cValue == item) return true;
        }
        // 返回结果
        return false;
    }

    public static bool IsMajorSplitter(char cValue)
    {
        foreach (char item in MAJOR_SPLITTERS)
        {
            if (cValue == item) return true;
        }
        // 返回结果
        return false;
    }

    public static bool HasMajorSplitter(string strValue)
    {
        foreach (char cValue in strValue)
        {
            if (IsMajorSplitter(cValue)) return true;
        }
        // 返回结果
        return false;
    }

    // 成对分割符
    private readonly static
        char[] PAIR_SPLITTERS = { '“', '”', '（', '）', '《', '》', 
                                  '‘', '’', '【', '】', '〈', '〉',
                                  '「', '」', '『', '』', '〔', '〕',
                                  '〖', '〗', '〝', '〞', '﹙', '﹚',
                                  '﹛', '﹜', '﹝', '﹞', '﹤', '﹥',
                                  // 半角字符
                                  /*'(', ')', '[', ']', '{', '}', '<', '>'*/};

    public static bool IsPairSplitter(char cValue)
    {
        foreach (char item in PAIR_SPLITTERS)
        {
            // 返回结果
            if (cValue == item) return true;
        }
        // 返回结果
        return false;
    }

    public static string GetPairStarts()
    {
        return "“|（|《|‘|【|〈|「|『|〔|〖|〝|﹙|﹛|﹝|﹤";
    }

    public static char GetPairEnd(char cValue)
    {
        for (int i = 0; i < PAIR_SPLITTERS.Length; i += 2)
        {
            // 返回结果
            if (PAIR_SPLITTERS[i] == cValue)
                    return PAIR_SPLITTERS[i + 1];
        }
        // 返回结果
        return cValue;
    }

    public static bool HasPairSplitter(string strValue)
    {
        foreach (char cValue in strValue)
        {
            if (IsPairSplitter(cValue)) return true;
        }
        // 返回结果
        return false;
    }

    public static bool IsPairEnd(char cValue)
    {
        for (int i = 1;i < PAIR_SPLITTERS.Length;i += 2)
        {
            // 返回结果
            if (PAIR_SPLITTERS[i] == cValue) return true;
        }
        // 返回结果
        return false;
    }

    public static bool IsPairStart(char cValue)
    {
        for (int i = 0; i < PAIR_SPLITTERS.Length; i += 2)
        {
            // 返回结果
            if (PAIR_SPLITTERS[i] == cValue) return true;
        }
        // 返回结果
        return false;
    }

    public static bool IsPairMatched(char cStart, char cEnd)
    {
        for (int i = 0; i < PAIR_SPLITTERS.Length; i += 2)
        {
            // 返回结果
            if (PAIR_SPLITTERS[i] == cStart &&
                PAIR_SPLITTERS[i + 1] == cEnd) return true;
        }
        // 返回结果
        return false;
    }
}