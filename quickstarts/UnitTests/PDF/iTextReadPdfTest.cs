using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Xobject;
using System.Text;

namespace UnitTests.iText;


public class iTextReadPdfTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        string pdfPath = @"C:\Users\shuai\Desktop\IFPUG功能点估算含示例.pdf";
        string textOutputPath = Path.Join(AppContext.BaseDirectory, "itext", "test.txt");
        string imageOutputDir = Path.Join(AppContext.BaseDirectory, "itext");
        ExtractTextAndImagesFromPdf(pdfPath, textOutputPath, imageOutputDir);
    }


    public void ExtractTextAndImagesFromPdf(string pdfPath, string textOutputPath, string imageOutputDir)
    {
        using (PdfReader pdfReader = new PdfReader(pdfPath))
        using (PdfDocument pdfDoc = new PdfDocument(pdfReader))
        {
            StringBuilder textContent = new StringBuilder();

            for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
            {
                // 提取文本  
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page), strategy);
                textContent.Append(currentText);

                // 提取图片  
                PdfPage pdfPage = pdfDoc.GetPage(page);
                PdfResources resources = pdfPage.GetResources();
                foreach (PdfName key in resources.GetResourceNames())
                {
                    PdfObject obj = resources.GetResource(key);
                    if (obj is PdfDictionary)
                    {
                        PdfDictionary dict = (PdfDictionary)obj;
                        if (dict.ContainsKey(PdfName.Subtype) && dict.Get(PdfName.Subtype).Equals(PdfName.Image))
                        {
                            PdfImageXObject imageObject = new PdfImageXObject((PdfStream)dict);
                            byte[] imageBytes = imageObject.GetImageBytes(true);
                            string fileName = Path.Combine(imageOutputDir, $"image_{page}_{key.ToString()}.png");
                            File.WriteAllBytes(fileName, imageBytes);
                            output.WriteLine($"Image extracted and saved to {fileName}");
                        }
                    }
                }
            }

            // 保存文本内容到文件  
            File.WriteAllText(textOutputPath, textContent.ToString());
            output.WriteLine($"Text extracted and saved to {textOutputPath}");
        }
    }
}