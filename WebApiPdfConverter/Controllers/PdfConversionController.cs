using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace AppPdfConverterWApi.Controllers
{
    /// <summary>
    /// Controller responsible for handling PDF conversion requests.
    /// </summary>
    [Route("api/pdf")]
    [ApiController]
    public class PdfConversionController : ControllerBase
    {
        private readonly ILogger<PdfConversionController> _logger;
        private readonly IPdfConversionService _pdfService;

        public PdfConversionController(IPdfConversionService pdfService, ILogger<PdfConversionController> logger)
        {
            _pdfService = pdfService;
            _logger = logger;
        }

        [HttpPost("convertUrl")]
        public IActionResult ConvertUrlToPdf([FromBody] UrlRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state during URL to PDF conversion.");
                    return BadRequest(ModelState);
                }

                if (!_pdfService.IsValidUrl(request.Url))
                {
                    _logger.LogWarning($"Invalid URL format provided: {request.Url}.");
                    return BadRequest("Invalid URL format.");
                }

                byte[] pdfBytes = _pdfService.ConvertUrlToPdfBytes(request.Url);
                return File(pdfBytes, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF Conversion failed");
                return BadRequest($"PDF Conversion failed: {ex.Message}");
            }
        }

        [HttpPost("convertHtmlFile")]
        public IActionResult ConvertHtmlFileToPdf(IFormFile htmlFile)
        {
            try
            {
                if (htmlFile == null || htmlFile.Length == 0)
                {
                    _logger.LogWarning("HTML file missing or empty.");
                    return BadRequest("Please provide an HTML file.");
                }

                if (Path.GetExtension(htmlFile.FileName).ToLower() != ".html")
                {
                    _logger.LogWarning($"Uploaded file '{htmlFile.FileName}' is not an HTML file.");
                    return BadRequest("Please provide a valid HTML file.");
                }

                // Read the HTML content from the uploaded file
                using (var reader = new StreamReader(htmlFile.OpenReadStream()))
                {
                    var htmlContent = reader.ReadToEnd();

                    byte[] pdfBytes = _pdfService.ConvertHtmlContentToPdfBytes(htmlContent);
                    return File(pdfBytes, "application/pdf", Guid.NewGuid().ToString() + ".pdf");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF Conversion failed");
                return BadRequest($"PDF Conversion failed: {ex.Message}");
            }
        }

    }
}