using Aspose.Words;
using Aspose.Words.Saving;
using QuickStart;

namespace UnitTests.AsposeNet;

public class AsposeWordsTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void RunConvertToMarkdown()
    {
        string wordDirectory = Path.Join(AppContext.BaseDirectory, "docs", "word");

        Document doc = new(Path.Join(wordDirectory, "test.docx"));

        MarkdownSaveOptions saveOptions = new()
        {
            ForcePageBreaks = true,
            ImagesFolderAlias = "./imgs/",
            ImagesFolder = Path.Combine(wordDirectory, "imgs")
        };

        doc.Save(Path.Join(wordDirectory, "test.md"), saveOptions);
    }

    [Fact]
    public void RunConvertToPdf()
    {
        string wordDirectory = Path.Join(AppContext.BaseDirectory, "docs", "word");

        Document doc = new(Path.Join(wordDirectory, "test.docx"));

        doc.Save(Path.Join(wordDirectory, "test.pdf"));
    }
}
