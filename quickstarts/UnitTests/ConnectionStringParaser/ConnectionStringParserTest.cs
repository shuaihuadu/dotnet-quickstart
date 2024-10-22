using System.Text.RegularExpressions;

namespace UnitTests.ConnectionStringParaser;

public class ConnectionStringParserTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Parse()
    {
        string connectionString = "Host=myHost;Port=5432;UserName=myUser;Password=my=Password;Extra=NotAllowed;";
        var parsedConn = ConnectionStringParser.Parse(connectionString);

        foreach (var kvp in parsedConn)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}

public static class ConnectionStringParser
{
    public static Dictionary<string, string> Parse(string connStr)
    {
        var connDict = new Dictionary<string, string>();

        // 使用正则表达式匹配键值对  
        Regex regex = new(@"(?<key>[^=;]+)=(?<value>[^;]*);?");
        var matches = regex.Matches(connStr);

        // 允许的键  
        var allowedKeys = new HashSet<string> { "Host", "Port", "UserName", "Password" };

        foreach (Match match in matches)
        {
            var key = match.Groups["key"].Value.Trim();
            var value = match.Groups["value"].Value.Trim();

            if (allowedKeys.Contains(key) && !string.IsNullOrEmpty(key))
            {
                connDict[key] = value;
            }
        }

        return connDict;
    }
}
