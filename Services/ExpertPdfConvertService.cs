using System;
using System.IO;
using ExpertPdf.HtmlToPdf;
using Microsoft.Extensions.Logging;
namespace Services
{
    public class ExpertPdfConvertService : IPdfConverterUtility
    {
        private readonly ILogger<ExpertPdfConvertService> _logger;
        private readonly PdfConverter _pdfConverter;
        public ExpertPdfConvertService(ILogger<ExpertPdfConvertService> logger)
        {
            _logger = logger;
            _pdfConverter = CreatePdfConverter();
        }
        public void ConvertHtmlToPdf(string htmlContent, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                _logger.LogError("Html Content is empty or null.");
                throw new ArgumentException("Html Content cannot be null or empty.");
            }
                byte[] pdfBytes = _pdfConverter.GetPdfBytesFromHtmlString(htmlContent);
                File.WriteAllBytes(outputPath, pdfBytes);
        }

        public void ConvertUrlToPdf(string urlContent, string outputPath)
        {
                byte[] pdfBytes = _pdfConverter.GetPdfBytesFromUrl(urlContent);
                File.WriteAllBytes(outputPath, pdfBytes);
        }

        private PdfConverter CreatePdfConverter()
        {
            PdfConverter pdfConverter = new PdfConverter
            {
                PdfDocumentOptions =
                {
                    PdfPageSize = PdfPageSize.A4,
                    PdfPageOrientation = PDFPageOrientation.Portrait,
                    PdfCompressionLevel = PdfCompressionLevel.Normal,
                    ShowHeader = false,
                    ShowFooter = false
                }
            };

            return pdfConverter;
        }
    }
}