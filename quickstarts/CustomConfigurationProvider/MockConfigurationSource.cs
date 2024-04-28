using Microsoft.Extensions.Configuration;

namespace CustomConfigurationProvider;

public sealed class MockConfigurationSource(string connectionString) : IConfigurationSource
{
    public IConfigurationProvider Build(IConfigurationBuilder builder) => new MockConfigurationProvider(connectionString);
}
