using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VidShareWebApi.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }


        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<VideoInfo> Videos { get; set; }
    }
}