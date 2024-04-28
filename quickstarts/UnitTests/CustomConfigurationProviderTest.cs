namespace UnitTests;

public class CustomConfigurationProviderTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void TestConfiguration()
    {
        IServiceCollection services = new ServiceCollection();

        ConfigurationManager configuration = new();

        configuration.AddMockConfiguration();

        IConfigurationSection section = configuration.GetSection("MockOptions");

        services.Configure<MockOptions>(section);

        IServiceProvider serviceProvider = services.BuildServiceProvider();

        IOptions<MockOptions> options = serviceProvider.GetRequiredService<IOptions<MockOptions>>();

        WriteLine(options.Value?.AgentId);
        WriteLine(options.Value?.FlowId);
    }
}