using ClosedXML.Excel;
using System.Text;

namespace UnitTests.ClosedXML;

public class ClosedXML_ExcelRegion_Tests(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        List<ProductSizeData> datas = [];

        string filePath = Path.Join(AppContext.BaseDirectory, "ClosedXML", "1.xlsx");
        var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(1); // 假设数据在第一个工作表中  

        int currentRow = 2; // 从第二行开始读取数据  

        while (currentRow <= worksheet.LastRowUsed().RowNumber())
        {
            var skuCell = worksheet.Cell(currentRow, 1);

            if (!skuCell.IsEmpty())
            {
                var markdownBuilder = new StringBuilder();

                string currentSKU = skuCell.GetString();
                var region = skuCell.CurrentRegion;

                // 添加产品代码  
                //markdownBuilder.AppendLine($"### {currentSKU}");

                // 添加表头
                var headerRow = region.FirstRowUsed();

                var headers = new List<string>();

                for (int j = 2; j <= headerRow.CellCount(); j++)
                {
                    headers.Add(headerRow.Cell(j).GetString());
                }
                markdownBuilder.AppendLine($"| {string.Join(" | ", headers)} |");
                markdownBuilder.AppendLine($"| {string.Join(" | ", new string[headers.Count].Select(_ => "---"))} |");

                // 添加表格内容  
                foreach (var row in region.Rows())
                {
                    if (row.RowNumber() == currentRow) continue; // 跳过产品代码行

                    var cells = new List<string>();
                    for (int j = 2; j <= 10; j++)
                    {
                        cells.Add(row.Cell(j).GetString());
                    }
                    markdownBuilder.AppendLine($"| {string.Join(" | ", cells)} |");
                }

                currentRow = region.LastRow().RowNumber() + 1; // 跳到下一个产品代码行

                datas.Add(new ProductSizeData
                {
                    SKU = currentSKU,
                    SizeContent = markdownBuilder.ToString()
                });

                // 输出Markdown表格  
                //this.Console.WriteLine(markdownBuilder.ToString());
            }
            else
            {
                currentRow++;
            }
        }

        foreach (var row in datas)
        {
            this.Console.WriteLine(row.SizeContent);
        }
    }
}
public class ProductSizeData
{
    public string SKU { get; set; }
    public string SizeContent { get; set; }
}