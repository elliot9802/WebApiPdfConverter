using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UrlRequest
    {
        [Required]
        [Url]
        public string Url { get; set; }
    }
}