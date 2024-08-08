using System.Security.Cryptography;
using System.Text;

namespace UnitTests.Security;

public class RandomPasswordGenerator
{
    public static string GeneratePassword(int length)
    {
        const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        string allChars = upperCase + lowerCase + digits + specialChars;

        StringBuilder password = new StringBuilder();

        RandomNumberGenerator rng = RandomNumberGenerator.Create();

        // 确保密码包含至少一个大写字母、一个小写字母、一个数字和一个特殊符号  
        password.Append(upperCase[GetRandomNumber(rng, upperCase.Length)]);
        password.Append(lowerCase[GetRandomNumber(rng, lowerCase.Length)]);
        password.Append(digits[GetRandomNumber(rng, digits.Length)]);
        password.Append(specialChars[GetRandomNumber(rng, specialChars.Length)]);

        // 填充剩余的密码长度  
        for (int i = 4; i < length; i++)
        {
            password.Append(allChars[GetRandomNumber(rng, allChars.Length)]);
        }

        // 打乱密码字符顺序  
        return ShuffleString(password.ToString(), rng);
    }

    static int GetRandomNumber(RandomNumberGenerator rng, int max)
    {
        byte[] randomNumber = new byte[4];
        rng.GetBytes(randomNumber);
        int value = BitConverter.ToInt32(randomNumber, 0) & int.MaxValue;
        return value % max;
    }

    static string ShuffleString(string input, RandomNumberGenerator rng)
    {
        char[] array = input.ToCharArray();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = GetRandomNumber(rng, i + 1);
            char temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        return new string(array);
    }
}