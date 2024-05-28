namespace QuickStart.JsonSerialization;

public class JsonStringParseTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        string jsonString = @"  
        {  
            ""name"": ""John"",  
            ""age"": 30,  
            ""address"": {  
                ""street"": ""123 Main St"",  
                ""city"": ""New York""  
            },  
            ""phones"": [  
                ""123-456-7890"",  
                ""987-654-3210""  
            ]  
        }";

        var result = ParseJsonToDictionary(jsonString);

        foreach (var kvp in result)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }
    }

    static Dictionary<string, string> ParseJsonToDictionary(string jsonString)
    {
        var dictionary = new Dictionary<string, string>();
        using (JsonDocument doc = JsonDocument.Parse(jsonString))
        {
            ParseElement(doc.RootElement, dictionary, "");
        }
        return dictionary;
    }

    static void ParseElement(JsonElement element, Dictionary<string, string> dictionary, string prefix)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    ParseElement(property.Value, dictionary, $"{prefix}{property.Name}.");
                }
                break;
            case JsonValueKind.Array:
                int index = 0;
                foreach (JsonElement item in element.EnumerateArray())
                {
                    ParseElement(item, dictionary, $"{prefix.TrimEnd('.')}[{index}].");
                    index++;
                }
                break;
            default:
                dictionary[prefix.TrimEnd('.')] = element.ToString();
                break;
        }
    }
}

public class JsonParser
{
    public static void ParseJson(JsonElement element, Dictionary<string, object?> allProperties, string parentKey)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    string key = parentKey == "" ? property.Name : $"{parentKey}.{property.Name}";
                    ParseJson(property.Value, allProperties, key);
                }
                break;

            case JsonValueKind.Array:
                int index = 0;
                foreach (JsonElement arrayElement in element.EnumerateArray())
                {
                    string key = $"{parentKey}[{index}]";
                    ParseJson(arrayElement, allProperties, key);
                    index++;
                }
                break;

            case JsonValueKind.String:
                allProperties[parentKey] = element.GetString();
                break;

            case JsonValueKind.Number:
                if (element.TryGetInt32(out int intValue))
                {
                    allProperties[parentKey] = intValue;
                }
                else if (element.TryGetInt64(out long longValue))
                {
                    allProperties[parentKey] = longValue;
                }
                else if (element.TryGetDouble(out double doubleValue))
                {
                    allProperties[parentKey] = doubleValue;
                }
                break;

            case JsonValueKind.True:
            case JsonValueKind.False:
                allProperties[parentKey] = element.GetBoolean();
                break;

            case JsonValueKind.Null:
                allProperties[parentKey] = null;
                break;
        }
    }
}