namespace Services
{
    /// <summary>
    /// Represents errors that occur during PDF conversion.
    /// </summary>
    public class PdfConversionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfConversionException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that caused the current exception.</param>
        public PdfConversionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
