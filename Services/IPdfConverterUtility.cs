namespace Services
{
    public interface IPdfConverterUtility
    {
        void ConvertHtmlToPdf(string htmlContent, string outputPath);
        void ConvertUrlToPdf(string urlContent, string outputPath);
    }
}