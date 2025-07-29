namespace VidShareWebApi.DTOs
{
    public class VideoInfoDtos
    {
        public string KeyId { get; set; }
        public string? PreSignedUrl { get; set; }
        public string? DownloadUrlRaw { get; set; }
        public string? DownloadUrl480 { get; set; }
        
    }
}