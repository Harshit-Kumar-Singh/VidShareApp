using System.Net.Http.Headers;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.Models;
using VidShareWebApi.Repositories;
using VidShareWebApi.Services.KafkaService;
using VidShareWebApi.Utils.S3;

namespace VidShareWebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class VideoUploadController : ControllerBase
    {
        private readonly IVideoRepository _repository;
        private readonly IKafkaService _kafkaService;
        private readonly IS3Service _s3Service;
        public VideoUploadController(IVideoRepository repository, IS3Service s3Service, IKafkaService kafkaService)
        {
            this._repository = repository;
            this._s3Service = s3Service;
            this._kafkaService = kafkaService;
        }

        [HttpPut]
        [Route("save-video-info")]
        public async Task<IActionResult> GetUploadAndDownloadUrl([FromBody] UploadItem item)
        {
            // Saving video info and getting upload and download url at the same time.
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

                // save presignedUrl and downloadUrl in db 
                item.downloadRawVideoUrl = _downloadUrl;
                item.preSignedUrl = preSignedUrl;
                await _repository.UpdateAsync(item);

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
        [HttpPost]
        [Route("upload-video/{id}")]
        public async Task<IActionResult> UploadVideo([FromRoute] string id,[FromForm] IFormFile mediaFile)
        {
            var info = await _repository.GetByIdAsync(id);
            var httpClient = new HttpClient();
            var streamContent = new StreamContent(mediaFile.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaFile.ContentType);
            var response = await httpClient.PutAsync(info.preSignedUrl, streamContent);

            // connect to kafka and send the meta info in it for transcoder service
            await _kafkaService.SendMessageAsync("test-topic", info.Id);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new
                {
                    Message = "Video Uploaded Successfully",
                    DownloadUrl = info.downloadRawVideoUrl
                });
            }
            return StatusCode((int)response.StatusCode, "Failed to upload on S3");
        }



    }
}