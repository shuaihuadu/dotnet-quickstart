using Aspose.Words;
using Aspose.Words.Saving;
using QuickStart;
using System.Text;

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

    [Fact]
    public void RunConvertToMarkwonWithPageNumber()
    {
        StringBuilder markdownContentBuilder = new();

        string wordDirectory = Path.Join(AppContext.BaseDirectory, "docs", "word");

        Document doc = new(Path.Join(wordDirectory, "test.docx"));

        for (int i = 0; i < doc.PageCount; i++)
        {
            Document pageDocument = doc.ExtractPages(i, 1);

            MarkdownSaveOptions saveOptions = new()
            {
                ForcePageBreaks = true,
                ImagesFolderAlias = $"./imgs/{i}/",
                ImagesFolder = Path.Combine(wordDirectory, "imgs", i.ToString())
            };

            string markdownSavePath = Path.Join(wordDirectory, $"test-{i}.md");

            pageDocument.Save(markdownSavePath, saveOptions);

            string markdownContent = File.ReadAllText(markdownSavePath);

            markdownContentBuilder.AppendLine(markdownContent);

            markdownContentBuilder.AppendLine($"<!-- 转换后的Markdown的页码标识符，当前是第{i + 1}页 -->");

            File.Delete(markdownSavePath);
        }

        File.WriteAllText(Path.Join(wordDirectory, "test.md"), markdownContentBuilder.ToString(), Encoding.UTF8);
    }
}
