using Amazon.S3;
using Amazon.S3.Model;

namespace VidShareWebApi.Utils.S3
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        public S3Service(IAmazonS3 s3Client)
        {
            this._s3Client = s3Client;
        }
        public string GetPreSignedUploadUrl(string bucketName, string objectKey, TimeSpan expiry)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = objectKey,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.Add(expiry)
            };

            return _s3Client.GetPreSignedURL(request);
        }

        public string GetDownloadUrl(string bucketName, string objectKey, TimeSpan expiry)
        {
            return _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = objectKey,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.Add(expiry)
            });

        }
        
    }
}