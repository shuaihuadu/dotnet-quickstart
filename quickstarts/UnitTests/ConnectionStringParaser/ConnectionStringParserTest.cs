using System.Text.RegularExpressions;

namespace UnitTests.ConnectionStringParaser;

public class ConnectionStringParserTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Parse()
    {
        string connectionString = "Host=myHost;Port=5432;UserName=myUser;Password=my=Passwors;asd;Extra=NotAllowed;";
        var parsedConn = ConnectionStringParser.ParseV2(connectionString);

        foreach (var kvp in parsedConn)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }
}

public static class ConnectionStringParser
{
    public static Dictionary<string, string> ParseV1(string connStr)
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
    public static Dictionary<string, string> ParseV2(string connStr)
    {
        var connDict = new Dictionary<string, string>();
        var allowedKeys = new HashSet<string> { "Host", "Port", "UserName", "Password" };

        int start = 0;
        while (start < connStr.Length)
        {
            int end = connStr.IndexOf(';', start);
            if (end == -1) end = connStr.Length;

            string part = connStr.Substring(start, end - start);
            int separatorIndex = part.IndexOf('=');

            if (separatorIndex != -1)
            {
                string key = part.Substring(0, separatorIndex).Trim();
                string value = part.Substring(separatorIndex + 1).Trim();

                if (allowedKeys.Contains(key))
                {
                    connDict[key] = value;
                }
            }

            start = end + 1;
        }

        return connDict;
    }
}
