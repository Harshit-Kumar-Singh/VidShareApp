using System.Net.Http.Headers;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.Models;
using VidShareWebApi.Repositories;
using VidShareWebApi.Services.KafkaService;
using VidShareWebApi.Services.VideoUploadService;
using VidShareWebApi.Utils;
using VidShareWebApi.Utils.S3;

namespace VidShareWebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class VideoUploadController : ControllerBase
    {
        private readonly IVideoUploadService videoUploadService;
        public VideoUploadController(IVideoUploadService _videoUploadService)
        {
            videoUploadService = _videoUploadService;
        }

        [HttpPut]
        [Route("save-video-info")]
        public async Task<ServiceResult<string>> SaveVideoInfo([FromBody] VideoInfo info)
        {

            return await videoUploadService.SaveVideoInfo(info);

        }
        /*
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

        */

    }
}