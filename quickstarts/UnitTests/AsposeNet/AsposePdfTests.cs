using Aspose.Pdf;

namespace UnitTests.AsposeNet;

public class AsposePdfTests
{
    [Fact]
    public void RunConvertToMarkdown()
    {
        // 先将pdf转换为word
        // 再将word转换为markdown
        string wordDirectory = Path.Join(AppContext.BaseDirectory, "docs", "pdf");

        Document doc = new(Path.Join(wordDirectory, "test.pdf"));

        doc.Save("test.docx", SaveFormat.Doc);

        var outputDocument = new Aspose.Words.Document(Path.Join(AppContext.BaseDirectory, "test.docx"));

        outputDocument.Save(Path.Join(wordDirectory, "test.md"));
    }
}
