using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VidShareWebApi.Models
{
    public class VideoInfo
    {

        [Key]
        public int VideoInfoId { get; set; }


        public string? KeyId { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }

        public User? User { get; set; }

        public ICollection<VideoDownloadUrls>?   VideoDownloadUrls { get; set; }

        
    }
}