using Microsoft.SemanticKernel;

namespace UnitTests.SK;

public class SemanticKernel_Tests(ITestOutputHelper output) : BaseTest(output)
{

    private readonly KernelArguments _arguments = new KernelArguments()
    {
        ["p1"] = "p1_content",
        ["p2"] = "p2_content"
    };

    [Fact]
    public async Task RunAsync()
    {
        KernelPromptTemplateFactory factory = new KernelPromptTemplateFactory(null);

        var template = "{{$p1}}.{{$p2}}";

        var target = factory.Create(new PromptTemplateConfig(template));
        // Act
        var result = await target.RenderAsync(new Kernel(), this._arguments);

        Assert.NotNull(result);
        Assert.Equal("p1_content.p2_content", result);
    }
}
