namespace Services
{
    public interface IPdfConvertService
    {
        void ConvertHtmlToPdf(string htmlContent, string outputPath);
        void ConvertUrlToPdf(string urlContent, string outputPath);
    }
}