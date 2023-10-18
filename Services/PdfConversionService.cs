using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    /// <summary>
    /// Provides functionalities for converting URLs and HTML content to PDF.
    /// </summary>
    public class PdfConversionService : IPdfConversionService
    {
        private readonly IPdfConverterUtility _pdfUtility;
        private readonly IFileService _fileService;

        public PdfConversionService(IPdfConverterUtility pdfUtility, IFileService fileService)
        {
            _pdfUtility = pdfUtility;
            _fileService = fileService;
        }

        /// <summary>
        /// Validate if the provided string is a valid URL.
        /// </summary>
        /// <param name="url">The string to validate</param>
        /// <returns>True if the string is a valid URL, otherwise false.</returns>
        public bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public string GetTemporaryPdfFilePath()
        {
            string tempDirectory = Path.GetTempPath();
            string fileName = Guid.NewGuid().ToString() + "pdf";
            return Path.Combine(tempDirectory, fileName);
        }

        private byte[] ConvertToPdfBytes(Action<string> conversionAction)
        {
            string outputPath = GetTemporaryPdfFilePath();
            conversionAction(outputPath);

            byte[] pdfBytes = _fileService.ReadAllBytes(outputPath);
            _fileService.Delete(outputPath);

            return pdfBytes;
        }

        public byte[] ConvertHtmlContentToPdfBytes(string htmlContent)
        {
            return ConvertToPdfBytes(outputPath => _pdfUtility.ConvertHtmlToPdf(htmlContent, outputPath));
        }

        public byte[] ConvertUrlToPdfBytes(string url)
        {
            return ConvertToPdfBytes(outputPath => _pdfUtility.ConvertUrlToPdf(url, outputPath));
        }
    }
}
