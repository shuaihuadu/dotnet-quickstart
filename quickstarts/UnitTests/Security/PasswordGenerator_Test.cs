using System.Security.Cryptography;
using System.Text;

namespace UnitTests.Security;

public class PasswordGenerator_Test(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        this.Output.WriteLine(GeneratePassword1(8));
        this.Output.WriteLine(GeneratePassword2(8));
        this.Output.WriteLine(GeneratePassword3(8));
        this.Output.WriteLine(RandomPasswordGenerator.GeneratePassword(8));

    }

    public static string GeneratePassword1(int length)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
        StringBuilder result = new StringBuilder(length);
        using (RNGCryptoServiceProvider rng = new())
        {
            byte[] uintBuffer = new byte[sizeof(uint)];

            while (length-- > 0)
            {
                rng.GetBytes(uintBuffer);
                uint num = BitConverter.ToUInt32(uintBuffer, 0);
                result.Append(validChars[(int)(num % (uint)validChars.Length)]);
            }
        }
        return result.ToString();
    }

    public static string GeneratePassword2(int length)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
        StringBuilder result = new StringBuilder(length);
        byte[] randomBytes = new byte[length];

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        foreach (byte b in randomBytes)
        {
            result.Append(validChars[b % validChars.Length]);
        }

        return result.ToString();
    }

    public static string GeneratePassword3(int length)
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
        var random = new Random();
        string password = new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
        return password;
    }
}
