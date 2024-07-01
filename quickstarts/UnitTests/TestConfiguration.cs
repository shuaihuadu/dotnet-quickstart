namespace QuickStart;

public sealed class TestConfiguration
{
    private readonly IConfigurationRoot _configurationRoot;

    private static TestConfiguration? _instance;

    public static void Initialize()
    {
        IConfigurationRoot configurationRoot = new ConfigurationBuilder()
            .AddJsonFile(@"D:\appsettings\test_configuration.json", true)
            .Build();

        Initialize(configurationRoot);
    }

    private TestConfiguration(IConfigurationRoot configurationRoot)
    {
        this._configurationRoot = configurationRoot;
    }

    private static void Initialize(IConfigurationRoot configurationRoot)
    {
        _instance = new TestConfiguration(configurationRoot);
    }
    public static AzureOpenAIEmbeddingsConfig AzureOpenAIEmbeddings => LoadSection<AzureOpenAIEmbeddingsConfig>();

    public class AzureOpenAIEmbeddingsConfig
    {
        public string DeploymentName { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
    private static T LoadSection<T>([CallerMemberName] string? caller = null)
    {
        if (_instance == null)
        {
            throw new InvalidOperationException("TestConfiguration must be initialized with a call to Initialize(IConfigurationRoot) before accessing configuration values.");
        }

        if (string.IsNullOrEmpty(caller))
        {
            throw new ArgumentNullException(nameof(caller));
        }

        return _instance._configurationRoot.GetSection(caller).Get<T>() ?? throw new KeyNotFoundException("Configuration not found.");
    }
}