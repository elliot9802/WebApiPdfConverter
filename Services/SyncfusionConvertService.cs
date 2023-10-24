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
            _htmlToPdfConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink);

            var converterSettings = new BlinkConverterSettings
            {
                ViewPortSize = new Syncfusion.Drawing.Size(1024, 0),
                Margin = new PdfMargins
                {
                    All = 0
                }
            };

            _htmlToPdfConverter.ConverterSettings = converterSettings;
        }

        public void ConvertHtmlToPdf(string htmlContent, string outputPath)
        {
            string tempHtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
            _fileService.WriteAllBytes(tempHtmlPath, Encoding.UTF8.GetBytes(htmlContent));

            try
            {
                PdfDocument pdfDocument = _htmlToPdfConverter.Convert(tempHtmlPath);
                using (FileStream stream = new FileStream(outputPath, FileMode.Create))
                {
                    pdfDocument.Save(stream);
                }
                pdfDocument.Close(true);
            }
            finally
            {
                _fileService.Delete(tempHtmlPath);
            }
        }

        public void ConvertUrlToPdf(string urlContent, string outputPath)
        {
            PdfDocument pdfDocument = _htmlToPdfConverter.Convert(urlContent);
            using (FileStream stream = new FileStream(outputPath, FileMode.Create))
            {
                pdfDocument.Save(stream);
            }
            pdfDocument.Close(true);
        }
    }
}