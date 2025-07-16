namespace TranscoderService.Models
{
    public class VideoUploadedEvent
    {
        public string VideoId { get; set; }
        public string S3Key { get; set; }
    }
}