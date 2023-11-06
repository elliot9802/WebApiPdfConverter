using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Services
{
    /// <summary>
    /// Provides functionalities for converting URLs and HTML content to PDF.
    /// </summary>
    public class PdfConversionService : IPdfConversionService
    {
        private readonly IFileService _fileService;
        private readonly IPdfConverterUtility _pdfUtility;
        private readonly ILogger<PdfConversionService> _logger;
        //private readonly IMemoryCache _cache;

        public PdfConversionService(IFileService fileService, IPdfConverterUtility pdfUtility, ILogger<PdfConversionService> logger/*, IMemoryCache cache*/)
        {
            _fileService = fileService;
            _pdfUtility = pdfUtility;
            _logger = logger;
            //_cache = cache;
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
            string fileName = Guid.NewGuid().ToString() + ".pdf";
            return Path.Combine(tempDirectory, fileName);
        }

        private async Task<byte[]> ConvertToPdfBytesAsync(Func<string, Task> conversionAction)
        {
            string outputPath = GetTemporaryPdfFilePath();
            try
            {
                _logger.LogInformation($"Starting PDF conversion. Temporary output path: {outputPath}");
                await conversionAction(outputPath);
                if (!_fileService.Exists(outputPath))
                {
                    throw new FileNotFoundException("Pdf file not found after conversion", outputPath);
                }
                byte[] pdfBytes = await _fileService.ReadAllBytesAsync(outputPath);
                _logger.LogInformation($"Pdf conversion completed. Temporary output path: {outputPath}");
                return pdfBytes;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to convert to PDF. Temporary output path: {outputPath}";
                _logger.LogError(errorMessage, ex);
                throw new PdfConversionException($"{errorMessage}. Exception: {ex.Message}", ex);
            }
            finally
            {
                if (_fileService.Exists(outputPath))
                {
                    await _fileService.DeleteAsync(outputPath);
                }
            }
        }
       
        public async Task<byte[]> ConvertHtmlContentToPdfBytesAsync(string htmlContent)
        {
            return await ConvertToPdfBytesAsync(outputPath => _pdfUtility.ConvertHtmlToPdfAsync(htmlContent, outputPath));
        }

        public async Task<byte[]> ConvertUrlToPdfBytesAsync(string url)
        {
            /*return await _cache.GetOrCreateAsync(url, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);*/
            return await ConvertToPdfBytesAsync(outputPath => _pdfUtility.ConvertUrlToPdfAsync(url, outputPath));
            /*});*/
        }

        public async Task<byte[]> CreateAndSavePdfAsync()
        {
            return await ConvertToPdfBytesAsync(outputPath => _pdfUtility.CreatePdfAsync(outputPath));
        }
    }
}
