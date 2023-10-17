using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace AppPdfConverterWApi.Controllers
{
    [Route("api/pdf")]
    [ApiController]
    public class PdfConversionController : ControllerBase
    {
        private readonly ILogger <PdfConversionController> _logger;
        private readonly IPdfConvertService _pdfService;
        
        public PdfConversionController(IPdfConvertService pdfService, ILogger<PdfConversionController> logger)
        {
            _pdfService = pdfService;
            _logger = logger;
        }

        public class UrlRequest
        {
            public string Url { get; set; }
        }

        [HttpPost("convertUrl")]
        public IActionResult ConvertUrlToPdf([FromBody] UrlRequest request)
        {
            string url = request.Url;
            try
            {
                if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                {
                    return BadRequest("Invalid URL format.");
                }

                string tempDirectory = Path.GetTempPath();
                string fileName = Guid.NewGuid().ToString() + ".pdf";
                string outputPath = Path.Combine(tempDirectory, fileName);


                // Convert the URL to PDF using the ExpertPdfConvertService
                _pdfService.ConvertUrlToPdf(url, outputPath);

                // Check if the PDF file was successfully created
                if (System.IO.File.Exists(outputPath))
                {
                    // Read the generated PDF file as bytes
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(outputPath);

                    // After reading the PDF bytes and before returning:
                    System.IO.File.Delete(outputPath);

                    // Return the PDF as a response
                    return File(pdfBytes, "application/pdf", fileName);
                }
                else
                {
                    return NotFound("PDF Conversion failed: File not found");
                }
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
                    _logger.LogWarning("Uploaded file is not an HTML file.");
                    return BadRequest("Please provide a valid HTML file.");
                }

                // Generate a unique output file path for the PDF
                string tempDirectory = Path.GetTempPath();
                string fileName = Guid.NewGuid().ToString() + ".pdf";
                string outputPath = Path.Combine(tempDirectory, fileName);

                // Read the HTML content from the uploaded file
                using (var reader = new StreamReader(htmlFile.OpenReadStream()))
                {
                    var htmlContent = reader.ReadToEnd();

                    // Convert the HTML content to PDF using the ExpertPdfConvertService
                    _pdfService.ConvertHtmlToPdf(htmlContent, outputPath);

                    // Check if the PDF file was successfully created
                    if (System.IO.File.Exists(outputPath))
                    {
                        // Read the generated PDF file as bytes
                        byte[] pdfBytes = System.IO.File.ReadAllBytes(outputPath);

                        // After reading the PDF bytes and before returning:
                        System.IO.File.Delete(outputPath);

                        // Return the PDF as a response with the appropriate content type and file name
                        return File(pdfBytes, "application/pdf", fileName);
                    }
                    else
                    {
                        return NotFound("PDF Conversion failed: File not found");
                    }
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
