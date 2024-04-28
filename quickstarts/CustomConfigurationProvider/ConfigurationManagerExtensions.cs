using Microsoft.Extensions.Configuration;

namespace CustomConfigurationProvider;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddMockConfiguration(this ConfigurationManager manager)
    {
        string connectionString = "connection string";

        IConfigurationBuilder builder = manager;

        builder.Add(new MockConfigurationSource(connectionString));

        return manager;
    }
}
