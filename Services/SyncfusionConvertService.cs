using Microsoft.Extensions.Logging;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.HtmlToPdf;
using System.Text;

namespace Services
{
    public class SyncfusionConvertService : IPdfConverterUtility
    {
        private readonly IFileService _fileService;
        private readonly HtmlToPdfConverter _htmlToPdfConverter;
        private readonly ILogger<SyncfusionConvertService> _logger;

        public SyncfusionConvertService(IFileService fileService, ILogger<SyncfusionConvertService> logger)
        {
            _fileService = fileService;
            _logger = logger;
            _htmlToPdfConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink)
            {
                ReuseBrowserProcess = true,
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

        private async Task ConvertAndSavePdfAsync(string inputData, string outputPath)
        {
            try
            {
                _logger.LogInformation($"Starting conversion of input data to PDF. Output path: {outputPath}");
                PdfDocument pdfDocument = _htmlToPdfConverter.Convert(inputData);
                await Task.Run(() =>
                {
                    using (FileStream stream = new FileStream(outputPath, FileMode.Create))
                    {
                        pdfDocument.Save(stream);
                    }
                });
                pdfDocument.Close(true);
                _logger.LogInformation($"Conversion and saving of PDF completed. Output path: {outputPath}");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to convert and save PDF. Input data: {inputData}, Output path: {outputPath}";
                _logger.LogError(ex, errorMessage);
                throw new PdfConversionException($"{errorMessage}. Exception: {ex.Message}", ex);
            }
        }
        public async Task ConvertHtmlToPdfAsync(string htmlContent, string outputPath)
        {
            string tempHtmlPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".html");
            await _fileService.WriteAllBytesAsync(tempHtmlPath, Encoding.UTF8.GetBytes(htmlContent));

            try
            {
                _logger.LogInformation($"Converting HTML to PDF. Temporary HTML path: {tempHtmlPath}, Output path: {outputPath}");
                await ConvertAndSavePdfAsync(tempHtmlPath, outputPath);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to convert HTML file to PDF. Temporary output path: {outputPath}";
                _logger.LogError(errorMessage, ex);
                throw new PdfConversionException($"{errorMessage}. Exception: {ex.Message}", ex);
            }
            finally
            {
                if (_fileService.Exists(tempHtmlPath))
                {
                    await _fileService.DeleteAsync(tempHtmlPath);
                }
            }
        }

        public async Task ConvertUrlToPdfAsync(string urlContent, string outputPath)
        {
            try
            {
                _logger.LogInformation($"Converting URL content to PDF. URL: {urlContent}, Output path: {outputPath}");
                await ConvertAndSavePdfAsync(urlContent, outputPath);

            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to convert URL content to PDF. URL: {urlContent}, Output path: {outputPath}";
                _logger.LogError(ex, errorMessage);
                throw new PdfConversionException($"{errorMessage}. Exception: {ex.Message}", ex);
            }
        }
    }
}
