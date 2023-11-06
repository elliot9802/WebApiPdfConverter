using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Syncfusion.Drawing;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using System.Text;

namespace Services
{
    public class SyncfusionConvertService : IPdfConverterUtility
    {
        private readonly IFileService _fileService;
        private readonly HtmlToPdfConverter _htmlToPdfConverter;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SyncfusionConvertService> _logger;

        public SyncfusionConvertService(IFileService fileService, ILogger<SyncfusionConvertService> logger, IConfiguration configuration)
        {
            _fileService = fileService;
            _configuration = configuration;
            _logger = logger;
            _htmlToPdfConverter = new HtmlToPdfConverter(HtmlRenderingEngine.Blink)
            {
                ReuseBrowserProcess = true,
                ConverterSettings = new BlinkConverterSettings()
                {
                    ViewPortSize = new Size(1024, 0),
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

        public async Task CreatePdfAsync(string outputPath)
        {
            // Initialize PDF document
            using PdfDocument document = new PdfDocument();
            string backgroundImagePath = _configuration["BackgroundImagePath"];
            PdfFont regularFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
            PdfFont boldFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);

            for (int i = 0; i < 5; i++)
            {
                PdfPage page = document.Pages.Add();
                float scaleFactor = Math.Min(page.GetClientSize().Width / 1024f, 1);
                PointF ticketOrigin = new PointF((page.GetClientSize().Width - (1024 * scaleFactor)) / 2, 0);

                DrawBackgroundImage(page, backgroundImagePath, ticketOrigin, scaleFactor);
                DrawTextContent(page.Graphics, ticketOrigin, scaleFactor, regularFont, boldFont);
                DrawBarcode(page, ticketOrigin, scaleFactor);
            }
            // Save and close the document
            try
            {
                using FileStream stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None);
                document.Save(stream);
            }
            catch (Exception ex)
            {
                _logger.LogError("PDF Ticket creation failed: " + ex.Message);
                throw;
            }

            _logger.LogInformation("PDF Ticket Creation succeeded and saved to " + outputPath);
        }

        private void DrawBackgroundImage(PdfPage page, string backgroundImagePath, PointF origin, float scale)
        {
            using FileStream imageStream = new FileStream(backgroundImagePath, FileMode.Open, FileAccess.Read);
            PdfBitmap background = new PdfBitmap(imageStream);
            page.Graphics.DrawImage(background, origin.X, origin.Y, 1024 * scale, 364 * scale);
        }

        private void DrawTextContent(PdfGraphics graphics, PointF origin, float scale, PdfFont regularFont, PdfFont boldFont)
        {
            // Use 'graphics' to draw strings on the PDF, adjusting positions based on 'origin' and 'scale'
            graphics.DrawString("e.a.e.karlsson@gmail.com", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 80 * scale));
            graphics.DrawString("Elin Karlsson", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 105 * scale));
            graphics.DrawString("Webbokningsnr: 5935456247E", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 130 * scale));
            graphics.DrawString("Bokningsnr: 3526678", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 155 * scale));
            graphics.DrawString("Vuxen/Adult", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 180 * scale));
            graphics.DrawString("150,00", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 205 * scale));
            graphics.DrawString("Köpdatum: 2023-11-02", regularFont, PdfBrushes.Black, new PointF(origin.X + 30 * scale, origin.Y + 230 * scale));
            graphics.DrawString("- Köpt biljett återlöses ej -", regularFont, PdfBrushes.Black, new PointF(origin.X + 140 * scale, origin.Y + 265 * scale));
            graphics.DrawString("Serviceavgift", regularFont, PdfBrushes.Black, new PointF(origin.X + 330 * scale, origin.Y + 180 * scale));
            graphics.DrawString("0,00", regularFont, PdfBrushes.Black, new PointF(origin.X + 400 * scale, origin.Y + 205 * scale));
            graphics.DrawString("Drottningholms slott", boldFont, PdfBrushes.Black, new PointF(origin.X + 600 * scale, origin.Y + 40 * scale));
            graphics.DrawString("Drottningholm Palace", regularFont, PdfBrushes.Black, new PointF(origin.X + 620 * scale, origin.Y + 70 * scale));
            graphics.DrawString("Drottningholms slott", regularFont, PdfBrushes.Black, new PointF(origin.X + 630 * scale, origin.Y + 120 * scale));
            graphics.DrawString("2023-10-15 13:00", regularFont, PdfBrushes.Black, new PointF(origin.X + 640 * scale, origin.Y + 150 * scale));

            // Draw "Powered by Vitec Smart Visitor System AB" text at the bottom
            string bottomTxt = "Powered by Vitec Smart Visitor System AB";
            SizeF pageSize = graphics.ClientSize; // Assuming graphics is the PdfGraphics of the page
            PdfFont bottomTxtFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Italic); // You can adjust the size as needed
            SizeF bottomTxtSize = bottomTxtFont.MeasureString(bottomTxt);
            PointF bottomTxtPosition = new PointF(
                (pageSize.Width - bottomTxtSize.Width) / 2, // Centered horizontally
                pageSize.Height - bottomTxtSize.Height - 30 * scale // 30 units from the bottom, adjust as needed
            );
            graphics.DrawString(bottomTxt, bottomTxtFont, PdfBrushes.Black, bottomTxtPosition);

            // Draw scissors line below the ticket
            DrawScissorsLine(graphics, origin, scale);
        }

        private void DrawScissorsLine(PdfGraphics graphics, PointF origin, float scale)
        {
            // Assuming you have an image for the scissors line
            string scissorsLineImagePath = _configuration["ScissorsLineImagePath"];
            using FileStream scissorsImageStream = new FileStream(scissorsLineImagePath, FileMode.Open, FileAccess.Read);
            PdfBitmap scissorsLineImage = new PdfBitmap(scissorsImageStream);

            // Position for the scissors line, adjust the Y position as needed
            PointF scissorsPosition = new PointF(
                origin.X, // Aligned with the left edge of the ticket
                origin.Y + 364 * scale + 10 * scale // Just below the ticket, 10 units of space
            );
            SizeF scissorsSize = new SizeF(1024 * scale, scissorsLineImage.Height * scale); // Full width and scaled height

            graphics.DrawImage(scissorsLineImage, scissorsPosition.X, scissorsPosition.Y, scissorsSize.Width, scissorsSize.Height);
        }

        private void DrawBarcode(PdfPage page, PointF origin, float scale)
        {
            PdfCode39Barcode barcode = new PdfCode39Barcode
            {
                Text = "W012R09",
                Size = new SizeF(300 * scale, 100 * scale)
            };

            PointF barcodePosition = new PointF(
                origin.X + (1024 * scale) - (barcode.Size.Height * scale) - 80 * scale,
                origin.Y + 330 * scale
            );

            page.Graphics.Save();
            page.Graphics.TranslateTransform(barcodePosition.X, barcodePosition.Y);
            page.Graphics.RotateTransform(-90);
            barcode.Draw(page.Graphics, PointF.Empty);
            page.Graphics.Restore();

        }
    }
}
