namespace UlongArray2ByteArray;

class Program
{
    static void Main(string[] args)
    {
        ulong[] digits1 = [0x76543210, 0xFEDCBA98, 0x01020304];
        Console.WriteLine($"{digits1[0]:x8} {digits1[1]:x8} {digits1[2]:x8}");

        byte[] bytes = AnySizeIntegerDigitsToBytes(digits1);

        string bytes_str = "";
        foreach (var b in bytes)
        {
            bytes_str += $"{b:x2}";
        }

        Console.WriteLine(bytes_str);

        ulong[] digits2 = AnySizeIntegerDigitsFromBytes(bytes);
        Console.WriteLine($"{digits2[0]:x8} {digits2[1]:x8} {digits2[2]:x8}");
    }

    public static byte[] AnySizeIntegerDigitsToBytes(ulong[] digits)
    {
        byte[] result = new byte[digits.Length * sizeof(uint)];

        for (int i = 0; i < digits.Length; i++)
        {
            byte[] digitBytes = BitConverter.GetBytes((uint)digits[i]);
            Array.Copy(digitBytes, 0, result, i * sizeof(uint), digitBytes.Length);
        }

        return result;
    }

    public static ulong[] AnySizeIntegerDigitsFromBytes(byte[] bytes)
    {
        int digitsLen = bytes.Length / sizeof(uint);
        if (digitsLen * sizeof(uint) != bytes.Length)
        {
            throw new ArgumentException($"{nameof(bytes)} length is not a multiple of sizeof(uint).");
        }

        ulong[] digits = new ulong[digitsLen];
        for (int i = 0; i < digitsLen; i++)
        {
            uint digit = BitConverter.ToUInt32(bytes, i * sizeof(uint));
            digits[i] = digit;
        }

        return digits;
    }
}
