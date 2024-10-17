using System.Text;
using System.Data.SqlTypes;
using Microsoft.VisualBasic;

public partial class Chinese
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlTraditionalConvert(SqlString strValue)
    {
        // 转繁体
        return TraditionalConvert(strValue.Value);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlSimplifiedConvert(SqlString strValue)
    {
        // 转简体
        return SimplifiedConvert(strValue.Value);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlNarrowConvert(SqlString strValue)
    {
        // 转半角
        return NarrowConvert(strValue.Value);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlWideConvert(SqlString strValue)
    {
        // 转全角
        return WideConvert(strValue.Value);
    }

    public static bool IsChinese(char cValue)
    {
        int value = cValue & 0xFFFF;
        // 返回结果
        return value >= 0x4E00 && value <= 0x9FA5;
    }

    public static bool IsChinese(string strValue)
    {
        for (int i = 0; i < strValue.Length; i++)
        {
            if (!IsChinese(strValue[i])) return false;
        }
        // 返回结果
        return true;
    }

    public static string TraditionalConvert(string strValue)
    {
        // 转繁体
        return Strings.StrConv(strValue, VbStrConv.TraditionalChinese, 0);
    }

    public static string SimplifiedConvert(string strValue)
    {
        // 转简体
        return Strings.StrConv(strValue, VbStrConv.SimplifiedChinese, 0);
    }

    public static string NarrowConvert(string strValue)
    {
        // 全角转半角
        //return Strings.StrConv(strValue.Value, VbStrConv.Narrow, 0);

        // 创建字符串
        StringBuilder sb = new StringBuilder(strValue.Length);
        // 循环处理
        foreach (char cValue in strValue)
        {
            // 特殊处理
            if (cValue == 12288) sb.Append(' ');
            // 检查字符范围
            else if (cValue < 65281) sb.Append(cValue);
            else if (cValue > 65374) sb.Append(cValue);
            // 转换成半角
            else sb.Append((char)(cValue - 65248));
        }
        // 返回结果
        return sb.ToString();
    }

    public static string WideConvert(string strValue)
    {
        // 半角转全角
        //return Strings.StrConv(strValue.Value, VbStrConv.Wide, 0);

        // 创建字符串
        StringBuilder sb = new StringBuilder(strValue.Length);
        // 循环处理
        foreach (char cValue in strValue)
        {
            // 特殊处理
            if (cValue == 32) sb.Append((char)12288);
            // 检查字符范围
            else if (cValue < 33) sb.Append(cValue);
            else if (cValue > 126) sb.Append(cValue);
            // 转换成全角
            else sb.Append((char)(cValue + 65248));
        }
        // 返回结果
        return sb.ToString();
    }
}
