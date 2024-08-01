using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.XObjects;

namespace UnitTests.iText;


public class PdfPigWriteTest(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public void Run()
    {
        string pdfPath = @"C:\Users\shuai\Desktop\IFPUG功能点估算含示例.pdf";
        string textOutputPath = Path.Join(AppContext.BaseDirectory, "UglyToad", "test.txt");
        string imageOutputDir = Path.Join(AppContext.BaseDirectory, "UglyToad");
        ExtractTextAndImagesFromPdf(pdfPath, textOutputPath, imageOutputDir);
    }


    //public void ExtractTextAndImagesFromPdf(string pdfPath, string textOutputPath, string imageOutputDir)
    //{
    //    using (Stream stream = File.OpenRead(pdfPath))
    //    {
    //        StringBuilder textContent = new StringBuilder();

    //        using PdfDocument? pdfDocument = PdfDocument.Open(stream);

    //        foreach (Page? page in pdfDocument.GetPages().Where(x => x != null))
    //        {
    //            // Note: no trimming, use original spacing
    //            string pageContent = ContentOrderTextExtractor.GetText(page) ?? string.Empty;

    //            textContent.Append(pageContent);

    //            foreach (var item in page.GetImages())
    //            {
    //                textContent.Append("Image" + item.ToString());
    //            }

    //            textContent.AppendLine();
    //            textContent.AppendLine("=========================================================================");
    //        }

    //        // 保存文本内容到文件  
    //        File.WriteAllText(textOutputPath, textContent.ToString());
    //        output.WriteLine($"Text extracted and saved to {textOutputPath}");
    //    }
    //}
    public void ExtractTextAndImagesFromPdf(string pdfPath, string textOutputPath, string imageOutputDir)
    {
        var isOnWords = true;
        var isOnImages = true;
        var isOnHyperLinks = true;

        var isOnListOnlyFirst5WordsPerPage = true;

        var isOnDebugPdfFileName = false;
        var isOnDebugWordLocations = false;
        var isOnDebugHyperLinkLocations = false;
        var isOnDebugImageBytes = false;
        var isOnDebugHighLevelImageType = false;

        using (var doc = PdfDocument.Open(pdfPath))
        {
            StringBuilder contentBuilder = new();

            foreach (var page in doc.GetPages())
            {
                var pn = page.Number;
                var itemNumber = 0;
                var LocationDictionary = new Dictionary<(double X, double Y, int itemNum), object>();
                if (isOnWords)
                {
                    var w = 0; // Word count
                    foreach (var word in page.GetWords())
                    {
                        w++;
                        itemNumber++;
                        var topLeft = word.BoundingBox.TopLeft;
                        var wordStart = word.Text.Length > 5 ? word.Text.Substring(0, 5) + " ... " : word.Text;
                        if (isOnDebugWordLocations)
                        {
                            var outputLine = $"Page: {pn} Word: {w} X: {topLeft.X:0000.00} Y:{topLeft.Y:0000.00} word: {wordStart}";

                            output.WriteLine(outputLine);
                        }

                        LocationDictionary.Add((topLeft.X, topLeft.Y, itemNumber), word);

                        contentBuilder.Append(word.Text);
                    }
                }

                if (isOnHyperLinks)
                {
                    var h = 0; // Hyperlink count
                    foreach (var hyperLink in page.GetHyperlinks())
                    {
                        h++;
                        itemNumber++;
                        var topLeft = hyperLink.Bounds.TopLeft;
                        var hlStart = hyperLink.Text.Length > 5 ? hyperLink.Text.Substring(0, 5) + " ... " : hyperLink.Text;
                        var outputLine = $"Page: {pn} Hyperlink: {h} X: {topLeft.X:0000.00} Y:{topLeft.Y:0000.00} hyperlink: {hlStart}";
                        if (isOnDebugHyperLinkLocations)
                        {
                            output.WriteLine(outputLine);
                        }

                        LocationDictionary.Add((topLeft.X, topLeft.Y, itemNumber), hyperLink);

                        contentBuilder.Append(hyperLink.Text);
                    }
                }

                if (isOnImages)
                {
                    var i = 0; // Image Number on page. First image is 1.
                    foreach (var image in page.GetImages())
                    {
                        i++;
                        itemNumber++;
                        var topLeft = image.Bounds.TopLeft;
                        var outputLine = $"Page: {pn} Image: {i} X: {topLeft.X:0000.00} Y:{topLeft.Y:0000.00}";

                        if (isOnDebugImageBytes)
                        {
                            #region Get Image Bytes
                            byte[] imageBytes = null;
                            if (image.TryGetPng(out imageBytes) == false)
                            {
                                if (image.TryGetBytes(out var bytesEnumerated))
                                {
                                    imageBytes = bytesEnumerated.ToArray();
                                }
                            }
                            var imageSizeString = (imageBytes is null) ? "unknown" : $"{imageBytes.Length}";
                            outputLine += $" Bytes: {imageSizeString}";

                            #endregion
                        }


                        contentBuilder.AppendLine();
                        contentBuilder.Append("========== This is an image ==========");
                        contentBuilder.AppendLine();

                        if (isOnDebugHighLevelImageType)
                        {
                            #region Additional Details using two high level images types: XObjectImage and InLineImage  
                            switch (image)
                            {
                                case XObjectImage ximg:
                                    outputLine += " Type: XObject";
                                    break;

                                case InlineImage inline:
                                    outputLine += " Type: Inline ";
                                    break;
                            }
                            #endregion
                        }

                        output.WriteLine(outputLine);

                        LocationDictionary.Add((topLeft.X, topLeft.Y, itemNumber), image);
                    }
                }

                var itemsOnPageOrderedByLocation = LocationDictionary.Select(v => (v.Key, v.Value)).ToList();
                itemsOnPageOrderedByLocation.Sort((a, b) =>
                {
                    switch (a.Key.Y.CompareTo(b.Key.Y))
                    {
                        case -1: return -1;
                        case 0: return a.Key.X.CompareTo(b.Key.X);
                        case 1: return 1;
                    }
                    return 0;
                }
                );

                var orderedItemNumber = 0;
                var orderedWordNumber = 0;
                foreach (var itemOnPage in itemsOnPageOrderedByLocation)
                {
                    orderedItemNumber++;
                    var location = (itemOnPage.Key.X, itemOnPage.Key.Y);
                    var item = itemOnPage.Value;

                    var itemTypeString = $"{item.GetType().Name}".PadRight(12, '_');

                    if (isOnListOnlyFirst5WordsPerPage)
                    {
                        if (item.GetType().Name == "Word")
                        {
                            orderedWordNumber++;
                            if (orderedWordNumber > 5) continue;
                        }
                    }

                    var outputLine = $"Page: {pn:D3} Item: {orderedItemNumber:D3} Type: {itemTypeString} X: {location.X:0000.00} Y:{location.Y:0000.00} File: {pdfPath}";

                    output.WriteLine(outputLine);
                }


                contentBuilder.AppendLine();
                contentBuilder.Append("============================================================");
                contentBuilder.AppendLine();
            }


            File.WriteAllText(textOutputPath, contentBuilder.ToString());
            output.WriteLine($"Text extracted and saved to {textOutputPath}");
        }
    }
}