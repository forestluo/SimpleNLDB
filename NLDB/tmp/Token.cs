public partial class Token
{
    public static bool IsDigit(char cToken)
    {
        // ���ؽ��
        return cToken >= '0' && cToken <= '9';
    }

    //public static int GetTokenCount(string strValue, char cToken)
    //{
    //    int count = 0;
    //    // ѭ������
    //    for (int i = 0; i < strValue.Length; i++)
    //    {
    //        if (strValue[i] == cToken) count++;
    //    }
    //    // ���ؽ��
    //    return count;
    //}

    public static string GetDescription(char cToken)
    {
        if (cToken <= 0x007F)
        {
            return "C0���Ʒ�������������";
        }
        else if (cToken <= 0x00FF) { return "C1���Ʒ��������Ĳ���-1"; }
        else if (cToken <= 0x017F) { return "��������չ-A"; }
        else if (cToken <= 0x024F) { return "��������չ-B"; }
        else if (cToken <= 0x02AF) { return "����������չ"; }
        else if (cToken <= 0x02FF) { return "�հ�������ĸ"; }
        else if (cToken <= 0x036F) { return "����ö�������"; }
        else if (cToken <= 0x03FF) { return "ϣ���ļ���������"; }
        else if (cToken <= 0x04FF) { return "�������ĸ"; }
        else if (cToken <= 0x052F) { return "�������ĸ����"; }
        else if (cToken <= 0x058F) { return "����������"; }
        else if (cToken <= 0x05FF) { return "ϣ������"; }
        else if (cToken <= 0x06FF) { return "��������"; }
        else if (cToken <= 0x074F) { return "��������"; }
        else if (cToken <= 0x077F) { return "�������Ĳ���"; }
        else if (cToken <= 0x07BF) { return "���������"; }
        else if (cToken <= 0x07FF) { return "���������Z��"; }
        else if (cToken <= 0x085F) { return "��ά˹���Ｐ����ά��"; }
        else if (cToken <= 0x087F) { return "Mandaic"; }
        else if (cToken <= 0x08AF) { return "����������"; }
        else if (cToken <= 0x097F) { return "�������"; }
        else if (cToken <= 0x09FF) { return "�ϼ�����"; }
        else if (cToken <= 0x0A7F) { return "���˽���"; }
        else if (cToken <= 0x0AFF) { return "�ż�������"; }
        else if (cToken <= 0x0B7F) { return "��������"; }
        else if (cToken <= 0x0BFF) { return "̩�׶���"; }
        else if (cToken <= 0x0C7F) { return "̩¬����"; }
        else if (cToken <= 0x0CFF) { return "���ɴ���"; }
        else if (cToken <= 0x0D7F) { return "����ά����"; }
        else if (cToken <= 0x0DFF) { return "ɮ٤����"; }
        else if (cToken <= 0x0E7F) { return "̩��"; }
        else if (cToken <= 0x0EFF) { return "������"; }
        else if (cToken <= 0x0FFF) { return "����"; }
        else if (cToken <= 0x109F) { return "�����"; }
        else if (cToken <= 0x10FF) { return "��³������"; }
        else if (cToken <= 0x11FF) { return "������"; }
        else if (cToken <= 0x137F) { return "�����������"; }
        else if (cToken <= 0x139F) { return "����������ﲹ��"; }
        else if (cToken <= 0x13FF) { return "���޻���"; }
        else if (cToken <= 0x167F) { return "ͳһ���ô�����������"; }
        else if (cToken <= 0x169F) { return "ŷ����ĸ"; }
        else if (cToken <= 0x16FF) { return "������"; }
        else if (cToken <= 0x171F) { return "��������"; }
        else if (cToken <= 0x173F) { return "Hanun��o"; }
        else if (cToken <= 0x175F) { return "Buhid"; }
        else if (cToken <= 0x177F) { return "Tagbanwa"; }
        else if (cToken <= 0x17FF) { return "������"; }
        else if (cToken <= 0x18AF) { return "�ɹ���"; }
        else if (cToken <= 0x18FF) { return "Cham"; }
        else if (cToken <= 0x194F) { return "Limbu"; }
        else if (cToken <= 0x197F) { return "�º�̩��"; }
        else if (cToken <= 0x19DF) { return "�´�����"; }
        else if (cToken <= 0x19FF) { return "������Ǻ�"; }
        else if (cToken <= 0x1A1F) { return "Buginese"; }
        else if (cToken <= 0x1A5F) { return "Batak"; }
        else if (cToken <= 0x1AEF) { return "Lanna"; }
        else if (cToken <= 0x1B7F) { return "������"; }
        else if (cToken <= 0x1BB0) { return "������"; }
        else if (cToken <= 0x1BFF) { return "Pahawh Hmong"; }
        else if (cToken <= 0x1C4F) { return "�ײ�����"; }
        else if (cToken <= 0x1C7F) { return "Ol Chiki"; }
        else if (cToken <= 0x1CDF) { return "�����ն���"; }
        else if (cToken <= 0x1D7F) { return "����ѧ��չ"; }
        else if (cToken <= 0x1DBF) { return "����ѧ��չ����"; }
        else if (cToken <= 0x1DFF) { return "����ö������Ų���"; }
        else if (cToken <= 0x1EFF) { return "���������丽��"; }
        else if (cToken <= 0x1FFF) { return "ϣ��������"; }
        else if (cToken <= 0x206F) { return "���ñ��"; }
        else if (cToken <= 0x209F) { return "�ϱ꼰�±�"; }
        // ���ҷ���
        else if (cToken <= 0x20CF) { return "���ҷ���"; }
        else if (cToken <= 0x20FF) { return "����üǺ�"; }
        else if (cToken <= 0x214F) { return "��ĸʽ����"; }
        else if (cToken <= 0x218F) { return "������ʽ"; }
        else if (cToken <= 0x21FF) { return "��ͷ"; }
        else if (cToken <= 0x22FF) { return "��ѧ�����"; }
        else if (cToken <= 0x23FF) { return "���ҵ����"; }
        else if (cToken <= 0x243F) { return "����ͼƬ"; }
        else if (cToken <= 0x245F) { return "��ѧʶ���"; }
        else if (cToken <= 0x24FF) { return "���ʽ��ĸ����"; }
        else if (cToken <= 0x257F) { return "�Ʊ��"; }
        else if (cToken <= 0x259F) { return "����Ԫ��"; }
        else if (cToken <= 0x25FF) { return "����ͼ��"; }
        else if (cToken <= 0x26FF) { return "�������"; }
        else if (cToken <= 0x27BF) { return "ӡˢ����"; }
        else if (cToken <= 0x27EF) { return "������ѧ����-A"; }
        else if (cToken <= 0x27FF) { return "׷�Ӽ�ͷ-A"; }
        else if (cToken <= 0x28FF) { return "ä�ĵ���ģ��"; }
        else if (cToken <= 0x297F) { return "׷�Ӽ�ͷ-B"; }
        else if (cToken <= 0x29FF) { return "������ѧ����-B"; }
        else if (cToken <= 0x2AFF) { return "׷����ѧ�����"; }
        else if (cToken <= 0x2BFF) { return "������źͼ�ͷ"; }
        else if (cToken <= 0x2C5F) { return "����������ĸ"; }
        else if (cToken <= 0x2C7F) { return "��������չ-C"; }
        else if (cToken <= 0x2CFF) { return "�Ű�����"; }
        else if (cToken <= 0x2D2F) { return "��³�����ﲹ��"; }
        else if (cToken <= 0x2D7F) { return "�������"; }
        else if (cToken <= 0x2DDF) { return "�������������չ"; }
        else if (cToken <= 0x2E7F) { return "׷�ӱ��"; }
        else if (cToken <= 0x2EFF) { return "CJK ���ײ���"; }
        else if (cToken <= 0x2FDF) { return "�����ֵ䲿��"; }
        else if (cToken <= 0x2FFF) { return "��������������"; }
        else if (cToken <= 0x303F) { return "CJK ���źͱ��"; }
        else if (cToken <= 0x309F) { return "����ƽ����"; }
        else if (cToken <= 0x30FF) { return "����Ƭ����"; }
        else if (cToken <= 0x312F) { return "ע����ĸ"; }
        else if (cToken <= 0x318F) { return "�����ļ�����ĸ"; }
        else if (cToken <= 0x319F) { return "������ע�ͱ�־"; }
        else if (cToken <= 0x31BF) { return "ע����ĸ��չ"; }
        else if (cToken <= 0x31EF) { return "CJK �ʻ�"; }
        else if (cToken <= 0x31FF) { return "����Ƭ����������չ"; }
        else if (cToken <= 0x32FF) { return "���ʽ CJK ���ֺ��·�"; }
        else if (cToken <= 0x33FF) { return "CJK ����"; }
        else if (cToken <= 0x4DBF) { return "CJK ͳһ���������չ A"; }
        else if (cToken <= 0x4DFF) { return "�׾���ʮ���Է���"; }
        // ��������
        else if (cToken <= 0x9FBF) { return "CJK ͳһ�������"; }
        else if (cToken <= 0xA48F) { return "��������"; }
        else if (cToken <= 0xA4CF) { return "�����ָ�"; }
        else if (cToken <= 0xA61F) { return "Vai"; }
        else if (cToken <= 0xA6FF) { return "ͳһ���ô����������ڲ���"; }
        else if (cToken <= 0xA71F) { return "����������ĸ"; }
        else if (cToken <= 0xA7FF) { return "��������չ-D"; }
        else if (cToken <= 0xA82F) { return "Syloti Nagri"; }
        else if (cToken <= 0xA87F) { return "��˼����"; }
        else if (cToken <= 0xA8DF) { return "Saurashtra"; }
        else if (cToken <= 0xA97F) { return "צ����"; }
        else if (cToken <= 0xA9DF) { return "Chakma"; }
        else if (cToken <= 0xAA3F) { return "Varang Kshiti"; }
        else if (cToken <= 0xAA6F) { return "Sorang Sompeng"; }
        else if (cToken <= 0xAADF) { return "Newari"; }
        else if (cToken <= 0xAB5F) { return "Խ�ϴ���"; }
        else if (cToken <= 0xABA0) { return "Kayah Li"; }
        else if (cToken <= 0xD7AF) { return "����������"; }
        // ���ɼ��ַ�
        else if (cToken <= 0xDBFF) { return "High-half zone of UTF-16"; }
        // ���ɼ��ַ�
        else if (cToken <= 0xDFFF) { return "Low-half zone of UTF-16"; }
        else if (cToken <= 0xF8FF) { return "����ʹ������"; }
        else if (cToken <= 0xFAFF) { return "CJK ������������"; }
        else if (cToken <= 0xFB4F) { return "��ĸ���_��ʽ"; }
        else if (cToken <= 0xFDFF) { return "���������_��ʽA"; }
        else if (cToken <= 0xFE0F) { return "����ѡ���"; }
        else if (cToken <= 0xFE1F) { return "������ʽ"; }
        else if (cToken <= 0xFE2F) { return "����ð����"; }
        else if (cToken <= 0xFE4F) { return "CJK ������ʽ"; }
        else if (cToken <= 0xFE6F) { return "С�ͱ�����ʽ"; }
        else if (cToken <= 0xFEFF) { return "���������_��ʽB"; }
        else if (cToken <= 0xFFEF) { return "���ͼ�ȫ����ʽ"; }
        // ���ɼ��ַ�
        else if (cToken <= 0xFFFF) { return "����"; }
        // ���ؽ��
        return null;
    }
}