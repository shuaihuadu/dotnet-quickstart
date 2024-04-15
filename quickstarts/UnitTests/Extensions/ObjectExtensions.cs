
namespace QuickStart.Extensions;

public static class ObjectExtensions
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public static string AsJson(this object obj)
    {
        return JsonSerializer.Serialize(obj, jsonSerializerOptions);
    }

    public static T? FromJson<T>(this string json)
    {
        try
        {
            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
        }
        catch (JsonException)
        {
            return default;
        }
    }
}
