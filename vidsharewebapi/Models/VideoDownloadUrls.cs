using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Amazon.DynamoDBv2;

namespace VidShareWebApi.Models
{
    public class VideoDownloadUrls
    {
        [Key]
        public int VideoDownloadUrlId { get; set; }

        [Required]
        public string KeyId { get; set; }

        public string PreSignedUrl { get; set; }
        public string DownloadUrlRaw { get; set; }
        public string DownloadUrl480 { get; set; }
        public string DownloadUrl720 { get; set; }
        public string DownloadUrl1080 { get; set; }

        public int VideoInfoId { get; set; }
        public VideoInfo VideoInfo { get; set; }

   } 
}