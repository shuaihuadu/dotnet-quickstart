using ClosedXML.Excel;
using Newtonsoft.Json;

namespace UnitTests.ClosedXML;

public class ClosedXML_ExcelMerge_Tests(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        string filePath = Path.Join(AppContext.BaseDirectory, "ClosedXML", "2.xlsx");

        var workbook = new XLWorkbook(filePath);

        var data = new List<Dictionary<string, object>>();

        foreach (var worksheet in workbook.Worksheets)
        {
            // 检查工作表是否可见
            if (worksheet.Visibility != XLWorksheetVisibility.Visible)
            {
                continue;
            }

            // 获取表头
            var headers = new List<string>();

            foreach (var cell in worksheet.Row(1).Cells())
            {
                headers.Add(cell.Value.ToString());
            }

            // 读取数据
            var skuData = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var rowData = new Dictionary<string, object>();
                for (int i = 1; i <= headers.Count; i++)
                {
                    rowData[headers[i - 1]] = row.Cell(i).Value.ToString();
                }

                string sku = rowData["货号"].ToString();

                if (!skuData.ContainsKey(sku))
                {
                    skuData[sku] = [];
                }
                skuData[sku].Add(rowData);
            }

            // 将数据转换为所需的JSON格式
            foreach (var sku in skuData)
            {
                var skuObject = new Dictionary<string, object>
                {
                    { "SKU", sku.Key },
                    { "Content", sku.Value }
                };
                data.Add(skuObject);
            }
        }

        // 转换为JSON
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText("output.json", json);

        this.Console.WriteLine("JSON data has been written to output.json");
    }
}