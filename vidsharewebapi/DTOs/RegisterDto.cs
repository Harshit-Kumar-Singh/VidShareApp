using System.ComponentModel.DataAnnotations;

namespace VidShareWebApi.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}