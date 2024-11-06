using System.ComponentModel.DataAnnotations;

namespace ShortLinkApp.Data
{
    public class UrlRequest
    {
        [Required]
        public string Url { get; set; } = string.Empty; // Значение по умолчанию для избежания null-значений
    }
}