﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly IPdfConverterUtility _pdfUtility;
        private readonly IFileService _fileService;

        public PdfConversionController(IPdfConversionService pdfService, IPdfConverterUtility pdfUtility, IFileService fileService, ILogger<PdfConversionController> logger)
        {
            _pdfService = pdfService;
            _pdfUtility = pdfUtility;
            _fileService = fileService;
            _logger = logger;
        }

        [HttpPost("convertUrl")]
        public async Task<IActionResult> ConvertUrlToPdf([FromBody] UrlRequest request)
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

                byte[] pdfBytes = await _pdfService.ConvertUrlToPdfBytesAsync(request.Url);
                return File(pdfBytes, "application/pdf", $"{Guid.NewGuid()}.pdf");
            }
            catch (PdfConversionException pce)
            {
                _logger.LogError(pce, "Pdf Conversion failed due to a PdfConversionException");
                return BadRequest($"Pdf Conversion failed: {pce.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF Conversion failed due to an unknown error");
                return StatusCode(500, "An unexpected error occured.");
            }
        }

        [HttpPost("convertHtmlFile")]
        public async Task<IActionResult> ConvertHtmlFileToPdf(IFormFile htmlFile)
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

                using (var reader = new StreamReader(htmlFile.OpenReadStream()))
                {
                    var htmlContent = reader.ReadToEnd();

                    byte[] pdfBytes = await _pdfService.ConvertHtmlContentToPdfBytesAsync(htmlContent);
                    return File(pdfBytes, "application/pdf", $"{Guid.NewGuid()}.pdf");
                }
            }
            catch (PdfConversionException pce)
            {
                _logger.LogError(pce, "Pdf Conversion failed due to a PdfConversionException");
                return BadRequest($"Pdf Conversion failed: {pce.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PDF Conversion failed due to an unknown error");
                return StatusCode(500, "An unexpected error occured.");
            }
        }
    }
}
