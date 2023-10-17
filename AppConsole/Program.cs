using Services;
using System;
using System.IO;

namespace AppConsole
{
    class Program
    {
        private const string BaseOutputPath = "HTML-to-PDF";
        private const string FileExtension = ".pdf";

        // Dependency injection can be done here
        private static IPdfConvertService _pdfService;

        static void Main(string[] args)
        {
            string htmlContent = File.ReadAllText(@"htmlfilepath");
            string urlContent = @"https://messagequeue.actorsmartbook.se/Templates/ticket.aspx?orderid=3545624&uid=411ffdec-dcbc-491f-a629-8939d26dd031";

            // Choose which service to use
            _pdfService = new ExpertPdfConvertService();
            // Or
            // _pdfService = new NRecoConvertService();

            try
            {
                ConvertHtmlToPdf(htmlContent);
                ConvertUrlToPdf(urlContent);
                Console.WriteLine("PDF Conversion done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during PDF conversion: {ex.Message}");
            }

            Console.Read();
        }

        private static void ConvertHtmlToPdf(string htmlContent)
        {
            string outputPath = GenerateOutputFileName("HtmlPDF");
            _pdfService.ConvertHtmlToPdf(htmlContent, outputPath);
        }

        private static void ConvertUrlToPdf(string urlContent)
        {
            string outputPath = GenerateOutputFileName("UrlPDF");
            _pdfService.ConvertUrlToPdf(urlContent, outputPath);
        }

        private static string GenerateOutputFileName(string identifier)
        {
            return $"{BaseOutputPath}-{identifier}{FileExtension}";
        }
    }
}