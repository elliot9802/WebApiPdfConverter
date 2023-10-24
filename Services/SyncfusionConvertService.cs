using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Text;

namespace Services
{
    public class SyncfusionConvertService : IPdfConverterUtility
    {
        private readonly IFileService _fileService;
        private readonly HtmlToPdfConverter _htmlToPdfConverter;

        public SyncfusionConvertService(IFileService fileService)
        {
            _fileService = fileService;
            _htmlToPdfConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink)
            {
                ConverterSettings = new BlinkConverterSettings()
                {
                    ViewPortSize = new Syncfusion.Drawing.Size(1024, 0),
                    Margin = new PdfMargins
                    {
                        All = 0
                    }
                }
            };
        }

        private void ConvertAndSavePdf(string inputData, string outputPath)
        {
            PdfDocument pdfDocument = _htmlToPdfConverter.Convert(inputData);
            using (FileStream stream = new FileStream(outputPath, FileMode.Create))
            {
                pdfDocument.Save(stream);
            }
            pdfDocument.Close(true);
        }

        public void ConvertHtmlToPdf(string htmlContent, string outputPath)
        {
            string tempHtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
            _fileService.WriteAllBytes(tempHtmlPath, Encoding.UTF8.GetBytes(htmlContent));

            try
            {
                ConvertAndSavePdf(outputPath, tempHtmlPath);
            }
            finally
            {
                _fileService.Delete(tempHtmlPath);
            }
        }

        public void ConvertUrlToPdf(string urlContent, string outputPath)
        {
            ConvertAndSavePdf(outputPath, urlContent);
        }
    }
}