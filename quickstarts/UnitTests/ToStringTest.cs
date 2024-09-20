using System.Globalization;
using System.Text;

namespace UnitTests;

public class ToStringTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void ToStringWithNumberFormatInfoTest()
    {
        WriteLine(3.ToString(new NumberFormatInfo()));
    }


    [Fact]
    public void TestBase64()
    {
        // 多行字符串  
        string multiLineString = @"
{
  ""user_name"": ""string"",
  ""password"": ""string""
}";

        //// 将字符串转换为字节数组  
        //byte[] bytes = Encoding.UTF8.GetBytes(multiLineString);

        //// 将字节数组转换为Base64字符串  
        //string base64String = Convert.ToBase64String(bytes);

        //// 输出Base64字符串  
        //this.Console.WriteLine("Base64编码后的字符串:");
        //this.Console.WriteLine(base64String);

        // 如果需要解码回原始字符串  
        byte[] decodedBytes = Convert.FromBase64String("ew0KICAidXNlcm5hbWUiOiAic3RyaW5nIiwNCiAgInBhc3N3b3JkIjogInN0cmluZyINCn0=");
        string decodedString = Encoding.UTF8.GetString(decodedBytes);

        // 输出解码后的字符串  
        this.Console.WriteLine("\n解码后的字符串:");
        this.Console.WriteLine(decodedString);
    }

    [Fact]
    public void TestRemoveNewLine()
    {
        string multiLineString = @"This is a test string.  
  
With some empty lines.  
  
And some more text.";

        // Split the string into lines  
        var lines = multiLineString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        // Filter out empty lines  
        var nonEmptyLines = lines.Where(line => !string.IsNullOrWhiteSpace(line));

        // Join the non-empty lines back into a single string  
        string result = string.Join(string.Empty, nonEmptyLines);

        Console.WriteLine("Original String:");
        Console.WriteLine(multiLineString);
        Console.WriteLine();
        Console.WriteLine("String without empty lines:");
        Console.WriteLine(result);
    }
}
