using System;
using System.IO;
using ExpertPdf.HtmlToPdf;
using Microsoft.Extensions.Logging;
namespace Services
{
    /// <summary>
    /// PDF conversion service using the ExpertPdf library
    /// </summary>
    public class ExpertPdfConvertService : IPdfConverterUtility
    {
        private readonly ILogger<ExpertPdfConvertService> _logger;
        private readonly PdfConverter _pdfConverter;
        private readonly IFileService _fileService;

        public ExpertPdfConvertService(ILogger<ExpertPdfConvertService> logger, IFileService fileService)
        {
            _logger = logger;
            _pdfConverter = CreatePdfConverter();
            _fileService = fileService;
        }
        public void ConvertHtmlToPdf(string htmlContent, string outputPath)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                _logger.LogError("Html Content is empty or null.");
                throw new ArgumentException("Html Content cannot be null or empty.");
            }
                byte[] pdfBytes = _pdfConverter.GetPdfBytesFromHtmlString(htmlContent);
                _fileService.WriteAllBytes(outputPath, pdfBytes);
        }

        public void ConvertUrlToPdf(string urlContent, string outputPath)
        {
                byte[] pdfBytes = _pdfConverter.GetPdfBytesFromUrl(urlContent);
                _fileService.WriteAllBytes(outputPath, pdfBytes);
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