public partial class Lowercase
{
    public static bool IsLowercase(char cValue)
    {
        // ���ؽ��
        return cValue >= 97 && cValue <= 122;
    }

    public static char GetLowercase(int index)
    {
        // ���ؽ��
        return index >= 1 && index <= 26 ? (char)(96 + index) : '?';
    }
}
