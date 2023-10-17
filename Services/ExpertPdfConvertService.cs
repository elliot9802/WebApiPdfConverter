using System;
using System.IO;
using ExpertPdf.HtmlToPdf;
using Microsoft.Extensions.Logging;
namespace Services
{
    public class ExpertPdfConvertService : IPdfConvertService
    {
        private readonly ILogger<ExpertPdfConvertService> _logger;

        public ExpertPdfConvertService(ILogger<ExpertPdfConvertService> logger)
        {
            _logger = logger;
        }
        public void ConvertHtmlToPdf(string htmlContent, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                _logger.LogError("Html Content is empty or null.");
            }
            PdfConverter pdfConverter = CreatePdfConverter();

            try
            {
                byte[] pdfBytes = pdfConverter.GetPdfBytesFromHtmlString(htmlContent);
                File.WriteAllBytes(outputPath, pdfBytes);
            }
            catch (Exception)
            {
                // Throw the exception so it can be caught and logged/handled at the caller.
                throw;
            }
        }

        public void ConvertUrlToPdf(string urlContent, string outputPath)
        {
            PdfConverter pdfConverter = CreatePdfConverter();

            try
            {
                byte[] pdfBytes = pdfConverter.GetPdfBytesFromUrl(urlContent);
                File.WriteAllBytes(outputPath, pdfBytes);
            }
            catch (Exception)
            {
                // Throw the exception so it can be caught and logged/handled at the caller.
                throw;
            }
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