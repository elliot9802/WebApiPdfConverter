using System;
using System.IO;
using ExpertPdf.HtmlToPdf;
namespace Services
{
    public class ExpertPdfConvertService : IPdfConvertService
    {
        public void ConvertHtmlToPdf(string htmlContent, string outputPath)
        {
            PdfConverter pdfConverter = CreatePdfConverter();

            try
            {
                byte[] pdfBytes = pdfConverter.GetPdfBytesFromHtmlFile(htmlContent);
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