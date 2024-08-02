using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace UnitTests.iText;

public class iTextWritePdfTest
{
    [Fact]
    public void Run()
    {
        string fontPath = Path.Combine(Environment.CurrentDirectory, "PDF", "simsun.ttf");

        byte[] imageContent = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "PDF", "sk.png"));
        byte[] imageContent2 = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "PDF", "2.png"));

        if (fontPath == null)
        {
            Console.WriteLine("找不到中文字体文件！");
            return;
        }

        // 定义输出PDF文件的路径  
        string outputPath = "test_write.pdf";

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


        PdfWriter writer = new(outputPath);

        PdfDocument pdf = new(writer);

        Document document = new(pdf);

        // 加载中文字体

        PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, pdf);

        foreach (PdfPageContent page in pageContents)
        {
            PdfPage newPage = pdf.AddNewPage();

            Canvas canvas = new(newPage, pdf.GetDefaultPageSize());

            foreach (var item in page.TextContents)
            {
                Paragraph paragraph = new Paragraph(item)
                    .SetFont(font)
                    .SetFontSize(10);

                canvas.Add(paragraph);
            }

            for (int i = 0; i < page.ImageContents.Count; i++)
            {
                ImageData imageData = ImageDataFactory.Create(page.ImageContents[i]);

                Image image = new(imageData);

                //image.SetWidth(pdf.GetDefaultPageSize().GetWidth() - 300);

                //image.SetFixedPosition(i + 1, 0, 0);

                //canvas.Add(image);
            }

            //foreach (var item in page.ImageContents)
            //{
            //    ImageData imageData = ImageDataFactory.Create(item);

            //    Image image = new(imageData);
            //    //image.SetWidth(pdf.GetDefaultPageSize().GetWidth() - 300);
            //    //image.SetAutoScaleHeight(true);
            //    image.ScaleToFit(pdf.GetDefaultPageSize().GetWidth(), pdf.GetDefaultPageSize().GetHeight());

            //    canvas.Add(image);
            //}
        }

        // 关闭文档  
        document.Close();
    }
}

public class PdfPageContent
{
    public List<string> TextContents { get; set; }
    public List<byte[]> ImageContents { get; set; }
}