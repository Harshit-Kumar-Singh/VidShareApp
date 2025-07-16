using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.Models;
using VidShareWebApi.Repositories;
using VidShareWebApi.Utils.S3;

namespace VidShareWebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class VideoUploadController : ControllerBase
    {
        private readonly IVideoRepository _repository;
        private readonly IS3Service _s3Service;
        public VideoUploadController(IVideoRepository repository, IS3Service s3Service)
        {
            this._repository = repository;
            this._s3Service = s3Service;
        }
        // [HttpGet]
        // [Route("videoUpload")]
        // public ActionResult<string> VideoUpload()
        // {
        //     return Ok("Video Uploaded");
        // }

        [HttpPut]
        [Route("videoUpload")]
        public async Task<IActionResult> VideoUpload([FromBody] UploadItem item)
        {
            try
            {
                item.Id = Guid.NewGuid().ToString();
                item.UploadTime = DateTimeOffset.UnixEpoch.ToUnixTimeMilliseconds();
                // Generate object key like: videos/{id}.mp4
        

                // Save metadata to DynamoDB
                await _repository.SaveAsync(item);

                // Generate pre-signed URL
                string preSignedUrl = _s3Service.GetPreSignedUploadUrl(
                    bucketName: "vidshare-video-upload-bucket",
                    objectKey: item.Id,
                    expiry: TimeSpan.FromMinutes(15)
                );

                string _downloadUrl = _s3Service.GetDownloadUrl(
                    bucketName: "vidshare-video-upload-bucket",
                    objectKey: item.Id,
                    expiry: TimeSpan.FromMinutes(60)
                );

                return Ok(new
                {
                    message = "Metadata saved",
                    itemId = item.Id,
                    uploadUrl = preSignedUrl,
                    downloadUrl = _downloadUrl
                });
               
            }
            catch (AmazonDynamoDBException ex)
            {
                // Specific to DynamoDB errors (e.g., table not found, permission denied)
                return StatusCode(500, new { error = "DynamoDB error", details = ex.Message });
            }
            catch (Exception ex)
            {
                // General unexpected error
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }

        }
    }
}