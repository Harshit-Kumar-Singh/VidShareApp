using System.ComponentModel.DataAnnotations;

namespace VidShareWebApi.DTOs
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}