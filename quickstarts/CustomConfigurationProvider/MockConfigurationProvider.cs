using Microsoft.Extensions.Configuration;

namespace CustomConfigurationProvider;

public class MockConfigurationProvider(string? connectionString) : ConfigurationProvider
{
    public override void Load()
    {
        MockDbContext dbContext = new(connectionString ?? string.Empty);

        Data = dbContext.MockSettings.Any()
            ? dbContext.MockSettings.ToDictionary(static c => c.Id, static c => c.Value, StringComparer.OrdinalIgnoreCase)
            : CreateAndSaveDefaultValues(dbContext);
    }

    private Dictionary<string, string?> CreateAndSaveDefaultValues(MockDbContext dbContext)
    {
        Dictionary<string, string?> options = new(StringComparer.OrdinalIgnoreCase)
        {
            ["MockOptions:AgentId"] = "10001",
            ["MockOptions:FlowId"] = Guid.NewGuid().ToString()
        };

        dbContext.SaveChanges(options.Select(static kvp => new Settings(kvp.Key, kvp.Value)));

        return options;
    }
}
