namespace VidShareWebApi.Utils.S3
{
    public interface IS3Service
    {
        string GetPreSignedUploadUrl(string bucketName, string objectKey, TimeSpan expiry);
        string GetDownloadUrl(string bucketName, string objectKey, TimeSpan expiry);
    }
}