using Amazon.DynamoDBv2.DataModel;

namespace VidShareWebApi.Models
{
    [DynamoDBTable("vidshare-video")]
    public class UploadItem
    {
        [DynamoDBHashKey]
        public string? Id { get; set; }

        [DynamoDBProperty("status")]
        public string? Status { get; set; }

        [DynamoDBProperty("title")]
        public string Title { get; set; }

        [DynamoDBProperty("uploadTime")]
        public long UploadTime { get; set; }

        [DynamoDBProperty("userId")]
        public string UserId { get; set; }

        [DynamoDBProperty("preSignedUrl")]
        public string? preSignedUrl { get; set; }

        [DynamoDBProperty("downloadRawVideoUrl")]
        public string? downloadRawVideoUrl { get; set; }

    }
}
