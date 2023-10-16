using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace AppPdfConverterWApi.Controllers
{
    [Route("api/pdf")]
    [ApiController]
    public class PdfConversionController : ControllerBase
    {
        private readonly IPdfConvertService? _pdfService;

        public PdfConversionController(IPdfConvertService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost("convertUrl")]
        public IActionResult ConvertUrlToPdf([FromBody] string url)
        {
            try
            {
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
                return BadRequest($"PDF Conversion failed: {ex.Message}");
            }
        }

    }
}
