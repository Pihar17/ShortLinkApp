using System.ComponentModel.DataAnnotations;

namespace ShortLinkApp.Models
{
    public class LoginModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
    }
}
