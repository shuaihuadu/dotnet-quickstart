using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
//using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace UnitTests.iText;

public class iTextWritePdfTest2
{

    [Fact]
    public static void Run()
    {
        string outputPath = "test_write2.pdf";

        string fontPath = System.IO.Path.Combine(Environment.CurrentDirectory, "PDF", "simsun.ttf");

        byte[] imageContent = File.ReadAllBytes(System.IO.Path.Combine(Environment.CurrentDirectory, "PDF", "sk.png"));
        byte[] imageContent2 = File.ReadAllBytes(System.IO.Path.Combine(Environment.CurrentDirectory, "PDF", "2.png"));

        List<PdfPageContent> pageContents =
        [
            new PdfPageContent
            {
                TextContents = [
                     "在这个示例中，我们创建了一个PdfWriter实例来指定文件的输出位置，创建了一个PdfDocument实例并添加了三页到PDF文档中。",
                     "这个示例展示了如何在PDF的指定页面上同时添加文本和图片。如果你有更多的需求，比如在不同的页面添加不同的内容或调整内容的位置和样式，你可以根据需要进一步修改代码。",
                     "希望这个示例对你有所帮助！如果你有更多问题，欢迎继续提问。"],
                ImageContents = [imageContent,imageContent2]
            },
        ];

        PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outputPath));

        pdfDoc.SetDefaultPageSize(PageSize.A4);

        Document doc = new Document(pdfDoc);

        PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, pdfDoc);

        foreach (PdfPageContent page in pageContents)
        {
            PageSize pageSize = pdfDoc.GetDefaultPageSize();

            Table table = new Table(UnitValue.CreatePercentArray(1));

            foreach (var item in page.TextContents)
            {
                table.AddCell(CreateTextCell(font, item));
            }
            table.AddCell(CreateTextCell(font, "一个新的文本内容1"));

            foreach (var item in page.ImageContents)
            {
                table.AddCell(CreateImageCell(item, pageSize.GetWidth() * 0.5f));
            }

            table.AddCell(CreateTextCell(font, "一个新的文本内容2"));

            doc.Add(table);
        }

        doc.Close();
    }

    private static Cell CreateImageCell(byte[] imageContent, float width)
    {
        Cell cell = new Cell();

        Image image = new Image(ImageDataFactory.Create(imageContent));

        image.SetWidth(width);
        image.SetAutoScaleHeight(false);

        cell.Add(image);

        //cell.SetBorder(Border.NO_BORDER);

        return cell;
    }

    private static Cell CreateTextCell(PdfFont font, string text)
    {
        Cell cell = new Cell();
        Paragraph paragraph = new Paragraph(text)
                    .SetFont(font)
                    .SetFontSize(3);

        cell.Add(paragraph);

        //cell.SetBorder(Border.NO_BORDER);

        return cell;
    }
}