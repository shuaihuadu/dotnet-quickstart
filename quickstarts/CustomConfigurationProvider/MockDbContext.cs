using System.Collections.Concurrent;

namespace CustomConfigurationProvider;

internal class MockDbContext
{
    private readonly ConcurrentDictionary<string, string?> _entities;

    public MockDbContext(string connectionString)
    {
        this._entities = new ConcurrentDictionary<string, string?>();
    }

    public List<Settings> MockSettings => [.. _entities.Select(static KeyValuePair => new Settings(KeyValuePair.Key, KeyValuePair.Value))];

    public void SaveChanges(IEnumerable<Settings> settings)
    {
        foreach (var option in settings)
        {
            this._entities.TryAdd(option.Id, option.Value);
        }
    }
}
