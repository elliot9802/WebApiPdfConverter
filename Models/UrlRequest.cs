using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// Represents a request containing a url to be converted to a PDF
    /// </summary>
    public class UrlRequest
    {
        /// <summary>
        /// Gets or sets the URL to be converted to PDF
        /// </summary>
        [Required]
        [Url]
        public string Url { get; set; }
    }
}