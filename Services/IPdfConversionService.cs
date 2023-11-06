namespace Services
{
    /// <summary>
    /// Defines functionalities related to PDF conversion operations.
    /// </summary>
    public interface IPdfConversionService
    {
        bool IsValidUrl(string url);
        string GetTemporaryPdfFilePath();
        Task <byte[]> ConvertHtmlContentToPdfBytesAsync(string htmlContent);
        Task <byte[]> ConvertUrlToPdfBytesAsync(string url);
        Task<byte[]> CreateAndSavePdfAsync();

    }
}
