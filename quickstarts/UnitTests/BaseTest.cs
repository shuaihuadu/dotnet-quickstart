namespace QuickStart;

public abstract class BaseTest
{
    protected ITestOutputHelper Output { get; }

    protected List<string> SimulatedInputText = [];

    protected int SimulatedInputTextIndex = 0;

    public BaseTest Console => this;

    protected BaseTest(ITestOutputHelper output)
    {
        this.Output = output;

        LoadUserSecrets();
    }

    private void LoadUserSecrets()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", true)
            .Build();

        TestConfiguration.Initialize(configuration);
    }

    public void WriteLine(object? target = null)
    {
        this.Output.WriteLine(target?.ToString() ?? string.Empty);
    }

    public void WriteLine(string? format, params object?[] args) => this.Output.WriteLine(format);// ?? string.Empty, args);

    public void Write(object? target = null)
    {
        this.Output.WriteLine(target?.ToString() ?? string.Empty);
    }

    public string? ReadLine()
    {
        if (SimulatedInputTextIndex < SimulatedInputText.Count)
        {
            return SimulatedInputText[SimulatedInputTextIndex++];
        }

        return null;
    }
}