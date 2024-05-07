using Aspose.Cells;

namespace UnitTests.AsposeNet;

public class AsposeCellsTest
{
    [Fact]
    public void RunConvertToWord()
    {
        string xlsx = Path.Join(AppContext.BaseDirectory, "docs", "xlsx", "1.xlsx");

        LoadOptions loadOptions = new();

        loadOptions.SetPaperSize(PaperSizeType.PaperA4);
        loadOptions.IgnoreNotPrinted = true;

        var workbook = new Workbook(xlsx, loadOptions);
        workbook.Save("1.docx");
    }
}
