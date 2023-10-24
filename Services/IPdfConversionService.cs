namespace Services
{
    /// <summary>
    /// Defines functionalities related to PDF conversion operations.
    /// </summary>
    public interface IPdfConversionService
    {
        bool IsValidUrl(string url);
        string GetTemporaryPdfFilePath();
        public byte[] ConvertHtmlContentToPdfBytes(string htmlContent);
        public byte[] ConvertUrlToPdfBytes(string url);
    }
}
