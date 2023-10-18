namespace Services
{
    /// <summary>
    /// Defines functionalities for PDF conversion.
    /// </summary>
    public interface IPdfConverterUtility
    {
        void ConvertHtmlToPdf(string htmlContent, string outputPath);
        void ConvertUrlToPdf(string urlContent, string outputPath);
    }
}