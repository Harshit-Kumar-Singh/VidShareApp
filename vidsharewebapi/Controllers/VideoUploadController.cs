using System.Net.Http.Headers;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Mvc;
using VidShareWebApi.Models;
using VidShareWebApi.Repositories;
using VidShareWebApi.Repositories.VideoDownloadUrlsRepo;
using VidShareWebApi.Services.KafkaService;
using VidShareWebApi.Services.VideoDownloadService;
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
        private readonly IVideoDownloadService videoDownloadService;
        private readonly IKafkaService kafkaService;
        public VideoUploadController(IVideoUploadService _videoUploadService, IKafkaService _kafkaService, IVideoDownloadService _videoDownloadService)
        {
            videoUploadService = _videoUploadService;
            videoDownloadService = _videoDownloadService;

        }

        [HttpPut]
        [Route("save-video-info")]
        public async Task<ServiceResult<string>> SaveVideoInfo([FromBody] VideoInfo info)
        {

            return await videoUploadService.SaveVideoInfo(info);

        }

        [HttpPost]
        [Route("upload-video/{id}")]
        public async Task<ServiceResult<bool>> UploadVideo([FromRoute] string id, [FromForm] IFormFile mediaFile)
        {
            return await videoUploadService.VideoUpload(id, mediaFile);
        }

        

    }
}