namespace Services
{
    /// <summary>
    /// Defines functionalities for PDF conversion.
    /// </summary>
    public interface IPdfConverterUtility
    {
        Task ConvertHtmlToPdfAsync(string htmlContent, string outputPath);
        Task ConvertUrlToPdfAsync(string urlContent, string outputPath);

        Task CreatePdfAsync(string outputPath);
    }
}