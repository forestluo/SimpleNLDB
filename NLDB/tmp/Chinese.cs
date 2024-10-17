using System.Text;
using System.Data.SqlTypes;
using Microsoft.VisualBasic;

public partial class Chinese
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlTraditionalConvert(SqlString strValue)
    {
        // ת����
        return TraditionalConvert(strValue.Value);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlSimplifiedConvert(SqlString strValue)
    {
        // ת����
        return SimplifiedConvert(strValue.Value);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlNarrowConvert(SqlString strValue)
    {
        // ת���
        return NarrowConvert(strValue.Value);
    }

    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString SqlWideConvert(SqlString strValue)
    {
        // תȫ��
        return WideConvert(strValue.Value);
    }

    public static bool IsChinese(char cValue)
    {
        int value = cValue & 0xFFFF;
        // ���ؽ��
        return value >= 0x4E00 && value <= 0x9FA5;
    }

    public static bool IsChinese(string strValue)
    {
        for (int i = 0; i < strValue.Length; i++)
        {
            if (!IsChinese(strValue[i])) return false;
        }
        // ���ؽ��
        return true;
    }

    public static string TraditionalConvert(string strValue)
    {
        // ת����
        return Strings.StrConv(strValue, VbStrConv.TraditionalChinese, 0);
    }

    public static string SimplifiedConvert(string strValue)
    {
        // ת����
        return Strings.StrConv(strValue, VbStrConv.SimplifiedChinese, 0);
    }

    public static string NarrowConvert(string strValue)
    {
        // ȫ��ת���
        //return Strings.StrConv(strValue.Value, VbStrConv.Narrow, 0);

        // �����ַ���
        StringBuilder sb = new StringBuilder(strValue.Length);
        // ѭ������
        foreach (char cValue in strValue)
        {
            // ���⴦��
            if (cValue == 12288) sb.Append(' ');
            // ����ַ���Χ
            else if (cValue < 65281) sb.Append(cValue);
            else if (cValue > 65374) sb.Append(cValue);
            // ת���ɰ��
            else sb.Append((char)(cValue - 65248));
        }
        // ���ؽ��
        return sb.ToString();
    }

    public static string WideConvert(string strValue)
    {
        // ���תȫ��
        //return Strings.StrConv(strValue.Value, VbStrConv.Wide, 0);

        // �����ַ���
        StringBuilder sb = new StringBuilder(strValue.Length);
        // ѭ������
        foreach (char cValue in strValue)
        {
            // ���⴦��
            if (cValue == 32) sb.Append((char)12288);
            // ����ַ���Χ
            else if (cValue < 33) sb.Append(cValue);
            else if (cValue > 126) sb.Append(cValue);
            // ת����ȫ��
            else sb.Append((char)(cValue + 65248));
        }
        // ���ؽ��
        return sb.ToString();
    }
}
