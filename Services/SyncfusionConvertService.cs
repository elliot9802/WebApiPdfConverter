using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.IO;

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
            PdfDocument pdfDocument = _htmlToPdfConverter.Convert(htmlContent);
            using (FileStream stream = new FileStream(outputPath, FileMode.Create))
            {
                pdfDocument.Save(stream);
            }
            pdfDocument.Close(true);
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